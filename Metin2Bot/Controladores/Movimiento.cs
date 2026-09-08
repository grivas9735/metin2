using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using System.Numerics;

namespace Metin2Bot.Controladores
{
    public static class Movimiento
    {
        private static readonly MiButton btn = new MiButton();

        public static async Task MoverPersonaje(Metin2 metin, Vector2 destino)
        {
            Vector2 posicionInicial = await LeerPosicionActualHastaHallarValor(metin);

            Vector2 posicionPosterior = new();
            Vector2 posicionAnterior = new();

            int distanciaMinimaValida = 5;
            int tiempoMovimientoMs = 500;

            do
            {
                await btn.MoverWA(tiempoMovimientoMs);
                posicionAnterior = new Vector2(posicionInicial.X, posicionInicial.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverWA(tiempoMovimientoMs);
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverWD(tiempoMovimientoMs);
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverWD(tiempoMovimientoMs);
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverSA(tiempoMovimientoMs);
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverSA(tiempoMovimientoMs);
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }

                if (Vector2.Distance(destino, posicionPosterior) <= distanciaMinimaValida) break;

                await btn.MoverSD(tiempoMovimientoMs);
                posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);

                while (Vector2.Distance(destino, posicionAnterior) > Vector2.Distance(destino, posicionPosterior))
                {
                    await btn.MoverSD(tiempoMovimientoMs);
                    posicionAnterior = new Vector2(posicionPosterior.X, posicionPosterior.Y);
                    posicionPosterior = await LeerPosicionActualHastaHallarValor(metin);
                }
            } while (Vector2.Distance(destino, posicionPosterior) > distanciaMinimaValida);
        }

        private static async Task<Vector2> LeerPosicionActualHastaHallarValor(Metin2 metin)
        {
            User.MouseToPosition(metin.StartX + Resolution.WatchCoords().X, metin.StartY + Resolution.WatchCoords().Y);
            await AccionesImg.PicCoordenadas.TakePic(metin);
            await AccionesImg.PicCoordenadas.ProcessText(metin, btn);

            if (metin.Coordenadas == null)
            {
                await Task.Delay(50);
                await btn.MoverCamaraE(10);
                Console.WriteLine("Reintentando leer coordenadas...");
                return await LeerPosicionActualHastaHallarValor(metin);
            }

            return metin.Coordenadas.Value;
        }
    }
}