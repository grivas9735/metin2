using Metin2Bot.Screenshots;
using System.Text.RegularExpressions;

namespace Metin2Bot
{
    public static class GMDetect
    {
        private static string GetGMsImageName(int metin) => AppConfig.GetRouteValue("GMs") + @$"\captcha{metin}.png";
        private static string GetMetinHPImage(int metin) => AppConfig.GetRouteValue("GMs") + @$"\metin_hp_{metin}.png";

        public static async Task<Tuple<bool, string?>> DetectGM(Metin2 metin, MiButton btn)
        {
            //ScreenShot.SacarScreenshotMPs(metin.StartX, metin.StartY);
            var imagePath = await ImageReader.RecrearImagen(metin, AppConfig.GetRouteValue("MPs") + @$"\mps.png");

            var text = await ImageReader.ProcessImageLocal(imagePath, btn);

            var copiaText = text ?? string.Empty;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Verificacion de GM en metin {metin.Id} encontro: {Regex.Replace(copiaText, @"\s+", "")}");
            Console.ResetColor();
            return Tuple.Create(!string.IsNullOrWhiteSpace(text) && MPEsGM(text), text);
        }

        public static async Task<string> BuscarGMConectados(Metin2 metin)
        {
            try
            {
                //ScreenShot.SacarScreenshotPantalla(GetGMsImageName(metin.Id), metin.StartX + 1245, metin.StartY + 530, 150, 160);
                var imagePath = await ImageReader.RecrearImagen(metin, GetGMsImageName(metin.Id));
                return await ImageReader.ProcessImageLocal(imagePath, new MiButton()) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task MetinSeleccionado(Metin2 metin)
        {
            try
            {
                // cuadro entero: metin.StartX + 800, metin.StartY - 100, 600, 50
                //ScreenShot.SacarScreenshotPantalla(GetMetinHPImage(metin.Id), metin.StartX + 1000, metin.StartY - 80, 300, 20);
                var imagePath = await ImageReader.RecrearImagen(metin, GetMetinHPImage(metin.Id));
                var metinhp = await ImageReader.ProcessImageLocal(imagePath, new MiButton()) ?? string.Empty;
            }
            catch
            {
            }
        }

        private static bool MPEsGM(string text)
        {
            var pattern = @"\[.*?\]";
            return Regex.IsMatch(text, pattern);
        }
    }
}