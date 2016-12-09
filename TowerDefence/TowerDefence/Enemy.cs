using Jypeli;

namespace TowerDefence
{
    /// <summary>
    /// Enemy class, base class for BasicEnemy, FastEnemy and TankEnemy
    /// </summary>
    public class Enemy : PhysicsObject
    {
        public int health { get; set; }
        public double speed { get; set; }
        public int pathCounter { get; set; } //How many path points object has passed.

        public Enemy(double leveys, double korkeus) : base(leveys, korkeus)
        {
            pathCounter = 0;
            Mass = 1500;
            CanRotate = false; //prevents bullets from rotating enemies
            Tag = "Enemy";
        }
    }
}
