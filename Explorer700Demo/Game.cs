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
    private readonly List<Entity> _entities = [];
    private Player _player;
    public static Keys KeyStates;
    private bool _running = false;
    public static readonly int FPS = 50;

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
        _player = new Player(imgBlock, new Vector2(0, 43), new Vector2(11, 11));
        _entities.Add(_player);

        var g = Exp700.Display.Graphics;

        bool stateUpOld = (Game.KeyStates & Keys.Up) != 0;
        while (_running)
        {
            // TODO: add entities (enemies)
            // TODO: implement hit-box validating
            foreach (var entity in _entities)
            {
                entity.UpdatePos();
                entity.Draw();
            }

            bool StateUpCurr = (Game.KeyStates & Keys.Up) != 0;
            if (StateUpCurr && !stateUpOld)
            {   
                _player.Jump();
            }
            stateUpOld = StateUpCurr;
            
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
        Game.KeyStates = e.Keys;
    }
}