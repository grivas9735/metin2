namespace Metin2Bot.Exceptions
{
    public class HakaelConectadoException() : Exception("Hakael conectado")
    {
        private const string ErrorMessage = "Hakael conectado";
        public static new string Message = ErrorMessage;
    }
}