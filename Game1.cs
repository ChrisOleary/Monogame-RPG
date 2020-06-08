using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended; // camera
using Microsoft.Xna.Framework.Audio; // sounds FX
using Microsoft.Xna.Framework.Media; // Music

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

    public static class MySounds
    {
        public static SoundEffect projectileSound;
        public static Song bgMusic;

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

        // camera
        Camera2D cam;


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
            // TiledMap
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            // Camera
            cam = new Camera2D(GraphicsDevice);

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

            // Sound FX
            MySounds.projectileSound = Content.Load<SoundEffect>("Sounds/blip");
            MySounds.bgMusic = Content.Load<Song>("Sounds/original");
            MediaPlayer.Play(MySounds.bgMusic);

            // comment out while not in use
            //Enemy.enemies.Add(new Snake(new Vector2(100, 400)));
            //Enemy.enemies.Add(new Eye(new Vector2(100, 600)));

            // put enemies from TiledMap in array 
            TiledMapObject[] allEnemies = myMap.GetLayer<TiledMapObjectLayer>("enemies").Objects;
            foreach (var en in allEnemies)
            {
                // this retieves from TiledMap, the value from each custom property we added manually, eg Snake or Eye
                string type;
                en.Properties.TryGetValue("type", out type);
                if (type == "Snake")
                {
                    Enemy.enemies.Add(new Snake(en.Position)); // en.Position is from TiledMap
                }
                else if (type == "Eye")
                {
                    Enemy.enemies.Add(new Eye(en.Position));
                }
            }

            // put obstacles in using TiledMap
            TiledMapObject[] allObstacles = myMap.GetLayer<TiledMapObjectLayer>("obstacles").Objects;
            foreach (var obj in allObstacles)
            {
                // this retieves from TiledMap, the value from each custom property we added manually, eg Tree or Bush
                string type;
                obj.Properties.TryGetValue("type", out type);
                if (type == "Tree")
                {
                    Obstacle.obstacles.Add(new Tree(obj.Position)); // en.Position is from TiledMap
                }
                else if (type == "Bush")
                {
                    Obstacle.obstacles.Add(new Bush(obj.Position));
                }
            }


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
                player.Update(gameTime, myMap.WidthInPixels, myMap.HeightInPixels);
            }

            // for CAMERA to follow player but to stop when you get near the edge
            float tempX = player.Position.X;
            float tempY = player.Position.Y;
            int camW = graphics.PreferredBackBufferWidth; // LEFT side of the map
            int camH = graphics.PreferredBackBufferHeight; // TOP side of the map
            int mapW = myMap.WidthInPixels; // RIGHT side of the map
            int mapH = myMap.HeightInPixels; // BOTTOM side of the map

            if (tempX < camW / 2) // if the player is too close to the LEFT side of the screen edge
            {
                tempX = camW / 2; // set cam.LookAt to be graphics.PreferredBackBufferWidth
            }
            if (tempY < camH / 2) // if the player is too close to the TOP side of the screen edge
            {
                tempY = camH / 2; // set cam.LookAt to be graphics.PreferredBackBufferHeight
            }
            if (tempX > (mapW - (camW/2))) // RIGHT
            {
                tempX = mapW - (camW / 2); 
            }
            if (tempY > (mapH - (camH / 2))) // RIGHT
            {
                tempY = mapH - (camH / 2); // BOTTOM
            }


            cam.LookAt(new Vector2(tempX, tempY));
           

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
            mapRenderer.Draw(myMap, cam.GetViewMatrix());

            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix()); // transformMatrix: cam.GetViewMatrix() is for camera

            // if health is not 0 then draw player
            if (player.Health > 0) 
            {
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48));
            }

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

            spriteBatch.End();


            // Hearts will stay in top corner 
            spriteBatch.Begin();

            // draw health hearts based on player.Health
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(heart_Sprite, new Vector2(3 + i * 63, 3), Color.White); // Vector2(i * 63, 0) means a new heart is drawn in a different position
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
