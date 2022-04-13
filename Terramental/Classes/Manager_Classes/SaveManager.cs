using Newtonsoft.Json;
using System.IO;

namespace Terramental
{
    public static class SaveManager
    {
        public static GameManager gameManager;

        public static PlayerData playerData;

        public static GameManager.GameData currentData;

        public static void LoadGame(GameManager.GameData gameData)
        {
            if(GameManager.currentGameState == GameManager.GameState.LoadGame)
            {
                if (gameData == GameManager.GameData.Game1)
                {
                    string strResultJson = File.ReadAllText(@"GameSave1.json");
                    playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
                }

                if (gameData == GameManager.GameData.Game2)
                {
                    string strResultJson = File.ReadAllText(@"GameSave2.json");
                    playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
                }

                if (gameData == GameManager.GameData.Game3)
                {
                    string strResultJson = File.ReadAllText(@"GameSave3.json");
                    playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
                }

                if (gameData == GameManager.GameData.Game4)
                {
                    string strResultJson = File.ReadAllText(@"GameSave4.json");
                    playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
                }

                GameManager.currentGameState = GameManager.GameState.LevelSelect;
                currentData = gameData;
            }
        }

        public static void UnlockLevel(int levelIndex)
        {
            playerData.levelUnlocked[levelIndex] = true;
        }


        //Call this function when the player reaches a checkpoint or completes a level
        public static void SaveGame()
        {
            playerData.playerCheckpoint = GameManager.playerCheckpoint;
            playerData.levelIndex = GameManager.levelIndex;

            if(currentData == GameManager.GameData.Game1)
            {
                string strResultJson = JsonConvert.SerializeObject(playerData);
                File.WriteAllText(@"GameSave1.json", strResultJson);

                strResultJson = File.ReadAllText(@"GameSave1.json");
                playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
            }

            if (currentData == GameManager.GameData.Game2)
            {
                string strResultJson = JsonConvert.SerializeObject(playerData);
                File.WriteAllText(@"GameSave2.json", strResultJson);

                strResultJson = File.ReadAllText(@"GameSave2.json");
                playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
            }

            if (currentData == GameManager.GameData.Game3)
            {
                string strResultJson = JsonConvert.SerializeObject(playerData);
                File.WriteAllText(@"GameSave3.json", strResultJson);

                strResultJson = File.ReadAllText(@"GameSave3.json");
                playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
            }

            if (currentData == GameManager.GameData.Game4)
            {
                string strResultJson = JsonConvert.SerializeObject(playerData);
                File.WriteAllText(@"GameSave4.json", strResultJson);

                strResultJson = File.ReadAllText(@"GameSave4.json");
                playerData = JsonConvert.DeserializeObject<PlayerData>(strResultJson);
            }

        }

    }
}
