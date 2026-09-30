using System.Numerics;
using static Metin2Bot.ImageReader;

namespace Metin2Bot
{
    public class Metin2
    {
        public int Id { get; set; }

        public nint ProcessId { get; set; }

        public DateTime StartTime { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public bool EstaEnPantallaLogin { get; set; }

        public bool EstaEnChampSelect { get; set; }

        public bool EstaMuerto { get; set; }

        public bool MurioAlgunaVez { get; set; }

        public TextRegion? TextRegion { get; set; }

        public Vector2? Coordenadas { get; set; }

        #region Bitmaps

        public Bitmap? FragmentosBM { get; set; }

        public Bitmap? EstaMuertoBM { get; set; }

        public Bitmap? LoginBM { get; set; }

        public Bitmap? ChampSelectBM { get; set; }

        #endregion

        public TimeSpan timerPocionRoja = TimeSpan.FromSeconds(1);
        public DateTime timerPocionRojaDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerPocionAzul = TimeSpan.FromSeconds(100);
        public DateTime timerPocionAzulDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerDonarExp = TimeSpan.FromMinutes(20);
        public DateTime timerDonarExpDate = DateTime.Now.AddMinutes(2);

        public TimeSpan timerHabF1 = TimeSpan.FromSeconds(118);
        public DateTime timerHabF1Date = DateTime.Now.AddDays(-1);
        
        public TimeSpan timerHabF2 = TimeSpan.FromSeconds(66);
        public DateTime timerHabF2Date = DateTime.Now.AddDays(-1);

        public TimeSpan timerRelogin = TimeSpan.FromSeconds(60);
        public DateTime timerReloginDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerBuffs = TimeSpan.FromSeconds(20);
        public DateTime timerBuffsDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerEstaMuerto = TimeSpan.FromSeconds(20);
        public DateTime timerEstaMuertoDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerFragmentos = TimeSpan.FromSeconds(15);
        public DateTime timerFragmentosDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerAutocaza = TimeSpan.FromMinutes(5);
        public DateTime timerAutocazaDate = DateTime.Now.AddDays(-1);

        public TimeSpan timerPesca = TimeSpan.FromSeconds(2);
        public DateTime timerPescaDate = DateTime.Now.AddDays(-1);
    }
}