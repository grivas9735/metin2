using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;

namespace Metin2Bot.Operaciones
{
    public static class Cliente
    {
        public static async Task PerspectivaDesdeArriba(Metin2 metin)
        {
            Console.WriteLine($"ACOMODANDO CAMARA\n");
            await Task.Delay(50);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G, 1800);
            await Task.Delay(50);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_F, 1500);
            await Task.Delay(50);
        }

        public static async Task IniciarAutocaza(Metin2 metin)
        {
            Console.WriteLine("ACTIVANDO AUTOCAZA\n");
            await Task.Delay(50);

            // ABRIR AUTOCAZA
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);

            // DETENER
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaDetener().X, metin.StartY + Resolution.ClickAutocazaDetener().Y);

            // RESETEAR
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaResetear().X, metin.StartY + Resolution.ClickAutocazaResetear().Y);

            // ATACAR
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaAtacar().X, metin.StartY + Resolution.ClickAutocazaAtacar().Y);

            // ALCANCE
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaAlcance().X, metin.StartY + Resolution.ClickAutocazaAlcance().Y);

            // EMPEZAR
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaEmpezar().X, metin.StartY + Resolution.ClickAutocazaEmpezar().Y);

            // CERRAR AUTOCAZA
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);
        }

        public static async Task DetenerAutocaza(Metin2 metin)
        {
            Console.WriteLine("DETENIENDO AUTOCAZA\n");
            await Task.Delay(50);

            // ABRIR AUTOCAZA
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);

            // DETENER
            await User.ClickAt(metin.StartX + Resolution.ClickAutocazaDetener().X, metin.StartY + Resolution.ClickAutocazaDetener().Y);

            // CERRAR AUTOCAZA
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);
        }

        public static async Task EvalEstaMuerto(Metin2 metin, bool activarAutocaza = false)
        {
            if (metin.EstaMuerto)
            {
                metin.MurioAlgunaVez = true;
                Console.WriteLine("REVIVIENDO\n");

                await User.ClickAt(
                    metin.StartX + Resolution.ClickRevivir().X,
                    metin.StartY - Resolution.ClickRevivir().Y);

                await Task.Delay(800);
                metin.EstaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);

                if (!metin.EstaMuerto)
                {
                    await MetinKeyboard.Instance.PocionRoja(10);

                    if (activarAutocaza)
                        await IniciarAutocaza(metin);
                    else
                        metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                }
            }

            if (DateTime.Now - metin.timerEstaMuertoDate >= metin.timerEstaMuerto && !metin.EstaMuerto)
            {
                Console.WriteLine("VALIDANDO ESTA MUERTO\n");
                metin.timerEstaMuertoDate = DateTime.Now;
                metin.EstaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
            }
        }

        public static async Task EvalRelogin(Metin2 metin)
        {
            if (metin.EstaEnPantallaLogin || metin.EstaEnChampSelect)
            {
                if (metin.EstaEnPantallaLogin)
                {
                    await MetinKeyboard.Instance.ApretarEnter(100); // Este enter es para sacar cualquier posible cartel de error
                    await Task.Delay(100);

                    await User.ClickAt(
                        metin.StartX + Resolution.ClickLoginOK().X,
                        metin.StartY + Resolution.ClickLoginOK().Y);

                    await Task.Delay(15000);

                    metin.EstaEnChampSelect = AccionesImg.PicChampSelect.ProcessText(metin);
                    metin.EstaEnPantallaLogin = false;
                }

                if (metin.EstaEnChampSelect)
                {
                    await MetinKeyboard.Instance.ApretarEnter(100);
                    await Task.Delay(15000);
                    await IniciarAutocaza(metin);
                    metin.EstaEnPantallaLogin = false;
                    metin.EstaEnChampSelect = false;
                }
            }

            if (DateTime.Now - metin.timerReloginDate >= metin.timerRelogin)
            {
                Console.WriteLine("VALIDANDO RELOGIN\n");
                metin.EstaEnPantallaLogin = AccionesImg.PicLogin.ProcessText(metin);
                metin.EstaEnChampSelect = AccionesImg.PicChampSelect.ProcessText(metin);
                metin.timerReloginDate = DateTime.Now;
            }
        }

        public static async Task<bool> AbrirInventario(Metin2 metin)
        {
            var inventarioAbierto = AccionesImg.PicTextoInventario.ProcessText(metin);

            if (!inventarioAbierto)
            {
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_I);
                await Task.Delay(100);
                return AccionesImg.PicTextoInventario.ProcessText(metin);
            }

            return inventarioAbierto;
        }

        public static async Task CerrarInventario(Metin2 metin)
        {
            var inventarioAbierto = AccionesImg.PicTextoInventario.ProcessText(metin);

            if (inventarioAbierto)
            {
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_I);
                await Task.Delay(50);
            }
        }

        public static async Task CambiarCH(Metin2 metin, int channel)
        {
            await Salir(metin);

            var estaEnPantallaLogin = AccionesImg.PicLogin.ProcessText(metin);

            if (!estaEnPantallaLogin)
            {
                throw new Exception("Se esperaba que este en pantalla de login");
            }

            await User.ClickAt(
                metin.StartX + Resolution.ClickOtroServer().X,
                metin.StartY + Resolution.ClickOtroServer().Y,
                100);

            await User.ClickAt(
                metin.StartX + Resolution.ClickIberia().X,
                metin.StartY + Resolution.ClickIberia().Y,
                100);

            await User.ClickAt(
                metin.StartX + Resolution.ClickChannel(channel).X,
                metin.StartY + Resolution.ClickChannel(channel).Y,
                100);

            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();

            await Task.Delay(TimeSpan.FromSeconds(10));

            var estaEnChampSelect = AccionesImg.PicChampSelect.ProcessText(metin);

            if (!estaEnChampSelect)
            {
                throw new Exception("Se esperaba que este en pantalla de seleccion de campeon");
            }

            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        public static async Task Salir(Metin2 metin)
        {
            await User.ClickAt(
                metin.StartX + Resolution.ClickESC().X,
                metin.StartY + Resolution.ClickESC().Y,
                50);

            await Task.Delay(200);

            await User.ClickAt(
                metin.StartX + Resolution.ClickSalir().X,
                metin.StartY + Resolution.ClickSalir().Y,
                50);

            await Task.Delay(TimeSpan.FromSeconds(11));
        }
    }
}
