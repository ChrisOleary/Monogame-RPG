using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    class Player
    {
        // fields
        private Vector2 position = new Vector2(100, 100);
        private int speed = 200;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        public AnimatedSprite anim;
        public AnimatedSprite[] animations = new AnimatedSprite[4]; // 4 to hold the 4 enums
        private KeyboardState kStateOld = Keyboard.GetState();
        public int health = 3;
        private int radius = 20;
        private float healthTimer = 0f;

        // properties
        public float HealthTimer
        {
            get { return healthTimer; }
            set { healthTimer = value; }
        }

      
        public int Radius
        {
            get { return radius; }
        }
        
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        // using a set for a Vector2 is tricker and is done below 
        public Vector2 Position {
            get{ return position; }
        }
        public void setX(float newX)
        {
            position.X = newX;
        }
        public void setY(float newY)
        {
            position.Y = newY;
        }

        // Methods
        // player movement
        public void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // so players health doesnt decline too fast when hit
            if (healthTimer > 0)
            {
                healthTimer -= dt;
            }

            // set the animation based on direction player is facing
            anim = animations[(int)direction];

            // this cycles through the player animations
            if (isMoving)
                anim.Update(gameTime);
            else
                anim.setFrame(1); // return animation to standing still frame
                
            // this means every frame the player stops moving
            isMoving = false;
            // however if the keys are held, player will move
            if (kstate.IsKeyDown(Keys.Right))
            {
                direction = Dir.Right;
                isMoving = true;
            }
            if (kstate.IsKeyDown(Keys.Left))
            {
                direction = Dir.Left;
                isMoving = true;
            }
            if (kstate.IsKeyDown(Keys.Up))
            {
                direction = Dir.Up;
                isMoving = true;
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                direction = Dir.Down;
                isMoving = true;
            }
            
            // movement speed
            if (isMoving)
            {
                Vector2 tempPos = position; // for obstable collision

                switch (direction)
                {
                    case Dir.Down:
                        tempPos.Y += speed * dt; // put next move into variable and pass to Obstacle.didCollide
                        if (!Obstacle.didCollide(tempPos, radius)) // if collision is NOT made
                        {
                            position.Y += speed * dt; // do the actual movement
                        }
                        break;
                    case Dir.Up:
                        tempPos.Y -= speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius))
                        {
                            position.Y -= speed * dt;
                        }
                        break;
                    case Dir.Left:
                        tempPos.X -= speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius))
                        {
                            position.X -= speed * dt;
                        }
                        break;
                        case Dir.Right:
                        tempPos.X += speed * dt;
                        if (!Obstacle.didCollide(tempPos, radius))
                        {
                            position.X += speed * dt;
                        }
                        break;
                    default:
                        break;
                }
            }

            // when spacebar is pressed, create ONE projectile
            if (kstate.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
            {
                Projectile.projectiles.Add(new Projectile(position, direction));
            }

            kStateOld = kstate;
        }
    }
}
