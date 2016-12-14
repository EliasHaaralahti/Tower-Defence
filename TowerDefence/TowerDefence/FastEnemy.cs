using TowerDefence;

namespace TowerDefenceGame
{
    ///<summary>
    ///Fast enemy class, inherites Enemy
    ///</summary>
    class FastEnemy : Enemy
    {
        public FastEnemy(double leveys, double korkeus): base(leveys, korkeus)
        {
            health = 65;
            speed = 2.1;
        }
    }
}
