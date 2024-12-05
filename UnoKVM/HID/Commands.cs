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
        public const uint8_t COMMAND_RESET = 0xff;
        public const int MOUSE_COMMAND_SIZE = 4;
        public const int KEYBOARD_COMMAND_SIZE = 8;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct KeyboardCommand
        {
            public HIDModifiers modifiers;    // Bit mask of modifiers combination
            public HIDKey reserved;     // OEM reserved, should always set to 0
            public fixed byte keys[6];
            public override string ToString()
            {
                var str = modifiers.ToString() + ':';
                for (int i = 0; i < 6; i++)
                {
                    str += ((HIDKey)keys[i]).ToString() + ',';
                }
                return str;
            }
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