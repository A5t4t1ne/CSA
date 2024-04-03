using System.Numerics;
using System.Drawing;
using System;

namespace Explorer700Demo;

public abstract class Entity
{
    protected Image Img { get; }
    protected Vector2 Pos { get; set; }
    public Vector2 Hitbox { get; } // TODO
    
    protected static readonly int[] Res = [128, 64];
    
    private readonly Graphics _graphics = Exp700Singleton.Instance.Display.Graphics;
    
    public Entity(Image img, Vector2 startPos, Vector2 hitbox)
    {
        this.Img = img;
        this.Pos = startPos;
        this.Hitbox = hitbox;
    }
    
    /// <summary>
    /// Change the position of the Entity
    /// </summary>
    /// <param name="diff">The difference by which to move</param>
    /// <returns></returns>
    protected bool Move(Vector2 diff)
    {
    
        // TODO Check if operation is valid / result in valid boundary
        this.Pos += diff;
    
        return true;
    }

    protected bool SetPos(int x, int y)
    {
        if (x > Entity.Res[0] || y > Entity.Res[1]) return false;

        this.Pos = new Vector2(x, y);
        return true;
    }

    /// <summary>
    /// Change of position after 1 Frame.
    /// Each subclass has different events on which to move, so implement individually and do the actual moving
    /// with the <see cref="Move"/> method.
    /// </summary>
    public abstract void UpdatePos();
    
    /// <summary>
    /// Draw the entity on the screen at the current position.
    /// </summary>
    public void Draw()
    {
        _graphics.DrawImage(Img, Pos.X, Pos.Y);
    }

    /// <summary>
    /// Checks if the player is colliding with enemy
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected bool IsCollidingWith(Entity enemy)
    {
        if (enemy == null)
        {
            return false;
        }

        var bottomOfPlayer = this.Pos.Y - this.Hitbox.Y;
        var bottomOfEnemy = enemy.Pos.Y - enemy.Hitbox.Y;
        var rightOfPlayer = this.Pos.X + this.Hitbox.X;
        var rightOfEnemy = enemy.Pos.X + enemy.Hitbox.X;

        // Bottom-left Edge is in Hitbox of Enemy
        if ((bottomOfEnemy <= bottomOfPlayer && bottomOfPlayer <= enemy.Pos.Y)
            && (enemy.Pos.X <= this.Pos.X && this.Pos.X <= rightOfEnemy))
        {
            Console.WriteLine("Bottom-left edge touched enemy hitbox");
            return true;
        }

        // Right Edge is in Hitbox of Enemy
        if ((bottomOfEnemy <= bottomOfPlayer && bottomOfPlayer <= enemy.Pos.Y)
          && (enemy.Pos.X <= rightOfPlayer && rightOfPlayer <= rightOfEnemy))
        {
            Console.WriteLine("Bottom-right edge touched enemy hitbox");
            return true;
        }

        return false;
      }
}