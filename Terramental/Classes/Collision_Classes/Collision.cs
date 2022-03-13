using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    class Collision
    {
        private Rectangle _collisionRectangle;

        private bool _isActive = true;

        public Collision(Rectangle rectangle)
        {
            _collisionRectangle = rectangle;
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool OnCollision(Rectangle otherObject)
        {
            if(_isActive)
            {
                _isActive = false;

                if (_collisionRectangle.Intersects(otherObject))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
