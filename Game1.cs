using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            player.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // goes outside as begin/end exists in the anim.draw method
            player.anim.Draw(spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48));

            spriteBatch.Begin();

            spriteBatch.End();
          

            base.Draw(gameTime);
        }
    }
}
