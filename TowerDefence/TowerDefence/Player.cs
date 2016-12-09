namespace TowerDefenceGame
{
    /// <summary>
    /// Handles Player stats
    /// </summary>
    class Player
    {
        //How many hits player can take
        public int health { get; set; }
        //Amount of money
        public int money { get; set; }

        public Player()
        {
            health = 10;
            money = 500;
        }
    }
}
