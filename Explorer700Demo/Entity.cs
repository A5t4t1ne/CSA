using System;
using System.Net.Mime;
using System.Numerics;
using System.Drawing;
using Explorer700Library;

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
}