using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	class Archer : Tower
	{
		public Archer(Point location, Bitmap texture, Bitmap shotTexture, Uri shotSoundURI)
		{
			Name = "Archer";
			Location = location;
			Size = 25;
			Texture = texture;
			ShotTexture = shotTexture;
			/*ShotSound = new System.Windows.Media.MediaPlayer();
			ShotSound.Open(shotSoundURI);
			ShotSound.Volume = 0.1875;*/
			Cost = 8;
			Range = 100;
			Damage = 1;
			Reload = 1.0;
			AnimationTicks = Int32.MaxValue; //наверняка превышает (Reload / GameAnimation_timer.Interval * 1000), чтобы башня могла выстрелить сразу же после установки. Изменить на 0 для высокого уровня сложности. Для начала добавить уровни сложности.
		}

		public Archer()
		{
			Name = "Archer Info";
			Cost = 8;
			Range = 100;
			Damage = 1;
			Reload = 1.0;
		}

		public override bool Shoot(List<Enemy> foeList, ref int scr, ref int mny)
		{
			int i = 0;
			bool isShot = false, hasKilled = false;
			Enemy foe;
			while (!isShot && (i < foeList.Count))
			{
				foe = foeList[i];
				double d = Math.Sqrt((foe.Shape.X + foe.Shape.Width / 2 - Location.X) * (foe.Shape.X + foe.Shape.Width / 2 - Location.X) + (foe.Shape.Y + foe.Shape.Height / 2 - Location.Y) * (foe.Shape.Y + foe.Shape.Height / 2 - Location.Y));
				if (d < Range)
				{
					//ShotSound.Stop();
					//ShotSound.Play();
					isShot = true;
					hasKilled = foe.ShotBy(this);
					if (hasKilled)
					{
						foe.Die(ref scr, ref mny);
						//foeList.Remove(foe);
					}
				}
				i++;
			}
			return isShot;
		}
	}
}
