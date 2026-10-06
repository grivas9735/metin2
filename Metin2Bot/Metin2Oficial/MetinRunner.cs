using Metin2Bot.Controladores;
using Metin2Bot.Operaciones;
using Metin2Bot.Pathings;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;
using System.Diagnostics;
using System.Numerics;

namespace Metin2Bot.Metin2Oficial
{
    public static class MetinRunner
    {
        private static readonly int minutosApagado = 99999; // (60,1) (120,2) (180,3) (240,4) (360,6) (480,8) (600,10)
        private static readonly int minutosPausado = 99999;

        private static readonly int channelInicio = 1;
        private static readonly PathingMetineo pathing = new PathingTierraDeFuegoCirculo();

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
            await Cliente.EvalAutocaza(metin1);

            while (true)
            {
                await User.MostrarMetin(metin1.ProcessId);
                await Cliente.EvalRelogin(metin1);
                await Cliente.EvalPocionRoja(metin1);
                await Cliente.EvalPocionAzul(metin1);
                await Habilidades.EvalHabF1(metin1);
                await Habilidades.EvalHabF2(metin1);
                await Cliente.EvalEstaMuerto(metin1, activarAutocaza: true);
                await MetinKeyboard.Instance.AgarrarItems();

                await User.MostrarMetin(metin2.ProcessId);
                await Cliente.EvalRelogin(metin2);
                await Habilidades.EvalBuffs(metin2);
                await Cliente.EvalEstaMuerto(metin2);

                if (!revivirAlMorir && metin1.MurioAlgunaVez)
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

                    await Cliente.EvalPocionRoja(metin);
                    await Cliente.EvalPocionAzul(metin);
                    await Habilidades.EvalHabF1(metin);
                    await Habilidades.EvalHabF2(metin);
                    await Cliente.EvalAutocaza(metin);
                    await Cliente.EvalEstaMuerto(metin);
                    await Cliente.EvalRelogin(metin);
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

            await Fragmenteo.PrepararFragmenteros(metins);

            while (true)
            {
                foreach (var metin in metins)
                {
                    await User.MostrarMetin(metin.ProcessId);

                    await Cliente.EvalDonarExp(metin);
                    await Cliente.EvalRelogin(metin);
                    await Cliente.EvalEstaMuerto(metin);
                    await Cliente.EvalPocionRoja(metin);
                    await Cliente.EvalAutocaza(metin);
                    await Fragmenteo.BuscarFragmentos(metin);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
        }

        public static async Task BackearFragmenteros()
        {
            var activeWindow = User.GetForegroundWindow();
            var metins = MetinFactory.GetAll();

            foreach (var metin in metins)
            {
                await User.MostrarMetin(metin.ProcessId);

                await Cliente.PerspectivaDesdeArriba(metin);

                await Cliente.CerrarInventario(metin);

                await Movimiento.MoverAPotera(metin);

                var encontroPotera = await Fragmenteo.BuscarTiendaGeneral(metin);

                if (!encontroPotera)
                    continue;

                var cantidadPociones = await Fragmenteo.ContarPociones(metin);

                await Fragmenteo.ComprarPociones(metin, 55 - cantidadPociones);

                await Cliente.CerrarInventario(metin);

                await Movimiento.MoverAAlquimista(metin);

                await Fragmenteo.BuscarAlquimista(metin);

                await User.MostrarVentanaActual(activeWindow);
            }

            Environment.Exit(0);
        }

        public static async Task Metinear()
        {
            try
            {
                var metins = MetinFactory.GetAll();
                var metin = metins[0];

                await User.MostrarMetin(metin.ProcessId);

                var channel = channelInicio;

                while (true)
                {
                    await Cliente.CerrarInventario(metin);

                    User.MouseToPosition(
                        metin.StartX + Resolution.WatchCoords().X,
                        metin.StartY + Resolution.WatchCoords().Y);

                    await Task.Delay(50);
                    AccionesImg.PicCoordenadas.ContainsText(metin);
                    await Task.Delay(50);

                    pathing.Inicializar();
                    if (metin.Coordenadas != null)
                    {
                        pathing.Inicializar((int)metin.Coordenadas.Value.X, (int)metin.Coordenadas.Value.Y);
                    }

                    while (pathing.ObtenerSiguientePunto(out var punto))
                    {
                        if (AccionesImg.PicEstaMuerto.ContainsText(metin))
                            Environment.Exit(0);

                        await Cliente.CerrarInventario(metin);
                        await Movimiento.Cabalgar(metin, new Vector2(punto.X, punto.Y));

                        var (encontroMetin, coordenadaX, coordenadaY) = await Metineo.BuscarMetin(metin);

                        if (encontroMetin)
                        {
                            await Cliente.AbrirInventario(metin);

                            await User.ClickAt(
                                metin.StartX + Resolution.ClickInventario2().X,
                                metin.StartY + Resolution.ClickInventario2().Y,
                                50);

                            await Metineo.MatarMetin(metin, coordenadaX, coordenadaY);
                            break;
                        }
                    }

                    channel = channel == 6 ? 1 : channel + 1;
                    await Cliente.CambiarCH(metin, channel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en metineo: {ex.Message}");
                Environment.Exit(0);
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

                    await Cliente.EvalEstaMuerto(metin);
                    await MetinKeyboard.Instance.PocionRoja();

                    await Task.Delay(1000);
                }

                await User.MostrarVentanaActual(activeWindow);
            }
        }

        public static async Task Test()
        {
            var metins = MetinFactory.GetAll();

            foreach (var metin in metins)
            {
                await User.MostrarMetin(metin.ProcessId);
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