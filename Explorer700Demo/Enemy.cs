using System.Drawing;
using System.Numerics;

namespace Explorer700Demo;

public class Enemy(Image img, Vector2 startPos, Vector2 hitbox, EnemyOutOfScreenHandler handler) : Entity(img, startPos, hitbox)
{
    private int MoveSpeed = 5;

    /// <summary>
    /// Updates the position of the enemy
    /// Enemies are moving by the value of <c>MoveSpeed</c> from right to left
    /// If the enemy moves out of the screen, the passed handler is called
    /// </summary>
    public override void UpdatePos()
    {
        this.Move(new Vector2(-MoveSpeed, 0));

        if (this.Pos.X < 0)
        {
            handler(this);
        }
    }
}