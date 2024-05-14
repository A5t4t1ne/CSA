using System;

namespace Explorer700Demo
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");
            Game game = new Game();
            game.Start();
            Console.ReadKey();
            game.Stop();
        }
    }
}
