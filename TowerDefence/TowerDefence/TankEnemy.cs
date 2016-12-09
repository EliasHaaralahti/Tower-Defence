using TowerDefence;

namespace TowerDefenceGame
{
    ///<summary>
    ///Tank enemy class, inherites Enemy
    ///</summary>
    class TankEnemy : Enemy
    {
        public TankEnemy(double leveys, double korkeus): base(leveys, korkeus)
        {
            health = 220;
            speed =  0.9;
        }
    }
}
