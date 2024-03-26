using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Explorer700Library;

namespace Explorer700Demo;

public class Game()
{
    private Explorer700 Exp700 { get; } = Exp700Singleton.Instance;
    private List<Entity> _entities = new ();
    private Player _player;
    public static bool KeyUp;
    private bool _running = false;
    private const int FPS = 10;

    /// <summary>
    /// Starts the game cycle.
    /// </summary>
    public void Start()
    {
        Exp700.Joystick.JoystickChanged += OnJoyStickChange;
        _running = true;
        
        // TODO: Change logic, to draw StartScreen first then start logic with JoyStick press
        Thread mainThread = new Thread(new ThreadStart(Run));
        mainThread.Start();
    }

    public void Stop()
    {
        Exp700.Joystick.JoystickChanged -= OnJoyStickChange;
        _running = false;
    }
    
    /// <summary>
    /// Method which runs the main logic of the game. Should be started in a new Thread.
    /// </summary>
    private void Run()
    {
        // TODO: Adjust location, hit-box and general size numbers
        // Debug lol
        var imgFloor = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.boden.png"));
        var imgBlock = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.block.png"));
        _player = new Player(imgBlock, new Vector2(0, 30), new Vector2(20, 20));
        _entities.Add(_player);

        var g = Exp700.Display.Graphics;

        bool stateUpOld = KeyUp;
        while (_running)
        {
            // TODO: add entities (enemies)
            // TODO: implement hit-box validating
            foreach (var entity in _entities)
            {
                entity.UpdatePos();
                entity.Draw();
            }

            if (KeyUp && !stateUpOld)
            {   
                _player.Jump();
            }
            stateUpOld = KeyUp;
            
            g.DrawImage(imgFloor, 0, 54);
            
            Exp700.Display.Update();
            Thread.Sleep(1000 / FPS);
            Exp700.Display.Clear();
        }
    }
    
    /// <summary>
    /// Draw Start Screen
    /// </summary>
    /// <param name="g"></param>
    private void StartScreen(Graphics g)
    {
        // TODO
    }
    
    
    private static void OnJoyStickChange(object? sender, KeyEventArgs e)
    {
        Console.WriteLine("Joystick: " + e.Keys);
        Game.KeyUp = (e.Keys & Keys.Up) != 0;
    }
}