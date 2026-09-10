namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        public static void SacarScreenshotPantallaLogin(Metin2 metin)
        {
            Rectangle captureArea = Resolution.RectScreenshotPantallaLogin(metin);

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
            Rectangle captureArea = Resolution.RectScreenshotFragmentos(metin);

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
            Rectangle captureArea = Resolution.RectScreenshotChampSelect(metin);

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
            Rectangle captureArea = Resolution.RectScreenshotEstaMuerto(metin);

            // Tomar el screenshot
            using Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgEstaMuertoName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SacarScreenshotCoordenadas(Metin2 metin)
        {
            Rectangle captureArea = Resolution.RectScreenshotCoordenadas(metin);

            // Tomar el screenshot
            using Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            // Guardar el screenshot para pruebas
            screenshot.Save(metin.ImgCoordenadasName, System.Drawing.Imaging.ImageFormat.Png);
        }

        #region Bitmaps

        public static Bitmap SacarScreenshotCoordenadasBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotCoordenadas(metin);

            // Tomar el screenshot
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            return screenshot;
        }

        public static Bitmap SacarScreenshotEstaMuertoBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotEstaMuerto(metin);

            // Tomar el screenshot
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }

            return screenshot;
        }

        #endregion
    }
}