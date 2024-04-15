using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Explorer700Library;

namespace Explorer700Demo;

public class Game()
{
    private Explorer700 Exp700 { get; } = Exp700Singleton.Instance;
    private readonly BlockingCollection<Entity> _entities = [];
    private Player _player;
    private static Keys KeyStates;
    private bool _running = false;
    public static readonly int FPS = 50;

    /// <summary>
    /// Starts the game cycle.
    /// </summary>
    public void Start()
    {
        Exp700.Joystick.JoystickChanged += OnJoyStickChange;
        _running = false;
        
        DrawStartScreen();
        DrawScore();
        
        // TODO: Change logic, to draw StartScreen first then start logic with JoyStick press
        while (!(_running))
        {
            bool stateleft = (Game.KeyStates & Keys.Left) != 0;
            bool stateright = (Game.KeyStates & Keys.Right) != 0;
            if (stateleft)
            {
                Exp700.Display.Clear();
                _running = true;
            } else if (stateright)
            {
                Exp700.Display.Clear();
                System.Environment.Exit(0);
            }
        }
        Thread mainThread = new Thread(new ThreadStart(Run));
        mainThread.Start();
    }

    public void Stop()
    {
        //Exp700.Joystick.JoystickChanged -= OnJoyStickChange;
        _running = false;
    }
    
    /// <summary>
    /// Method which runs the main logic of the game. Should be started in a new Thread.
    /// </summary>
    private void Run()
    {
        Exp700.Display.Clear();
        while (_running) 
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var imgFloor = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.boden.png"));
            var imgBlock = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.block.png"));
            _player = new Player(imgBlock, new Vector2(0, 43), new Vector2(11, 11));
            _entities.Add(_player);

            var g = Exp700.Display.Graphics;
            Thread enemyThread = new Thread(new ThreadStart(GenerateEnemies));
            enemyThread.Start();
            bool stateUpOld = (Game.KeyStates & Keys.Up) != 0;
            while (_running)
            {
                foreach (var entity in _entities)
                {
                    entity.UpdatePos();
                    entity.Draw();

                    if (entity.GetType() == typeof(Player) && ((Player)entity).IsEnemyInHitbox(_entities)) {
                        Stop();
                        stopwatch.Stop();
                        Console.WriteLine("Sie haben " + (int)stopwatch.Elapsed.TotalSeconds + " Sekunden überlebt");
                    }
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
            DrawStartScreen();
            DrawScore(stopwatch);
            while (!(_running)) 
            {
                bool stateleft = (Game.KeyStates & Keys.Left) != 0;
                bool stateright = (Game.KeyStates & Keys.Right) != 0;
                if (stateleft)
                {
                    foreach(var entity in _entities)
                    {
                        _entities.Take();
                    }
                    _running = true;
                }
                else if (stateright)
                {
                    Exp700.Display.Clear();
                    Stop();
                    break;
                }
            }
        }
    }
    
    /// <summary>
    /// Draw Start Screen
    /// </summary>
    /// <param name="g"></param>
    private void DrawStartScreen()
    {
        var g = Exp700.Display.Graphics;
        Exp700.Display.Clear();
        var imgStart = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.startscrn.png"));
        g?.DrawImage(imgStart, 0, 0);
        Exp700.Display.Update();
    }

    /// <summary>
    /// Method which handles the generation of the enemies
    /// </summary>
    private void GenerateEnemies()
    {
        var imgEnemyBig = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.spitze_gross.png"));
        var imgEnemySmall = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.spitze_klein.png"));
        int timeout;
        Random rnd = new Random();

        while (_running)
        {
            timeout = rnd.Next(500, 2000);
            var enemy = timeout % 2 == 0 ? 
                new Enemy(imgEnemySmall, new Vector2(125, 43), new Vector2(11, 11), new EnemyOutOfScreenHandler(RemoveEnemy)) 
                : new Enemy(imgEnemyBig, new Vector2(125, 32), new Vector2(11, 22), new EnemyOutOfScreenHandler(RemoveEnemy));


           this._entities.Add(enemy);
           Thread.Sleep(timeout);
        }
    }


    /// <summary>
    /// Callback method to remove an enemy out of screen
    /// </summary>
    public void RemoveEnemy(object source)
    {
        if (source.GetType() ==  typeof(Enemy))
        {
            Entity localItem;
            this._entities.TryTake(out localItem);

            if (localItem.GetType() == typeof(Player))
            {
                this._entities.Add((Player)localItem);
                this._entities.Take();
            }
        }
    }

    private static void OnJoyStickChange(object? sender, KeyEventArgs e)
    {
        Console.WriteLine("Joystick: " + e.Keys);
        Game.KeyStates = e.Keys;
    }

    /// <summary>
    /// Method to Draw the Score without argument
    /// </summary>
    private void DrawScore()
    {
        var g = Exp700.Display.Graphics;
        var imgScore = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.0.png"));
        g?.DrawImage(imgScore, 75, 5);
        Exp700.Display.Update();
    }

    /// <summary>
    /// Method to Draw the Score with stopwatch argument
    /// </summary>
    private void DrawScore(Stopwatch stopwatch)
    {
        var g = Exp700.Display.Graphics;
        if (stopwatch == null)
        {
            var imgScore = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.0.png"));
            g?.DrawImage(imgScore, 75, 5);
            Exp700.Display.Update();
        } else if ((int)stopwatch.Elapsed.TotalSeconds < 299)
        {
            int score = (int)stopwatch.Elapsed.TotalSeconds * 10;
            string resourceName = $"Explorer700Demo.Ressources.1.png";
            Exp700.Display.Clear();
            var imgScore = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
            g?.DrawImage(imgScore, 75, 5);
            Exp700.Display.Update();
        } else
        {
            var imgScore = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Explorer700Demo.Ressources.2990.png"));
            g?.DrawImage(imgScore, 75, 5);
            Exp700.Display.Update();
        }
    }
}
