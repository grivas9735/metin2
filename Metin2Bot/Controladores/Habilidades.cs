using Metin2Bot.Singletons;

namespace Metin2Bot.Controladores
{
    public static class Habilidades
    {
        public static async Task EvalBuffs(Metin2 metinBuffi)
        {
            if (DateTime.Now - metinBuffi.timerBuffsDate >= metinBuffi.timerBuffs)
            {
                Console.WriteLine("BUFFS F1");
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F1);
                await Task.Delay(2000);

                Console.WriteLine("BUFFS F2\n");
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F2);
                await Task.Delay(100);

                metinBuffi.timerBuffsDate = DateTime.Now;
            }
        }

        public static async Task EvalHabF1(Metin2 metin)
        {
            if (DateTime.Now - metin.timerHabF1Date >= metin.timerHabF1)
            {
                Console.WriteLine("USANDO F1\n");
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F1);
                await MetinKeyboard.Instance.PocionRoja(5);
                metin.timerHabF1Date = DateTime.Now;
                await Task.Delay(4000);
            }
        }

        public static async Task EvalHabF2(Metin2 metin)
        {
            if (DateTime.Now - metin.timerHabF2Date >= metin.timerHabF2)
            {
                Console.WriteLine("USANDO F2\n");
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F2);
                await MetinKeyboard.Instance.PocionRoja(5);
                metin.timerHabF2Date = DateTime.Now;
                await Task.Delay(4000);
            }
        }

        public static async Task BajarseDelCaballo()
        {
            Console.WriteLine("BAJANDO DEL CABALLO\n");
            await Task.Delay(10);
            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL);
            await Task.Delay(10);
        }

        public static async Task SubirseAlCaballo()
        {
            Console.WriteLine("SUBIENDO AL CABALLO\n");
            await Task.Delay(10);
            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL);
            await Task.Delay(10);
        }

        public static async Task UsarAura()
        {
            Console.WriteLine("USANDO AURA\n");

            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F1);
            await MetinKeyboard.Instance.PocionRoja(5);

            await Task.Delay(10);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.KEY_G);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL);

            await Task.Delay(3500);
        }

        public static async Task UsarBerserk()
        {
            Console.WriteLine("USANDO BERSERK\n");
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.F2);
            await MetinKeyboard.Instance.PocionRoja(5);
            await Task.Delay(3500);
        }
    }
}
