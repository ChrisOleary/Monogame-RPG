using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

namespace RPG
{
    // make it available across the game
    enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // player sprites
        Texture2D player_Sprite;
        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;
        
        // enemy sprites
        Texture2D eyeEnemy_Sprite;
        Texture2D snakeEnemy_Sprite;

        // obstacle sprites
        Texture2D bush_Sprite;
        Texture2D tree_Sprite;

        // misc sprites
        Texture2D heart_Sprite;
        Texture2D bullet_Sprite;

        // map stuff
        TiledMapRenderer mapRenderer;
        TiledMap myMap;

        Player player = new Player();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // game window size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

       
        protected override void Initialize()
        {
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            base.Initialize();
        }

       
        protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // player
            player_Sprite = Content.Load<Texture2D>("Player/player");
            playerDown = Content.Load<Texture2D>("Player/playerDown");
            playerUp = Content.Load<Texture2D>("Player/playerUp");
            playerLeft = Content.Load<Texture2D>("Player/playerLeft");
            playerRight = Content.Load<Texture2D>("Player/playerRight");

            // enemies
            eyeEnemy_Sprite = Content.Load<Texture2D>("Enemies/eyeEnemy");
            snakeEnemy_Sprite = Content.Load<Texture2D>("Enemies/snakeEnemy");

            // obstacles
            bush_Sprite = Content.Load<Texture2D>("Obstacles/bush");
            tree_Sprite = Content.Load<Texture2D>("Obstacles/tree");

            // misc
            heart_Sprite = Content.Load<Texture2D>("Misc/heart");
            bullet_Sprite = Content.Load<Texture2D>("Misc/bullet");

            // player direction animation into array
            player.animations[0] = new AnimatedSprite(playerDown, 1, 4);
            player.animations[1] = new AnimatedSprite(playerUp, 1, 4);
            player.animations[2] = new AnimatedSprite(playerLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(playerRight, 1, 4);

            // Load map xmb
            myMap = Content.Load<TiledMap>("Misc/gameMap");

            // comment out while not in use
            Enemy.enemies.Add(new Snake(new Vector2(100, 400)));
            Enemy.enemies.Add(new Eye(new Vector2(100, 600)));

            Obstacle.obstacles.Add(new Bush(new Vector2(200, 200)));
            Obstacle.obstacles.Add(new Tree(new Vector2(500, 300)));

        }

      
        protected override void UnloadContent()
        {
            
        }

        
      
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            // player movement
            if (player.Health > 0)
            {
                player.Update(gameTime);
            }
           

            // projectiles
            foreach (Projectile proj in Projectile.projectiles)
            {
                proj.Update(gameTime);
            }

            // enemies
            foreach (Enemy en in Enemy.enemies)
            {
                en.Update(gameTime, player.Position);
            }

            // hit detection between projectile and enemy and obstacle
            foreach (Projectile proj in Projectile.projectiles)
            {
                foreach (Enemy en in Enemy.enemies)
                {
                    int sum = proj.Radius + en.Radius;
                    if (Vector2.Distance(proj.Position, en.Position) < sum) // if the distance of the projectle and enemy is less than sum, then theres a collision
                    {
                        proj.Collided = true; // projectile disappers when hitting enemy
                        en.Health--; // take 1 health away
                    }
                }

                // collision between projectile and obstacle
                if (Obstacle.didCollide(proj.Position, proj.Radius))
                {
                    proj.Collided = true; // projectile disappers when hitting obstacle
                }
            }

            // check for collision between player and enemy
            foreach (Enemy en  in Enemy.enemies)
            {
                int sum = en.Radius + player.Radius;
                // if player and enemy are too close
                if (Vector2.Distance(player.Position, en.Position) < sum && player.HealthTimer <= 0) // check distancing
                {
                    player.Health--; // reduce player health by 1
                    player.HealthTimer = 1.5F; // 1.5 second delay between hits
                }
            }

            // remove all projectiles that have collided
            Projectile.projectiles.RemoveAll(p => p.Collided);

            // remove enemies with health at 0
            Enemy.enemies.RemoveAll(e => e.Health <= 0);

        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // MAP
            mapRenderer.Draw(myMap);

            // PLAYER goes outside as begin/end exists in the anim.draw method
            if (player.Health > 0) // is health is not 0 then draw player
            {
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48));

            }

            spriteBatch.Begin();

            //  ENEMIES
            foreach (Enemy en in Enemy.enemies) // foreach works with Lists
            {
                Texture2D spriteToDraw;
                int rad;

                if (en.GetType() == typeof(Snake)) // if the type in the list is a Snake
                {
                    spriteToDraw = snakeEnemy_Sprite; // draw the snake
                    rad = 50;
                }
                else // otherwise
                {
                    spriteToDraw = eyeEnemy_Sprite; // draw the eye
                    rad = 73;
                }
                // then draw the sprite depending on the above type
                spriteBatch.Draw(spriteToDraw, new Vector2(en.Position.X - rad, en.Position.Y - rad), Color.White);
            }

            // PROJECTILES
            foreach (Projectile proj in Projectile.projectiles) // foreach works with Lists
            {
                spriteBatch.Draw(bullet_Sprite, new Vector2(proj.Position.X - proj.Radius, proj.Position.Y - proj.Radius), Color.White);
            }

            // OBSTACLES
            foreach (Obstacle ob in Obstacle.obstacles)
            {
                Texture2D spriteToDraw;
                if (ob.GetType() == typeof(Bush))  // if the type in the list is a bush
                {
                    spriteToDraw = bush_Sprite; // draw bush
                }
                else
                {
                    spriteToDraw = tree_Sprite; // else draw tree
                }
                spriteBatch.Draw(spriteToDraw, ob.Postition, Color.White);
                
            }
           
           

            // draw health hearts based on player.Health
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(heart_Sprite, new Vector2(3+ i * 63, 3), Color.White); // Vector2(i * 63, 0) means a new heart is drawn in a different position
            }

            spriteBatch.End();
          

            base.Draw(gameTime);
        }
    }
}
