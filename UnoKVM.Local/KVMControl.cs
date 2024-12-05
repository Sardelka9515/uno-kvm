using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Gma.System.MouseKeyHook;
using UnoKVM.HID;
using static UnoKVM.HID.Commands;
namespace UnoKVM.Local
{
    class VideoSourceItem(FilterInfo cam)
    {
        public string Name { get; } = cam.Name;
        public string MonikerString { get; } = cam.MonikerString;
        public override string ToString() => Name;
    }

    public partial class KVMControl : Form
    {
        VideoCaptureDevice? videoSource;
        InputChannel? channel;
        IKeyboardMouseEvents? inputHook;

        public KVMControl()
        {
            InitializeComponent();
            SizeChanged += (s, e) => UpdateVideoSize();
            RefreshDevices(null, null);
            pbVideo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbVideo.Paint += (s, e) =>
            {
                lock (pbVideo)
                {
                    if (tempImage != null)
                    {
                        pbVideo.Image?.Dispose();

                        pbVideo.Image = (Bitmap)tempImage.Clone();
                        tempImage.Dispose();
                        tempImage = null;
                    }
                }
            };
            UpdateVideoSize();
        }

        void UpdateVideoSize()
        {
            pbVideo.Size = new(Size.Width - 20, Size.Height - 150);
        }

        private void RefreshDevices(object? sender, EventArgs? e)
        {
            cbInputDevices.Items.Clear();
            cbInputDevices.Items.AddRange(SerialPort.GetPortNames());

            var cams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo cam in cams)
            {
                cbVideoSources.Items.Add(new VideoSourceItem(cam));
            }
        }
        List<Keys> downedKeys = [];
        private void Connect(object sender, EventArgs e)
        {
            if (videoSource == null)
            {
                var camItem = cbVideoSources.SelectedItem as VideoSourceItem;
                if (camItem == null)
                {
                    MessageBox.Show("Please select a video source");
                    return;
                }

                var inputPort = cbInputDevices.SelectedItem as string;
                if (string.IsNullOrEmpty(inputPort))
                {
                    MessageBox.Show("Please select an input device");
                    return;
                }

                channel = new InputChannel(inputPort);
                channel.Open();
                channel.Reset();

                videoSource = new VideoCaptureDevice(camItem.MonikerString);
                videoSource.Start();
                videoSource.NewFrame += UpdatePicture;


                inputHook = Hook.GlobalEvents();
                inputHook.KeyDown += (s, e) =>
                {
                    if (!downedKeys.Contains(e.KeyCode))
                    {
                        downedKeys.Add(e.KeyCode);
                    }
                    e.SuppressKeyPress = true;
                    // Debug.WriteLine(e.KeyData);
                    UpdateKeyboardState();
                };
                inputHook.KeyUp += (s, e) =>
                {
                    downedKeys.Remove(e.KeyCode);
                    e.SuppressKeyPress = true;
                    // Debug.WriteLine(e.KeyData);
                    UpdateKeyboardState();
                };

                btConnect.Text = "Disconnect";
            }
            else
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
                btConnect.Text = "Connect";
                channel?.Close();
                channel = null;
                inputHook?.Dispose();
                inputHook = null;
            }
        }

        Bitmap? tempImage = null;
        private void UpdatePicture(object sender, NewFrameEventArgs e)
        {
            lock (pbVideo)
            {
                tempImage?.Dispose();
                tempImage = e.Frame.Clone(new Rectangle(default, e.Frame.Size), e.Frame.PixelFormat);
                GC.Collect();
                pbVideo.Invalidate();
            }
        }

        private void KVMControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(uint vKey);

        const uint MAPVK_VK_TO_VSC = 0x00;
        [DllImport("user32.dll")]
        static extern uint MapVirtualKeyA(uint uCode, uint uMapType);


        Array keyValues = Enum.GetValues(typeof(Keys));

        static readonly HashSet<Keys> modifierKeys = new()
        {
            Keys.LWin,
            Keys.RWin,
            Keys.ShiftKey,
            Keys.LShiftKey,
            Keys.RShiftKey,
            Keys.ControlKey,
            Keys.LControlKey,
            Keys.RControlKey,
            Keys.Menu,
            Keys.LMenu,
            Keys.RMenu
        };
        unsafe void UpdateKeyboardState()
        {
            int keysDown = 0;
            KeyboardCommand command = default;
            for (int i = 0; i < downedKeys.Count; i++)
            {
                if (modifierKeys.Contains(downedKeys[i]))
                {
                    command.modifiers |= HIDUtil.VKToHIDModifier((VK)downedKeys[i]);
                }
                else
                {
                    var code = HIDUtil.VKToHID((VK)downedKeys[i]);
                    if (code != 0)
                    {
                        command.keys[keysDown++] = (byte)code;
                        if (keysDown >= 6)
                        {
                            break;
                        }
                    }
                }
            }
            //for (uint key = 8; key < 0xff; key++)
            //{
            //    if (GetAsyncKeyState(key) != 0)
            //    {
            //        if (modifierKeys.Contains((Keys)key))
            //        {
            //            command.modifiers |= HIDUtil.VKToHIDModifier((VK)key);
            //        }
            //        else
            //        {
            //            var code = HIDUtil.VKToHID((VK)key);
            //            if (code != 0)
            //            {
            //                command.keys[keysDown++] = (byte)code;
            //                if (keysDown >= 6)
            //                {
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            Debug.WriteLine(command.ToString());
            channel?.SendKeyboardCommand(ref command);
        }
    }
}
