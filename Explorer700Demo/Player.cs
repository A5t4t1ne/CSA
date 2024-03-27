using System.Numerics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using Explorer700Library;
using Iot.Device.SenseHat;

namespace Explorer700Demo;

public class Player : Entity
{
    private const int MaxJumpHeight = 5;
    private const int JumpSpeed = 2;
    private const int FallSpeed = JumpSpeed;
    private const int GroundLevel = 50;
    private static readonly int[] Res = [128, 64];
    private bool _jumping = false;
    private bool _directionUp = false;

    public Player(Image img, Vector2 startPos, Vector2 hitbox) : base(img, startPos, hitbox) { }

    public override void UpdatePos()
    {
        // If player is not in a jumping animation, his position doesn't change.
        // TODO Fix
        if (!_jumping) return;
        
        if (this.Pos.Y >= Res[0] - GroundLevel)
        {
            // adjust if character went below the ground line
            this.Move(new Vector2(0, this.Pos.Y - GroundLevel));
            _jumping = false;
        }
        else if (_directionUp)
        {
            if (this.Pos.Y < MaxJumpHeight)
                this.Move(new Vector2(0, -JumpSpeed));
            else
                _directionUp = false;
        }
        else if (!_directionUp && this.Pos.Y > GroundLevel)
        {
            this.Move(new Vector2(0, FallSpeed));
        }
    }

    /// <summary>
    /// Initiate the upwards and fall animation
    /// </summary>
    /// <param name="g"></param>
    public void Jump()
    {
        this._jumping = true;
        this._directionUp = true;
    }
}