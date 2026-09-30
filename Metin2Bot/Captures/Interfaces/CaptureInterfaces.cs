using static Metin2Bot.ImageReader;

namespace Metin2Bot.Captures.Interfaces
{
    public interface IProcessMobHP
    {
        (bool, double) ProcessMobHP(Metin2 metin);
    }

    public interface ICaptureProcessCoordinates
    {
        TextRegion? ProcessCoordinates(Metin2 metin);
    }

    public interface ICaptureProcessCoordinatesSplit
    {
        TextRegion? ProcessCoordinates(Metin2 metin);
    }

    public interface ICaptureContainsText
    {
        bool ContainsText(Metin2 metin);
    }

    public interface ICaptureContainsTextSplit
    {
        bool ContainsText(Metin2 metin);
    }

    public interface ICapture
    {
        void Capture(Metin2 metin);
    }
}
