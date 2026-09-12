using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using System.Numerics;

namespace Metin2Bot.Controladores
{
    public static class Movimiento
    {
        private static readonly MiButton btn = new MiButton();
        private static readonly int distanciaMaximaTolerada = 2;
        private static readonly int tiempoMovimientoStep = 150;

        public static async Task MoverPersonaje(Metin2 metin, Vector2 destino)
        {
            Vector2 posicionActual = await LeerPosicionActualHastaHallarValor(metin);

            while (Vector2.Distance(destino, posicionActual) > distanciaMaximaTolerada)
            {
                await btn.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "W",
                    btn.MoverW);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await btn.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "A",
                    btn.MoverA);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await btn.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "S",
                    btn.MoverS);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await btn.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "D",
                    btn.MoverD);
            }

            await btn.Quieto(100);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Metin {metin.Id} llegó a ({destino.X},{destino.Y})");
            Console.ResetColor();
        }

        private static async Task<Vector2> MoverHastaEmpeorar(
            Metin2 metin,
            Vector2 destino,
            Vector2 posicionActual,
            int tiempoMovimientoMs,
            string direccion,
            Func<int, Task> mover)
        {
            var contadorMismaPosicion = 0;

            while (true)
            {
                Vector2 posicionAnterior = posicionActual;

                await btn.PocionRoja();
                await mover(tiempoMovimientoMs);

                posicionActual = await LeerPosicionActualHastaHallarValor(metin);

                double distanciaAnterior = Vector2.Distance(destino, posicionAnterior);
                double distanciaActual = Vector2.Distance(destino, posicionActual);

                Console.WriteLine(
                    $"{direccion} | " +
                    $"Anterior: {posicionAnterior} | " +
                    $"Dist. anterior: {distanciaAnterior:F2} | " +
                    $"Actual: {posicionActual} | " +
                    $"Dist. actual: {distanciaActual:F2}"
                );

                // Llegamos al final
                if (distanciaActual <= distanciaMaximaTolerada)
                {
                    return posicionActual;
                }

                // SOLO si realmente empeoró cambiamos de dirección
                if (distanciaActual > distanciaAnterior)
                {
                    return posicionActual;
                }

                if (distanciaActual == distanciaAnterior)
                {
                    contadorMismaPosicion++;
                    // Si se queda quieto por mas de 5 iteraciones, cambiamos de dirección
                    if (contadorMismaPosicion == 5) return posicionActual;
                }
                else
                {
                    contadorMismaPosicion = 0;
                }

                // Si mejoró, seguimos en la misma dirección
            }
        }

        private static async Task<Vector2> LeerPosicionActualHastaHallarValor(Metin2 metin)
        {
            User.MouseToPosition(
                metin.StartX + Resolution.WatchCoords().X,
                metin.StartY + Resolution.WatchCoords().Y);

            await AccionesImg.PicCoordenadas.ProcessText(metin);

            if (metin.Coordenadas == null)
            {
                await btn.MoverCamaraE(20);
                await Task.Delay(20);

                Console.WriteLine("Reintentando leer coordenadas...");

                return await LeerPosicionActualHastaHallarValor(metin);
            }

            return metin.Coordenadas.Value;
        }
    }
}