using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Topic2l2
{
    enum Screen
    {
        MainScreen,
        playing
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window;
        Random random = new Random();
        Texture2D spaceBackground;
        List<Rectangle> planetRects;
        List<Texture2D> textures;
        List<Texture2D> planetTextures;
        float planetRotation;
        float planetRotationSpeed;
        Vector2 origin;
        MouseState mouseState, prevMouseState;
        int size;
        float seconds;
        float respawnTime;
        int y, x;
        float planetSpeed; // pixels per second
        SoundEffectInstance spaceTheme;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0,0, 800, 600);
            random = new Random();
            textures = new List<Texture2D>();
            planetRects = new List<Rectangle>();
            planetTextures = new List<Texture2D>();
           origin = new Vector2();
            planetRotationSpeed = planetSpeed + random.Next(1,7);

            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            seconds = 0f;
            respawnTime = 3f;


            base.Initialize();
            for (int i = 0; i < 7; i++)
            {
                size = random.Next(25,75);
                 x = random.Next(0, window.Width - size);
                 y = random.Next(0, window.Height - size);
                planetRects.Add(new Rectangle(x, y, size, size));
                planetTextures.Add(textures[random.Next(textures.Count)]);
                
            }





            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceBackground = Content.Load<Texture2D>("Images/space_background");
            for (int i = 1; i <= 13; i++)
            {
                textures.Add(Content.Load<Texture2D>($"Images/16-bit-planet" + i));

            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
           
            float dt = + (float)gameTime.ElapsedGameTime.TotalSeconds;
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            planetRotation += dt;
            
            if (seconds >= respawnTime)
            {
                int size = random.Next(25, 75);
                int x = random.Next(0, window.Width - size);
                int y = random.Next(0, window.Height - size);
                planetRects.Add(new Rectangle(x, y, size, size));
                planetTextures.Add(textures[random.Next(textures.Count)]);
                seconds = 0f;
            }
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed )
            {
                for (int i = 0; i < planetRects.Count; i++)
                {
                    if (planetRects[i].Contains(mouseState.Position))
                    {
                        planetRects[i] = new Rectangle(mouseState.X - planetRects[i].Width / 2, mouseState.Y - planetRects[i].Height / 2, planetRects[i].Width, planetRects[i].Height);
                        break;
                    }
                }
            }
            else if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                for (int i = 0; i < planetRects.Count; i++)
                {
                    if (planetRects[i].Contains(mouseState.Position))
                    {
                        planetRects.RemoveAt(i);
                        planetTextures.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }


            if (planetRects.Count > 0)
            {
                for (int i = 0; i < planetRects.Count; i++)
                {
                    planetSpeed = ++planetSpeed;

                }
            }

           
            
                // TODO: Add your update logic here

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(spaceBackground, window, Color.White);

            for (int i = 0; i < planetRects.Count; i++)
            {
                Rectangle tempRect = new Rectangle(planetRects[i].Center, planetRects[i].Size);
                _spriteBatch.Draw(planetTextures[i], tempRect, null, Color.White, planetRotation / 2f, new Vector2(size, size), SpriteEffects.None, 1f);
            }
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
