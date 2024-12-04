/*
* HID input commands for the firmware
*/
/* Author: Sardelka
 * Released into the public domain.
 */
using System.Runtime.InteropServices;

namespace UnoKVM.HID
{
    public static class Commands
    {


        public const uint8_t COMMAND_KEYBOARD = 1;
        public const uint8_t COMMAND_MOUSE = 2;
        public const int MOUSE_COMMAND_SIZE = 4;
        public const int KEYBOARD_COMMAND_SIZE = 8;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct KeyboardCommand
        {
            public uint8_t modifiers;    // Bit mask of modifiers combination
            public uint8_t reserved;     // OEM reserved, should always set to 0
            public fixed uint8_t keys[6];
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseCommand
        {
            public uint8_t buttons;      // Bit mask mouse buttons combination
            public int8_t deltaX;        // Mouse x delta movement, left to right
            public int8_t deltaY;        // Mouse y delta movemnt, top to bottom
            public int8_t wheel;         // Not implmented
        };


        public static readonly KeyboardCommand KEYBOARD_COMMAND_NULL = default;
        public static readonly MouseCommand MOUSE_COMMAND_NULL = default;
    }
}