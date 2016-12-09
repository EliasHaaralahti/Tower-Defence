using System;
using System.Collections.Generic;
using Jypeli;
using TowerDefence;

namespace TowerDefenceGame
{
    public class SpawnEnemyArgs : EventArgs
    {
        public Enemy enemyArg { get; set; }
    }

    ///<summary>
    ///Handles everything level related, such as enemy spawning.
    ///</summary>
    class LevelHandler
    {
        //How many enemies to spawn
        private int spawnAmount;
        //Game level, affects enemy health and spawnAmount
        private int level;
        //Counts how many enemies have been spawned in the current round
        private int enemyCounter;
        //list of enemies
        private List<Enemy> enemies = new List<Enemy>();
        //Has level changed recently
        private bool levelChanged;

        public event EventHandler<SpawnEnemyArgs> EnemySpawned;

        ///<summary>
        ///LevelHandler constructor
        ///</summary>
        public LevelHandler()
        {
            spawnAmount = 10;
            level = 1;
            enemyCounter = 0;
            levelChanged = false;
        }

        /// <summary>
        /// Determines which unit will be spawned and spawns it.
        /// </summary>
        /// <param name="game">Game</param>
        public void SpawnUnit(Game game)
        {
            if (!levelChanged)
            {
                if (enemyCounter < spawnAmount)
                {
                    enemyCounter++;
                    int randomValue = RandomGen.NextInt(0, 10);
                    Enemy enemy = null;
                    //19, -1 = temp spawn pos
                    if (randomValue == 6 || randomValue == 7) enemy = SpawnFastEnemy(game, 19, -1);
                    else if (randomValue == 4 || randomValue == 2) enemy = SpawnTankEnemy(game, 19, -1);
                    else enemy = SpawnBasicEnemy(game, 19, -1);
                    //Increase hp for each level
                    enemy.health += (int)(level * 2.2);
                    OnEnemySpawned(enemy); //Event
                }
                else IncreaseLevel();
            }
            else if (enemies.Count == 0) levelChanged = false;
        }

        ///<summary>
        ///Called on enemy spawn, fires EnemySpawned event
        ///</summary>
        ///<param name="enemy">Spawned enemy</param>
        protected virtual void OnEnemySpawned(Enemy enemy)
        {
            //Null check is automaticly generated
            EnemySpawned(this, new SpawnEnemyArgs() {enemyArg = enemy });
        }

        ///<summary>
        ///Increases level and every third level also spawnAmount
        ///</summary>
        private void IncreaseLevel()
        {
            levelChanged = true;
            enemyCounter = 0;
            spawnAmount += 2;
            level++;
            if (level % 3 == 0)
                spawnAmount += 3;
        }

        /// <summary>
        /// Removes Enemy object from enemies list.
        /// </summary>
        /// <param name="enemy">Enemy object to be removed</param>
        public void OnEnemyDestroyed(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        /// <summary>
        /// Spawns enemy type basic
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Spawned object</returns>
        private BasicEnemy SpawnBasicEnemy(Game game, int x, int y)
        {
            BasicEnemy enemy = new BasicEnemy(0.45, 0.6);
            enemy.X = x; enemy.Y = y;
            enemy.Color = Color.Red;
            enemy.CollisionIgnoreGroup = 1;
            enemies.Add(enemy);
            game.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// Spawns enemy type fast
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Spawned object</returns>
        private FastEnemy SpawnFastEnemy(Game game, int x, int y)
        {
            FastEnemy enemy = new FastEnemy(0.35, 0.55);
            enemy.X = x; enemy.Y = y;
            enemy.Color = Color.Red;
            enemy.CollisionIgnoreGroup = 1;
            enemies.Add(enemy);
            game.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// Spawns enemy type tank
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Spawned object</returns>
        private TankEnemy SpawnTankEnemy(Game game, int x, int y)
        {
            TankEnemy enemy = new TankEnemy(0.6, 0.8);
            enemy.X = x; enemy.Y = y;
            enemy.Color = Color.Red;
            enemy.CollisionIgnoreGroup = 1;
            enemies.Add(enemy);
            game.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// Returns level as a string
        /// </summary>
        /// <returns>Level in a string</returns>
        public string ReturnLevel()
        {
            return level.ToString();
        }
    }
}
