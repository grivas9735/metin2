using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Metin2Bot
{
    public static class User
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // Importamos IsIconic de la API de Windows
        // Devuelve 'true' si la ventana referenciada por la Handle está minimizada
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        private const int SW_SHOW = 5;  // Muestra la ventana si está oculta
        private const int SW_RESTORE = 9; // Restaura la ventana si está minimizada

        public static void MouseToPosition(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        public static async Task ClickAt(int x, int y, int ms = 40)
        {
            Cursor.Position = new Point(x, y);
            await Task.Delay(ms);
            User.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(ms);
            User.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            await Task.Delay(ms);
        }

        public static async Task RightClickAt(int x, int y, int ms = 40)
        {
            Cursor.Position = new Point(x, y);
            await Task.Delay(ms);
            User.mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            await Task.Delay(ms);
            User.mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            await Task.Delay(ms);
        }

        public static async Task MostrarVentanaActual(IntPtr activeWindow)
        {
            User.SetForegroundWindow(activeWindow);
            await Task.Delay(50);
        }

        public static async Task MostrarMetin(IntPtr metin)
        {
            User.SetForegroundWindow(metin);
            User.ShowWindow(metin, SW_RESTORE);
            await Task.Delay(50);
        }

        public static bool EsMetinEnPrimerPlano(IntPtr metin)
        {
            if (metin == IntPtr.Zero)
                return false;

            return User.GetForegroundWindow() == metin;
        }

        public static void PresionarDigito(MiButton btn, char digito)
        {
            if (digito == '-')
            {
                btn.PresionarYSoltar(MiButton.BT7.OEM_MINUS).Wait();
            }
            if (digito == '+')
            {
                btn.PresionarYSoltar(MiButton.BT7.OEM_PLUS).Wait();
            }
            else if (digito == '0')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_0).Wait();
            }
            else if (digito == '1')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_1).Wait();
            }
            else if (digito == '2')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_2).Wait();
            }
            else if (digito == '3')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_3).Wait();
            }
            else if (digito == '4')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_4).Wait();
            }
            else if (digito == '5')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_5).Wait();
            }
            else if (digito == '6')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_6).Wait();
            }
            else if (digito == '7')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_7).Wait();
            }
            else if (digito == '8')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_8).Wait();
            }
            else if (digito == '9')
            {
                btn.PresionarYSoltar(MiButton.BT7.KEY_9).Wait();
            }
        }
    }

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}