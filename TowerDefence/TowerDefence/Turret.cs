using Jypeli;

namespace TowerDefenceGame
{
    /// <summary>
    /// Handles turret stats and reloading
    /// </summary>
    class Turret : PhysicsObject
    {
        public double fireRate { get;}
        public int range { get; }
        public double projectileSpeed { get; }
        public bool isLoaded { get; set; }

        public Turret(double leveys, double korkeus) : base(leveys, korkeus) //leveys & korkeus = width & height, part of Jypeli.
        {
            fireRate = 3;
            range = 4;
            projectileSpeed = 250;
            isLoaded = true;
            CollisionIgnoreGroup = 2;

            Timer reloadTimer = new Timer();
            reloadTimer.Interval = fireRate;
            reloadTimer.Timeout += Reload;
            reloadTimer.Start();
        }

        public void Reload()
        {
            if(isLoaded == false)
                isLoaded = true;
        }
    }
}
