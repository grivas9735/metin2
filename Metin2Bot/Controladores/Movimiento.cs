using System.Numerics;
using System.Text.RegularExpressions;
using Metin2Bot.Screenshots;

namespace Metin2Bot.Controladores
{
    public static class Movimiento
    {
        private static readonly MiButton btn = new MiButton();

        private static string GetCaptchaImageName(int metin) => AppConfig.GetRouteValue("Captcha") + @$"\captcha{metin}.png";

        public static async Task MoverPersonaje(Metin2 metin, Vector2 destino)
        {
            Vector2 posicionInicial = await LeerPosicionActualHastaHallarValor(metin);

            Vector2 posicionPosterior = new();
            Vector2 posicionAnterior = new();

            int distanciaMinimaValida = 5;

            do
            {
                await btn.MoverWA();
                posicionAnterior = new Vector2(posicionInicial.X, posicionInicial.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverWA();
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverWD();
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverWD();
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverSA();
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverSA();
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverSD();
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverSD();
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }
            } while (Vector2.Distance(destino, posicionPosterior) > distanciaMinimaValida);
        }

        private static async Task<Vector2> LeerPosicionActualHastaHallarValor(Metin2 metin)
        {
            Vector2? posicionActual = await LeerPosicionActual(metin);

            if (posicionActual == null)
            {
                await Task.Delay(50);
                btn.MoverCamaraE();
                Console.WriteLine("Reintentando leer coordenadas...");
                return await LeerPosicionActualHastaHallarValor(metin);
            }

            return posicionActual.Value;
        }

        public static async Task<Vector2?> LeerPosicionActual(Metin2 metin)
        {
            try
            {
                //ScreenShot.SacarScreenshotPantalla(GetCaptchaImageName(metin.Id), metin.StartX + 1488, metin.StartY + 60, 74, 20);
                var imagePath = await ImageReader.RecrearImagen(metin, GetCaptchaImageName(metin.Id));
                var result = await ImageReader.ProcessImageLocal(imagePath, new MiButton());

                if (!string.IsNullOrEmpty(result) && result.Contains(','))
                {
                    var split = result.Split(',');
                    var validx = int.TryParse(Regex.Match(split[0], @"\d+").Value, out var coordx);
                    var validy = int.TryParse(Regex.Match(split[1], @"\d+").Value, out var coordy);
                    Console.WriteLine(coordx + "," + coordy);

                    if (validx && validy)
                    {
                        return new Vector2(coordx, coordy);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}