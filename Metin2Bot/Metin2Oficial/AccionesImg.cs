using Metin2Bot.Captures.Interfaces;
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
            bool ProcessText(Metin2 metin);
            TextRegion? ProcessCoordinates(Metin2 metin);
            (bool, double) ProcessMobHP(Metin2 metin);
        }

        // Instancias estáticas únicas (reutilización de memoria)
        public static PictureChampSelect PicChampSelect { get; } = new PictureChampSelect();
        public static PictureEstaMuerto PicEstaMuerto { get; } = new PictureEstaMuerto();
        public static PictureEstaMuertoSplit PicEstaMuertoSplit { get; } = new PictureEstaMuertoSplit();
        public static PictureLogin PicLogin { get; } = new PictureLogin();
        public static PictureFragmentosSplit PicFragmentosSplit { get; } = new PictureFragmentosSplit();
        public static PictureAlquimista PicAlquimista { get; } = new PictureAlquimista();
        public static PictureMisionAlquimia PicMisionAlquimia { get; } = new PictureMisionAlquimia();
        public static PictureCoordenadas PicCoordenadas { get; } = new PictureCoordenadas();
        public static PictureTiendaGeneral PicTiendaGeneral { get; } = new PictureTiendaGeneral();
        public static PictureTiendaGeneralAbierta PicTiendaGeneralAbierta { get; } = new PictureTiendaGeneralAbierta();
        public static PictureTextoInventario PicTextoInventario { get; } = new PictureTextoInventario();
        public static PicturePiedraMetin PicPiedraMetin { get; } = new PicturePiedraMetin();
        public static PictureNombrePiedraMetin PicNombrePiedraMetin { get; } = new PictureNombrePiedraMetin();

        public class PictureCoordenadas : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
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

        public class PictureChampSelect : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotChampSelectBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("seleccionar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("personaje", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureEstaMuerto : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotEstaMuertoBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureEstaMuertoSplit : ICapture, ICaptureContainsText
        {
            public void Capture(Metin2 metin)
            {
                metin.EstaMuertoBM = ScreenShot.SacarScreenshotEstaMuertoBM(metin);
            }

            public bool ContainsText(Metin2 metin)
            {
                if (metin.EstaMuertoBM == null)
                {
                    return false;
                }

                var text = ProcessInMemory(metin.EstaMuertoBM);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("volver", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("empezar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("ciudad", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureLogin : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotPantallaLoginBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("ok", StringComparison.CurrentCultureIgnoreCase)
                    && text.Contains("salir", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureMisionAlquimia : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotMisionAlquimiaBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("abrir", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("tienda", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("refinamiento", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("cerrar", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureTextoInventario : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTextoInventarioBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("inventario", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureTiendaGeneralAbierta : ICaptureContainsText
        {
            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotTiendaGeneralAbiertaBM(metin);

                var text = ProcessInMemory(bm);
                
                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("comprar", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("vender", StringComparison.CurrentCultureIgnoreCase)
                    || text.Contains("recomprar", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class PictureFragmentosSplit : ICapture, ICaptureProcessCoordinates
        {
            public void Capture(Metin2 metin)
            {
                metin.FragmentosBM = ScreenShot.SacarScreenshotFragmentosBM(metin);
            }

            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                if (metin.FragmentosBM == null)
                {
                    return null;
                }

                return ProcessInMemoryV2(metin.FragmentosBM, ListaItemsAgarrar);
            }
        }

        public class PictureAlquimista : ICaptureProcessCoordinates
        {
            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return ProcessInMemoryV2(bm, LstAlquimista);
            }
        }

        public class PictureTiendaGeneral : ICaptureProcessCoordinates
        {
            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNPCBM(metin);
                return ProcessInMemoryV2(bm, LstTiendaGeneral);
            }
        }

        public class PicturePiedraMetin : ICaptureProcessCoordinates
        {
            public TextRegion? ProcessCoordinates(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotPiedraMetinBM(metin);
                return ProcessInMemoryV2(bm, LstPiedraMetin);
            }
        }

        public class PictureNombrePiedraMetin : ICaptureContainsText, IProcessMobHP
        {
            public (bool, double) ProcessMobHP(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNombrePiedraMetinBM(metin);

                var hp = GetMobHpPercentage(bm);
                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return (false, 0);

                return (text.Contains("metin", StringComparison.CurrentCultureIgnoreCase), hp);
            }

            public bool ContainsText(Metin2 metin)
            {
                using var bm = ScreenShot.SacarScreenshotNombrePiedraMetinBM(metin);

                var text = ProcessInMemory(bm);

                if (string.IsNullOrWhiteSpace(text)) return false;

                return text.Contains("metin", StringComparison.CurrentCultureIgnoreCase);
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