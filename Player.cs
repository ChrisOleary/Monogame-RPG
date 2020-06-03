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
        private Vector2 position = new Vector2(100, 100);
        private int speed = 200;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        public AnimatedSprite anim;
        public AnimatedSprite[] animations = new AnimatedSprite[4];


        // this may need changing
        public int Health { get; set; } = 3;

        // using a set for a Vector2 is tricker and is done below 
        public Vector2 Position {
            get
            {
                return position;
            }
        }
        public void setX(float newX)
        {
            position.X = newX;
        }
        public void setY(float newY)
        {
            position.Y = newY;
        }

        // player movement
        public void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
}
