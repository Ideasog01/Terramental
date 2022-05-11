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
        private float _descriptionWidth;

        public ObjectiveManager(SpriteFont defaultFont)
        {
            _defaultFont = defaultFont;
        }

        public void SetObjective(Objective objective)
        {
            switch(objective)
            {
                case Objective.CollectGems:

                    _objectiveDescription = "Objective: Collect all Gems";
                    _descriptionWidth = _defaultFont.MeasureString(_objectiveDescription).X;
                    currentObjective = Objective.CollectGems;
                    break;

                case Objective.DefeatEnemies:

                    _objectiveDescription = "Objective: Defeat all Enemies";
                    _descriptionWidth = _defaultFont.MeasureString(_objectiveDescription).X;
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
                    _descriptionWidth = _defaultFont.MeasureString(_objectiveDescription).X;
                    SpawnManager.levelFragment.IsActive = true;
                }
            }

            if(currentObjective == Objective.DefeatEnemies)
            {
                if(objectiveProgress >= SpawnManager.enemyCount)
                {
                    _objectiveDescription = "Objective Complete";
                    _descriptionWidth = _defaultFont.MeasureString(_objectiveDescription).X;
                    SpawnManager.levelFragment.IsActive = true;
                }
            }
        }

        public void DrawObjectiveString(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_defaultFont, _objectiveDescription, new Vector2(CameraController.cameraCentre.X + 5, CameraController.cameraCentre.Y + (GameManager.screenHeight / 2) - 165), Color.White);
        }
    }
}
