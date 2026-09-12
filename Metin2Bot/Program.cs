using Metin2Bot.Metin2Oficial;

namespace Metin2Bot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WindowWidth = 100;
            Console.WindowHeight = 20;

            while (true) 
            {
                try
                {
                    //await Metin1600x900.LevearConChami();
                    //await Metin1600x900.LevearAll();
                    await Metin1600x900.Fragmentar();
                    //await Metin1600x900.Idle();
                    //await Metin1600x900.Metinear();
                    //await Metin1600x900.Test();
                    //await Metin1600x900.BackearFragmenteros();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"EXCEPCION => {ex.Message}\n");
                    Console.ResetColor();
                }
            }
        }
    }
}