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

		public string planetName()
		{
			Name = "Planet " + Guid.NewGuid().ToString().Substring(0, 8);
			return Name;
		}
		public string planetType()
		{
			string[] types = { "Terrestrial", "Gas Giant", "Ice Giant", "Dwarf Planet" };
			Type = types[new Random().Next(types.Length)];
			return Type;
		}
		public string planetMineral()
		{
			string[] minerals = { "Iron", "Copper", "Gold", "Platinum", "Uranium", "Titanium", "Silicon", "Water" };
			Mineral = minerals[new Random().Next(minerals.Length)];
			return Mineral;
		}
		public string planetSpecies()
		{
			string[] species = { "Humans", "Aliens", "Robots", "Cyborgs", "Mutants", "Insects", "Reptiles", "Birds" };
			Species = species[new Random().Next(species.Length)];
			return Species;
		}
		public int planetPopulation()
		{
			Population = new Random().Next(1000, 1000000);
			return Population;
		}

	}
}
