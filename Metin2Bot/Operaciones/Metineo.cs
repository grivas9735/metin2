using Metin2Bot.Controladores;
using Metin2Bot.Metin2Oficial;
using Metin2Bot.Singletons;
using System.Diagnostics;

namespace Metin2Bot.Operaciones
{
    public static class Metineo
    {
        public static List<(int, int)> PathingTierraFuego()
        {
            return new List<(int, int)>()
            {
                (600, 464),
                (594, 428),
                (594, 383),
                (634, 383),
                (660, 408),
                (700, 426),
                (664, 457),
                (659, 493),
                (623, 447),
                (600, 464)
            };
        }

        public static async Task<(bool, int, int)> BuscarMetin(Metin2 metin)
        {
            var encontroMetin = false;
            var seleccionado = false;
            var timeoutBusqueda = 0;
            var coordenadaX = 0;
            var coordenadaY = 0;

            do
            {
                await MetinKeyboard.Instance.MoverCamaraE(250);
                await Task.Delay(100);

                metin.TextRegion = AccionesImg.PicPiedraMetin.ProcessCoordinates(metin);

                if (metin.TextRegion != null && metin.TextRegion.HasCoordinates)
                {
                    encontroMetin = true;
                    var timeoutSeleccionado = 0;

                    do
                    {
                        coordenadaX = metin.StartX + metin.TextRegion.X + 15;
                        coordenadaY = metin.StartY + metin.TextRegion.Y + -80 + (timeoutSeleccionado * 20);
                        await User.RightClickAt(coordenadaX, coordenadaY, 50);
                        await Task.Delay(1200);

                        seleccionado = AccionesImg.PicNombrePiedraMetin.ProcessText(metin);
                        timeoutSeleccionado++;
                    } while (!seleccionado && timeoutSeleccionado < 15);
                }

                await Task.Delay(50);
                timeoutBusqueda++;
            } while (!encontroMetin && timeoutBusqueda < 20);

            return (encontroMetin && seleccionado, coordenadaX, coordenadaY);
        }

        public static async Task MatarMetin(Metin2 metin, int coordenadaX, int coordenadaY)
        {
            await Habilidades.BajarseDelCaballo();
            await Habilidades.UsarAura();
            await Habilidades.UsarBerserk();
            await Habilidades.SubirseAlCaballo();
            await MetinKeyboard.Instance.PocionAzul();

            await User.ClickAt(coordenadaX, coordenadaY, 50);
            await Task.Delay(1200);

            var estaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
            var (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

            while (!estaMuerto && seleccionado)
            {
                Console.WriteLine($"{seleccionado}: {hp}");
                await MetinKeyboard.Instance.AgarrarItems();
                await MetinKeyboard.Instance.PocionRoja();

                estaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
                (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

                await Task.Delay(50);
            }

            if (estaMuerto)
                Environment.Exit(0);

            var cronometro = Stopwatch.StartNew();

            while (cronometro.Elapsed < TimeSpan.FromSeconds(25))
            {
                Console.WriteLine($"AGARRANDO ITEMS METIN");
                await MetinKeyboard.Instance.AgarrarItems();
                await MetinKeyboard.Instance.PocionRoja();
                await Task.Delay(100);
            }

            cronometro.Stop();
        }
    }
}
