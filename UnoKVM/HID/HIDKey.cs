namespace UnoKVM.HID
{
    [Flags]
    public enum HIDModifiers : byte
    {
        // Modifier masks
        LeftCtrl = 0x01,
        LeftShift = 0x02,
        LeftAlt = 0x04,
        LeftMeta = 0x08,
        RightCtrl = 0x10,
        RightShift = 0x20,
        RightAlt = 0x40,
        RightMeta = 0x80,
    }

    /// <summary>
    /// USB HID Keyboard scan codes as per USB spec 1.11
    /// Adapted from original C header by MightyPork
    /// </summary>
    public enum HIDKey : byte
    {

        // Special key codes
        None = 0x00,
        ErrorRollOver = 0x01,

        // Alphabetic keys
        KeyA = 0x04,
        KeyB = 0x05,
        KeyC = 0x06,
        KeyD = 0x07,
        KeyE = 0x08,
        KeyF = 0x09,
        KeyG = 0x0A,
        KeyH = 0x0B,
        KeyI = 0x0C,
        KeyJ = 0x0D,
        KeyK = 0x0E,
        KeyL = 0x0F,
        KeyM = 0x10,
        KeyN = 0x11,
        KeyO = 0x12,
        KeyP = 0x13,
        KeyQ = 0x14,
        KeyR = 0x15,
        KeyS = 0x16,
        KeyT = 0x17,
        KeyU = 0x18,
        KeyV = 0x19,
        KeyW = 0x1A,
        KeyX = 0x1B,
        KeyY = 0x1C,
        KeyZ = 0x1D,

        // Number keys
        Key1 = 0x1E,
        Key2 = 0x1F,
        Key3 = 0x20,
        Key4 = 0x21,
        Key5 = 0x22,
        Key6 = 0x23,
        Key7 = 0x24,
        Key8 = 0x25,
        Key9 = 0x26,
        Key0 = 0x27,

        // Control and special keys
        KeyEnter = 0x28,
        KeyEscape = 0x29,
        KeyBackspace = 0x2A,
        KeyTab = 0x2B,
        KeySpace = 0x2C,
        KeyMinus = 0x2D,
        KeyEqual = 0x2E,
        KeyLeftBrace = 0x2F,
        KeyRightBrace = 0x30,
        KeyBackslash = 0x31,
        KeyHashTilde = 0x32,
        KeySemicolon = 0x33,
        KeyApostrophe = 0x34,
        KeyGrave = 0x35,
        KeyComma = 0x36,
        KeyDot = 0x37,
        KeySlash = 0x38,
        KeyCapsLock = 0x39,

        // Function keys
        KeyF1 = 0x3A,
        KeyF2 = 0x3B,
        KeyF3 = 0x3C,
        KeyF4 = 0x3D,
        KeyF5 = 0x3E,
        KeyF6 = 0x3F,
        KeyF7 = 0x40,
        KeyF8 = 0x41,
        KeyF9 = 0x42,
        KeyF10 = 0x43,
        KeyF11 = 0x44,
        KeyF12 = 0x45,

        // Navigation and system keys
        KeyPrintScreen = 0x46,
        KeyScrollLock = 0x47,
        KeyPause = 0x48,
        KeyInsert = 0x49,
        KeyHome = 0x4A,
        KeyPageUp = 0x4B,
        KeyDelete = 0x4C,
        KeyEnd = 0x4D,
        KeyPageDown = 0x4E,
        KeyRightArrow = 0x4F,
        KeyLeftArrow = 0x50,
        KeyDownArrow = 0x51,
        KeyUpArrow = 0x52,

        // Numpad keys
        KeyNumLock = 0x53,
        KeyNumpadSlash = 0x54,
        KeyNumpadAsterisk = 0x55,
        KeyNumpadMinus = 0x56,
        KeyNumpadPlus = 0x57,
        KeyNumpadEnter = 0x58,
        KeyNumpad1 = 0x59,
        KeyNumpad2 = 0x5A,
        KeyNumpad3 = 0x5B,
        KeyNumpad4 = 0x5C,
        KeyNumpad5 = 0x5D,
        KeyNumpad6 = 0x5E,
        KeyNumpad7 = 0x5F,
        KeyNumpad8 = 0x60,
        KeyNumpad9 = 0x61,
        KeyNumpad0 = 0x62,
        KeyNumpadDot = 0x63,

        // Additional keys
        Key102nd = 0x64,
        KeyCompose = 0x65,
        KeyPower = 0x66,
        KeyNumpadEqual = 0x67,

        // Extended function keys
        KeyF13 = 0x68,
        KeyF14 = 0x69,
        KeyF15 = 0x6A,
        KeyF16 = 0x6B,
        KeyF17 = 0x6C,
        KeyF18 = 0x6D,
        KeyF19 = 0x6E,
        KeyF20 = 0x6F,
        KeyF21 = 0x70,
        KeyF22 = 0x71,
        KeyF23 = 0x72,
        KeyF24 = 0x73,

        // Media and special function keys
        KeyOpen = 0x74,
        KeyHelp = 0x75,
        KeyProps = 0x76,
        KeyFront = 0x77,
        KeyStop = 0x78,
        KeyAgain = 0x79,
        KeyUndo = 0x7A,
        KeyCut = 0x7B,
        KeyCopy = 0x7C,
        KeyPaste = 0x7D,
        KeyFind = 0x7E,
        KeyMute = 0x7F,
        KeyVolumeUp = 0x80,
        KeyVolumeDown = 0x81,

        // International and language keys
        KeyRo = 0x87,
        KeyKatakanaHiragana = 0x88,
        KeyYen = 0x89,
        KeyHenkan = 0x8A,
        KeyMuhenkan = 0x8B,
        KeyNumpadJpComma = 0x8C,
        KeyHangeul = 0x90,
        KeyHanja = 0x91,
        KeyKatakana = 0x92,
        KeyHiragana = 0x93,
        KeyZenkakuHankaku = 0x94,

        // Keypad extended keys
        KeyNumpadLeftParen = 0xB6,
        KeyNumpadRightParen = 0xB7,

        // Modifier keys
        KeyLeftControl = 0xE0,
        KeyLeftShift = 0xE1,
        KeyLeftAlt = 0xE2,
        KeyLeftGui = 0xE3,
        KeyRightControl = 0xE4,
        KeyRightShift = 0xE5,
        KeyRightAlt = 0xE6,
        KeyRightGui = 0xE7,

        // Media control keys
        KeyMediaPlayPause = 0xE8,
        KeyMediaStopCd = 0xE9,
        KeyMediaPreviousSong = 0xEA,
        KeyMediaNextSong = 0xEB,
        KeyMediaEjectCd = 0xEC,
        KeyMediaVolumeUp = 0xED,
        KeyMediaVolumeDown = 0xEE,
        KeyMediaMute = 0xEF,
        KeyMediaWww = 0xF0,
        KeyMediaBack = 0xF1,
        KeyMediaForward = 0xF2,
        KeyMediaStop = 0xF3,
        KeyMediaFind = 0xF4,
        KeyMediaScrollUp = 0xF5,
        KeyMediaScrollDown = 0xF6,
        KeyMediaEdit = 0xF7,
        KeyMediaSleep = 0xF8,
        KeyMediaCoffee = 0xF9,
        KeyMediaRefresh = 0xFA,
        KeyMediaCalc = 0xFB
    }
}