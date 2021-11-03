using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	class Dragon : Enemy
	{
		public Dragon(string number, Rectangle shape, List<Bitmap> textureList, Uri deathSoundURI, int maxHealth = 100, int damage = 10, double speed = 99.9, int score = 100, int reward = 100)
																																	   //double speed = 33.3
		{
			Name = "Dragon " + number;
			Shape = shape;
			TextureList = textureList;
			/*DeathSound = new System.Windows.Media.MediaPlayer();
			DeathSound.Open(deathSoundURI);
			DeathSound.Volume = 0.25;*/
			MaxHealth = maxHealth;
			Health = MaxHealth;
			Damage = damage;
			Speed = speed;
			Score = score;
			Reward = reward;
		}
	}
}
