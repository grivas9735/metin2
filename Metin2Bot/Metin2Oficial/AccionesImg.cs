using Metin2Bot.Screenshots;
using System.Numerics;
using System.Text.RegularExpressions;
using static Metin2Bot.ImageReader;

namespace Metin2Bot.Metin2Oficial
{
    public static class AccionesImg
    {
        private static List<string> ListFragmentos = new List<string>()
        { "piedra", "dragon", "dragón", "de pie", "fragm" };

        private static List<string> ItemsCity2 = new List<string>()
        { "ébano", "ebano", "cuerno" };

        private static List<string> ItemsValle = new List<string>()
        { "cartilla" };

        private static List<string> ItemsSiempre = new List<string>()
        { "weiliao", "arte guerra", "ao zi", "arte guerra", "arteguerra",
          "bola", "polimorf", "wu zi", "luz luna", "luzluna", "pendiente",
          "collar", "bota", "zapato", "morad"
        };

        public static List<string> ListaItemsAgarrar()
        {
            var lst = new List<string>();
            lst.AddRange(ListFragmentos);
            lst.AddRange(ItemsCity2);
            lst.AddRange(ItemsValle);
            lst.AddRange(ItemsSiempre);
            return lst;
        }

        public interface IPicture
        {
            Task TakePic(Metin2 metin);
            Task<bool> ProcessText(Metin2 metin, MiButton btn);
            Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn);
        }

        public static IPicture PicChampSelect 
        {
            get
            {
                return new PictureChampSelect();
            }
        }

        public static IPicture PicEstaMuerto
        {
            get
            {
                return new PictureEstaMuerto();
            }
        }

        public static IPicture PicLogin
        {
            get
            {
                return new PictureLogin();
            }
        }

        public static IPicture PicFragmentos
        {
            get
            {
                return new PictureFragmentos();
            }
        }

        public static IPicture PicCoordenadas
        {
            get
            {
                return new PictureCoordenadas();
            }
        }

        public class PictureCoordenadas : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin, MiButton btn)
            {
                var bm = ScreenShot.SacarScreenshotCoordenadasBM(metin);
                var text = ProcessInMemory(ref bm);

                if (string.IsNullOrWhiteSpace(text))
                {
                    metin.Coordenadas = null;
                    return false;
                }

                var match = Regex.Match(text, @"\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                if (!match.Success)
                {
                    metin.Coordenadas = null;
                    return false;
                }

                if (!int.TryParse(match.Groups[1].Value, out var coordx) ||
                    !int.TryParse(match.Groups[2].Value, out var coordy))
                {
                    metin.Coordenadas = null;
                    return false;
                }

                metin.Coordenadas = new Vector2(coordx, coordy);

                return true;
            }

            public async Task TakePic(Metin2 metin)
            {
                await Task.Delay(50);
                ScreenShot.SacarScreenshotCoordenadas(metin);
            }
        }

        public class PictureChampSelect : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin, MiButton btn)
            {
                var imagePath = await RecrearImagen(metin, metin.ImgChampSelectName);
                var text = ProcessImageLocal(imagePath);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("seleccionar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("personaje", StringComparison.CurrentCultureIgnoreCase);
            }

            public async Task TakePic(Metin2 metin)
            {
                await Task.Delay(50);
                Console.WriteLine("FOTO CHAMP SELECT\n");
                ScreenShot.SacarScreenshotChampSelect(metin);
            }
        }

        public class PictureEstaMuerto : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin, MiButton btn)
            {
                var bm = ScreenShot.SacarScreenshotEstaMuertoBM(metin);
                var text = ProcessInMemory(ref bm);

                if (string.IsNullOrWhiteSpace(text))
                {
                    return false;
                }

                return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
            }

            public async Task TakePic(Metin2 metin)
            {
            }
        }

        public class PictureLogin : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin, MiButton btn)
            {
                var imagePath = await RecrearImagen(metin, metin.ImgLoginName);
                var text = ProcessImageLocal(imagePath);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("ok", StringComparison.CurrentCultureIgnoreCase)
                    && text.Contains("salir", StringComparison.CurrentCultureIgnoreCase);
            }

            public async Task TakePic(Metin2 metin)
            {
                await Task.Delay(50);
                Console.WriteLine("FOTO LOGIN\n");
                ScreenShot.SacarScreenshotPantallaLogin(metin);
            }
        }

        public class PictureFragmentos : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin, MiButton btn)
            {
                var imagePath = await RecrearImagen(metin, metin.ImgFragmentosName);
                return ProcessImageLocalV2(imagePath, ListaItemsAgarrar());
            }

            public Task<bool> ProcessText(Metin2 metin, MiButton btn)
            {
                throw new NotImplementedException();
            }

            public async Task TakePic(Metin2 metin)
            {
                await Task.Delay(50);
                Console.WriteLine("FOTO FRAGMENTOS\n");
                ScreenShot.SacarScreenshotFragmentos(metin);
            }
        }
    }
}