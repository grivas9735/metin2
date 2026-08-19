using System.Diagnostics;

namespace Metin2Bot.Metin2Oficial
{
    public static class Metin1600x900
    {
        private static int minutosApagado = 99999; // (60,1) (120,2) (180,3) (240,4) (360,6) (480,8)
        private static int minutosPausado = 99999;

        private static MiButton btn = new MiButton();

        public static TimeSpan timerBuffs = TimeSpan.FromSeconds(20);
        public static DateTime timerBuffsDate = DateTime.Now.AddDays(-1);

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
                var aux = metin1;
                metin1 = metin2;
                metin2 = aux;
            }
        }

        public static async Task LevearConChami()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetLeveleoConChami();

            AwaitShutdown();
            AwaitPause();

            var metin1 = metins.First();
            var metin2 = metins.Count == 2 ? metins.LastOrDefault() : null;
            IntercambiarMetines(ref metin1, ref metin2);

            await User.MostrarMetin(metin1.ProcessId);
            
            while (true)
            {
                await EvalPocionRoja(metin1);
                await EvalPocionAzul(metin1);
                await EvalHabF1(metin1);
                await EvalHabF2(metin1);
                await EvalAutocaza(metin1);
                await EvalEstaMuerto(metin1);
                await EvalRelogin(metin1);

                if (metin2 != null)
                {
                    await EvalBuffs(metin2);
                    await User.MostrarMetin(metin1.ProcessId);
                }

                await btn.AgarrarItems();
                await Task.Delay(100);
            }
        }

        public static async Task LevearAll()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();
            
            AwaitShutdown();
            AwaitPause();

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

            AwaitShutdown();
            AwaitPause();

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

            AwaitShutdown();
            AwaitPause();

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
                await btn.PocionRoja(10);
                //await Task.Delay(1200);
                //AccionesImg.SacarScreenshotFragmentos(metin);
                //metin.TextRegion = await AccionesImg.BuscarFragmentoEnergia(metin, btn);
                if (metin.TextRegion != null && metin.TextRegion.HasCoordinates)
                {
                    await User.ClickAt(metin.StartX + metin.TextRegion.X, metin.StartY - 100 + metin.TextRegion.Y, 10);
                    await btn.PocionRoja(10);
                    await Task.Delay(1600);
                    await btn.AgarrarItems();
                }
                metin.TextRegion = null;
                await btn.MoverCamaraE(50);
                await Task.Delay(50);
                metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                return;
            }

            if (DateTime.Now - metin.timerFragmentosDate >= metin.timerFragmentos)
            {
                AccionesImg.SacarScreenshotFragmentos(metin);
                _ = Task.Run(async () =>
                {
                    metin.TextRegion = await AccionesImg.BuscarFragmentoEnergia(metin, btn);
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
                await User.MostrarMetin(metinBuffi.ProcessId);
                await Task.Delay(50);

                Console.WriteLine("BUFFS F1");
                await btn.PresionarYSoltar(MiButton.BT7.F1);
                await Task.Delay(2000);

                Console.WriteLine("BUFFS F2\n");
                await btn.PresionarYSoltar(MiButton.BT7.F2);
                await Task.Delay(80);

                timerBuffsDate = DateTime.Now;
            }
        }

        private static async Task EvalPocionRoja(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionRojaDate >= metin.timerPocionRoja)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("USANDO POCION ROJA\n");
                Console.ResetColor();
                await btn.PocionRoja();
                metin.timerPocionRojaDate = DateTime.Now;
            }
        }

        private static async Task EvalPocionAzul(Metin2 metin)
        {
            if (DateTime.Now - metin.timerPocionAzulDate >= metin.timerPocionAzul)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("USANDO POCION AZUL\n");
                Console.ResetColor();
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
                Console.WriteLine("ACTIVANDO AUTOCAZA POR LAS DUDAS\n");
                await IniciarAutocaza(metin);
                metin.timerAutocazaDate = DateTime.Now;
            }
        }

        private static async Task IniciarAutocaza(Metin2 metin)
        {
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
            if (DateTime.Now - metin.timerEstaMuertoDate >= metin.timerEstaMuerto)
            {
                if (await AccionesImg.EstaMuerto(metin, btn))
                {
                    Console.WriteLine("REVIVIENDO\n");
                    await User.ClickAt(metin.StartX + 100, metin.StartY - 40);
                    await Task.Delay(800);
                    var sigueMuerto = await AccionesImg.EstaMuerto(metin, btn);

                    if (!sigueMuerto)
                    {
                        await btn.PocionRoja(10);
                        metin.timerEstaMuertoDate = DateTime.Now;
                        metin.timerAutocazaDate = DateTime.Now.AddDays(-1);
                    }
                }
            }
        }

        private static async Task EvalRelogin(Metin2 metin)
        {
            if (DateTime.Now - metin.timerReloginDate >= metin.timerRelogin)
            {
                bool isInGame = false;

                while (!isInGame)
                {
                    Console.WriteLine("VALIDANDO RELOGIN");

                    var esPantallaLogin = await AccionesImg.EsPantallaLogin(metin, btn);
                    var esChampSelect = await AccionesImg.EsChampSelect(metin, btn);

                    Console.WriteLine($"Login: {esPantallaLogin}");
                    Console.WriteLine($"Champ: {esChampSelect}\n");

                    if (esPantallaLogin || esChampSelect)
                    {
                        if (esPantallaLogin)
                        {
                            await btn.ApretarEnter(100); // Este enter es para sacar cualquier posible cartel de error
                            await Task.Delay(100);
                            await User.ClickAt(metin.StartX + 900, metin.StartY + 670);
                            await Task.Delay(15000);

                            esChampSelect = await AccionesImg.EsChampSelect(metin, btn);

                            if (esChampSelect)
                            {
                                await btn.ApretarEnter(100);
                                await Task.Delay(15000);
                                await IniciarAutocaza(metin);
                            }
                        }
                        else
                        {
                            await btn.ApretarEnter(100);
                            await Task.Delay(15000);
                            await IniciarAutocaza(metin);
                        }
                    }

                    metin.timerReloginDate = DateTime.Now;
                    
                    // Por ahora lo dejo en true para que no valide si esta ingame. Para comportamiento viejo descomentar lo de abajo
                    isInGame = true;

                    //var isInGameTuple = await AccionesImg.InsideGame(metin, btn);
                    //isInGame = isInGameTuple.Item1;
                    //Console.WriteLine($"InGame: {isInGameTuple.Item1} => {isInGameTuple.Item2?.Replace(" ", "")}\n");
                    //await btn.PocionRoja(10);
                }
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