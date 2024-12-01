/* Arduino USB Keyboard and Mouse HID demo
*
* Sends "Hello World" to the host PC and draw a CCW square with the mouse
*
*/

/* Author: Sardelka
 * Released into the public domain.
 */
#include "uno_hid.h"

void setup();
void loop();

#define KEY_LEFT_CTRL 0x01
#define KEY_LEFT_SHIFT 0x02
#define KEY_RIGHT_CTRL 0x10
#define KEY_RIGHT_SHIFT 0x20

void setup() {
  Serial.begin(9600);
  delay(200);
}

void keyboardDemo() {
  SendText("Hello World");
}

void mouseDemo() {
  uint8_t ind = 0;
  MouseCommand command = {};
  command.buttons = 0;
  command.wheel = 0;
  command.deltaY = 0;

  command.deltaX = -2;
  for (ind = 0; ind < 20; ind++) {
    SendMouseCommand(&command);
    SendMouseCommand(&mouse_command_null);
  }

  command.deltaX = 0;
  command.deltaY = -2;
  for (ind = 0; ind < 20; ind++) {
    SendMouseCommand(&command);
    SendMouseCommand(&mouse_command_null);
  }

  command.deltaX = 2;
  command.deltaY = 0;
  for (ind = 0; ind < 20; ind++) {
    SendMouseCommand(&command);
    SendMouseCommand(&mouse_command_null);
  }

  command.deltaX = 0;
  command.deltaY = 2;
  for (ind = 0; ind < 20; ind++) {
    SendMouseCommand(&command);
    SendMouseCommand(&mouse_command_null);
  }
}

void loop() {
  delay(3000);
  mouseDemo();
  delay(3000);
  keyboardDemo();
}
