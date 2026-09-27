using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;
using System.Text.RegularExpressions;
using static Metin2Bot.ImageReader;

namespace Metin2Bot.Operaciones
{
    public static class Fragmenteo
    {
        public static async Task BuscarFragmentos(Metin2 metin)
        {
            if (metin.TextRegion != null && metin.TextRegion.HasCoordinates)
            {
                await Cliente.DetenerAutocaza(metin);
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
                await Task.Delay(50);
                metin.TextRegion = AccionesImg.PicFragmentos.ProcessCoordinates(metin);
                metin.timerFragmentosDate = DateTime.Now;
            }
        }

        public static async Task PrepararFragmenteros(List<Metin2> metins)
        {
            foreach (var metin in metins)
            {
                await User.MostrarMetin(metin.ProcessId);

                await Cliente.PerspectivaDesdeArriba(metin);

                await Cliente.AbrirInventario(metin);
            }
        }

        public static async Task ComprarPociones(Metin2 metin, int cantidadPocionesAComprar)
        {
            await Task.Delay(500);

            var cantidadCompradas = 0;
            while (cantidadCompradas < cantidadPocionesAComprar && cantidadPocionesAComprar < 55 /*tope de seguridad*/)
            {
                await User.RightClickAt(
                    metin.StartX + Resolution.ClickComprarPocion().X,
                    metin.StartY + Resolution.ClickComprarPocion().Y);

                await Task.Delay(500);
                cantidadCompradas++;
            }
        }

        public static async Task<bool> BuscarAlquimista(Metin2 metin)
        {
            TextRegion? textRegion;
            var intentosBusquedaAlquimista = 0;

            do
            {
                Console.WriteLine($"BUSCANDO ALQUIMISTA {intentosBusquedaAlquimista + 1}\n");
                await MetinKeyboard.Instance.MoverCamaraE(180);
                await Task.Delay(200);
                textRegion = AccionesImg.PicAlquimista.ProcessCoordinates(metin);
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

        public static async Task<int> ContarPociones(Metin2 metin)
        {
            var pocionesTotales = 0;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario1().X,
                metin.StartY + Resolution.ClickInventario1().Y);

            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL, 50);
            using var bm1 = ScreenShot.SacarScreenshotInventarioBM(metin);
            var text1 = ProcessInMemory(bm1);
            await Task.Delay(100);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL, 50);
            pocionesTotales += Regex.Matches(text1 ?? string.Empty, "200").Count;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario2().X,
                metin.StartY + Resolution.ClickInventario2().Y);

            await MetinKeyboard.Instance.MantenerTeclaApretada(MiButton.BT7.CONTROL, 50);
            using var bm2 = ScreenShot.SacarScreenshotInventarioBM(metin);
            var text2 = ProcessInMemory(bm2);
            await Task.Delay(100);
            await MetinKeyboard.Instance.SoltarTecla(MiButton.BT7.CONTROL, 50);
            pocionesTotales += Regex.Matches(text2 ?? string.Empty, "200").Count;

            await User.ClickAt(
                metin.StartX + Resolution.ClickInventario1().X,
                metin.StartY + Resolution.ClickInventario1().Y);

            return pocionesTotales;
        }

        public static async Task<bool> BuscarTiendaGeneral(Metin2 metin)
        {
            TextRegion? textRegion;
            var intentosBusquedaTiendaGeneral = 0;

            do
            {
                Console.WriteLine($"BUSCANDO TIENDA GENERAL {intentosBusquedaTiendaGeneral + 1}\n");
                await MetinKeyboard.Instance.MoverCamaraE(180);
                await Task.Delay(200);
                textRegion = AccionesImg.PicTiendaGeneral.ProcessCoordinates(metin);
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

            return AccionesImg.PicTiendaGeneralAbierta.ProcessText(metin);
        }
    }
}
