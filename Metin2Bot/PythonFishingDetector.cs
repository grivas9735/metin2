namespace Metin2Bot
{
    using System.Diagnostics;

    public static class PythonFishingDetector
    {
        public static FishDetection? Detect(string imagePath)
        {
            string python = @"C:\Users\Gon\repos\MetinFishingCV\.venv\Scripts\python.exe";

            string command =
                "import cv2; " +
                "from MetinFishingCV.FishingDetection import FishingVision; " +
                $"img=cv2.imread(r'{imagePath}'); " +
                "v=FishingVision(debug=False); " +
                "print(v.get_fishing_state(img))";

            ProcessStartInfo psi = new()
            {
                FileName = python,
                WorkingDirectory = @"C:\Users\Gon\repos\MetinFishingCV",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add(command);

            using Process process = new();
            process.StartInfo = psi;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("PYTHON ERROR:");
                Console.WriteLine(error);
                return null;
            }

            return ParseResult(output.Trim());
        }

        private static FishDetection? ParseResult(string output)
        {
            output = output.Trim();

            // El resultado viene:
            // (x, y, width, height, fishable, fishDetected, gameDetected)

            if (!output.StartsWith("(") || !output.EndsWith(")"))
                return null;

            string content = output[1..^1];

            string[] values = content.Split(',');

            if (values.Length != 7)
                return null;

            if (!int.TryParse(values[0].Trim(), out int x))
                return null;

            if (!int.TryParse(values[1].Trim(), out int y))
                return null;

            if (!int.TryParse(values[2].Trim(), out int width))
                return null;

            if (!int.TryParse(values[3].Trim(), out int height))
                return null;

            if (!bool.TryParse(values[4].Trim(), out bool fishable))
                return null;

            if (!bool.TryParse(values[5].Trim(), out bool fishDetected))
                return null;

            if (!bool.TryParse(values[6].Trim(), out bool gameDetected))
                return null;

            return new FishDetection
            {
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Fishable = fishable,
                FishDetected = fishDetected,
                GameDetected = gameDetected
            };
        }
    }

    public class FishDetection
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Fishable { get; set; }
        public bool FishDetected { get; set; }
        public bool GameDetected { get; set; }

        public int CenterX => X + Width / 2;
        public int CenterY => Y + Height / 2;
    }
}
