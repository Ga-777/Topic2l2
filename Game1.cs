using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Topic2l2
{
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
        MouseState mouseState;

        float seconds;
        float respawnTime;

        float planetSpeed = 100f; // pixels per second

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
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            seconds = 0f;
            respawnTime = 3f;


            base.Initialize();
            for (int i = 0; i < 7; i++)
            {
                int size = random.Next(25,75);
                int x = random.Next(0, window.Width - size);
                int y = random.Next(0, window.Height - size);
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
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (seconds >= respawnTime)
            {
                int size = random.Next(25, 75);
                int x = random.Next(0, window.Width - size);
                int y = random.Next(0, window.Height - size);
                planetRects.Add(new Rectangle(x, y, size, size));
                planetTextures.Add(textures[random.Next(textures.Count)]);
                seconds = 0f;
            }
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
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
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                for (int i = 0; i < planetRects.Count; i++)
                {
                    if (planetRects[i].Contains(mouseState.Position))
                    {
                        planetRects.RemoveAt(i);
                        planetTextures.RemoveAt(i);
                        break;
                    }
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
                _spriteBatch.Draw(planetTextures[i], planetRects[i], Color.White);
            }
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
