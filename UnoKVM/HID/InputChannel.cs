using static UnoKVM.HID.Commands;
namespace UnoKVM.HID
{
    public interface IInputChannel : IDisposable
    {
        public void SendKeyboardCommand(KeyboardCommand command) => SendKeyboardCommand(ref command);
        public void SendMouseCommand(MouseCommand command) => SendMouseCommand(ref command);
        public void SendKeyboardCommand(ref KeyboardCommand command);
        public void SendMouseCommand(ref MouseCommand command);
        public void Reset();
    }
}
