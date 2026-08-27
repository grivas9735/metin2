using System.Diagnostics;

namespace Metin2Bot.Metin2Oficial
{
    public static class Metin1600x900
    {
        private static readonly int minutosApagado = 99999; // (60,1) (120,2) (180,3) (240,4) (360,6) (480,8)
        private static readonly int minutosPausado = 99999;

        private static readonly MiButton btn = new();

        private static readonly TimeSpan timerBuffs = TimeSpan.FromSeconds(20);
        private static DateTime timerBuffsDate = DateTime.Now.AddDays(-1);

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

        public static void IntercambiarMetines(ref Metin2 metin1, ref Metin2? metin2)
        {
            if (metin2 != null)
            {
                (metin2, metin1) = (metin1, metin2);
            }
        }

        public static async Task LevearConChami()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetLeveleoConChami();

            _ = AwaitShutdown();
            _ = AwaitPause();

            var metin1 = metins.First();
            var metin2 = metins.Count == 2 ? metins.LastOrDefault() : null;
            IntercambiarMetines(ref metin1, ref metin2);

            while (true)
            {
                await User.MostrarMetin(metin1.ProcessId);
                await EvalPocionRoja(metin1);
                await EvalPocionAzul(metin1);
                await EvalHabF1(metin1);
                await EvalHabF2(metin1);
                await EvalAutocaza(metin1);
                await EvalEstaMuerto(metin1);
                await EvalRelogin(metin1);

                if (metin2 != null)
                {
                    await User.MostrarMetin(metin2.ProcessId);
                    await Task.Delay(100);
                    await EvalBuffs(metin2);
                    await EvalEstaMuerto(metin2);
                    await EvalRelogin(metin2);
                    await User.MostrarMetin(metin1.ProcessId);
                    await Task.Delay(100);
                }

                await btn.AgarrarItems();
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
                    await btn.AgarrarItems();
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

                    await EvalDonarExp(metin);
                    await EvalEstaMuerto(metin);
                    await EvalRelogin(metin);
                    await EvalPocionRoja(metin);
                    await EvalAutocaza(metin);
                    await BuscarFragmentos(metin);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
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
                    await btn.PocionRoja();

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
                        await btn.PocionRoja();
                        await btn.PocionAzul();

                        await Task.Delay(100);
                    }
                }
            }
        }

        private static async Task BuscarFragmentos(Metin2 metin)
        {
            if (metin.TextRegion != null && metin.TextRegion.HasCoordinates)
            {
                await DetenerAutocaza(metin);
                await User.ClickAt(metin.StartX + metin.TextRegion.X, metin.StartY - 100 + metin.TextRegion.Y, 10);
                await btn.PocionRoja(10);
                await Task.Delay(1000);
                await btn.AgarrarItems();
                metin.TextRegion = null;
                metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                return;
            }

            if (DateTime.Now - metin.timerFragmentosDate >= metin.timerFragmentos)
            {
                await btn.MoverCamaraE(50);
                await Task.Delay(50);
                await AccionesImg.PicFragmentos.TakePic(metin);
                _ = Task.Run(async () =>
                {
                    metin.TextRegion = await AccionesImg.PicFragmentos.ProcessCoordinates(metin, btn);
                });

                metin.timerFragmentosDate = DateTime.Now;
            }
        }

        private static async Task EvalDonarExp(Metin2 metin)
        {
            if (DateTime.Now - metin.timerDonarExpDate >= metin.timerDonarExp)
            {
                // Abrir menu gremio
                btn.MantenerTeclaApretada(MiButton.BT7.MENU);
                await Task.Delay(150);
                await btn.PresionarYSoltar(MiButton.BT7.KEY_G);
                await Task.Delay(150);
                btn.SoltarTecla(MiButton.BT7.MENU);
                await Task.Delay(150);

                // Click flechita exp
                await User.ClickAt(metin.StartX + 180, metin.StartY + 20);
                await Task.Delay(150);

                // Apretar 6 veces 9
                await btn.PresionarYSoltarNVeces(MiButton.BT7.KEY_9, 6);
                await Task.Delay(150);

                // Apretar boton OK del cartelito de numero
                await User.ClickAt(metin.StartX + 150, metin.StartY - 25);
                await Task.Delay(150);

                // Apretar boton en caso de error de 0 exp
                await User.ClickAt(metin.StartX + 785, metin.StartY + 352);
                await Task.Delay(150);

                // Cerrar ventana gremio
                btn.MantenerTeclaApretada(MiButton.BT7.MENU);
                await Task.Delay(150);
                await btn.PresionarYSoltar(MiButton.BT7.KEY_G);
                await Task.Delay(150);
                btn.SoltarTecla(MiButton.BT7.MENU);
                await Task.Delay(150);

                metin.timerDonarExpDate = DateTime.Now;
            }
        }

        private static async Task EvalBuffs(Metin2 metinBuffi)
        {
            if (DateTime.Now - timerBuffsDate >= timerBuffs)
            {
                Console.WriteLine("BUFFS F1");
                await btn.PresionarYSoltar(MiButton.BT7.F1);
                await Task.Delay(2000);

                Console.WriteLine("BUFFS F2\n");
                await btn.PresionarYSoltar(MiButton.BT7.F2);
                await Task.Delay(100);

                timerBuffsDate = DateTime.Now;
            }
        }

        private static async Task EvalPocionRoja(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionRojaDate >= metin.timerPocionRoja)
            {
                await btn.PocionRoja();
                metin.timerPocionRojaDate = DateTime.Now;
            }
        }

        private static async Task EvalPocionAzul(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionAzulDate >= metin.timerPocionAzul)
            {
                await btn.PocionAzul();
                metin.timerPocionAzulDate = DateTime.Now;
            }
        }

        private static async Task EvalHabF1(Metin2 metin)
        {
            if (DateTime.Now - metin.timerHabF1Date >= metin.timerHabF1)
            {
                Console.WriteLine("USANDO F1\n");
                await btn.PresionarYSoltar(MiButton.BT7.F1);
                await btn.PocionRoja(5);
                metin.timerHabF1Date = DateTime.Now;
                await Task.Delay(4000);
            }
        }

        private static async Task EvalHabF2(Metin2 metin)
        {
            if (DateTime.Now - metin.timerHabF2Date >= metin.timerHabF2)
            {
                Console.WriteLine("USANDO F2\n");
                await btn.PresionarYSoltar(MiButton.BT7.F2);
                await btn.PocionRoja(5);
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
            await btn.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);

            // DETENER
            await User.ClickAt(metin.StartX + 890, metin.StartY + 555);

            // RESETEAR
            await User.ClickAt(metin.StartX + 890, metin.StartY + 400);

            // ATACAR
            await User.ClickAt(metin.StartX + 890, metin.StartY + 420);

            // ALCANCE
            await User.ClickAt(metin.StartX + 890, metin.StartY + 450);

            // EMPEZAR
            await User.ClickAt(metin.StartX + 800, metin.StartY + 555);

            // CERRAR AUTOCAZA
            await btn.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);
        }

        private static async Task DetenerAutocaza(Metin2 metin)
        {
            Console.WriteLine("DETENIENDO AUTOCAZA\n");
            await Task.Delay(50);

            // ABRIR AUTOCAZA
            await btn.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);

            // DETENER
            await User.ClickAt(metin.StartX + 890, metin.StartY + 555);

            // CERRAR AUTOCAZA
            await btn.PresionarYSoltar(MiButton.BT7.KEY_K, 100);
            await Task.Delay(50);
        }

        private static async Task EvalEstaMuerto(Metin2 metin)
        {
            if (metin.EstaMuerto)
            {
                Console.WriteLine("REVIVIENDO\n");
                await User.ClickAt(metin.StartX + 100, metin.StartY - 40);
                await Task.Delay(800);
                await AccionesImg.PicEstaMuerto.TakePic(metin);
                metin.EstaMuerto = await AccionesImg.PicEstaMuerto.ProcessText(metin, btn);

                if (!metin.EstaMuerto)
                {
                    await btn.PocionRoja(10);
                    metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                }
            }

            if (DateTime.Now - metin.timerEstaMuertoDate >= metin.timerEstaMuerto && !metin.EstaMuerto)
            {
                metin.timerEstaMuertoDate = DateTime.Now;
                await AccionesImg.PicEstaMuerto.TakePic(metin);
                _ = Task.Run(async () =>
                {
                    metin.EstaMuerto = await AccionesImg.PicEstaMuerto.ProcessText(metin, btn);
                });
            }
        }

        private static async Task EvalRelogin(Metin2 metin)
        {
            if (metin.EstaEnPantallaLogin || metin.EstaEnChampSelect)
            {
                if (metin.EstaEnPantallaLogin)
                {
                    await btn.ApretarEnter(100); // Este enter es para sacar cualquier posible cartel de error
                    await Task.Delay(100);
                    await User.ClickAt(metin.StartX + 900, metin.StartY + 670);
                    await Task.Delay(15000);

                    await AccionesImg.PicChampSelect.TakePic(metin);
                    metin.EstaEnChampSelect = await AccionesImg.PicChampSelect.ProcessText(metin, btn);
                    metin.EstaEnPantallaLogin = false;
                }

                if (metin.EstaEnChampSelect)
                {
                    await btn.ApretarEnter(100);
                    await Task.Delay(15000);
                    await IniciarAutocaza(metin);
                    metin.EstaEnPantallaLogin = false;
                    metin.EstaEnChampSelect = false;
                }
            }

            if (DateTime.Now - metin.timerReloginDate >= metin.timerRelogin)
            {
                Console.WriteLine("VALIDANDO RELOGIN");

                var p1 = AccionesImg.PicLogin.TakePic(metin);
                var p2 = AccionesImg.PicChampSelect.TakePic(metin);
                await Task.WhenAll(p1, p2);

                _ = Task.Run(async () =>
                {
                    var t1 = AccionesImg.PicLogin.ProcessText(metin, btn);
                    var t2 = AccionesImg.PicChampSelect.ProcessText(metin, btn);
                    await Task.WhenAll(t1, t2);

                    metin.EstaEnPantallaLogin = t1.Result;
                    metin.EstaEnChampSelect = t2.Result;
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