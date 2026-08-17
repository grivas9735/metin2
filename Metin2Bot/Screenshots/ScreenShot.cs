using System.Diagnostics.Metrics;

namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        public static void SacarScreenshotPantallaLogin(Metin2 metin)
        {
            Rectangle captureArea = new Rectangle(metin.StartX + 850, metin.StartY + 600, 180, 150);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\metin_login_{metin.Id}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotFragmentos(Metin2 metin)
        {
            Rectangle captureArea = new Rectangle(metin.StartX, metin.StartY - 100, 1500, 820);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\metin_frag_{metin.Id}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotChampSelect(Metin2 metin)
        {
            Rectangle captureArea = new Rectangle(metin.StartX - 20, metin.StartY - 20, 150, 50);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\metin_champ_select_{metin.Id}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotInsideGame(Metin2 metin)
        {
            Rectangle captureArea = new Rectangle(metin.StartX + 1480, metin.StartY + 20, 70, 25);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\metin_inside_game_{metin.Id}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotEstaMuerto(Metin2 metin)
        {
            Rectangle captureArea = new Rectangle(metin.StartX + 40, metin.StartY - 70, 200, 80);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\metin_esta_muerto_{metin.Id}.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}