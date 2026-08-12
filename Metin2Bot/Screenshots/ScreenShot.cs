namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        public static void SacarScreenshot1024x768(string fileName, int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 340, height + 160, 300, 200);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshot1600x900(string fileName, int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 550, height + 200, 450, 250);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotSoloEcuacion(string fileName, int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 465, height + 225, 55, 40);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotPantalla(string fileName, int x, int y, int width, int height)
        {
            Rectangle captureArea = new Rectangle(x, y, width, height);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotMPs(int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 2180, height + 200, 110, 700);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\mps.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotPantallaLogin(int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 850, height + 600, 180, 150);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\mps.png", System.Drawing.Imaging.ImageFormat.Png);
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

        public static void SacarScreenshotChampSelect(int width, int height)
        {
            Rectangle captureArea = new Rectangle(width - 20, height - 20, 150, 50);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\mps.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotInsideGame(int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 1480, height + 20, 70, 25);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\mps.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotEstaMuerto(int width, int height)
        {
            Rectangle captureArea = new Rectangle(width + 40, height - 70, 200, 80);

            // Tomar el screenshot
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(AppConfig.GetRouteValue("MPs") + @$"\mps.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}