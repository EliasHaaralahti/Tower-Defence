using TowerDefence;

namespace TowerDefenceGame
{
    ///<summary>
    ///Basic enemy class, inherites Enemy
    ///</summary>
    public class BasicEnemy : Enemy
    {
        public BasicEnemy(double leveys, double korkeus): base(leveys, korkeus)
        {
            health = 130;
            speed = 1.15;
        }
    }
}
