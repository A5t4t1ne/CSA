using System;

namespace Explorer700Demo
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Webserver ws = new Webserver("0.0.0.0", 80);
            ws.Start();
            Console.ReadKey();
            ws.Stop();
            // Console.WriteLine("Start...");
            // Game game = new Game();
            // game.Start();
            // Console.ReadKey();
            // game.Stop();
        }
    }
}
