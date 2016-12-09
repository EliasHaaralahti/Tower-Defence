using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Controls;
using Jypeli.Widgets;
using TowerDefence;

/// <summary>
/// @Author: Elias Haaralahti
/// Version: 9.12.2016
/// </summary>

namespace TowerDefenceGame
{
    ///<summary>
    ///"Main" class, used to create objects and control them
    ///</summary>
    public class TowerDefence : PhysicsGame
    {
        const int tileSize = 1;
        const int mapSizeX = 22; 
        const int mapSizeY = 11;
        const int turretPrice = 150;
        const double spawnRate = 1.0;

        //All enemies
        private List<Enemy> enemies;
        //All turrets
        private List<Turret> turrets;
        //All pathpoints
        private List<Vector> pathPoints;
        //All game tiles
        private GameObject[,] tiles;

        //Player object(health, money)
        Player player;
        //Levelhandler(spawning, level...)
        LevelHandler levelHandler;

        //Labels
        Label label_Background;
        Label label_Health;
        Label label_EnemyAmount;
        Label label_Level;
        Label label_Money;

        //Timers
        Timer updateTimer;
        Timer spawnTimer;

        #region Images/Textures
        //My textures
<<<<<<< HEAD
        readonly Image textureTurret = LoadImage("Turret");
        readonly Image textureTank = LoadImage("Tank");
        readonly Image textureLightTank = LoadImage("LightTank");
        readonly Image textureFastTank = LoadImage("FastTank");
        readonly Image textureUI = LoadImage("UI_background");

        //Other textures (Free from opengameart.org/textures)
        readonly Image textureSand = LoadImage("Sand");
        readonly Image textureGrass = LoadImage("Grass");
        readonly Image textureTurretFloor = LoadImage("TurretFloor");
=======
        Image textureTurret = LoadImage("Turret");
        Image textureTank = LoadImage("Tank");
        Image textureLightTank = LoadImage("LightTank");
        Image textureFastTank = LoadImage("FastTank");
        Image textureUI = LoadImage("UI_background");

        //Other textures (Free from opengameart.org/textures)
        Image textureSand = LoadImage("Sand");
        Image textureGrass = LoadImage("Grass");
        Image textureTurretFloor = LoadImage("TurretFloor");
>>>>>>> 1f349f9f915a1e15de5a90e7d8f236b02fdabdde
        #endregion

        ///<summary>
        ///Program begins executing here
        ///</summary>
        public override void Begin()
        {
            Mouse.IsCursorVisible = true;
            Camera.ZoomToAllObjects(-0.8);

            //Main menu
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, GameOver, "End game");
            //Mouse click to change tiles
            Mouse.Listen(MouseButton.Left, ButtonState.Pressed, MouseClick, "Mouse click.");

            StartGame();
        }

        /// <summary>
        /// Displays main menu
        /// </summary>
        private void ShowMainMenu()
        {
            MultiSelectWindow valikko = new MultiSelectWindow("Tower Defence",
            "Start Game", "Quit Game");
            valikko.ItemSelected += ButtonPress;
            Add(valikko);
        }
        
