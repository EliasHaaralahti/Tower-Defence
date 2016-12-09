static class Ohjelma
{
#if WINDOWS || XBOX

    static void Main(string[] args)
    {
        //Launches the game (game starts in "TowerDefence.cs" Begin()
        using (TowerDefenceGame.TowerDefence game = new TowerDefenceGame.TowerDefence())
        {
#if !DEBUG
            game.IsFullScreen = true;
#endif
            game.Run();
        }
    }
#endif
}
