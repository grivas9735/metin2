using System.Diagnostics.Metrics;

namespace Metin2Bot.Screenshots
{
    public static class ScreenshotHelper
    {
        public enum Captura 
        {
            Fragmentos, 
            Login,
            ChampSelect,
            Revivir,
            InsideGame
        }

        public static void TakePicture(Metin2 metin, Captura captura)
        {
            switch (captura)
            {
                case Captura.Fragmentos:
                    ScreenShot.SacarScreenshotFragmentos(metin);
                    break;
                case Captura.Login:
                    ScreenShot.SacarScreenshotPantallaLogin(metin.StartX, metin.StartY);
                    break;
                case Captura.InsideGame:
                    ScreenShot.SacarScreenshotInsideGame(metin.StartX, metin.StartY);
                    break;
                case Captura.Revivir:
                    ScreenShot.SacarScreenshotEstaMuerto(metin.StartX, metin.StartY);
                    break;
                    
            }
        }

        public static void GetPictureText()
        {

        }
    }
}
