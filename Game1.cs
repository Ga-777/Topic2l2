using System;
using System.Collections.Generic;
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

		// UI Elements
		Texture2D UI_Panel;
		bool panelVisible = false;
		Rectangle panelRect;
		Planet selectedPlanet = null; // Track which planet was clicked

		SpriteFont font;

		// Master pool of data for randomization
		List<string> possibleNames = new List<string> { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Kepler-22b", "Gliss-581" };
		List<string> possibleMinerals = new List<string> { "Iron", "Copper", "Gold", "Platinum", "Uranium", "Titanium", "Silicon", "Water" };
		List<string> possibleTypes = new List<string> { "Terrestrial", "Gas Giant", "Ice Giant", "Dwarf Planet" };
		List<string> possibleSpecies = new List<string> { "Humans", "Aliens", "Robots", "Cyborgs", "Mutants", "Insects", "Reptiles", "Birds" };

		// Active planets in the game world
		List<Planet> planets;
		List<Texture2D> planetTextures;

		MouseState mouseState, prevMouseState;
		float seconds;
		float respawnTime;

		SoundEffect spaceThemeEffect;
		SoundEffectInstance spaceTheme;

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

			UI_Panel = Content.Load<Texture2D>("UI/UI_Planel");

			spaceThemeEffect = Content.Load<SoundEffect>("Sounds/leberch-space-440026");
			spaceTheme = spaceThemeEffect.CreateInstance();

			// font
			font = Content.Load<SpriteFont>("Font/maintext");

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

				bool isHabitable = random.Next(0, 2) == 1; // 50% chance to be habitable

				Planet newPlanet = new Planet
				{
					Name = possibleNames[random.Next(possibleNames.Count)],
					Type = possibleTypes[random.Next(possibleTypes.Count)],
					Mineral = possibleMinerals[random.Next(possibleMinerals.Count)],
					IsHabitable = isHabitable,
					Species = isHabitable ? possibleSpecies[random.Next(possibleSpecies.Count)] : "None (Barren)",
					Population = isHabitable ? random.Next(100, 500000) : 0,
					TechLevel = isHabitable ? random.Next(1, 10) : 0,
					Bounds = new Rectangle(x, y, size, size),
					Texture = planetTextures[random.Next(planetTextures.Count)],
					Rotation = 0f,
					RotationSpeed = (float)random.NextDouble() * 1.5f + 0.5f
				};

				planets.Add(newPlanet);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			seconds += dt;

			KeyboardState keyboardState = Keyboard.GetState();

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
				foreach (var planet in planets)
				{
					planet.Rotation += dt * planet.RotationSpeed;
				}

				// Handle Spacebar spawning new planets
				if (keyboardState.IsKeyDown(Keys.Space) && seconds >= respawnTime)
				{
					SpawnPlanets(1);
					seconds = 0f;
				}

				// Mouse Dragging & UI Interaction
				prevMouseState = mouseState;
				mouseState = Mouse.GetState();

				if (mouseState.LeftButton == ButtonState.Pressed)
				{
					for (int i = 0; i < planets.Count; i++)
					{
						if (planets[i].Bounds.Contains(mouseState.X, mouseState.Y))
						{
							// Move planet with mouse drag
							planets[i].Bounds = new Rectangle(mouseState.X - planets[i].Bounds.Width / 2, mouseState.Y - planets[i].Bounds.Height / 2,planets[i].Bounds.Width,planets[i].Bounds.Height );

							if (panelVisible && selectedPlanet == planets[i])
							{
								panelRect = new Rectangle(mouseState.X - panelRect.Width + 150, mouseState.Y - panelRect.Height + 50, 160, 140);
							}
							break;
						}
					}
				}
				else if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
				{
					for (int i = 0; i < planets.Count; i++)
					{
						if (planets[i].Bounds.Contains(mouseState.X, mouseState.Y))
						{
							panelVisible = true;
							selectedPlanet = planets[i]; // Lock onto this specific planet's data!
							panelRect = new Rectangle(planets[i].Bounds.X + 50, planets[i].Bounds.Y - 30, 160, 140);
							break;
						}
					}
				}

				// Manage Audio state
				if (spaceTheme.State != SoundState.Playing)
				{
					spaceTheme.Play();
				}
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
			}
			else if (currentScreen == Screen.Playing)
			{
				_spriteBatch.Draw(spaceBackground, window, Color.White);

				// Draw each unique planet
				foreach (var planet in planets)
				{
					Vector2 originCenter = new Vector2(planet.Texture.Width / 2f, planet.Texture.Height / 2f);
					Vector2 drawPosition = new Vector2(planet.Bounds.X + planet.Bounds.Width / 2f, planet.Bounds.Y + planet.Bounds.Height / 2f);
					float scale = (float)planet.Bounds.Width / planet.Texture.Width;

					_spriteBatch.Draw(
						planet.Texture,
						drawPosition,
						null,
						Color.White,
						planet.Rotation / 2f,
						originCenter,
						scale,
						SpriteEffects.None,
						1f
					);

				}



				// Draw UI Panel and the unique planet's info if selected
				if (panelVisible && selectedPlanet != null)
				{
					_spriteBatch.Draw(UI_Panel, panelRect, Color.White);

					// Note: To draw text strings like selectedPlanet.Name or selectedPlanet.Mineral, 
					// make sure you have a SpriteFont loaded and call _spriteBatch.DrawString() here!
					_spriteBatch.DrawString(font, selectedPlanet.Name, new Vector2(panelRect.X + 10, panelRect.Y + 10), Color.White);
					_spriteBatch.DrawString(font, "Type: " + selectedPlanet.Type, new Vector2(panelRect.X + 10, panelRect.Y + 30), Color.White);
					_spriteBatch.DrawString(font, "Mineral: " + selectedPlanet.Mineral, new Vector2(panelRect.X + 10, panelRect.Y + 50), Color.White);
					_spriteBatch.DrawString(font, "Species: " + selectedPlanet.Species, new Vector2(panelRect.X + 10, panelRect.Y + 70), Color.White);
					_spriteBatch.DrawString(font, "Population: " + selectedPlanet.Population.ToString(), new Vector2(panelRect.X + 10, panelRect.Y + 90), Color.White);
				}
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}