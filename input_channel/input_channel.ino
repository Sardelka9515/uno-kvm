#include <SoftwareSerial.h>
#define rxPin 3
#define txPin 4
SoftwareSerial channel(rxPin, txPin);
void setup() {  // Define pin modes for TX and RX

  pinMode(rxPin, INPUT);

  pinMode(txPin, OUTPUT);

  channel.begin(9600);
  Serial.begin(9600);
  delay(200);
}

int available;
int read;
void loop() {
  available = channel.available();
  for (int i = 0; i < available; i++) {
    read = channel.read();
    if (read == -1) {
      break;
    }
    Serial.write(read);
  }
}
