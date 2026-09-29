namespace Metin2Bot.Pathings
{
    public abstract class PathingMetineo
    {
        protected readonly List<(int X, int Y)> Puntos;

        private int indiceActual;
        private int indiceInicial;
        private bool primerPunto;

        protected PathingMetineo(IEnumerable<(int X, int Y)> puntos)
        {
            Puntos = puntos.ToList();

            if (Puntos.Count == 0)
                throw new ArgumentException("El pathing debe tener al menos un punto.");

            indiceActual = 0;
            indiceInicial = 0;
            primerPunto = true;
        }

        /// <summary>
        /// Inicializa el pathing desde el primer punto.
        /// </summary>
        public void Inicializar()
        {
            indiceInicial = 0;
            indiceActual = 0;
            primerPunto = true;
        }

        /// <summary>
        /// Inicializa el pathing desde el punto más cercano
        /// a las coordenadas indicadas.
        /// </summary>
        public void Inicializar(int x, int y)
        {
            indiceInicial = ObtenerIndiceDelPuntoMasCercano(x, y);
            indiceActual = indiceInicial;
            primerPunto = true;
        }

        /// <summary>
        /// Obtiene el siguiente punto del recorrido.
        /// Devuelve true mientras todavía haya puntos por recorrer.
        /// Devuelve false cuando se completó una vuelta completa.
        /// </summary>
        public bool ObtenerSiguientePunto(out (int X, int Y) punto)
        {
            // Si ya volvimos al punto donde comenzamos,
            // significa que completamos todo el recorrido.
            if (!primerPunto && indiceActual == indiceInicial)
            {
                punto = default;
                return false;
            }

            punto = Puntos[indiceActual];

            primerPunto = false;

            indiceActual++;

            // Volver al principio de la lista
            if (indiceActual >= Puntos.Count)
                indiceActual = 0;

            return true;
        }

        private int ObtenerIndiceDelPuntoMasCercano(int x, int y)
        {
            int indiceMasCercano = 0;
            double distanciaMinima = double.MaxValue;

            for (int i = 0; i < Puntos.Count; i++)
            {
                var punto = Puntos[i];

                double distancia = Math.Pow(punto.X - x, 2)
                                 + Math.Pow(punto.Y - y, 2);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    indiceMasCercano = i;
                }
            }

            return indiceMasCercano;
        }
    }
}