        /// <summary>
        /// Handles main menu button presses
        /// </summary>
        /// <param name="buttonID">Pressed button ID</param>
        private void ButtonPress(int buttonID)
        {
            switch (buttonID)
            {
                case 0:
                    StartGame();
                    break;
                case 1:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// Starts the game and calls the required methods
        /// </summary>
        private void StartGame()
        {
            enemies = new List<Enemy>();
            tiles = new GameObject[mapSizeY + 1, mapSizeX + 1];
            turrets = new List<Turret>();
            pathPoints = new List<Vector>();

            //Maphandler
            MapHandler mapHandler = new MapHandler();
            tiles = mapHandler.GenerateMap(this, mapSizeX, mapSizeY, tileSize);
            mapHandler.GeneratePath(this, 1, mapSizeX, mapSizeY);
            pathPoints = mapHandler.ReturnPathPoints();

            //Add textures to map
            for (int y = 0; y <= mapSizeY; y += tileSize) //Y coordinate in tiles
            {
                for (int x = 0; x <= mapSizeX; x += tileSize) //X coordinate in tiles
                {
                    if (tiles[y, x].Color == Color.Green)
                    {
                        tiles[y, x].Image = textureGrass;
                    }
                    else tiles[y, x].Image = textureSand;
                }
            }

            //Player(health, score, money)
            player = new Player();

            //LevelHandler
            levelHandler = new LevelHandler();
            levelHandler.EnemySpawned += OnEnemySpawn;

            //Spawn enemies
            spawnTimer = new Timer();
            spawnTimer.Interval = spawnRate; //How often we spawn units
            //Calls functions required for spawning a new enemy.
            spawnTimer.Timeout += delegate { levelHandler.SpawnUnit(this); };
            spawnTimer.Start();

            //Update timer
            updateTimer = new Timer();
            updateTimer.Interval = 1;
            updateTimer.Timeout += Updater;
            updateTimer.Start();

            #region All labels
            label_Background = new Label(175, 150);
            label_Background.Image = textureUI;
            Add(label_Background);
            label_Background.X = Screen.Left + 75;
            label_Background.Y = Screen.Top - 50;

            label_Health = new Label(125, 35);
            label_Health.Text = "Health: " + player.health.ToString();
            label_Health.X = Screen.Left + 75;
            label_Health.Y = Screen.Top - 25;
            Add(label_Health);

            label_Level = new Label(125, 35);
            label_Level.Text = "Level: " + levelHandler.ReturnLevel();
            label_Level.X = Screen.Left + 75;
            label_Level.Y = Screen.Top - 50;
            Add(label_Level);

            label_EnemyAmount = new Label(125, 35);
            label_EnemyAmount.Text = "Enemies: " + enemies.Count;
            label_EnemyAmount.X = Screen.Left + 75;
            label_EnemyAmount.Y = Screen.Top - 75;
            Add(label_EnemyAmount);

            label_Money = new Label(125, 35);
            label_Money.Text = "Money: " + player.money;
            label_Money.X = Screen.Left + 75;
            label_Money.Y = Screen.Top - 100;
            Add(label_Money);
            #endregion
        }

        /// <summary>
        /// Ends the game, destroys objects and displays main menu
        /// </summary>
        private void GameOver()
        {
            updateTimer.Stop();
            spawnTimer.Stop();

            for (int y = 0; y <= mapSizeY; y += tileSize) //Y coordinate in tiles
            {
                for (int x = 0; x <= mapSizeX; x += tileSize) //X coordinate in tiles
                {
                    tiles[y,x].Destroy();
                }
            }
            foreach(Turret t in turrets)
            {
                t.Destroy();
            }
            foreach (Enemy e in enemies) e.Destroy();

            label_Background.Destroy();
            label_EnemyAmount.Destroy();
            label_Health.Destroy();
            label_Level.Destroy();
            label_Money.Destroy();

            ShowMainMenu();
        }

        /// <summary>
        /// Called from levelHandler on enemy spawn, receiving spawned object as args.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="args">SpawnEnemyArgs, including enemy object.</param>
        public void OnEnemySpawn(object source, SpawnEnemyArgs args)
        {
            Enemy enemy = args.enemyArg; //Spawned enemy
            if(enemy.GetType() == typeof(BasicEnemy))
            {
                enemy.Image = textureLightTank;
            }
            if (enemy.GetType() == typeof(FastEnemy))
            {
                enemy.Image = textureFastTank;
            }
            if (enemy.GetType() == typeof(TankEnemy))
            {
                enemy.Image = textureTank;
            }
            enemies.Add(enemy);
            enemy.MoveTo(pathPoints[0], enemy.speed, new Action(delegate() { OnEnemyReachPathPoint(enemy); }));
            label_EnemyAmount.Text = "Enemies: " + enemies.Count;
            label_Level.Text = "Level: " + levelHandler.ReturnLevel();
        }

        ///<summary>
        ///Part of Jypeli MoveTo, called everytime object reaches destination.
        ///</summary>
        public void OnEnemyReachPathPoint(Enemy enemy)
        {
            enemy.pathCounter++;
            if (enemy.pathCounter < pathPoints.Count) //Regular path point
            {
                enemy.MoveTo(pathPoints[enemy.pathCounter], enemy.speed, new Action(delegate () { OnEnemyReachPathPoint(enemy); }));
                enemy.Angle = GetAngle<Enemy>(pathPoints[enemy.pathCounter], enemy, -90);
            }
            else //Path end
            {
                enemy.Destroy();
                enemies.Remove(enemy);
                levelHandler.OnEnemyDestroyed(enemy);
                player.health--;
                label_Health.Text = "Health: "+player.health.ToString();
                label_EnemyAmount.Text = "Enemies: " + enemies.Count;
                if (player.health <= 0) GameOver();
            }
        }

        /// <summary>
        /// Returns the angle between a vector and a physicsobject (or anything that inherits it)
        /// </summary>
        /// <typeparam name="T">Generic for any object that inherits physicsobject</typeparam>
        /// <param name="origin">Origin object position</param>
        /// <param name="target">Target object</param>
        /// <param name="AdditionalAngle">Additional angle to be added</param>
        /// <returns>Angle between objects + additionalAngle</returns>
        private Angle GetAngle<T>(Vector origin, T target, double AdditionalAngle) where T : PhysicsObject
        {
            Vector direction = (origin - target.AbsolutePosition).Normalize();
            Angle angle = direction.Angle + Angle.FromDegrees(AdditionalAngle);
            return angle;
        }
        
        /// <summary>
        /// Update method called every 1 second, checks turret and enemy distances
        /// </summary>
        private void Updater()
        {
            //Check enemy positions for nearby turrets
            foreach (Enemy enemy in enemies)
            {
                foreach (Turret turret in turrets)
                {
                    if (turret.isLoaded)
                    {
                        //If distance between turret and enemy is smaller than D*D !!! - (x1-x2)*(x1-x2)+(y1-y2)*(y1-y2)) < d*2 - !!!
                        if ((turret.X - enemy.X) * (turret.X - enemy.X) + (turret.Y - enemy.Y) * (turret.Y - enemy.Y) <= turret.range*turret.range) 
                        {
                            Shoot(turret, enemy);
                            break; //Without break turret fires at all enemies in range
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fires a projectile from turret to enemy
        /// </summary>
        /// <param name="turret"></param>
        /// <param name="enemy"></param>
        private void Shoot(Turret turret, PhysicsObject enemy)
        {
            turret.isLoaded = false;
            Projectile missile = new Projectile(0.05, 0.3);
            missile.X = turret.X; missile.Y = turret.Y;
            AddCollisionHandler(missile, OnCollisionEnter);
            Vector target = AimAtMovingTarget(enemy);
            turret.Angle = GetAngle<PhysicsObject>(target, missile, 0);
            Add(missile, 1);
            
            missile.Angle = GetAngle<PhysicsObject>(target, missile, 90);
            Vector direction = Vector.FromLengthAndAngle(turret.projectileSpeed, turret.Angle);
            missile.Push(direction);

        }

        /// <summary>
        /// Calculates (and adds imprecision) where the turret should shoot to hit a moving target.
        /// </summary>
        /// <param name="enemy">Target object</param>
        /// <returns></returns>
        private Vector AimAtMovingTarget(PhysicsObject enemy)
        {
            Vector targetPos = enemy.Position;
            double distance = 0; //How far ahead we have to aim to hit
            //Accuracy for each unit
            if (enemy.GetType() == typeof(BasicEnemy)) distance = RandomGen.NextDouble(0.4, 1.5);
            if (enemy.GetType() == typeof(TankEnemy)) distance = RandomGen.NextDouble(0.1, 1);
            if (enemy.GetType() == typeof(FastEnemy)) distance = RandomGen.NextDouble(1, 2.0);

            if (enemy.Angle.MainDirection == Direction.Right) //Up
                targetPos.Y += distance;

            if (enemy.Angle.MainDirection == Direction.Up) //Left
                targetPos.X += distance;

            if (enemy.Angle.MainDirection == Direction.Left) //Down
                targetPos.Y -= distance;

            if (enemy.Angle.MainDirection == Direction.Down) //Right
                targetPos.X -= distance;

            return targetPos;
        }

        /// <summary>
        /// Jypeli collision detection
        /// </summary>
        /// <param name="missile">Turret projectile</param>
        /// <param name="enemy">Enemy hit</param>
        private void OnCollisionEnter(PhysicsObject missile, PhysicsObject enemy)
        {
            if((string)enemy.Tag == "Enemy")
            {
                Enemy enemyObj = (Enemy)enemy;
                Projectile projectile = (Projectile)missile;
                missile.Destroy();
                if (enemyObj.health > projectile.damage) //Temp missile dmg   
                    enemyObj.health -= projectile.damage;
                else
                {
                    enemy.Destroy();
                    enemies.Remove(enemyObj);
                    levelHandler.OnEnemyDestroyed((Enemy)enemy);
                    player.money += 5;
                    label_Money.Text = "Money: " + player.money;
                    label_EnemyAmount.Text = "Enemies: " + enemies.Count;
                }
            }
        }

        ///<summary>
        ///Called on mouse click, Creates a turret
        ///</summary>
        private void MouseClick()
        {
            if (player != null && player.money >= turretPrice)
            {
                int mouseX = (int)(Mouse.PositionOnWorld.X + 0.5);
                int mouseY = (int)(Mouse.PositionOnWorld.Y + 0.5);

                try
                {
                    if (tiles[mouseY, mouseX].Image == textureGrass) //Grass
                    {
                        player.money -= turretPrice;
                        label_Money.Text = "Money: " + player.money;
                        tiles[mouseY, mouseX].Image = textureTurretFloor;
                        Turret turret = new Turret(0.75, 0.75);
                        turret.Image = textureTurret;
                        turret.X = tiles[mouseY, mouseX].X;
                        turret.Y = tiles[mouseY, mouseX].Y;
                        turrets.Add(turret);
                        Add(turret, 2);
                    }
                }
                catch { } //Most likely out of index error
            }
        }
    }
}
