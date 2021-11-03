using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	class Soldier : Enemy
	{
		public Soldier(string number, Rectangle shape, List<Bitmap> textureList, Uri deathSoundURI, int maxHealth = 10, int damage = 2, double speed = 40.0, int score = 2, int reward = 2)
		{
			Name = "Soldier " + number;
			Shape = shape;
			TextureList = textureList;
			/*DeathSound = new System.Windows.Media.MediaPlayer();
			DeathSound.Open(deathSoundURI);
			DeathSound.Volume = 0.125;*/
			MaxHealth = maxHealth;
			Health = MaxHealth;
			Damage = damage;
			Speed = speed;
			Score = score;
			Reward = reward;
		}
	}
}
