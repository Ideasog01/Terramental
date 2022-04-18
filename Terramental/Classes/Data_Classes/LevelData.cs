using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Terramental
{
    public class LevelData
    {
        public List<KnightCharacter> knightEnemies = new List<KnightCharacter>();
        public List<HealthPickup> _healthPickups = new List<HealthPickup>();
        public List<ElementPickup> _elementPickups = new List<ElementPickup>();
        public List<ScorePickup> _scorePickups = new List<ScorePickup>();
        public List<Sprite> effects = new List<Sprite>();
        public List<ElementWall> elementWallList = new List<ElementWall>();
        public List<DialogueController> dialogueControllerList = new List<DialogueController>();
        public List<Checkpoint> levelCheckpointList = new List<Checkpoint>();
        public Fragment levelFragment;

        public List<Dialogue> levelDialogueList = new List<Dialogue>();
        public List<Vector2> dialogueScaleList = new List<Vector2>();
    }
}
