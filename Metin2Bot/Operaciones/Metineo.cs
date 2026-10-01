using Metin2Bot.Controladores;
using Metin2Bot.Metin2Oficial;
using Metin2Bot.Singletons;
using System.Diagnostics;
using static Metin2Bot.User;

namespace Metin2Bot.Operaciones
{
    public static class Metineo
    {
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
                await MetinKeyboard.Instance.PresionarYSoltar(MiButton.BT7.KEY_F, 200);
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
                            await Task.Delay(1500);

                            seleccionado = AccionesImg.PicNombrePiedraMetin.ContainsText(metin);
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
            //await Habilidades.UsarBerserk();
            await Habilidades.SubirseAlCaballo();
            await MetinKeyboard.Instance.PocionAzul();

            int cont = 0;
            int timeoutSeleccionMetin = 30;
            // Este do-while es por si se mete un mob en el medio mientras tiro habs
            do
            {
                await User.RightClickAt(coordenadaX, coordenadaY, 50);
                await Task.Delay(1500);
                cont++;
            } while (!AccionesImg.PicNombrePiedraMetin.ContainsText(metin) && cont < timeoutSeleccionMetin);

            if (cont == timeoutSeleccionMetin)
            {
                Console.WriteLine($"SKIPEANDO METIN PORQUE NO SE PUDO VOLVER A SELECCIONAR");
                return;
            }

            await User.ClickAt(coordenadaX, coordenadaY, 50);
            await Task.Delay(1200);

            var estaMuerto = AccionesImg.PicEstaMuerto.ContainsText(metin);
            var (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

            if (!estaMuerto && seleccionado && hp < 90)
            {
                Console.WriteLine($"SKIPEANDO METIN PORQUE YA LE ESTABAN PEGANDO");
                return;
            }

            var toleranciaDarMetinPorMuerto = 0;
            while (!estaMuerto && (seleccionado || toleranciaDarMetinPorMuerto < 10))
            {
                Console.WriteLine($"{seleccionado}: {hp}");
                await MetinKeyboard.Instance.AgarrarItems();
                await MetinKeyboard.Instance.PocionRoja();

                estaMuerto = AccionesImg.PicEstaMuerto.ContainsText(metin);
                (seleccionado, hp) = AccionesImg.PicNombrePiedraMetin.ProcessMobHP(metin);

                if (seleccionado)
                    toleranciaDarMetinPorMuerto = 0;
                else
                    toleranciaDarMetinPorMuerto++;

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

            Console.WriteLine();
            cronometro.Stop();
        }
    }
}
