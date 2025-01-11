using System.Diagnostics;
using UnoKVM.HID;
using static UnoKVM.HID.Commands;

namespace UnoKVM.Local
{
    public abstract class InputBridgeBase : IDisposable
    {
        public static readonly HashSet<Keys> ModifierKeys = new()
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
        private bool disposedValue;

        public abstract void KeyDown(KeyEventArgs e);
        public abstract void KeyUp(KeyEventArgs e);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~InputBridge()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class InputBridge : InputBridgeBase, IDisposable
    {
        List<Keys> downedKeys = [];
        IInputChannel channel;
        private bool disposedValue;

        public InputBridge(IInputChannel inputChannel)
        {
            channel = inputChannel;
            channel.Reset();
        }
        public override void KeyDown(KeyEventArgs e)
        {
            if (!downedKeys.Contains(e.KeyCode))
            {
                downedKeys.Add(e.KeyCode);
            }
            e.SuppressKeyPress = true;
            // Debug.WriteLine(e.KeyData);
            UpdateKeyboardState();
        }
        public override void KeyUp(KeyEventArgs e)
        {
            downedKeys.Remove(e.KeyCode);
            e.SuppressKeyPress = true;
            // Debug.WriteLine(e.KeyData);
            UpdateKeyboardState();
        }

        unsafe void UpdateKeyboardState()
        {
            int keysDown = 0;
            KeyboardCommand command = default;
            for (int i = 0; i < downedKeys.Count; i++)
            {
                if (ModifierKeys.Contains(downedKeys[i]))
                {
                    command.modifiers |= HIDUtil.VKToHIDModifier((VK)downedKeys[i]);
                }
                else
                {
                    var code = HIDUtil.VKToHIDKey((VK)downedKeys[i]);
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
            Debug.WriteLine(command.ToString());
            channel?.SendKeyboardCommand(ref command);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                channel.Dispose();
            }
        }
    }
}
