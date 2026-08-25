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

        public bool EstaEnPantallaLogin { get; set; }

        public bool EstaEnChampSelect { get; set; }

        public bool PrenderAutocazaPostMuerte { get; set; }

        public bool EstaMuerto { get; set; }

        public TextRegion? TextRegion { get; set; }

        public TimeSpan timerPocionRoja = TimeSpan.FromSeconds(1);
        public DateTime timerPocionRojaDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerPocionAzul = TimeSpan.FromSeconds(70);
        public DateTime timerPocionAzulDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerDonarExp = TimeSpan.FromMinutes(20);
        public DateTime timerDonarExpDate = DateTime.Now.AddMinutes(2);

        public TimeSpan timerHabF1 = TimeSpan.FromSeconds(115);
        public DateTime timerHabF1Date = DateTime.Now.AddDays(-1);
        
        public TimeSpan timerHabF2 = TimeSpan.FromSeconds(63);
        public DateTime timerHabF2Date = DateTime.Now.AddDays(-1);

        public TimeSpan timerRelogin = TimeSpan.FromSeconds(60);
        public DateTime timerReloginDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerEstaMuerto = TimeSpan.FromSeconds(25);
        public DateTime timerEstaMuertoDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerFragmentos = TimeSpan.FromSeconds(15);
        public DateTime timerFragmentosDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerAutocaza = TimeSpan.FromMinutes(5);
        public DateTime timerAutocazaDate = DateTime.Now.AddDays(-1);
    }
}