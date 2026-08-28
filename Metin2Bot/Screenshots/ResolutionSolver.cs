namespace Metin2Bot.Screenshots
{
    public enum ResolutionEnum
    {
        R1600x900, R800x600
    }

    public class Coordenadas(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }

    public static class Resolution
    {
        private static readonly ResolutionEnum ResolutionEnum = ResolutionEnum.R1600x900;

        #region Donacion Exp

        public static Coordenadas ClickFlechitaExp()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(180, 20);
            }

            return new Coordenadas(180, 20);
        }

        public static Coordenadas ClickBotonOkDonar()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(150, 25);
            }

            return new Coordenadas(150, 25);
        }

        public static Coordenadas ClickBotonErrorDonarExp()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(785, 352);
            }

            return new Coordenadas(380, 210);
        }

        #endregion

        #region Autocaza

        public static Coordenadas ClickAutocazaDetener()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(890, 555);
            }

            return new Coordenadas(450, 410);
        }

        public static Coordenadas ClickAutocazaResetear()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(890, 400);
            }

            return new Coordenadas(490, 255);
        }

        public static Coordenadas ClickAutocazaAtacar()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(890, 420);
            }

            return new Coordenadas(470, 280);
        }

        public static Coordenadas ClickAutocazaAlcance()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(890, 450);
            }

            return new Coordenadas(470, 300);
        }

        public static Coordenadas ClickAutocazaEmpezar()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(800, 555);
            }

            return new Coordenadas(400, 410);
        }

        #endregion

        #region Revivir

        public static Coordenadas ClickRevivir()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(100, 40);
            }

            return new Coordenadas(100, 40);
        }

        #endregion

        #region Login

        public static Coordenadas ClickLoginOK()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(900, 670);
            }

            return new Coordenadas(900, 670);
        }

        #endregion

        #region Fragmentar

        public static Coordenadas ClickFragmentarItemPiso()
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Coordenadas(0, -100);
            }

            return new Coordenadas(0, -100);
        }

        #endregion

        #region Screenshots

        public static Rectangle RectScreenshotPantallaLogin(Metin2 metin)
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Rectangle(metin.StartX + 850, metin.StartY + 600, 180, 150);
            }

            return new Rectangle(metin.StartX + 850, metin.StartY + 600, 180, 150);
        }

        public static Rectangle RectScreenshotFragmentos(Metin2 metin)
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Rectangle(metin.StartX, metin.StartY - 100, 1500, 820);
            }

            return new Rectangle(metin.StartX, metin.StartY - 100, 1500, 820);
        }

        public static Rectangle RectScreenshotChampSelect(Metin2 metin)
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Rectangle(metin.StartX - 20, metin.StartY - 20, 150, 50);
            }

            return new Rectangle(metin.StartX - 20, metin.StartY - 20, 150, 50);
        }

        public static Rectangle RectScreenshotEstaMuerto(Metin2 metin)
        {
            if (ResolutionEnum == ResolutionEnum.R1600x900)
            {
                return new Rectangle(metin.StartX + 40, metin.StartY - 70, 200, 80);
            }

            return new Rectangle(metin.StartX + 40, metin.StartY - 70, 200, 80);
        }

        #endregion
    }
}