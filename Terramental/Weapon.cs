using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    class Weapon : Sprite
    {
        private float _timer;

        private bool _isActive;

        private bool _attackActive;

        public Weapon(float duration)
        {
            _timer = duration;
            _isActive = true;
        }

        public void Attack()
        {
            _attackActive = true;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer > 0 && _isActive)
            {
                _timer -= deltaTime;
            }
            
            if(_isActive && _timer <= 0)
            {
                //Delete
            }

            if(_attackActive)
            {
                CheckCollision();
            }
        }

        private void CheckCollision()
        {

        }

    }
}
