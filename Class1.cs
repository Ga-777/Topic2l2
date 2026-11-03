using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spacePlanets
{
	public class Planet
	{
		Random random = new Random();

		private Texture2D _texture;
        private Rectangle _location;
		private string _name;
		private SpriteFont _font;
		private float _rotateion;



        public Planet(Texture2D texture, Rectangle bounds, SpriteFont font, string name)
		{
			_texture = texture;
			_location = bounds;
			_name = name;
			_font = font;
        }
		public void Update(GameTime gameTime)
		{
			_rotateion += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, _location,null, Color.White, _rotateion, new Vector2(0,0), SpriteEffects.None, _rotateion);
			Vector2 nameSize = _font.MeasureString(_name);
			Vector2 namePosition = new Vector2(_location.X + (_location.Width - nameSize.X) / 2, _location.Y + _location.Height + 5);
			spriteBatch.DrawString(_font, _name, namePosition, Color.White);
        }

    }
}
