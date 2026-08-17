using Metin2Bot.Screenshots;
using static Metin2Bot.ImageReader;

namespace Metin2Bot.Metin2Oficial
{
    public static class AccionesImg
    {
        private static List<string> ListFragmentos = new List<string>() 
        { "piedra", "dragon", "dragón", "de pie", "fragm" };

        private static List<string> ItemsCity2 = new List<string>()
        { "ébano", "ebano", "cuerno" };

        private static List<string> ItemsSiempre = new List<string>()
        { "weiliao", "arte guerra", "ao zi", "arte guerra", "arteguerra",
          "bola", "polimorf", "wu zi", "luz luna", "luzluna" };

        public static List<string> ListaItemsAgarrar()
        {
            var lst = new List<string>();
            lst.AddRange(ListFragmentos);
            lst.AddRange(ItemsCity2);
            lst.AddRange(ItemsSiempre);
            return lst;
        }

        public static async Task<TextRegion?> BuscarFragmentoEnergia(Metin2 metin, MiButton btn)
        {
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + @$"\metin_frag_{metin.Id}.png");
            return await ImageReader.ProcessImageLocalV2(imagePath, ListaItemsAgarrar(), btn);
        }

        public static void SacarScreenshotFragmentos(Metin2 metin)
        {
            ScreenShot.SacarScreenshotFragmentos(metin);
        }

        public static async Task<bool> EsPantallaLogin(Metin2 metin, MiButton btn)
        {
            var str = ScreenShot.SacarScreenshotPantallaLogin(metin);
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + str);

            var text = await ImageReader.ProcessImageLocal(imagePath, btn);

            if (text == null)
            {
                return false;
            }

            return text.Contains("ok", StringComparison.CurrentCultureIgnoreCase) 
                && text.Contains("salir", StringComparison.CurrentCultureIgnoreCase);
        }

        public static async Task<bool> EsChampSelect(Metin2 metin, MiButton btn)
        {
            var str = ScreenShot.SacarScreenshotChampSelect(metin);
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + str);

            var text = await ImageReader.ProcessImageLocal(imagePath, btn);

            if (text == null)
            {
                return false;
            }

            return text.Contains("seleccionar", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("personaje", StringComparison.CurrentCultureIgnoreCase);
        }

        public static async Task<Tuple<bool, string?>> InsideGame(Metin2 metin, MiButton btn)
        {
            var str = ScreenShot.SacarScreenshotInsideGame(metin);
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + str);

            var text = await ImageReader.ProcessImageLocal(imagePath, btn);

            if (text == null)
            {
                return new Tuple<bool, string?>(false, text);
            }

            var contains = text.Contains("eria", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("iberia", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("lberia", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("iber", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("lber", StringComparison.CurrentCultureIgnoreCase);

            return new Tuple<bool, string?>(contains, text);
        }

        public static async Task<bool> EstaMuerto(Metin2 metin, MiButton btn)
        {
            var str = ScreenShot.SacarScreenshotEstaMuerto(metin);
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + str);

            var text = await ImageReader.ProcessImageLocal(imagePath, btn);

            if (text == null)
            {
                return false;
            }

            return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}