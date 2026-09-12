using PaddleOCRSharp;

namespace Metin2Bot.OCR
{
    public sealed class PaddleOCR
    {
        // Lazy<T> garantiza thread-safety y creación diferida de la instancia
        private static readonly Lazy<PaddleOCREngine> _instance =
            new(() => new PaddleOCREngine());

        // Propiedad pública para acceder a la única instancia
        public static PaddleOCREngine Instance => _instance.Value;
    }
}