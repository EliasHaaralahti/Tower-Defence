using System.Collections.Generic;
using Jypeli;

namespace TowerDefenceGame
{
    ///<summary>
    ///Handles everything map related
    ///</summary>
    class MapHandler
    {
        GameObject[,] tiles; //All tiles
        List<Vector> pathPoints = new List<Vector>(); //All pathpoints

        ///<summary>
        ///Generates a X*Y map
        ///</summary>
        ///<param name="game">Game</param>
        ///<param name="mapSizeX">map width</param>
        ///<param name="mapSizeY">map heigth</param>
        ///<param name="tileSize">size of the tiles</param>
        ///<returns>Map matrix</returns>
        public GameObject[,] GenerateMap(Game game, int mapSizeX, int mapSizeY, int tileSize)
        {
            tiles = new GameObject[mapSizeY+1, mapSizeX+1];
            for (int y = 0; y<=mapSizeY; y+= tileSize) //Y coordinate in tiles
            {
                for (int x = 0; x <= mapSizeX; x += tileSize) //X coordinate in tiles
                {
                    GameObject obj = new GameObject(tileSize, tileSize, Shape.Rectangle);
                    obj.X = x; obj.Y = y;
                    obj.Color = Color.Green;
                    game.Add(obj);
                    tiles[y, x] = obj; //Adds obj to tiles array
                }
            }
            return tiles;
        }

        ///<summary>
        ///Generates a randomized path for enemies
        ///</summary>
        ///<param name="game">Game</param>
        ///<param name="tileSize">size of the tiles</param>
        public void GeneratePath(Game game, int tileSize, int mapSizeX, int mapSizeY)
        {
            int[] pathValues = { 0, mapSizeX-3 }; //Starting point
            tiles[pathValues[0], pathValues[1]].Color = Color.Yellow; //Origin of the path

            #region CreatePathPart calls
            CreatePathPart(pathValues, 6, "up");
            CreatePathPart(pathValues, 2, "right");
            CreatePathPart(pathValues, 2, "up");
            CreatePathPart(pathValues, 5, "left");
            CreatePathPart(pathValues, 2, "up");
            CreatePathPart(pathValues, 5, "left");
            CreatePathPart(pathValues, 5, "down");
            CreatePathPart(pathValues, 4, "right");
            CreatePathPart(pathValues, 4, "down");
            CreatePathPart(pathValues, 8, "left");
            CreatePathPart(pathValues, 5, "up");
            CreatePathPart(pathValues, 2, "right");
            CreatePathPart(pathValues, 3, "up");
            CreatePathPart(pathValues, 5, "left");
            CreatePathPart(pathValues, 6, "down");
            CreatePathPart(pathValues, 3, "left");
            CreatePathPart(pathValues, 3, "up");
            CreatePathPart(pathValues, 1, "left");
            #endregion
        }

        ///<summary>
        ///Creates part of a path
        ///</summary>
        ///<param name="pathValues">Origin of path</param>
        ///<param name="rowLength">Length of the path row</param>
        ///<param name="direction">Direction of the path, use "up","down","left","right"</param>
        private void CreatePathPart(int[] pathValues, int rowLength, string direction)
        {
            for(int i = 0; i<rowLength; i++)
            {
                if (direction.ToLower() == "up") pathValues[0] += 1;
                if (direction.ToLower() == "down") pathValues[0] -= 1;
                if (direction.ToLower() == "left") pathValues[1] -= 1;
                if (direction.ToLower() == "right") pathValues[1] += 1;

                tiles[pathValues[0], pathValues[1]].Color = Color.Yellow;
            }
            pathPoints.Add(new Vector(pathValues[1], pathValues[0]));
            tiles[pathValues[0], pathValues[1]].Color = Color.DarkOrange; //path point
        }

        ///<summary>
        ///Returns all path points
        ///</summary>
        ///<returns>All pathpoints</returns>
        public List<Vector> ReturnPathPoints()
        {
            return pathPoints;
        }
    }
}
