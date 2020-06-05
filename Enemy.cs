using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    class Enemy
    {
        // fields
        private Vector2 position;
        protected int health; // protected so child elements can have their own health. used for inheritance
        protected int speed;
        protected int radius;
        // make static so we can use outside eg Enemy.enemies.Add(new Enemy(newPos))
        public static List<Enemy> enemies = new List<Enemy>();

        // properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public Vector2 Position
        {
            get { return position; }
        }
        public int Radius
        {
            get { return radius; }
        }

        public Enemy(Vector2 newPos)
        {
            position = newPos;
        }

        // Methods
        // moving the enemy towards the player
        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // get delta time

            Vector2 moveDir = playerPos - position;
            moveDir.Normalize(); // Normalize reduces the number to 1

            Vector2 tempPos = position; // varible used for collision detection

            tempPos += moveDir * speed * dt; // check for next movement
            if (!Obstacle.didCollide(tempPos, radius)) // then use that position to check if there are no collisions
            {
                position += moveDir * speed * dt; // if no collitions, move the enemy closer to the player
            }
        }
    }
    
    // Child Class
    class Snake : Enemy
    {
        // Constructors
        public Snake(Vector2 newPos) : base(newPos) // this constructor inherits Enemy constructor
        {
            speed = 90;
            radius = 42; // for hit box
            health = 3;
        }
    }

    // Child Class
    class Eye : Enemy
    {
        // Constructors
        public Eye(Vector2 newPos) : base(newPos)  // this constructor inherits Enemy constructor
        {
            speed = 80;
            radius = 45; // for hit box
            health = 5;
        }
    }


}
