using Metin2Bot.Controladores;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using static Metin2Bot.ImageReader;

namespace Metin2Bot.Metin2Oficial
{
    public static class Metin1600x900
    {
        private static readonly int minutosApagado = 99999; // (60,1) (120,2) (180,3) (240,4) (360,6) (480,8) (600,10)
        private static readonly int minutosPausado = 99999;

        public static async Task AwaitShutdown()
        {
            await Task.Delay(TimeSpan.FromMinutes(minutosApagado));
            Shutdown();
        }

        public static async Task AwaitPause()
        {
            await Task.Delay(TimeSpan.FromMinutes(minutosPausado));
            Environment.Exit(0);
        }

        public static void IntercambiarMetines(ref Metin2 metin1, ref Metin2 metin2)
        {
            if (metin2 != null)
            {
                (metin2, metin1) = (metin1, metin2);
            }
        }

        public static async Task LevearConChami(bool revivirAlMorir = false, bool apagarAlMorir = false)
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetLeveleoConChami();

            _ = AwaitShutdown();
            _ = AwaitPause();

            var metin1 = metins.First();
            var metin2 = metins.Last();
            IntercambiarMetines(ref metin1, ref metin2);

            await User.MostrarMetin(metin1.ProcessId);
            await EvalAutocaza(metin1);

            while (true)
            {
                await User.MostrarMetin(metin1.ProcessId);
                await EvalRelogin(metin1);
                await EvalPocionRoja(metin1);
                await EvalPocionAzul(metin1);
                await EvalHabF1(metin1);
                await EvalHabF2(metin1);
                await MetinKeyboard.Instance.AgarrarItems();

                if (revivirAlMorir)
                {
                    await EvalEstaMuerto(metin1, activarAutocaza: true);
                }

                await User.MostrarMetin(metin2.ProcessId);
                await EvalRelogin(metin2);
                await EvalBuffs(metin2);
                await EvalEstaMuerto(metin2);

                if (!revivirAlMorir && metin1.AlgunaVezMurio)
                {
                    if (apagarAlMorir)
                        Shutdown();

                    Environment.Exit(0);
                }

                await User.MostrarVentanaActual(activeWindow);
                await Task.Delay(100);
            }
        }

        public static async Task LevearAll()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();

            _ = AwaitShutdown();
            _ = AwaitPause();

