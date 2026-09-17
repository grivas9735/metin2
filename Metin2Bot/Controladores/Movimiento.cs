using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;
using System.Numerics;

namespace Metin2Bot.Controladores
{
    public static class Movimiento
    {
        private static readonly int distanciaMaximaTolerada = 2;
        private static readonly int tiempoMovimientoStep = 150;

        public static async Task MoverPersonaje(Metin2 metin, Vector2 destino)
        {
            Vector2 posicionActual = await LeerPosicionActualHastaHallarValor(metin);

            while (Vector2.Distance(destino, posicionActual) > distanciaMaximaTolerada)
            {
                await MetinKeyboard.Instance.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "W",
                    MetinKeyboard.Instance.MoverW);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await MetinKeyboard.Instance.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "A",
                    MetinKeyboard.Instance.MoverA);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await MetinKeyboard.Instance.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "S",
                    MetinKeyboard.Instance.MoverS);

                if (Vector2.Distance(destino, posicionActual) <= distanciaMaximaTolerada)
                    break;

                await MetinKeyboard.Instance.Quieto(0);

                posicionActual = await MoverHastaEmpeorar(
                    metin,
                    destino,
                    posicionActual,
                    tiempoMovimientoStep,
                    "D",
                    MetinKeyboard.Instance.MoverD);
            }

            await MetinKeyboard.Instance.Quieto(100);
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

                await MetinKeyboard.Instance.PocionRoja();
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

        public static async Task MoverAAlquimista(Metin2 metin)
        {
            await Movimiento.MoverPersonaje(metin, new Vector2(109, 572));
            await Movimiento.MoverPersonaje(metin, new Vector2(147, 546));
            await Movimiento.MoverPersonaje(metin, new Vector2(184, 532));
            await Movimiento.MoverPersonaje(metin, new Vector2(246, 519));
            await Movimiento.MoverPersonaje(metin, new Vector2(283, 514));
            await Movimiento.MoverPersonaje(metin, new Vector2(320, 502));
            await Movimiento.MoverPersonaje(metin, new Vector2(350, 499));
            await Movimiento.MoverPersonaje(metin, new Vector2(364, 499));
            await Movimiento.MoverPersonaje(metin, new Vector2(395, 499));
            await Movimiento.MoverPersonaje(metin, new Vector2(405, 499));
            await Movimiento.MoverPersonaje(metin, new Vector2(425, 499));
            await Movimiento.MoverPersonaje(metin, new Vector2(449, 505));
            await Movimiento.MoverPersonaje(metin, new Vector2(494, 514));
            await Movimiento.MoverPersonaje(metin, new Vector2(506, 544));

            // ARCO CIUDAD
            await Movimiento.MoverPersonaje(metin, new Vector2(526, 580));
            await Movimiento.MoverPersonaje(metin, new Vector2(537, 580));
            await Movimiento.MoverPersonaje(metin, new Vector2(550, 580));

            // MIRINE Y ALQUIMISTA
            await Movimiento.MoverPersonaje(metin, new Vector2(596, 569));
            await Movimiento.MoverPersonaje(metin, new Vector2(611, 553));
            await Movimiento.MoverPersonaje(metin, new Vector2(611, 529));
            await Movimiento.MoverPersonaje(metin, new Vector2(624, 522));
            await Movimiento.MoverPersonaje(metin, new Vector2(623, 512));
        }

        public static async Task MoverAPotera(Metin2 metin)
        {
            await Movimiento.MoverPersonaje(metin, new Vector2(631, 526));
            await Movimiento.MoverPersonaje(metin, new Vector2(639, 556));
            await Movimiento.MoverPersonaje(metin, new Vector2(674, 564));
        }

        private static async Task<Vector2> LeerPosicionActualHastaHallarValor(Metin2 metin)
        {
            User.MouseToPosition(
                metin.StartX + Resolution.WatchCoords().X,
                metin.StartY + Resolution.WatchCoords().Y);

            AccionesImg.PicCoordenadas.ProcessText(metin);

            if (metin.Coordenadas == null)
            {
                await MetinKeyboard.Instance.MoverCamaraE(20);
                await Task.Delay(20);

                Console.WriteLine("Reintentando leer coordenadas...");

                return await LeerPosicionActualHastaHallarValor(metin);
            }

            return metin.Coordenadas.Value;
        }
    }
}