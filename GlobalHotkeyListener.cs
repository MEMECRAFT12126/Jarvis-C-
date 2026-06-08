using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jarvis.Listeners
{
    public class GlobalHotkeyListener : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private const uint MOD_CONTROL = 0x0002;
        private const uint VK_SPACE = 0x20;

        private NativeWindow _window;
        private bool _registered = false;
        public event Action HotkeyPressed;

        public void Register()
        {
            if (_registered) return;
            _window = new HotkeyWindow(this);
            _window.CreateHandle(new CreateParams());
            if (RegisterHotKey(_window.Handle, HOTKEY_ID, MOD_CONTROL, VK_SPACE))
                _registered = true;
        }

        public void Unregister()
        {
            if (_registered && _window?.Handle != IntPtr.Zero)
                UnregisterHotKey(_window.Handle, HOTKEY_ID);
            _window?.DestroyHandle();
        }

        private void OnHotkeyPressed() => HotkeyPressed?.Invoke();

        private class HotkeyWindow : NativeWindow
        {
            private GlobalHotkeyListener _listener;
            public HotkeyWindow(GlobalHotkeyListener listener) => _listener = listener;
            protected override void WndProc(ref Message m)
            {
                const int WM_HOTKEY = 0x0312;
                if (m.Msg == WM_HOTKEY && (int)m.WParam == HOTKEY_ID)
                    _listener.OnHotkeyPressed();
                base.WndProc(ref m);
            }
        }

        public void Dispose() => Unregister();
    }
}