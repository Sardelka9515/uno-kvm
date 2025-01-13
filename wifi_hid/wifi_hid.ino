#include "WiFi.h"
#include "AsyncUDP.h"
#include "USB.h"
#include "USBHIDMouse.h"
#include "USBHIDKeyboard.h"
#include <string>
#include <ArduinoNvs.h>
#include <WebServer.h>
#include "config_server.h"
#include "ArduinoJson.hpp"

using namespace std;
typedef struct MouseCommand {
  uint8_t buttons;  // Bit mask mouse buttons combination
  int8_t x;         // Mouse x delta movement, left to right
  int8_t y;         // Mouse y delta movemnt, top to bottom
  int8_t wheel;     // Not implmented
} MouseCommand;

#define RXD2 12
#define TXD2 13
#define ACKNOWLEDGE_MSG "UNOKVM-ACKNOWLEDGE-"
String reply;
String name;
String ssid;
String password;
AsyncUDP udp;
IPAddress serverIp;
uint16_t serverPort = 0;
const uint8_t COMMAND_KEYBOARD = 1;
const uint8_t COMMAND_MOUSE = 2;
const uint8_t COMMAND_RESET = 0xff;
const int MOUSE_COMMAND_SIZE = 4;
const int KEYBOARD_COMMAND_SIZE = 8;
bool configMode = false;

USBHIDKeyboard Keyboard;
USBHIDRelativeMouse Mouse;
WebServer server(80);

bool strcmp_s(char* s1, char* s2, size_t count) {
  for (int i = 0; i < count; i++) {
    if (s1[i] != s2[i]) {
      return true;
    }
  }
  return false;
}
void packetHandler(AsyncUDPPacket packet);
void ConnectToWiFi() {

  Serial1.print("Connecting to WiFi \"");
  Serial1.print(ssid);
  Serial1.print("\" with password \"");
  Serial1.print(password);
  Serial1.println("\"");

  WiFi.setHostname(name.c_str());
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  delay(100);
  if (WiFi.waitForConnectResult() == WL_CONNECTED) {
    Serial1.println("WiFi connected");
  }
}

void packetHandler(AsyncUDPPacket packet) {
  if (packet.isBroadcast() || packet.isMulticast()) {
    if (strcmp_s("UNOKVM-DISCOVERY", (char*)packet.data(), packet.length()) == 0) {
      serverIp = packet.remoteIP();
      serverPort = packet.remotePort();
      AsyncUDPMessage msg;
      msg.print(reply);
      udp.sendTo(msg, serverIp, serverPort);
      udp.flush();
    }
  } else if (true) {
    int len = packet.length();
    uint8_t* data = packet.data();
    if (len > 0) {
      switch (data[0]) {
        case COMMAND_KEYBOARD:
          if (len - 1 == KEYBOARD_COMMAND_SIZE) {
            Keyboard.sendReport((KeyReport*)(data + 1));
          }
          break;
        case COMMAND_MOUSE:
          if (len - 1 == MOUSE_COMMAND_SIZE) {
            MouseCommand* cmd = (MouseCommand*)(data + 1);
            Mouse.buttons(cmd->buttons);
            Mouse.move(cmd->x, cmd->y, cmd->wheel);
          }
          break;
        case COMMAND_RESET:
          Keyboard.releaseAll();
          Mouse.buttons(0);
          break;
      }
    }
  }
}

void handleReboot() {
  server.send(200);
  delay(200);
  ESP.restart();
}

void handleReset() {
  NVS.eraseAll();
  server.send(200);
}

void handleIndex() {
  server.send(200, "text/html", CONFIG_HTML);
}
void handleApply() {
  server.send(200, "text/html", APPLY_HTML);
}

char jsonBuf[1024];

void handleGetConfig() {
  ArduinoJson::JsonDocument doc;
  doc["device_name"] = NVS.getString("device_name");
  doc["wifi_ssid"] = NVS.getString("wifi_ssid");
  doc["wifi_password"] = NVS.getString("wifi_password");
  ArduinoJson::serializeJsonPretty(doc, jsonBuf, sizeof(jsonBuf));
  server.sendHeader("Cache-Control", "no-cache");
  server.send(200, "text/javascript; charset=utf-8", jsonBuf);
}

void handleSetConfig() {
  NVS.setString("device_name", server.arg("device_name"));
  NVS.setString("wifi_ssid", server.arg("wifi_ssid"));
  NVS.setString("wifi_password", server.arg("wifi_password"));
  NVS.commit();
  server.sendHeader("Location", "/apply", true);
  server.send(302);
}
void startConfigServer() {
  Serial1.println("Entered config mode");
  WiFi.disconnect();
  WiFi.setHostname(name.c_str());
  WiFi.mode(WIFI_AP);
  WiFi.softAP("UNOKVM-CONFIG", "12345678");
  delay(100);
  Serial1.println("AP started");
  IPAddress Ip(192, 168, 0, 1);
  IPAddress NMask(255, 255, 255, 0);
  WiFi.softAPConfig(Ip, Ip, NMask);
  Serial1.println("Starting config server...");

  server.on("/", HTTP_GET, handleIndex);
  server.on("/apply", HTTP_GET, handleApply);
  server.on("/api/reboot", HTTP_POST, handleReboot);
  server.on("/api/config", HTTP_GET, handleGetConfig);
  server.on("/api/config", HTTP_POST, handleSetConfig);
  server.on("/api/reset", HTTP_POST, handleReset);
  server.begin();

  Serial1.print("Server started. Open http://");
  Serial1.print(WiFi.getHostname());
  Serial1.print(" or http://");
  Serial1.println(WiFi.softAPIP().toString().c_str());
}

void setup() {
  pinMode(0, INPUT_PULLUP);
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(115200);
  Serial1.begin(115200, SERIAL_8N1, RXD2, TXD2);
  NVS.begin();

  name = String(NVS.getString("device_name").c_str());
  ssid = String(NVS.getString("wifi_ssid").c_str());
  password = String(NVS.getString("wifi_password").c_str());
  if (name.length() == 0) {
    name = "UNOKVM";
    configMode = true;
  }

  Serial1.print("Device started: ");
  Serial1.println(name);

  if (configMode) {
    startConfigServer();
  } else {
    // Set up udp discovery reply
    reply = String(ACKNOWLEDGE_MSG) + name;
    ConnectToWiFi();
    udp.onPacket(packetHandler);
    udp.listen(16500);
    Mouse.begin();
    Keyboard.begin();
    USB.begin();
  }
}

void loop() {
  if (configMode) {
    server.handleClient();
  } else {
    if (!WiFi.isConnected()) {
      Serial1.println("Reconnecting WiFi...");
      ConnectToWiFi();
      delay(200);
    }
    if (digitalRead(0) == LOW) {
      int i = 0;
      // Wait for 5-second continuous press
      do {
        i++;
        delay(100);
      } while (i < 50 && digitalRead(0) == LOW);
      if (i == 50) {
        configMode = true;
        startConfigServer();
      }
    }
  }
}
