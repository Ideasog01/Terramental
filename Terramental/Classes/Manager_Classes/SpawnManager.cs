using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class SpawnManager
    {
        public List<BaseCharacter> enemyCharacters = new List<BaseCharacter>();

        public void Update(GameTime gameTime)
        {
            foreach(BaseCharacter character in enemyCharacters)
            {
                character.UpdateCharacter(gameTime);
            }
        }

    }
}
