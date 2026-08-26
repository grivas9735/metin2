namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        public static void SacarScreenshotPantallaLogin(Metin2 metin)
        {
            Rectangle captureArea = new(metin.StartX + 850, metin.StartY + 600, 180, 150);

            // Tomar el screenshot
            using Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgLoginName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotFragmentos(Metin2 metin)
        {
            Rectangle captureArea = new(metin.StartX, metin.StartY - 100, 1500, 820);

            // Tomar el screenshot
            using Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgFragmentosName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotChampSelect(Metin2 metin)
        {
            Rectangle captureArea = new(metin.StartX - 20, metin.StartY - 20, 150, 50);

            // Tomar el screenshot
            using Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgChampSelectName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotEstaMuerto(Metin2 metin)
        {
            Rectangle captureArea = new(metin.StartX + 40, metin.StartY - 70, 200, 80);

            // Tomar el screenshot
            using Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgEstaMuertoName, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}