using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static UnoKVM.HID.Commands;
using static UnoKVM.HID.HIDKey;


namespace UnoKVM.HID
{
    public class UdpInputChannel : UdpClient
    {
        public void SendKeyboardCommand(KeyboardCommand command) => SendKeyboardCommand(ref command);
        public void SendMouseCommand(MouseCommand command) => SendMouseCommand(ref command);
        public unsafe void SendKeyboardCommand(ref KeyboardCommand command)
        {
            var data = new byte[KEYBOARD_COMMAND_SIZE + 1];
            data[0] = COMMAND_KEYBOARD;
            data[1] = (byte)command.modifiers;
            data[2] = (byte)command.reserved;
            for (int i = 0; i < 6; i++)
            {
                data[3 + i] = command.keys[i];
            }
            Send(data, data.Length);
        }
        public void SendMouseCommand(ref MouseCommand command)
        {
            var data = new byte[MOUSE_COMMAND_SIZE + 1];
            data[0] = COMMAND_MOUSE;
            data[1] = command.buttons;
            data[2] = (byte)command.deltaX;
            data[3] = (byte)command.deltaY;
            data[4] = (byte)command.wheel;
            Send(data, data.Length);
        }
        public void Reset()
        {
            Send([COMMAND_RESET], 1);
        }
    }
}
