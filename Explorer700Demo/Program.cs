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
        private static Explorer700 exp;

        static void Main(string[] args)
        {
            // Console.WriteLine("Start...");
            // exp = new Explorer700();
            //
            //
            // Explorer700 e700 = new Explorer700();
            // e700.Joystick.JoystickChanged += OnJoyStickChange;
            // const int time = 200;
            // for (var i = 0; i < 4; i++)
            // {
            //     e700.Led1.Toggle();
            //     e700.Led2.Toggle();
            //     e700.Buzzer.Beep(time);
            //     Thread.Sleep(time);
            // }
            
            // Eingebettete Bild Ressource laden und auf dem Display darstellen
            // var resNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            // var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.arch_black.png");
            // if (imageStream == null || 0 == 1)
            // {
            //     Console.WriteLine("Whut?");
            //     return;
            // }
            //
            // var g = exp.Display.Graphics;
            // var image = Image.FromStream(imageStream);
            // g.DrawImage(image, new Point(0, 0));
            // g.DrawString("I use Arch\nbtw", new Font(new FontFamily("arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(64, 20));
            // exp.Display.Update();
            // Console.ReadKey();
            
            Console.WriteLine("Start...");
            Stopwatch sw = new Stopwatch();
            exp = new Explorer700();
            Graphics g = exp.Display.Graphics;
            List<Image> images = new List<Image>();
            List<int> position = new List<int>();

            //creating starting screen
            Game game = new Game();
            game.StartScreen(g);

            //generating and moving enemy
            Thread enmy = new Thread(() => Enemy(images, position, g));
            enmy.Start();

            //Jumping and gernerating player
            Thread jup = new Thread(() => jump(g));
            jup.Start();

            Console.WriteLine(exp.Joystick.Keys);

            Console.ReadKey();
        }

    }
}
