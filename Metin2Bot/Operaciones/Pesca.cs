using Metin2Bot.Screenshots;
using OpenCvSharp;
namespace Metin2Bot.Operaciones
{
    public static class Pesca
    {
        public static async Task Pescar()
        {
            var metins = MetinFactory.GetAll();
            var metin = metins[0];

            await User.MostrarMetin(metin.ProcessId);

            do
            {
                var bm = ScreenShot.SacarScreenshotPescaBM(metin);
                var path = @"C:\Users\Gon\repos\MetinFishingCV\resources\metin_fishing.png";

                bm.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                using Mat image = Cv2.ImRead(path);

                var isFishable = FishingDetector.IsFishable(image);

                var fish = FishingDetector.DetectObjectColor(
                    image,
                    new Scalar(73, 99, 116),
                    new Scalar(144, 154, 132));

                Console.WriteLine($"X: {fish.X}");
                Console.WriteLine($"Y: {fish.Y}");
                Console.WriteLine($"Width: {fish.Width}");
                Console.WriteLine($"Height: {fish.Height}");
                Console.WriteLine($"Is Fishable: {isFishable}");

                if (isFishable && DateTime.Now - metin.timerPescaDate >= metin.timerPesca)
                {
                    await User.ClickAt(
                        metin.StartX + fish.X + 410 + 5,
                        metin.StartY + fish.Y + 50 + 5, 1);
                    metin.timerPescaDate = DateTime.Now;
                }

                User.MouseToPosition(
                    metin.StartX + fish.X + 410 + 5,
                    metin.StartY + fish.Y + 50 + 5);

            } while (true);
        }
    }
}
