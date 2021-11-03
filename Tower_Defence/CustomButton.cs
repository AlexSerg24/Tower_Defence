using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tower_Defence
{
    class CustomButton
    {
		public string Name { get; set; }
        public Rectangle Shape { get; set; }
        public Bitmap Texture { get; set; }
        public bool Visible { get; set; }

        public CustomButton(string name, Rectangle shape, Bitmap texture = null, bool visible = false)
        {
			Name = name;
            Shape = shape;
            Texture = texture;
            Visible = visible;
        }

        public bool IsClicked(System.Windows.Forms.MouseEventArgs e)
        {
			if ((Visible) && (e.X >= Shape.X) && (e.X < Shape.X + Shape.Width) && (e.Y >= Shape.Y) && (e.Y < Shape.Y + Shape.Height))
				return true;
			else
				return false;
		}

		public void ChangePos(int x, int y)
		{
			Shape = new Rectangle(x, y, Shape.Width, Shape.Height);
		}
	}
}
