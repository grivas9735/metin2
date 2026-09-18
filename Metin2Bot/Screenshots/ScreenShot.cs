namespace Metin2Bot.Screenshots
{
    public static class ScreenShot
    {
        // Método privado genérico para evitar duplicación
        private static Bitmap CapturarRegion(Rectangle captureArea)
        {
            if (captureArea.Width <= 0 || captureArea.Height <= 0)
                return null!;

            Bitmap screenshot = new(captureArea.Width, captureArea.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);
            }
            return screenshot;
        }

        public static Bitmap SacarScreenshotTiendaGeneralAbiertaBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotTiendaGeneralAbierta(metin));
        public static Bitmap SacarScreenshotTextoInventarioBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotTextoInventario(metin));
        public static Bitmap SacarScreenshotInventarioBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotInventario(metin));
        public static Bitmap SacarScreenshotFragmentosBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotFragmentos(metin));
        public static Bitmap SacarScreenshotNPCBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotNPC(metin));
        public static Bitmap SacarScreenshotCoordenadasBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotCoordenadas(metin));
        public static Bitmap SacarScreenshotEstaMuertoBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotEstaMuerto(metin));
        public static Bitmap SacarScreenshotMisionAlquimiaBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotMisionAlquimia(metin));
        public static Bitmap SacarScreenshotPantallaLoginBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotPantallaLogin(metin));
        public static Bitmap SacarScreenshotChampSelectBM(Metin2 metin) => CapturarRegion(Resolution.RectScreenshotChampSelect(metin));
    }
}