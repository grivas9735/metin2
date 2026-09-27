using Metin2Bot.Controladores;
using Metin2Bot.Metin2Oficial;
using Metin2Bot.Singletons;
using System.Diagnostics;
using static Metin2Bot.User;

namespace Metin2Bot.Operaciones
{
    public static class Metineo
    {
        public static List<(int, int)> PathingTierraFuego()
        {
            return
            [
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
            ];
        }

        public static async Task<(bool, int, int)> BuscarMetin(Metin2 metin)
        {
            var encontroMetin = false;
            var seleccionado = false;
            var coordenadaX = 0;
            var coordenadaY = 0;

            await User.MoverCamaraConMouse(metin, 100, DireccionCamara.Abajo, 100);
            await Task.Delay(100);

            await User.MoverCamaraConMouse(metin, 650, DireccionCamara.Arriba, 5);
            await Task.Delay(100);

            for (int i = 0; i < 3 && !encontroMetin; i++)
            {
                await User.MoverCamaraConMouse(metin, i * 150, DireccionCamara.Arriba, 5);
                await Task.Delay(100);

                for (int  j = 0; j < 15 && !encontroMetin; j++)
                {
                    await MoverCamaraConMouse(metin, 30, DireccionCamara.Derecha, 15);
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
                }
            }

            return (encontroMetin && seleccionado, coordenadaX, coordenadaY);
        }

        public static async Task MatarMetin(Metin2 metin, int coordenadaX, int coordenadaY)
        {
            await Habilidades.BajarseDelCaballo();
            await Habilidades.UsarAura();
            await Habilidades.UsarBerserk();
            await Habilidades.SubirseAlCaballo();
            await MetinKeyboard.Instance.PocionAzul();

            // Este do-while es por si se mete un mob en el medio mientras tiro habs
            do
            {
                await User.RightClickAt(coordenadaX, coordenadaY, 50);
                await Task.Delay(1200);
            } while (!AccionesImg.PicNombrePiedraMetin.ProcessText(metin));

            await User.ClickAt(coordenadaX, coordenadaY, 50);
            await Task.Delay(1200);

            var estaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
            var (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

            if (!estaMuerto && seleccionado && hp < 90)
            {
                Console.WriteLine($"SKIPEANDO METIN PORQUE YA LE ESTABAN PEGANDO");
                return;
            }

            while (!estaMuerto && seleccionado)
            {
                Console.WriteLine($"{seleccionado}: {hp}");
                await MetinKeyboard.Instance.AgarrarItems();
                await MetinKeyboard.Instance.PocionRoja();

                estaMuerto = AccionesImg.PicEstaMuerto.ProcessText(metin);
                (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

                await Task.Delay(100);
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
