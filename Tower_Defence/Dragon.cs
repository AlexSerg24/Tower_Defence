using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
	class Dragon : Enemy
	{
		public Dragon(string number, Rectangle shape, List<Bitmap> textureList, int maxHealth = 100, int damage = 10, double speed = 33.3, int score = 100, int reward = 100)
		{
			Name = "Dragon " + number;
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
