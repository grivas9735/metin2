using static Metin2Bot.ImageReader;

namespace Metin2Bot
{
    public class Metin2
    {
        public int Id { get; set; }

        public nint ProcessId { get; set; }

        public RECT Rect { get; set; }

        public DateTime StartTime { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public TextRegion? TextRegion { get; set; }

        public bool RecogerFragmento { get; set; }

        public TimeSpan timerPocionRoja = TimeSpan.FromSeconds(2);
        public DateTime timerPocionRojaDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerPocionAzul = TimeSpan.FromSeconds(22);
        public DateTime timerPocionAzulDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerHabF1 = TimeSpan.FromSeconds(112);
        public DateTime timerHabF1Date = DateTime.Now.AddDays(-1);

        public TimeSpan timerHabF2 = TimeSpan.FromSeconds(60);
        public DateTime timerHabF2Date = DateTime.Now.AddDays(-1);

        public TimeSpan timerRelogin = TimeSpan.FromSeconds(60);
        public DateTime timerReloginDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerEstaMuerto = TimeSpan.FromSeconds(35);
        public DateTime timerEstaMuertoDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerFragmentos = TimeSpan.FromSeconds(16);
        public DateTime timerFragmentosDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerAutocaza = TimeSpan.FromMinutes(5);
        public DateTime timerAutocazaDate = DateTime.Now.AddDays(-1);
    }
}