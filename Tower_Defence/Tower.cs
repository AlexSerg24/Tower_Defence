using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace Tower_Defence
{
	public abstract class Tower
	{
		protected string name = "bad";
		public string Name { get { return name; } set { if (value != "") name = value; } }
		protected Point location = new Point(0, 0);
		public Point Location { get { return location; } set { location = value; } }
		protected int size = 0;
		public int Size { get { return size; } set { if (value > 0) size = value; } }
		protected Bitmap texture = null;
		public Bitmap Texture { get { return texture; } set { texture = value; } }
		protected Bitmap shotTexture = null;
		public Bitmap ShotTexture { get { return shotTexture; } set { shotTexture = value; } }
		/*protected System.Windows.Media.MediaPlayer shotSound = null;
		public System.Windows.Media.MediaPlayer ShotSound { get { return shotSound; } set { shotSound = value; } }*/
		protected bool isShot = false;
		public bool IsShot { get { return isShot; } set { isShot = value; } }
		protected int animationTicks = 0;
		public int AnimationTicks { get { return animationTicks; } set { animationTicks = value; } }
		protected int cost = 0;
		public int Cost { get { return cost; } set { if (value > 0) cost = value; } }
		protected int range = 0;
		public int Range { get { return range; } set { if (value > 0) range = value; } }
		protected int damage = 0;
		public int Damage { get { return damage; } set { if (value > 0) damage = value; } }
		protected double reload = 0.0;
		public double Reload { get { return reload; } set { if (value > 0) reload = value; } }

		public abstract bool Shoot(List<Enemy> foeList, ref int scr, ref int mny);
	}
}
