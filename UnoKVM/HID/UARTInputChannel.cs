using System.Diagnostics;
using System.IO.Ports;
using static UnoKVM.HID.Commands;
using static UnoKVM.HID.HIDKey;

namespace UnoKVM.HID
{
    public class UartInputChannel : SerialPort, IInputChannel
    {
        public UartInputChannel(string portName) : base(portName, 9600)
        {
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            ReadTimeout = 500;
            WriteTimeout = 500;
        }
        public void Reset() => BaseStream.WriteByte(COMMAND_RESET);
        public void SendKeyboardCommand(KeyboardCommand command) => SendKeyboardCommand(ref command);
        public void SendMouseCommand(MouseCommand command) => SendMouseCommand(ref command);
        public unsafe void SendKeyboardCommand(ref KeyboardCommand command)
        {
            Debug.Assert(sizeof(KeyboardCommand) == KEYBOARD_COMMAND_SIZE);
            BaseStream.WriteByte(COMMAND_KEYBOARD);
            fixed (KeyboardCommand* pCommand = &command)
            {
                BaseStream.Write(new ReadOnlySpan<byte>((byte*)pCommand, sizeof(KeyboardCommand)));
            }
        }

        public unsafe void SendMouseCommand(ref MouseCommand command)
        {
            Debug.Assert(sizeof(MouseCommand) == MOUSE_COMMAND_SIZE);
            BaseStream.WriteByte(COMMAND_KEYBOARD);
            fixed (MouseCommand* pCommand = &command)
            {
                BaseStream.Write(new ReadOnlySpan<byte>((byte*)pCommand, sizeof(MouseCommand)));
            }
        }

        /* Convert a ascii string to QWERTY keyboard input commands */
        unsafe void SendText(byte* str)
        {
            KeyboardCommand command = default;
            byte c = *str;
            do
            {
                command.modifiers = 0;

                if (c >= 'a' && c <= 'z')
                {
                    command.keys[0] = (byte)(c - 'a' + 4);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    command.modifiers = HIDModifiers.LeftShift; /* Caps */
                    command.keys[0] = (byte)(c - 'A' + 4);
                }
                else
                {
                    switch (c)
                    {
                        case (byte)' ':
                            command.keys[0] = (byte)KeySpace;  // Space
                            break;
                        case (byte)'!':
                            command.modifiers = HIDModifiers.LeftShift; /* Caps */
                            command.keys[0] = (byte)Key1;            // 1
                            break;
                        default:
                            /* Character not handled. To do: add rest of chars from HUT1_11.pdf */
                            command.keys[0] = 0x37;  // Period
                            break;
                    }
                }

                SendKeyboardCommand(ref command);
                SendKeyboardCommand(KEYBOARD_COMMAND_NULL);
            } while ((c = *++str) != 0);
        }
    }
}
