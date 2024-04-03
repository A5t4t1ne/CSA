using System.Drawing;
using System.Numerics;

namespace Explorer700Demo;

public class Enemy(Image img, Vector2 startPos, Vector2 hitbox, EnemyOutOfScreenHandler handler) : Entity(img, startPos, hitbox)
{
    private int moveSpeed = 5;
    public override void UpdatePos()
    {
        this.Move(new Vector2(-moveSpeed, 0));

        if (this.Pos.X < 0)
        {
            handler(this);
        }
    }
}