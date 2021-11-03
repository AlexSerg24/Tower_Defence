using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	class Mage : Tower
	{
		public Mage(Point location, Bitmap texture, Bitmap shotTexture, Uri shotSoundURI)
		{
			Name = "Mage";
			Location = location;
			Size = 29;
			Texture = texture;
			ShotTexture = shotTexture;
			/*ShotSound = new System.Windows.Media.MediaPlayer();
			ShotSound.Open(shotSoundURI);
			ShotSound.Volume = 0.25;*/
			Cost = 23;
			Range = 150;
			Damage = 4;
			Reload = 2.0;
			AnimationTicks = Int32.MaxValue;
		}

		public Mage()
		{
			Name = "Mage Info";
			Cost = 23;
			Range = 150;
			Damage = 4;
			Reload = 2.0;
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
