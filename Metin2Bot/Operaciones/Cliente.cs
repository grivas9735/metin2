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

        public static async Task EvalDonarExp(Metin2 metin)
        {
            if (DateTime.Now - metin.timerDonarExpDate >= metin.timerDonarExp)
            {
                Console.WriteLine("DONANDO EXP\n");

                // Abrir menu gremio
                await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.MENU);
                await Task.Delay(150);
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G);
                await Task.Delay(150);
                await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.MENU);
                await Task.Delay(150);

                // Click flechita exp
                await User.ClickAt(metin.StartX + Resolution.ClickFlechitaExp().X, metin.StartY + Resolution.ClickFlechitaExp().Y);
                await Task.Delay(150);

                // Apretar 6 veces 9
                await MetinKeyboard.Instance.PresionarYSoltarNVeces(MiButton.BT7.KEY_9, 6);
                await Task.Delay(150);

                // Apretar boton OK del cartelito de numero
                await User.ClickAt(metin.StartX + Resolution.ClickBotonOkDonar().X, metin.StartY - Resolution.ClickBotonOkDonar().Y);
                await Task.Delay(150);

                // Cerrar ventana gremio
                await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.MENU);
                await Task.Delay(150);
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G);
                await Task.Delay(150);
                await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.MENU);
                await Task.Delay(150);

                // Apretar boton en caso de error de 0 exp
                await User.ClickAt(metin.StartX + Resolution.ClickBotonErrorDonarExp().X, metin.StartY + Resolution.ClickBotonErrorDonarExp().Y);
                await Task.Delay(150);

                metin.timerDonarExpDate = DateTime.Now;
            }
        }

        public static async Task EvalPocionRoja(Metin2 metin, int? cantidad = null)
        {
            if (DateTime.Now - metin.timerPocionRojaDate >= metin.timerPocionRoja)
            {
                if (cantidad.HasValue)
                    await MetinKeyboard.Instance.PocionRoja(cantidad.Value);
                else
                    await MetinKeyboard.Instance.PocionRoja();

                metin.timerPocionRojaDate = DateTime.Now;
            }
        }

        public static async Task EvalPocionAzul(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionAzulDate >= metin.timerPocionAzul)
            {
                await MetinKeyboard.Instance.PocionAzul();
                metin.timerPocionAzulDate = DateTime.Now;
            }
        }

        public static async Task EvalAutocaza(Metin2 metin)
        {
            if (DateTime.Now - metin.timerAutocazaDate >= metin.timerAutocaza)
            {
                await Cliente.IniciarAutocaza(metin);
                metin.timerAutocazaDate = DateTime.Now;
            }
        }

        public static async Task EvalEstaMuerto(Metin2 metin, bool activarAutocaza = false)
        {
            if (metin.EstaMuerto)
            {
                metin.MurioAlgunaVez = true;
                Console.WriteLine($"REVIVIENDO {metin.Id}\n");

                await User.ClickAt(
                    metin.StartX + Resolution.ClickRevivir().X,
                    metin.StartY - Resolution.ClickRevivir().Y);

                await Task.Delay(800);
                metin.EstaMuerto = AccionesImg.PicEstaMuerto.ContainsText(metin);

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
                Console.WriteLine($"VALIDANDO ESTA MUERTO {metin.Id}\n");
                AccionesImg.PicEstaMuertoSplit.Capture(metin);
                _ = Task.Run(() =>
                {
                    metin.EstaMuerto = AccionesImg.PicEstaMuertoSplit.ContainsText(metin);
                    metin.timerEstaMuertoDate = DateTime.Now;
                });
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

                    metin.EstaEnChampSelect = AccionesImg.PicChampSelect.ContainsText(metin);
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
                Console.WriteLine($"VALIDANDO RELOGIN {metin.Id}\n");
                metin.EstaEnPantallaLogin = AccionesImg.PicLogin.ContainsText(metin);
                metin.EstaEnChampSelect = AccionesImg.PicChampSelect.ContainsText(metin);
                metin.timerReloginDate = DateTime.Now;
            }
        }

        public static async Task<bool> AbrirInventario(Metin2 metin)
        {
            var inventarioAbierto = AccionesImg.PicTextoInventario.ContainsText(metin);

            if (!inventarioAbierto)
            {
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_I);
                await Task.Delay(100);
                return AccionesImg.PicTextoInventario.ContainsText(metin);
            }

            return inventarioAbierto;
        }

        public static async Task CerrarInventario(Metin2 metin)
        {
            var inventarioAbierto = AccionesImg.PicTextoInventario.ContainsText(metin);

            if (inventarioAbierto)
            {
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_I);
                await Task.Delay(50);
            }
        }

        public static async Task CambiarCH(Metin2 metin, int channel)
        {
            var contadorTimeouts = 0;
            var timeoutSalir = 15;
            var timeoutChampSelect = 120;

            do
            {
                await Salir(metin);
                contadorTimeouts++;
            } while (!AccionesImg.PicLogin.ContainsText(metin) && contadorTimeouts < timeoutSalir);

            if (contadorTimeouts == timeoutSalir)
            {
                Console.WriteLine("Se esperaba que este en pantalla login");
                Environment.Exit(0);
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

            await Task.Delay(TimeSpan.FromSeconds(8));

            contadorTimeouts = 0;

            do
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                contadorTimeouts++;
            } while (!AccionesImg.PicChampSelect.ContainsText(metin) && contadorTimeouts < timeoutChampSelect);

            if (contadorTimeouts == timeoutChampSelect)
            {
                Console.WriteLine("Se esperaba que este en champ select");
                Environment.Exit(0);
            }

            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(TimeSpan.FromSeconds(15));
        }

        public static async Task Salir(Metin2 metin)
        {
            await User.ClickAt(
                metin.StartX + Resolution.ClickESC().X,
                metin.StartY + Resolution.ClickESC().Y,
                50);

            await Task.Delay(500);

            await User.ClickAt(
                metin.StartX + Resolution.ClickSalir().X,
                metin.StartY + Resolution.ClickSalir().Y,
                50);

            await Task.Delay(TimeSpan.FromSeconds(12));
        }
    }
}
