using System.Drawing;
using System.Numerics;

namespace Explorer700Demo;

public class Enemy(Image img, Vector2 startPos, Vector2 hitbox) : Entity(img, startPos, hitbox)
{
    public override void UpdatePos()
    {
        throw new System.NotImplementedException();
    }
}