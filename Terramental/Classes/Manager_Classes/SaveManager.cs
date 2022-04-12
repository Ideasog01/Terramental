using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public static class SaveManager
    {
        public static GameManager gameManager;

        public static void LoadGame(GameManager.GameData gameData)
        {
            if(gameData == GameManager.GameData.Game1)
            {
                gameManager.LoadNewGame(@"MapData.json");
            }
        }
    }
}
