/*
* HID input commands for the firmware
*/
/* Author: Sardelka
 * Released into the public domain.
 */

#include "hid_keys.h"
#define COMMAND_KEYBOARD (uint8_t)1
#define COMMAND_MOUSE (uint8_t)2
#define MOUSE_COMMAND_SIZE 4
#define KEYBOARD_COMMAND_SIZE 8

typedef struct KeyboardCommand {
  uint8_t modifiers = 0;
  uint8_t reserved = 0;
  uint8_t keys[6] = { 0 };
} KeyboardCommand;

typedef struct MouseCommand {
  uint8_t buttons;
  int8_t deltaX;
  int8_t deltaY;
  int8_t wheel;
} MouseCommand;


const KeyboardCommand keyboard_command_null = {};
const MouseCommand mouse_command_null = { .buttons = 0, .deltaX = 0, .deltaY = 0, .wheel = 0 };

void SendKeyboardCommand(KeyboardCommand* pCommand) {
  Serial.write(COMMAND_KEYBOARD);
  Serial.write((uint8_t*)pCommand, 8);
}

void SendMouseCommand(MouseCommand* pCommand) {
  Serial.write(COMMAND_MOUSE);
  Serial.write((uint8_t*)pCommand, sizeof(MouseCommand));
}

void SendText(const char* str) {
  KeyboardCommand command;
  char c = *str;
  do {
    command.modifiers = 0;

    if ((c >= 'a') && (c <= 'z')) {
      command.keys[0] = c - 'a' + 4;
    } else if ((c >= 'A') && (c <= 'Z')) {
      command.modifiers = MOD_SHIFT_LEFT; /* Caps */
      command.keys[0] = c - 'A' + 4;
    } else {
      switch (c) {
        case ' ':
          command.keys[0] = KEY_SPACE;  // Space
          break;
        case '!':
          command.modifiers = MOD_SHIFT_LEFT; /* Caps */
          command.keys[0] = KEY_1;            // Space
          break;
        default:
          /* Character not handled. To do: add rest of chars from HUT1_11.pdf */
          command.keys[0] = 0x37;  // Period
          break;
      }
    }

    SendKeyboardCommand(&command);
    SendKeyboardCommand(&keyboard_command_null);
  } while (c = *(++str));
}