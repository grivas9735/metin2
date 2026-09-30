using Metin2Bot.Screenshots;
using System.Numerics;
using System.Text.RegularExpressions;
using static Metin2Bot.ImageReader;

namespace Metin2Bot.Metin2Oficial
{
    public static class AccionesImg
    {
        private static readonly List<string> ListFragmentos = ["piedra", "dragon", "dragón", "de pie", "fragm"];
        private static readonly List<string> ItemsCity2 = ["ébano", "ebano", "cuerno", "dirk"];
        private static readonly List<string> ItemsValle = ["cartilla", "dirk"];
        private static readonly List<string> ItemsSiempre =
        [
            "weiliao", "arte guerra", "ao zi", "arte guerra", "arteguerra",
            "bola", "polimorf", "wu zi", "luz luna", "luzluna", "pendiente",
            "collar", "bota", "zapato", "casco", "morad"
        ];

        private static readonly List<string> LstAlquimista = ["alqui", "quimis"];
        private static readonly List<string> LstTiendaGeneral = ["general"];
        private static readonly List<string> LstPiedraMetin = ["metin"];

        // Lista estática en lugar de instanciar una nueva en cada llamado
        public static readonly List<string> ListaItemsAgarrar = [
            ..ListFragmentos,
            ..ItemsCity2,
            ..ItemsValle,
            ..ItemsSiempre
        ];

        public interface IPicture
        {
            void Capture(Metin2 metin);
            bool ProcessText(Metin2 metin);
            TextRegion? ProcessCoordinates(Metin2 metin);
            (bool, double) ProcessMobHP(Metin2 metin);
        }

        // Instancias estáticas únicas (reutilización de memoria)
        public static IPicture PicChampSelect { get; } = new PictureChampSelect();
        public static IPicture PicEstaMuerto { get; } = new PictureEstaMuerto();
        public static IPicture PicLogin { get; } = new PictureLogin();
        public static IPicture PicFragmentos { get; } = new PictureFragmentos();
        public static IPicture PicAlquimista { get; } = new PictureAlquimista();
        public static IPicture PicMisionAlquimia { get; } = new PictureMisionAlquimia();
        public static IPicture PicCoordenadas { get; } = new PictureCoordenadas();
        public static IPicture PicTiendaGeneral { get; } = new PictureTiendaGeneral();
        public static IPicture PicTiendaGeneralAbierta { get; } = new PictureTiendaGeneralAbierta();
        public static IPicture PicTextoInventario { get; } = new PictureTextoInventario();
        public static IPicture PicPiedraMetin { get; } = new PicturePiedraMetin();
        public static IPicture PicNombrePiedraMetin { get; } = new PictureNombrePiedraMetin();

        public class PictureCoordenadas : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotCoordenadasBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text))
                {
                    metin.Coordenadas = null;
                    return false;
                }

                var match = Regex.Match(text, @"\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                if (!match.Success ||
                    !int.TryParse(match.Groups[1].Value, out var coordx) ||
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
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotChampSelectBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);
                return text != null
                    && (text.Contains("seleccionar", StringComparison.CurrentCultureIgnoreCase)
                        || text.Contains("personaje", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public class PictureEstaMuerto : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotEstaMuertoBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureLogin : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotPantallaLoginBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);

                return text != null
                    && text.Contains("ok", StringComparison.CurrentCultureIgnoreCase)
                    && text.Contains("salir", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureMisionAlquimia : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotMisionAlquimiaBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("abrir", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("tienda", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("refinamiento", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("cerrar", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureFragmentos : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotFragmentosBM(metin);
                return bm == null ? null : ProcessInMemoryV2(bm, ListaItemsAgarrar);
            }

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin) => throw new NotImplementedException();
        }

        public class PictureAlquimista : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return bm == null ? null : ProcessInMemoryV2(bm, LstAlquimista);
            }

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin) => throw new NotImplementedException();
        }

        public class PictureTiendaGeneral : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return bm == null ? null : ProcessInMemoryV2(bm, LstTiendaGeneral);
            }

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin) => throw new NotImplementedException();
        }

        public class PictureTextoInventario : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTextoInventarioBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);
                return text != null
                    && text.Contains("inventario", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureTiendaGeneralAbierta : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTiendaGeneralAbiertaBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);
                return text != null
                    && (text.Contains("comprar", StringComparison.CurrentCultureIgnoreCase)
                        || text.Contains("vender", StringComparison.CurrentCultureIgnoreCase)
                        || text.Contains("recomprar", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public class PicturePiedraMetin : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotPiedraMetinBM(metin);
                return bm == null ? null : ProcessInMemoryV2(bm, LstPiedraMetin);
            }

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public bool ProcessText(Metin2 metin) => throw new NotImplementedException();
        }

        public class PictureNombrePiedraMetin : IPicture
        {
            public void Capture(Metin2 metin)
            {
                throw new NotImplementedException();
            }

            public TextRegion? ProcessCoordinates(Metin2 metin) => throw new NotImplementedException();

            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNombrePiedraMetinBM(metin);
                if (bm == null) return (false, 0);

                var hp = GetMobHpPercentage(bm);
                var text = ProcessInMemory(bm);
                return (text != null
                    && text.Contains("metin", StringComparison.CurrentCultureIgnoreCase),
                    hp);
            }

            public bool ProcessText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNombrePiedraMetinBM(metin);
                if (bm == null) return false;

                var text = ProcessInMemory(bm);
                return text != null
                    && text.Contains("metin", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public static double GetMobHpPercentage(Bitmap bitmap)
        {
            // Zona útil de la barra de HP
            int xStart = 341;
            int xEnd = 449;

            int yStart = 30;
            int yEnd = 34;

            int totalColumns = xEnd - xStart + 1;
            int filledColumns = 0;

            for (int x = xStart; x <= xEnd; x++)
            {
                int redPixels = 0;
                int totalPixels = 0;

                for (int y = yStart; y <= yEnd; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    int redness = pixel.R - ((pixel.G + pixel.B) / 2);

                    if (redness > 60)
                        redPixels++;

                    totalPixels++;
                }

                // Una columna se considera llena si la mayoría de
                // sus píxeles son claramente rojos.
                if (redPixels >= totalPixels * 0.5)
                {
                    filledColumns++;
                }
            }

            double percentage = (double)filledColumns / totalColumns * 100.0;

            return Math.Clamp(percentage, 0, 100);
        }
    }
}