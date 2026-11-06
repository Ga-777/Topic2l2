using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spacePlanets;

namespace spaceGame
{
	enum Screen
	{
	   
		MainScreen,
		Playing
	}

	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		Screen currentScreen = Screen.MainScreen;

		Rectangle window;
		Random random;
		Texture2D spaceBackground;

		
		// MainScreen Elements

		Rectangle mainBackgroundRect;

		Texture2D logo;

		Rectangle logoRect;

        Rectangle logoRect2;

        SpriteFont font, mainScreenFont;

		// Master pool of data for randomization
		List<string> possibleNames = new List<string> { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Kepler-22b", "Gliss-581", "Gav-117", "Endor", "Naboo"};


		// Active planets in the game world
		List<Planet> planets;
		List<Texture2D> planetTextures;

		MouseState mouseState, prevMouseState;
		float seconds;
		float respawnTime;

		SoundEffect spaceThemeEffect;
		SoundEffectInstance spaceTheme;

		string scoreMade, scoreGone;
		int scoreMadeHolder, scoreGoneHolder;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			window = new Rectangle(0, 0, 800, 600);
			random = new Random();
			mainBackgroundRect = new Rectangle(0, 0, window.Width, window.Height);
		
			logoRect = new Rectangle(window.Width / 2 - 250, window.Height / 2 - 400, 500, 500);
			planets = new List<Planet>();
			planetTextures = new List<Texture2D>();

			_graphics.PreferredBackBufferWidth = window.Width;
			_graphics.PreferredBackBufferHeight = window.Height;
			_graphics.ApplyChanges();

			seconds = 0f;
			respawnTime = 3f;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			spaceBackground = Content.Load<Texture2D>("Images/space_background");

			// Load planet textures
			for (int i = 1; i <= 13; i++)
			{
				planetTextures.Add(Content.Load<Texture2D>($"Images/16-bit-planet{i}"));
			}

			logo = Content.Load<Texture2D>("UI/logo");
			

			spaceThemeEffect = Content.Load<SoundEffect>("Sounds/leberch-space-440026");
			spaceTheme = spaceThemeEffect.CreateInstance();

			// font
			font = Content.Load<SpriteFont>("Font/maintext");

			mainScreenFont = Content.Load<SpriteFont>("Font/MainScreen");
			// Spawn initial unique planets
			SpawnPlanets(7);
			
		}

		private void SpawnPlanets(int count)
		{
			for (int i = 0; i < count; i++)
			{
				int size = random.Next(25, 75);
				int x = random.Next(0, window.Width - size);
				int y = random.Next(0, window.Height - size);

				

				
				
					
				planets.Add(new Planet(planetTextures[random.Next(planetTextures.Count)], new Rectangle(x, y, size, size), font, possibleNames[random.Next(possibleNames.Count)])); //E
				scoreMadeHolder++;
			}
		}
		

        

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            seconds += dt;

            KeyboardState keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Screen Switching Logic
            if (currentScreen == Screen.MainScreen)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    currentScreen = Screen.Playing;
                }
            }
            else if (currentScreen == Screen.Playing)
            {
                // Update planet rotations
                if (keyboardState.IsKeyDown(Keys.Space) && seconds >= respawnTime)
                {
                    SpawnPlanets(1);
                    seconds = 0f;
                }

                if (spaceTheme.State != SoundState.Playing)
                {
                    spaceTheme.Play();
                }
            }
			if (mouseState.RightButton == ButtonState.Pressed)
			{

                for (int i = planets.Count - 1; i >= 0; i--)
                {
                    if (planets[i]._location.Contains(mouseState.Position))
                    {
                        planets.RemoveAt(i);
						planetTextures.RemoveAt(i); //E
						--i;
						scoreGoneHolder++;
                    }
                }
             

            }
			scoreMade = scoreMadeHolder.ToString();
			scoreGone = scoreGoneHolder.ToString();
            foreach (Planet planet in planets)
            {
                planet.Update(gameTime);
            }

			base.Update(gameTime);
            
        }

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();
			
			if (currentScreen == Screen.MainScreen)
			{
				// Draw title screen instructions here if desired
				_spriteBatch.Draw(spaceBackground, mainBackgroundRect, Color.White);
				_spriteBatch.Draw(logo, logoRect, Color.White);
				_spriteBatch.Draw(logo, logoRect2, Color.Ivory);
				_spriteBatch.DrawString(mainScreenFont, "Press Enter to Start", new Vector2(window.Width / 2 - 150, window.Height / 2), Color.White);
				_spriteBatch.DrawString(font, "By Gavin, Anderson", new Vector2( 630, 570), Color.White);
			}
			else if (currentScreen == Screen.Playing)
			{
				_spriteBatch.Draw(spaceBackground, window, Color.White);

				// Draw each unique planet
				foreach (Planet planet in planets)
				{
					planet.Draw(_spriteBatch);
				}
				_spriteBatch.DrawString(font, "Planets Removed: " + scoreGone, new Vector2(500, 10), Color.White);
                _spriteBatch.DrawString(font, "Planets made: " + scoreMade, new Vector2(200, 10), Color.White);



            }

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}