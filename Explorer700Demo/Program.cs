#nullable enable
using Explorer700Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Explorer700Demo
{
    
    class Program
    {
        private static Explorer700 exp = Exp700Singleton.Instance;

        static void Main(string[] args)
        {
            Console.WriteLine("Start...");
            exp = Exp700Singleton.Instance;
            Game game = new Game();
            game.Start();
            Console.ReadKey();
            game.Stop();
        }
    }
}
