using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
	class Eye : Tower
	{
		public Eye(Point location, Bitmap texture, Bitmap shotTexture)
		{
			Name = "Eye";
			Location = location;
			Size = 30;
			Texture = texture;
			ShotTexture = shotTexture;
			Cost = 45;
			Range = 250;
			Damage = 10;
			Reload = 4.0;
			AnimationTicks = Int32.MaxValue;
		}

		public Eye()
		{
			Name = "Eye Info";
			Cost = 45;
			Range = 250;
			Damage = 10;
			Reload = 4.0;
		}

		public override bool Shoot(List<Enemy> foeList, ref int scr, ref int mny)
		{
			bool isShot = false, hasKilled = false;
			int maxHP = -1;
			Enemy foe;
			List<int> inRangeFoesList = new List<int>();

			int i = 0;
			while (i < foeList.Count)
			{
				foe = foeList[i];
				double d = Math.Sqrt((foe.Shape.X + foe.Shape.Width / 2 - Location.X) * (foe.Shape.X + foe.Shape.Width / 2 - Location.X) + (foe.Shape.Y + foe.Shape.Height / 2 - Location.Y) * (foe.Shape.Y + foe.Shape.Height / 2 - Location.Y));
				if (d < Range)
				{
					inRangeFoesList.Add(i);
				}
				i++;
			}

			int j = 0;
			while (j < inRangeFoesList.Count)
			{
				if (maxHP < foeList[inRangeFoesList[j]].Health)
				{
					maxHP = foeList[inRangeFoesList[j]].Health;
					i = inRangeFoesList[j];
				}
				j++;
			}

			if (inRangeFoesList.Count > 0)
			{
				isShot = true;
				hasKilled = foeList[i].ShotBy(this);
				if (hasKilled)
				{
					foeList[i].Die(ref scr, ref mny);
					foeList.RemoveAt(i);
				}
			}

			return isShot;
		}
	}
}
