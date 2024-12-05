using System.IO.Ports;
using UnoKVM.HID;
using static UnoKVM.HID.Commands;
namespace test
{
    internal class Program
    {
        unsafe static void Main(string[] args)
        {

            InputChannel? channel = null;
            while (true)
            {
                if (channel == null || !channel.IsOpen)
                {
                    try
                    {
                        var ports = SerialPort.GetPortNames();
                        foreach (var port in ports)
                        {
                            Console.WriteLine(port);
                        }
                        channel = new InputChannel(ports.First());
                        channel.Open();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    channel.Reset();
                    KeyboardCommand command = default;
                    command.keys[0] = (byte)HIDKey.KeyH;
                    channel.SendKeyboardCommand(ref command);
                    command = default;
                    channel.SendKeyboardCommand(ref command);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
