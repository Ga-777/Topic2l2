using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Topic2l2
{
	public class Planet
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Mineral { get; set; }
		public string Species { get; set; }
		public int Population { get; set; }
		public int TechLevel { get; set; }
		public bool IsHabitable { get; set; }

		public Rectangle Bounds { get; set; }
		public Texture2D Texture { get; set; }
		public float Rotation { get; set; }
		public float RotationSpeed { get; set; }
	}
}
