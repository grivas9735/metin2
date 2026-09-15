using Metin2Bot.Screenshots;
using System.Numerics;
using System.Text.RegularExpressions;
using static Metin2Bot.ImageReader;
using static System.Net.Mime.MediaTypeNames;

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
          "collar", "bota", "zapato", "casco", "morad"
        };

        private static List<string> LstAlquimista = new List<string>()
        { "alqui", "quimis" };

        private static List<string> LstTiendaGeneral = new List<string>()
        { "general" };

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
            Task<bool> ProcessText(Metin2 metin);
            Task<TextRegion?> ProcessCoordinates(Metin2 metin);
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

        public static IPicture PicAlquimista
        {
            get
            {
                return new PictureAlquimista();
            }
        }

        public static IPicture PicMisionAlquimia
        {
            get
            {
                return new PictureMisionAlquimia();
            }
        }

        public static IPicture PicCoordenadas
        {
            get
            {
                return new PictureCoordenadas();
            }
        }

        public static IPicture PicTiendaGeneral
        {
            get
            {
                return new PictureTiendaGeneral();
            }
        }

        public static IPicture PicTiendaGeneralAbierta
        {
            get
            {
                return new PictureTiendaGeneralAbierta();
            }
        }

        public static IPicture PicTextoInventario
        {
            get
            {
                return new PictureTextoInventario();
            }
        }

        public class PictureCoordenadas : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotCoordenadasBM(metin);
                var text = ProcessInMemory(bm);

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
        }

        public class PictureChampSelect : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotChampSelectBM(metin);
                var text = ProcessInMemory(bm);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("seleccionar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("personaje", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureEstaMuerto : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotEstaMuertoBM(metin);
                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text))
                {
                    return false;
                }

                return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureLogin : IPicture
        {
            public Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotPantallaLoginBM(metin);
                var text = ProcessInMemory(bm);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("ok", StringComparison.CurrentCultureIgnoreCase)
                    && text.Contains("salir", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureMisionAlquimia : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotMisionAlquimiaBM(metin);
                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text))
                {
                    return false;
                }

                return text.Contains("abrir", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("tienda", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("refinamiento", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("cerrar", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureFragmentos : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotFragmentosBM(metin);
                return ProcessInMemoryV2(bm, ListaItemsAgarrar());
            }

            public Task<bool> ProcessText(Metin2 metin)
            {
                throw new NotImplementedException();
            }
        }

        public class PictureAlquimista : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return ProcessInMemoryV2(bm, LstAlquimista);
            }

            public Task<bool> ProcessText(Metin2 metin)
            {
                throw new NotImplementedException();
            }
        }

        public class PictureTiendaGeneral : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return ProcessInMemoryV2(bm, LstTiendaGeneral);
            }

            public Task<bool> ProcessText(Metin2 metin)
            {
                throw new NotImplementedException();
            }
        }

        public class PictureTextoInventario : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTextoInventarioBM(metin);
                var text = ProcessInMemory(bm);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("inventario", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureTiendaGeneralAbierta : IPicture
        {
            public async Task<TextRegion?> ProcessCoordinates(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public async Task<bool> ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTiendaGeneralAbiertaBM(metin);
                var text = ProcessInMemory(bm);

                if (text == null)
                {
                    return false;
                }

                return text.Contains("comprar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("vender", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("recomprar", StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}