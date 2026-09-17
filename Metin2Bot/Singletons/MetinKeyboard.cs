namespace Metin2Bot.Singletons
{
    public sealed class MetinKeyboard
    {        
        // Lazy<T> garantiza thread-safety y creación diferida de la instancia
        private static readonly Lazy<MiButton> _instance =
            new(() => new MiButton());

        // Propiedad pública para acceder a la única instancia
        public static MiButton Instance => _instance.Value;
    }
}