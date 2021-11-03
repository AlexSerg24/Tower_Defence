using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	class Stickman : Enemy
	{
		public Stickman(string number, Rectangle shape, List<Bitmap> textureList, Uri deathSoundURI, int maxHealth = 5, int damage = 1, double speed = 50.0, int score = 1, int reward = 1)
		{
			Name = "Stickman " + number;
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
