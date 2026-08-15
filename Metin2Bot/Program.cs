using Metin2Bot.Metin2Oficial;

namespace Metin2Bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 100;
            Console.WindowHeight = 20;
            //Metin1600x900.LevearConChami().Wait();
            //Metin1600x900.LevearAll().Wait();
            Metin1600x900.Fragmentar().Wait();
            //Metin1600x900.Idle().Wait();
        }
    }
}