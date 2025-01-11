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

            cbVideoSources.Items.Clear();
            var cams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo cam in cams)
            {
                cbVideoSources.Items.Add(new VideoSourceItem(cam));
            }
        }

        InputBridge? bridge;
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

                bridge = new UARTInputBridge(inputPort);

                videoSource = new VideoCaptureDevice(camItem.MonikerString);
                videoSource.Start();
                videoSource.NewFrame += UpdatePicture;

                inputHook = Hook.GlobalEvents();
                inputHook.KeyDown += (s, e) =>
                {
                    bridge.KeyDown(e);
                };
                inputHook.KeyUp += (s, e) =>    
                {
                    bridge.KeyUp(e);
                };

                btConnect.Text = "Disconnect";
            }
            else
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
                btConnect.Text = "Connect";
                bridge?.Dispose();
                bridge = null;
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

    }
}
