using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace spacePlanets
{
	public class Planet
	{
		Random random = new Random();

		private Texture2D _texture;
        public Rectangle _location;
        private string _name;
		private SpriteFont _font;
		private new Vector2 _origin;
		private MouseState _mouseState;
		
        private float _rotationAngle = 0f; 
        private float _rotationSpeed = 2f;  


        public Planet(Texture2D texture, Rectangle bounds, SpriteFont font, string name)
		{
			_texture = texture;
			_location = bounds;
			_name = name;
			_font = font;
            _origin = new Vector2(_texture.Width/2, _texture.Height/2);
			
        }
		public void Update(GameTime gameTime)
		{
            MouseState mouseState = _mouseState;
			mouseState = Mouse.GetState();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _rotationAngle += _rotationSpeed * elapsed;
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (_location.Contains(mouseState.Position))
                {
					
                }
            }


        }


        public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, _location,null, Color.White,  _rotationAngle /2 , _origin, SpriteEffects.None, 0f);
			Vector2 nameSize = _font.MeasureString(_name);
			Vector2 namePosition = new Vector2(_location.X - 20, _location.Y + _location.Height);
			spriteBatch.DrawString(_font, _name, namePosition, Color.White);
        }

    }
}
