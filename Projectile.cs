using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    class Projectile
    {
        // fields
        private Vector2 position;
        private int speed = 800;
        private int radius = 15; // half the width of the sprite
        private Dir direction;
        private bool collided = false;
        public static List<Projectile> projectiles = new List<Projectile>();  // make static so can use outside like Projectile.projectiles.Add(new Projectile())


        // Properties
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }
        }

        // Constructors
        public Projectile(Vector2 newPos, Dir newDir)
        {
            position = newPos;
            direction = newDir;
        }


        // Methods
        public void Update(GameTime gameTime)
        {
            // projectile direction and speed
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (direction)
            {
                case Dir.Down:
                    position.Y += speed * dt;
                    break;
                case Dir.Up:
                    position.Y -= speed * dt;
                    break;
                case Dir.Left:
                    position.X -= speed * dt;
                    break;
                case Dir.Right:
                    position.X += speed * dt;
                    break;
                default:
                    break;
            }
        }

    }
}
