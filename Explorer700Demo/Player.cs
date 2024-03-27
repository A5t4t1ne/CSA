using System;
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
    private const int JumpHeight = 30; 
    private static readonly float JumpForce = 600f / Game.FPS; 
    private const int GroundLevel = 54; // Ground height is 10 pixels
    private bool _jumping = false;
    private float _speedMod = 100;

    public Player(Image img, Vector2 startPos, Vector2 hitbox) : base(img, startPos, hitbox) { }

    /// <summary>
    /// Updates the position of the player while in jumping animation.
    /// Gravity-like motion speed is influenced by <see cref="JumpForce"/>.
    /// Player always jumps up <see cref="JumpHeight"/> pixels.
    /// </summary>
    public override void UpdatePos()
    {
        // If player is not in a jumping animation, his position doesn't change.
        if (!_jumping) return;

        if ((this.Pos.Y + this.Hitbox.Y) > GroundLevel)
        {
            // adjust if character went below the ground line
            this.ResetPos();
            _jumping = false;
        }
        else
        {
            var maxHeight = GroundLevel - this.Hitbox.Y - JumpHeight; 
            var diff =  this.Pos.Y - maxHeight;
            
            _speedMod = _speedMod < 5 ? -100f / JumpHeight * diff : 100f / JumpHeight * diff;
            var speed = JumpForce / 100 * _speedMod;

            if (this.Pos.Y + (int)this.Hitbox.Y - speed > GroundLevel)
            {
                this.ResetPos();
                _jumping = false;
                _speedMod = 100;
            }
            else
            {
                this.Move(new Vector2(0, -speed));
            }
        }
    }

    /// <summary>
    /// Initiate the upwards and fall animation
    /// </summary>
    public void Jump()
    {
        // Skip if player is mid-air
        if (this.Pos.Y + this.Hitbox.Y < GroundLevel) return;
        this._jumping = true;
    }

    /// <summary>
    /// Sets player position to ground.
    /// </summary>
    private void ResetPos()
    {
        this.SetPos(0, GroundLevel - (int)this.Hitbox.Y);
        _speedMod = 0;
    }
}