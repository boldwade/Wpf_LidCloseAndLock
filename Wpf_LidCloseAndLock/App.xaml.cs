using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Wpf_LidCloseAndLock {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private ProcessIcon _pi;

        //http://www.pinvoke.net/default.aspx/user32.LockWorkStation]
        [DllImport(@"User32", SetLastError = true)]
        public static extern bool LockWorkStation();

        [DllImport(@"User32", SetLastError = true, EntryPoint = "RegisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid powerSettingGuid, int flags);

        internal struct PowerbroadcastSetting {
            public Guid PowerSetting;
            public uint DataLength;
            public byte Data;
        }

        private Guid _guidLidswitchStateChange = new Guid(0xBA3E0F4D, 0xB817, 0x4094, 0xA2, 0xD1, 0xD5, 0x63, 0x79, 0xE6, 0xA0, 0xF3);
        private const int DeviceNotifyWindowHandle = 0x00000000;
        private const int WmPowerbroadcast = 0x0218;
        private const int PbtPowersettingchange = 0x8013;

        private bool? _previousLidState;

        private void App_OnStartup(object sender, StartupEventArgs e) {
            Console.WriteLine(@"here: handle=" + Process.GetCurrentProcess().Handle);

            _pi = new ProcessIcon();
            _pi.Display();

            InitializePowerSettingNotification();
        }

        private void App_OnExit(object sender, ExitEventArgs e) {
            _pi.Dispose();
        }

        private void InitializePowerSettingNotification() {
            var stubWindow = new Window { Visibility = Visibility.Hidden, Height = 0, Width = 0, WindowStyle = WindowStyle.None, WindowState = WindowState.Minimized };
            stubWindow.Show();
            var hwnd = new WindowInteropHelper(stubWindow).Handle;
            RegisterPowerSettingNotification(hwnd, ref _guidLidswitchStateChange, DeviceNotifyWindowHandle);

            HwndSource.FromHwnd(hwnd)?.AddHook(WndProc);
            stubWindow.Hide();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            if (msg == WmPowerbroadcast)
                OnPowerBroadcast(wParam, lParam);
            return IntPtr.Zero;
        }

        private void OnPowerBroadcast(IntPtr wParam, IntPtr lParam) {
            if ((int)wParam != PbtPowersettingchange) return;

            var ps = (PowerbroadcastSetting)Marshal.PtrToStructure(lParam, typeof(PowerbroadcastSetting));
            var pData = (IntPtr)((int)lParam + Marshal.SizeOf(ps));
            var unused = (int)Marshal.PtrToStructure(pData, typeof(int));
            if (ps.PowerSetting != _guidLidswitchStateChange) return;

            var isLidOpen = ps.Data != 0;

            if (!isLidOpen == _previousLidState) {
                LidStatusChanged(isLidOpen);
            }

            _previousLidState = isLidOpen;
        }

        private static void LidStatusChanged(bool isLidOpen) {
            var message = string.Format(isLidOpen ? $@"{DateTime.Now}: Lid opened!" : $@"{DateTime.Now}: Lid closed!");
            Debug.WriteLine(message);

            if (!isLidOpen) {
                LockWorkStation();
            }
        }
    }
}
