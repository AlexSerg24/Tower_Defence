using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
	public abstract class Enemy
	{
		protected string name = "n4m3";
		public string Name { get { return name; } set { if (value != "") name = value; } }
		protected Rectangle shape = new Rectangle(0, 0, 0, 0);
		public Rectangle Shape { get { return shape; } set { shape = value; } }
		protected List<Bitmap> textureList;
		public List<Bitmap> TextureList { get { return textureList; } set { textureList = value; } }
		protected int pathNum = 1;
		public int PathNumber { get { return pathNum; } set { if (value > 0) pathNum = value; } }
		protected int animationTicks = 0;
		public int AnimationTicks { get { return animationTicks; } set { animationTicks = value; } }
		protected int steps = 0;
		public int Steps { get { return steps; } set { steps = value; } }
		protected int maxHealth = 0;
		public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
		protected int health = 0;
		public int Health { get { return health; } set { health = value; } }
		protected int damage = 0;
		public int Damage { get { return damage; } set { damage = value; } }
		protected double speed = 0.0;
		public double Speed { get { return speed; } set { if (value > 0) speed = value; } }
		protected int score = 0;
		public int Score { get { return score; } set { if (value > 0) score = value; } }
		protected int reward = 0;
		public int Reward { get { return reward; } set { if (value > 0) reward = value; } }

		/*public abstract bool ShotBy(Tower twr, ref int scr, ref int mny);
		public abstract void Die(ref int scr, ref int mny);
		public abstract void Finish(ref int lives);*/

		public bool ShotBy(Tower twr)
		{
			Health -= twr.Damage;
			if (Health <= 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void Die(ref int scr, ref int mny)
		{
			scr += Score;
			mny += Reward;
		}

		public void Finish(ref int lives)
		{
			lives -= Damage;
		}

		public void Move(int x, int y)
		{
			Shape = new Rectangle(x - Shape.Width / 2, y - Shape.Height / 2, Shape.Width, Shape.Height);
		}
	}
}
