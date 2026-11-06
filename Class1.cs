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
		private float _opacityValue;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private float _rotationAngle = 0f; 
        private float _rotationSpeed = 2f;  


        public Planet(Texture2D texture, Rectangle bounds, SpriteFont font, string name)
		{
			_texture = texture;
			_location = bounds;
			_name = name;
			_font = font;
            _origin = new Vector2(_texture.Width/2, _texture.Height/2);
			_opacityValue = 1;
			
        }
		public void Update(GameTime gameTime)
		{
            _currentMouseState = Mouse.GetState();
            _mouseState = Mouse.GetState();


            int scrollData = _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _rotationAngle += _rotationSpeed * elapsed;


			if (_location.Contains(_mouseState.Position))
			{
                if (scrollData < 0)
                {

                    _opacityValue -= 0.1f;

                }

            }
            if (_location.Contains(_mouseState.Position))
            {
                if (scrollData > 0)
                {

                    _opacityValue += 0.1f;

                }

            }


            _previousMouseState = _currentMouseState;

        }


        public void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(_texture, _location, null, Color.White * _opacityValue,  _rotationAngle /2 , _origin, SpriteEffects.None, 0f);
			Vector2 nameSize = _font.MeasureString(_name);
			Vector2 namePosition = new Vector2(_location.X - 20, _location.Y + _location.Height);
			spriteBatch.DrawString(_font, _name, namePosition, Color.White);
        }

    }
}
