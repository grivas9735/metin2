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
                    //await MetinRunner.LevearConChami(revivirAlMorir: false, apagarAlMorir: false);
                    //await MetinRunner.LevearAll();
                    await MetinRunner.Fragmentar();
                    //await MetinRunner.Idle();
                    //await MetinRunner.Metinear();
                    //await MetinRunner.BackearFragmenteros();
                    //await MetinRunner.Test();
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