            while (true)
            {
                foreach (var metin in metins)
                {
                    await User.MostrarMetin(metin.ProcessId);

                    await EvalPocionRoja(metin);
                    await EvalPocionAzul(metin);
                    await EvalHabF1(metin);
                    await EvalHabF2(metin);
                    await EvalAutocaza(metin);
                    await EvalEstaMuerto(metin);
                    await EvalRelogin(metin);
                    await MetinKeyboard.Instance.AgarrarItems();
                    await Task.Delay(100);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
        }

        public static async Task Fragmentar()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();

            _ = AwaitShutdown();
            _ = AwaitPause();

            while (true)
            {
                foreach (var metin in metins)
                {
                    await User.MostrarMetin(metin.ProcessId);

                    //await EvalDonarExp(metin);
                    await EvalRelogin(metin);
                    await EvalEstaMuerto(metin);
                    await EvalPocionRoja(metin);
                    await EvalAutocaza(metin);
                    await BuscarFragmentos(metin);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
        }

        public static async Task BackearFragmenteros()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await MetinKeyboard.Instance.PocionRoja();
                    await Task.Delay(1000);
                }
            });

            foreach (var metin in metins)
            {
                await User.MostrarMetin(metin.ProcessId);

                await Movimiento.MoverAAlquimista(metin);

                await PerspectivaDesdeArriba(metin);

                var encontroAlquimista = await BuscarAlquimista(metin);

                if (!encontroAlquimista)
                    continue;

                await Movimiento.MoverAPotera(metin);

                var inventarioAbierto = await AbrirInventario(metin);

                if (!inventarioAbierto)
                    continue;

                var cantidadPociones = await ContarPociones(metin);

                await BuscarTiendaGeneral(metin, 55 - cantidadPociones);

                await User.MostrarVentanaActual(activeWindow);
            }

            Environment.Exit(0);
        }

        public static async Task Idle()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();

            _ = AwaitShutdown();
            _ = AwaitPause();

            while (true)
            {
                foreach (var metin in metins)
                {
                    await User.MostrarMetin(metin.ProcessId);

                    await EvalEstaMuerto(metin);
                    await MetinKeyboard.Instance.PocionRoja();

                    await Task.Delay(1000);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
        }

        public static async Task Metinear()
        {
            var metins = MetinFactory.GetAll();

            while (true)
            {
                foreach (var metin in metins)
                {
                    if (User.EsMetinEnPrimerPlano(metin.ProcessId))
                    {
                        await MetinKeyboard.Instance.PocionRoja();
                        await MetinKeyboard.Instance.PocionAzul();

                        await Task.Delay(100);
                    }
                }
            }
        }

        public static async Task Test()
        {
            var metins = MetinFactory.GetAll();

            foreach (var metin in metins)
            {
                await User.MostrarMetin(metin.ProcessId);

                var inventarioAbierto = await AbrirInventario(metin);

                if (!inventarioAbierto)
                    continue;

                var cantidadPociones = await ContarPociones(metin);

                await PerspectivaDesdeArriba(metin);
                await BuscarTiendaGeneral(metin, 55 - cantidadPociones);
            }
        }

        public static async Task<bool> AbrirInventario(Metin2 metin)
        {
            var inventarioAbierto = AccionesImg.PicTextoInventario.ProcessText(metin);

            if (!inventarioAbierto)
            {
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_I);
                return AccionesImg.PicTextoInventario.ProcessText(metin);
            }

            return inventarioAbierto;
        }

        public static async Task<int> ContarPociones(Metin2 metin)
        {
            var pocionesTotales = 0;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario1().X,
                metin.StartY + Resolution.ClickInventario1().Y);

            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL, 50);
            using var bm1 = ScreenShot.SacarScreenshotInventarioBM(metin);
            var text1 = ProcessInMemory(bm1);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL, 50);
            pocionesTotales += Regex.Matches(text1 ?? string.Empty, "200").Count;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario2().X,
                metin.StartY + Resolution.ClickInventario2().Y);

            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL, 50);
            using var bm2 = ScreenShot.SacarScreenshotInventarioBM(metin);
            var text2 = ProcessInMemory(bm2);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL, 50);
            pocionesTotales += Regex.Matches(text2 ?? string.Empty, "200").Count;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario1().X,
                metin.StartY + Resolution.ClickInventario1().Y);

            return pocionesTotales;
        }

        public static async Task PerspectivaDesdeArriba(Metin2 metin)
        {
            Console.WriteLine($"ACOMODANDO CAMARA\n");
            await Task.Delay(100);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_G, 2000);
            await Task.Delay(100);
            await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_F, 2000);
            await Task.Delay(100);
        }

        private static async Task BuscarFragmentos(Metin2 metin)
        {
            if (metin.TextRegion != null && metin.TextRegion.HasCoordinates)
            {
                await DetenerAutocaza(metin);
                await User.ClickAt(
                    metin.StartX + metin.TextRegion.X + Resolution.ClickFragmentarItemPiso().X,
                    metin.StartY + metin.TextRegion.Y + Resolution.ClickFragmentarItemPiso().Y,
                    10);
                await MetinKeyboard.Instance.PocionRoja(10);
                await Task.Delay(1000);

                metin.TextRegion = null;
                metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                return;
            }

            if (DateTime.Now - metin.timerFragmentosDate >= metin.timerFragmentos)
            {
                Console.WriteLine($"BUSCANDO FRAGMENTOS\n");
                await MetinKeyboard.Instance.MoverCamaraE(180);
                _ = Task.Run(async () =>
                {
                    metin.TextRegion = await AccionesImg.PicFragmentos.ProcessCoordinates(metin);
                });

                metin.timerFragmentosDate = DateTime.Now;
            }
        }

        private static async Task<bool> BuscarTiendaGeneral(Metin2 metin, int cantidadPocionesAComprar)
        {
            TextRegion? textRegion;
            var intentosBusquedaTiendaGeneral = 0;

            do
            {
                Console.WriteLine($"BUSCANDO TIENDA GENERAL {intentosBusquedaTiendaGeneral + 1}\n");
                await MetinKeyboard.Instance.MoverCamaraE(180);
                await Task.Delay(200);
                textRegion = await AccionesImg.PicTiendaGeneral.ProcessCoordinates(metin);
                intentosBusquedaTiendaGeneral++;
            } while ((textRegion == null || !textRegion.HasCoordinates) && intentosBusquedaTiendaGeneral < 15);

            if (textRegion == null)
            {
                return false;
            }

            await User.ClickAt(
                metin.StartX + textRegion.X + Resolution.ClickTiendaGeneral().X,
                metin.StartY + textRegion.Y + Resolution.ClickTiendaGeneral().Y);

            await Task.Delay(1000);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(1000);

            var tiendaAbierta = AccionesImg.PicTiendaGeneralAbierta.ProcessText(metin);

            if (tiendaAbierta)
            {
                var cantidadCompradas = 0;
                while (cantidadCompradas < cantidadPocionesAComprar)
                {
                    await User.RightClickAt(
                        metin.StartX + Resolution.ClickComprarPocion().X,
                        metin.StartY + Resolution.ClickComprarPocion().Y);

                    await Task.Delay(500);
                    cantidadCompradas++;
                }

                return true;
            }

            return false;
        }

        private static async Task<bool> BuscarAlquimista(Metin2 metin)
        {
            TextRegion? textRegion;
            var intentosBusquedaAlquimista = 0;

            do
            {
                Console.WriteLine($"BUSCANDO ALQUIMISTA {intentosBusquedaAlquimista + 1}\n");
                await MetinKeyboard.Instance.MoverCamaraE(180);
                await Task.Delay(200);
                textRegion = await AccionesImg.PicAlquimista.ProcessCoordinates(metin);
                intentosBusquedaAlquimista++;
            } while ((textRegion == null || !textRegion.HasCoordinates) && intentosBusquedaAlquimista < 15);

            if (textRegion == null)
            {
                return false;
            }

            await User.ClickAt(
                metin.StartX + textRegion.X + Resolution.ClickAlquimista().X,
                metin.StartY + textRegion.Y + Resolution.ClickAlquimista().Y);

            await Task.Delay(1000);

            var textoMision = AccionesImg.PicMisionAlquimia.ProcessText(metin);

            if (textoMision)
            {
                await MetinKeyboard.Instance.ApretarEnter();
                await Task.Delay(1000);
                await MetinKeyboard.Instance.ApretarEnter();
                await Task.Delay(1000);
                return true;
            }

            return false;
        }

        private static async Task EvalDonarExp(Metin2 metin)
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

        private static async Task EvalBuffs(Metin2 metinBuffi)
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

        private static async Task EvalPocionRoja(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionRojaDate >= metin.timerPocionRoja)
            {
                await MetinKeyboard.Instance.PocionRoja();
                metin.timerPocionRojaDate = DateTime.Now;
            }
        }

        private static async Task EvalPocionAzul(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionAzulDate >= metin.timerPocionAzul)
            {
                await MetinKeyboard.Instance.PocionAzul();
                metin.timerPocionAzulDate = DateTime.Now;
            }
        }

        private static async Task EvalHabF1(Metin2 metin)
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

        private static async Task EvalHabF2(Metin2 metin)
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

        private static async Task EvalAutocaza(Metin2 metin)
        {
            if (DateTime.Now - metin.timerAutocazaDate >= metin.timerAutocaza)
            {
                await IniciarAutocaza(metin);
                metin.timerAutocazaDate = DateTime.Now;
            }
        }

        private static async Task IniciarAutocaza(Metin2 metin)
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

        private static async Task DetenerAutocaza(Metin2 metin)
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

        private static async Task EvalEstaMuerto(Metin2 metin, bool activarAutocaza = false)
        {
            if (metin.EstaMuerto)
            {
                metin.AlgunaVezMurio = true;
                Console.WriteLine("REVIVIENDO\n");
                await User.ClickAt(metin.StartX + Resolution.ClickRevivir().X, metin.StartY - Resolution.ClickRevivir().Y);
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
                _ = Task.Run(async () =>
                {
                    metin.EstaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
                });
            }
        }

        private static async Task EvalRelogin(Metin2 metin)
        {
            if (metin.EstaEnPantallaLogin || metin.EstaEnChampSelect)
            {
                if (metin.EstaEnPantallaLogin)
                {
                    await MetinKeyboard.Instance.ApretarEnter(100); // Este enter es para sacar cualquier posible cartel de error
                    await Task.Delay(100);
                    await User.ClickAt(metin.StartX + Resolution.ClickLoginOK().X, metin.StartY + Resolution.ClickLoginOK().Y);
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

                _ = Task.Run(() =>
                {
                    metin.EstaEnPantallaLogin = AccionesImg.PicLogin.ProcessText(metin);
                    metin.EstaEnChampSelect = AccionesImg.PicChampSelect.ProcessText(metin);
                    metin.timerReloginDate = DateTime.Now;
                });
            }
        }

        public static void Shutdown()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = $"/s /f /t 0",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            process.Start();
        }
    }
}