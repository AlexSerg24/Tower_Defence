using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
	class Soldier : Enemy
	{
		public Soldier(string number, Rectangle shape, List<Bitmap> textureList, int maxHealth = 10, int damage = 2, double speed = 40.0, int score = 2, int reward = 2)
		{
			Name = "Soldier " + number;
			Shape = shape;
			TextureList = textureList;
			MaxHealth = maxHealth;
			Health = MaxHealth;
			Damage = damage;
			Speed = speed;
			Score = score;
			Reward = reward;
		}
	}
}
