namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        public static Bitmap SacarScreenshotTiendaGeneralAbiertaBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotTiendaGeneralAbierta(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotTextoInventarioBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotTextoInventario(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotInventarioBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotInventario(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotFragmentosBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotFragmentos(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotNPCBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotNPC(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotCoordenadasBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotCoordenadas(metin);
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
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotMisionAlquimiaBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotMisionAlquimia(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotPantallaLoginBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotPantallaLogin(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotChampSelectBM(Metin2 metin)
        {
            var captureArea = Resolution.RectScreenshotChampSelect(metin);
            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }
    }
}