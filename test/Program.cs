using UnoKVM.HID;
using static UnoKVM.HID.Commands;

namespace test
{



    internal class Program
    {
        unsafe static void Main(string[] args)
        {

            var devices = HIDUtil.DiscoverUdpHIDs();
            foreach (var device in devices)
            {
                Console.WriteLine($"Found device {device.Name} at {device.EndPoint}");
            }
            var dev = devices.First();
            var channel = new UdpInputChannel();
            channel.Connect(dev.EndPoint);
            channel.Reset();
            Task.Run(() =>
            {
                while (true)
                {
                    var cmd = new KeyboardCommand { modifiers = 0, reserved = 0 };
                    cmd.keys[0] = (byte)HIDKey.KeyH;
                    channel.SendKeyboardCommand(ref cmd);
                    channel.SendKeyboardCommand(KEYBOARD_COMMAND_NULL);
                    cmd.modifiers = HIDModifiers.LeftShift;
                    channel.SendKeyboardCommand(ref cmd);
                    channel.SendKeyboardCommand(KEYBOARD_COMMAND_NULL);
                    channel.SendMouseCommand(new MouseCommand { buttons = 0, deltaX = 10, deltaY = 10, wheel = 0 });
                    channel.SendMouseCommand(MOUSE_COMMAND_NULL);
                    channel.SendMouseCommand(new MouseCommand { buttons = 0b10, deltaX = 10, deltaY = 10, wheel = 0 });
                    channel.SendMouseCommand(MOUSE_COMMAND_NULL);
                    Thread.Sleep(1000);
                }
            });
            Console.ReadLine();
        }
    }
}
