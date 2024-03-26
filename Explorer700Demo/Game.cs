using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using Explorer700Library;

namespace Explorer700Demo;

public class Game(Explorer700 exp700)
{
    private Explorer700 Exp700 { get;  } = exp700;

    public void Start()
    {
        
    }
    
    
    /**
     * Draw start screen
     */
    private void StartScreen(Graphics g)
        {
            exp.Display.Clear();

            var imageStreambdn = Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.boden.png");
            var imgbdn = Image.FromStream(imageStreambdn);

            g.DrawImage(imgbdn, 0, 54);
        }
        //generating and moving enemy
        private static void Enemy(List<Image> images, List<int> position,Graphics g)
        {
            Stream imageStreamsml = Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.spitze_klein.png");
            Image imgsml = Image.FromStream(imageStreamsml);

            Stream imageStreambig = Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.spitze_gross.png");
            Image imgbig = Image.FromStream(imageStreambig);

            images.Add(imgsml);
            position.Add(10);

            images.Add(imgbig);
            position.Add(50);
            while (exp.Joystick.Keys != Keys.Right)
            {
                
                if (images.Count < 2)
                {
                    images.Add(imgsml);
                    position.Add(10);
                }
                for (int i = 0; images.Count > i;)
                {
                    g.DrawImage(images[i], position[i], (images[i] == imgsml) ? 44 : (images[i] == imgbig) ? 34 : 0);
                    if (position[i] > 128)
                    {
                        images.RemoveAt(i);
                        position.RemoveAt(i);
                        continue;
                    }
                    position[i]++;
                    i++;
                }
                exp.Display.Update();
                Thread.Sleep(50);
            }
        }
        
        //Jumping and gernerating player
        static void jump(Graphics g)
        {
            int posyblk = 44;
            Stream imageStreamblk = Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.block.png");
            Image imgblk = Image.FromStream(imageStreamblk);
            g.DrawImage(imgblk, 85, posyblk);
            while (true)
            {
                if (exp.Joystick.Keys == Keys.Up)
                {
                    while (posyblk < 27)
                    {
                        posyblk++;
                        g.DrawImage(imgblk, 85, posyblk);
                    }
                    while (posyblk > 27)
                    {
                        posyblk--;
                        g.DrawImage(imgblk, 85, posyblk);
                    }
                    g.DrawImage(imgblk, 85, posyblk);
                }
                g.DrawImage(imgblk, 85, posyblk);
                Thread.Sleep(50);
            }
        }
        
        private static void OnJoyStickChange(object? sender, KeyEventArgs e)
        {
            Console.WriteLine("Joystick: " + e.Keys);
        }
}