using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
	class Stickman : Enemy
	{
		public Stickman(string number, Rectangle shape, List<Bitmap> textureList, int maxHealth = 5, int damage = 1, double speed = 50.0, int score = 1, int reward = 1)
		{
			Name = "Stickman " + number;
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
