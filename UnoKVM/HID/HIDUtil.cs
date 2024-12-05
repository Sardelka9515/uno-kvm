using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoKVM.HID
{
    public static class HIDUtil
    {
        static void ThrowModifier(VK key)
        {
            throw new InvalidOperationException("modifier key: " + key);
        }

        public static HIDModifiers VKToHIDModifier(VK key)
        {
            return key switch
            {
                VK.ShiftKey => HIDModifiers.LeftShift,
                VK.LShiftKey => HIDModifiers.LeftShift,
                VK.RShiftKey => HIDModifiers.RightShift,
                VK.ControlKey => HIDModifiers.LeftCtrl,
                VK.LControlKey => HIDModifiers.LeftCtrl,
                VK.RControlKey => HIDModifiers.RightCtrl,
                VK.Menu => HIDModifiers.LeftAlt,
                VK.LMenu => HIDModifiers.LeftAlt,
                VK.RMenu => HIDModifiers.RightAlt,
                VK.LWin => HIDModifiers.LeftMeta,
                VK.RWin => HIDModifiers.RightMeta,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }

        public static HIDKey VKToHID(VK key)
        {
            return key switch
            {
                VK.A => HIDKey.KeyA,
                VK.B => HIDKey.KeyB,
                VK.C => HIDKey.KeyC,
                VK.D => HIDKey.KeyD,
                VK.E => HIDKey.KeyE,
                VK.F => HIDKey.KeyF,
                VK.G => HIDKey.KeyG,
                VK.H => HIDKey.KeyH,
                VK.I => HIDKey.KeyI,
                VK.J => HIDKey.KeyJ,
                VK.K => HIDKey.KeyK,
                VK.L => HIDKey.KeyL,
                VK.M => HIDKey.KeyM,
                VK.N => HIDKey.KeyN,
                VK.O => HIDKey.KeyO,
                VK.P => HIDKey.KeyP,
                VK.Q => HIDKey.KeyQ,
                VK.R => HIDKey.KeyR,
                VK.S => HIDKey.KeyS,
                VK.T => HIDKey.KeyT,
                VK.U => HIDKey.KeyU,
                VK.V => HIDKey.KeyV,
                VK.W => HIDKey.KeyW,
                VK.X => HIDKey.KeyX,
                VK.Y => HIDKey.KeyY,
                VK.Z => HIDKey.KeyZ,
                VK.Enter => HIDKey.KeyEnter,
                VK.Escape => HIDKey.KeyEscape,
                VK.Back => HIDKey.KeyBackspace,
                VK.Tab => HIDKey.KeyTab,
                VK.Space => HIDKey.KeySpace,
                VK.OemMinus => HIDKey.KeyMinus,
                VK.Oemplus => HIDKey.KeyEqual,
                VK.OemBackslash => HIDKey.KeyBackslash,
                VK.OemSemicolon => HIDKey.KeySemicolon,
                VK.OemQuotes => HIDKey.KeyApostrophe,
                VK.Oemtilde => HIDKey.KeyGrave,
                VK.Oemcomma => HIDKey.KeyComma,
                VK.OemPeriod => HIDKey.KeyDot,
                VK.OemQuestion => HIDKey.KeySlash,
                VK.D0 => HIDKey.Key0,
                VK.D1 => HIDKey.Key1,
                VK.D2 => HIDKey.Key2,
                VK.D3 => HIDKey.Key3,
                VK.D4 => HIDKey.Key4,
                VK.D5 => HIDKey.Key5,
                VK.D6 => HIDKey.Key6,
                VK.D7 => HIDKey.Key7,
                VK.D8 => HIDKey.Key8,
                VK.D9 => HIDKey.Key9,
                VK.OemOpenBrackets => HIDKey.KeyLeftBrace,
                VK.OemCloseBrackets => HIDKey.KeyRightBrace,
                VK.F1 => HIDKey.KeyF1,
                VK.F2 => HIDKey.KeyF2,
                VK.F3 => HIDKey.KeyF3,
                VK.F4 => HIDKey.KeyF4,
                VK.F5 => HIDKey.KeyF5,
                VK.F6 => HIDKey.KeyF6,
                VK.F7 => HIDKey.KeyF7,
                VK.F8 => HIDKey.KeyF8,
                VK.F9 => HIDKey.KeyF9,
                VK.F10 => HIDKey.KeyF10,
                VK.F11 => HIDKey.KeyF11,
                VK.F12 => HIDKey.KeyF12,
                VK.Pause => HIDKey.KeyPause,
                VK.Insert => HIDKey.KeyInsert,
                VK.Home => HIDKey.KeyHome,
                VK.PageUp => HIDKey.KeyPageUp,
                VK.Delete => HIDKey.KeyDelete,
                VK.End => HIDKey.KeyEnd,
                VK.PageDown => HIDKey.KeyPageDown,
                VK.Down => HIDKey.KeyDownArrow,
                VK.Up => HIDKey.KeyUpArrow,
                VK.NumPad1 => HIDKey.KeyNumpad1,
                VK.NumPad2 => HIDKey.KeyNumpad2,
                VK.NumPad3 => HIDKey.KeyNumpad3,
                VK.NumPad4 => HIDKey.KeyNumpad4,
                VK.NumPad5 => HIDKey.KeyNumpad5,
                VK.NumPad6 => HIDKey.KeyNumpad6,
                VK.NumPad7 => HIDKey.KeyNumpad7,
                VK.NumPad8 => HIDKey.KeyNumpad8,
                VK.NumPad9 => HIDKey.KeyNumpad9,
                VK.NumPad0 => HIDKey.KeyNumpad0,
                // VK.Compose => HIDKey.KeyCompose,
                // VK.Power => HIDKey.KeyPower,
                VK.F13 => HIDKey.KeyF13,
                VK.F14 => HIDKey.KeyF14,
                VK.F15 => HIDKey.KeyF15,
                VK.F16 => HIDKey.KeyF16,
                VK.F17 => HIDKey.KeyF17,
                VK.F18 => HIDKey.KeyF18,
                VK.F19 => HIDKey.KeyF19,
                VK.F20 => HIDKey.KeyF20,
                VK.F21 => HIDKey.KeyF21,
                VK.F22 => HIDKey.KeyF22,
                VK.F23 => HIDKey.KeyF23,
                VK.F24 => HIDKey.KeyF24,
                // VK.Open => HIDKey.KeyOpen,
                // VK.Help => HIDKey.KeyHelp,
                // VK.Props => HIDKey.KeyProps,
                // VK.Front => HIDKey.KeyFront,
                // VK.Stop => HIDKey.KeyStop,
                // VK.Again => HIDKey.KeyAgain,
                // VK.Undo => HIDKey.KeyUndo,
                // VK.Cut => HIDKey.KeyCut,
                // VK.Copy => HIDKey.KeyCopy,
                // VK.Paste => HIDKey.KeyPaste,
                // VK.Find => HIDKey.KeyFind,
                // VK.Mute => HIDKey.KeyMute,
                // VK.ShiftKey => HIDKey.ShiftKey,
                // VK.ControlKey => HIDKey.ControlKey,
                // VK.IMEConvert => HIDKey.IMEConvert,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }
    }
}
