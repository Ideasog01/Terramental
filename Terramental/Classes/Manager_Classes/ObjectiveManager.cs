using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class ObjectiveManager
    {
        public enum Objective { CollectGems, DefeatEnemies };
        public Objective currentObjective;

        public int objectiveProgress;

        private SpriteFont _defaultFont;

        private string _objectiveDescription;

        public ObjectiveManager(SpriteFont defaultFont)
        {
            _defaultFont = defaultFont;
        }

        public string ObjectiveDescription
        {
            get { return _objectiveDescription; }
            set { _objectiveDescription = value; }
        }

        public void SetObjective(Objective objective)
        {
            objectiveProgress = 0;

            switch(objective)
            {
                case Objective.CollectGems:

                    _objectiveDescription = "Objective: Collect all Gems";
                    currentObjective = Objective.CollectGems;
                    break;

                case Objective.DefeatEnemies:

                    _objectiveDescription = "Objective: Defeat all Enemies";
                    currentObjective = Objective.DefeatEnemies;
                    break;
            }
        }

        public void UpdateObjective(Objective objective)
        {
            if(objective == currentObjective)
            {
                objectiveProgress++;
            }

            if(currentObjective == Objective.CollectGems)
            {
                if(objectiveProgress >= SpawnManager.gemCount)
                {
                    _objectiveDescription = "Objective Complete";
                    SpawnManager.levelFragment.IsActive = true;
                }
            }

            if(currentObjective == Objective.DefeatEnemies)
            {
                if(objectiveProgress >= SpawnManager.enemyCount)
                {
                    _objectiveDescription = "Objective Complete";
                    SpawnManager.levelFragment.IsActive = true;
                }
            }
        }

        public void DrawObjectiveString(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_defaultFont, _objectiveDescription, new Vector2(CameraController.cameraTopLeftAnchor.X + 5, CameraController.cameraTopLeftAnchor.Y + (GameManager.screenHeight / 2) - 165), Color.White);
        }
    }
}
