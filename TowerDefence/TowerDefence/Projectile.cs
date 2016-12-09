using System;
using Jypeli;

namespace TowerDefenceGame
{
    /// <summary>
    /// Handles projectiles, like turret shots.
    /// </summary>
    class Projectile : PhysicsObject
    {
        public double selfDestructTime { get; }
        public int damage { get; }

        public Projectile(double leveys, double korkeus): base(leveys, korkeus)
        {
            Shape = Shape.Rectangle;
            Color = Color.Orange;

            selfDestructTime = 2;
            damage = 75;
            CanRotate = false;
            CollisionIgnoreGroup = 2; //Same as enemies

            Timer selfDestruct = new Timer();
            selfDestruct.Interval = selfDestructTime;
            selfDestruct.Timeout += new Action(delegate () { this.Destroy(); });
            selfDestruct.Start();
        }
    }
}
