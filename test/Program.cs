﻿using System.IO.Ports;
using UnoKVM.HID;
using static UnoKVM.HID.Commands;
using System.Net;
using System.Net.Sockets;

namespace test
{
    public class WifiHID(IPEndPoint ep, string name)
    {
        public IPEndPoint EndPoint { get; } = ep;
        public string Name { get; } = name;
    }



    internal class Program
    {
        const string DISCOVERY_MESSAGE = "UNOKVM-DISCOVERY";
        const string ACKNOWLEDGE_MESSAGE = "UNOKVM-ACKNOWLEDGE-";
        public static WifiHID[] GetWifiHIDs(IPAddress subnet, int timeout = 500, int iterations = 5)
        {
            List<WifiHID> hids = new List<WifiHID>();
            if (subnet.ToString().Split('.').Last() != "255")
            {
                throw new ArgumentException("Invalid broadcast address");
            }

            var udp = new UdpClient() { EnableBroadcast = true };
            udp.Client.ReceiveTimeout = timeout;
            var ep = new IPEndPoint(IPAddress.Any, 0);
            // Send a broadcast message for discovery
            var msg = System.Text.Encoding.ASCII.GetBytes(DISCOVERY_MESSAGE);
            udp.Send(msg, msg.Length, new IPEndPoint(subnet, 16500));
            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    try
                    {
                        var data = udp.Receive(ref ep);
                        var str = System.Text.Encoding.ASCII.GetString(data);
                        if (str.StartsWith(ACKNOWLEDGE_MESSAGE))
                        {
                            hids.Add(new WifiHID(ep, str.Substring(ACKNOWLEDGE_MESSAGE.Length)));
                        }
                    }
                    catch { }
                } while (udp.Available > 0);
            }
            udp.Close();
            udp.Dispose();
            return hids.ToArray();
        }

        unsafe static void Main(string[] args)
        {
            var devices = GetWifiHIDs(IPAddress.Parse("192.168.247.255"));
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
