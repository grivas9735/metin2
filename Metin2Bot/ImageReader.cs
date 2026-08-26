using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Tesseract;

namespace Metin2Bot
{
    public static class ImageReader
    {
        private const string apiKey = "K86761202088957";

        public class TextRegion
        {
            public string Text { get; set; } = string.Empty;
            public int X { get; set; }
            public int Y { get; set; }
            public bool HasCoordinates { get; set; } = true;
        }

        public static async Task<string?> ProcessImageAPI(string imageRoute, MiButton btn)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(apiKey), "apikey");
                content.Add(new ByteArrayContent(File.ReadAllBytes(imageRoute)), "file", Path.GetFileName(imageRoute));

                content.Add(new StringContent("spa"), "language"); // Cambia el idioma si es necesario
                content.Add(new StringContent("true"), "isOverlayRequired");
                content.Add(new StringContent("true"), "detectOrientation"); // Detectar orientación
                content.Add(new StringContent("false"), "scale"); // Escalar la imagen si es necesario
                content.Add(new StringContent("2"), "ocrEngine"); // Usar el motor avanzado

                var response = await client.PostAsync("https://api.ocr.space/parse/image", content);
                string result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<RootObject>(result);
                btn.AgarrarItems();

                client.Dispose();
                content.Dispose();
                return json?.ParsedResults[0]?.ParsedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL EJECUTAR PROCESADO DE IMAGEN API: {ex.Message}");
                return null;
            }
        }

        public static async Task<string?> ProcessImageLocal(string imageRoute, MiButton btn)
        {
            try
            {
                var texto = (string?)null;

                using (var engine = new TesseractEngine(AppConfig.GetRouteValue("TesseractOCR-tessdata"), "spa", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_scale_mode", "1");
                    engine.SetVariable("isOverlayRequired", "true"); // Mostrar superposición (si aplica)
                    engine.SetVariable("tessedit_ocr_engine_mode", "2"); // Usar el motor avanzado (LSTM)
                    engine.SetVariable("textord_fast_xheight", "1"); // Mejorar reconocimiento de altura
                    engine.SetVariable("debug_file", "NUL"); // Para evitar logueos por consola

                    using var img = Pix.LoadFromFile(imageRoute).Scale(2.5f, 2.5f);
                    using var page = engine.Process(img, PageSegMode.SparseText);
                    texto = page.GetText();
                }

                return texto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL EJECUTAR PROCESADO DE IMAGEN LOCAL: {ex.Message}");
                return null;
            }
        }

        public static async Task<TextRegion> ProcessImageLocalV2(string imageRoute, List<string> palabras, MiButton btn)
        {
            try
            {
                var texto = (string?)null;
                var regiones = new List<TextRegion>();
                var txtRegion = new TextRegion();

                using (var engine = new TesseractEngine(AppConfig.GetRouteValue("TesseractOCR-tessdata"), "spa", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_scale_mode", "1");
                    engine.SetVariable("isOverlayRequired", "true"); // Mostrar superposición (si aplica)
                    engine.SetVariable("tessedit_ocr_engine_mode", "2"); // Usar el motor avanzado (LSTM)
                    engine.SetVariable("textord_fast_xheight", "1"); // Mejorar reconocimiento de altura
                    engine.SetVariable("debug_file", "NUL"); // Para evitar logueos por consola

                    using var img = Pix.LoadFromFile(imageRoute).Scale(2.5f, 2.5f);
                    using var page = engine.Process(img, PageSegMode.SparseText);
                    texto = page.GetText();
                    regiones = ExtractCoordinates(page, 2.5f);
                    Console.WriteLine("=======================================================");
                    Console.WriteLine(string.Join(",", regiones.Select(x => x.Text.Trim())));
                    Console.WriteLine("=======================================================");

                    foreach (var palabra in palabras)
                    {
                        txtRegion = regiones.FirstOrDefault(x => x.Text.Contains(palabra, StringComparison.CurrentCultureIgnoreCase));

                        if (txtRegion != null)
                        {
                            break;
                        }
                    }

                    if (txtRegion == null)
                    {
                        txtRegion = new TextRegion();
                        txtRegion.HasCoordinates = false;
                    }

                    txtRegion.Text = texto;
                }

                return txtRegion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL EJECUTAR PROCESADO DE IMAGEN LOCAL: {ex.Message}");
                return null;
            }
        }

        public static List<TextRegion> ExtractCoordinates(Page page, float scaleFactor = 1.0f, PageIteratorLevel level = PageIteratorLevel.Word)
        {
            var regions = new List<TextRegion>();

            using var iter = page.GetIterator();
            if (iter == null) return regions;

            iter.Begin();

            do
            {
                if (iter.TryGetBoundingBox(level, out Tesseract.Rect bounds))
                {
                    string text = iter.GetText(level);

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        var ancho = (int)(bounds.Width / scaleFactor);
                        var alto = (int)(bounds.Height / scaleFactor);

                        regions.Add(new TextRegion
                        {
                            Text = text.Trim(),
                            X = (int)(bounds.X1 / scaleFactor) + (ancho / 2),
                            Y = (int)(bounds.Y1 / scaleFactor) + (alto / 2),
                        });
                    }
                }
            } while (iter.Next(level));

            return regions;
        }

        public static Task<string> RecrearImagen(Metin2 metin, string imageRoute)
        {
            imageRoute = ConvertToBlackAndWhite(metin, imageRoute);
            return Task.FromResult(CambiarAFullHD(metin, imageRoute));
            //return Task.Run(() => { return CambiarAFullHD(metin, imageRoute); });
        }

        static string CambiarAFullHD(Metin2 metin, string imageRoute)
        {
            try
            {
                using Bitmap original = new Bitmap(imageRoute);
                // Crear una nueva imagen con el mismo tamaño pero mayor resolución
                using Bitmap resized = new Bitmap(original);
                resized.SetResolution(1920, 1080);

                // Guardar la nueva imagen con mayor resolución
                //imageRoute = Path.Combine(Path.GetDirectoryName(imageRoute), $"bw_fhd_captcha{metin.Id}.png");
                imageRoute = imageRoute.Replace("metin_bw_", "metin_bw_fhd_");
                resized.Save(imageRoute);
            }
            catch { }

            return imageRoute;
        }

        static string ConvertToBlackAndWhite(Metin2 metin, string inputPath)
        {
            using Mat img = Cv2.ImRead(inputPath, ImreadModes.Grayscale);

            // Aplicar umbral para convertir a blanco y negro puro
            Cv2.Threshold(img, img, 128, 255, ThresholdTypes.Binary);

            //string outputPath = Path.Combine(Path.GetDirectoryName(inputPath), $"bw_captcha{metin.Id}.png");
            string outputPath = inputPath.Replace("metin_", "metin_bw_");
            img.SaveImage(outputPath);

            return outputPath;
        }
    }

    public class RootObject
    {
        public List<ParsedResult> ParsedResults { get; set; }
    }

    public class ParsedResult
    {
        public string ParsedText { get; set; }
    }
}