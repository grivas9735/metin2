using System.Diagnostics;

namespace Metin2Bot
{
    public static class MetinFactory
    {
        private static string WindowName = "METIN2";

        public static Metin2 GetOne()
        {
            var metins = Process.GetProcesses()
                .Where(x => x.MainWindowTitle.Contains(WindowName) && !User.IsIconic(x.MainWindowHandle))
                .ToList();

            var metin = new Metin2 { ProcessId = metins[0].MainWindowHandle, Id = 1, StartTime = metins[0].StartTime };

            SetMetinConfiguration(metin);

            return metin;
        }

        public static List<Metin2> GetLeveleoConChami()
        {
            var metins = Process.GetProcesses()
                .Where(x => x.MainWindowTitle.Contains(WindowName) && !User.IsIconic(x.MainWindowHandle))
                .Select(x => new Metin2
                {
                    ProcessId = x.MainWindowHandle,
                    Id = x.Id,
                    StartTime = x.StartTime,
                })
                .OrderByDescending(x => x.StartTime)
                .ToList();

            for (var i = 0; i < metins.Count; i++)
            {
                metins[i].Id = i + 1;
                SetMetinConfiguration(metins[i]);
            }

            if (metins.Count > 2) throw new Exception("Mas de dos metines abiertos para levear ?");

            return metins;
        }

        public static List<Metin2> GetAll()
        {
            var metins = Process.GetProcesses()
                .Where(x => x.MainWindowTitle.Contains(WindowName) && !User.IsIconic(x.MainWindowHandle))
                .Select(x => new Metin2 
                {
                    ProcessId = x.MainWindowHandle,
                    Id = x.Id,
                    StartTime = x.StartTime,
                })
                .OrderByDescending(x => x.StartTime)
                .ToList();

            for (var i = 0; i < metins.Count; i++)
            {
                metins[i].Id = i + 1;
                SetMetinConfiguration(metins[i]);
            }

            return metins;
        }

        private static void SetMetinConfiguration(Metin2 metin)
        {
            User.GetWindowRect(metin.ProcessId, out RECT rect);

            metin.Rect = rect;
            metin.StartX = rect.Left + 20;
            metin.StartY = rect.Top + 150;
            metin.Width = rect.Right - rect.Left - 50;
            metin.Height = rect.Bottom - rect.Top - 250;
        }
    }
}
