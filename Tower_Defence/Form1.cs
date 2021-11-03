using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tower_Defence
{
    public partial class Main_form : Form
    {
        Random rand = new Random();
        const int fs = 10;
        int[,] M = new int[fs, fs];

        public Main_form()
        {
            InitializeComponent();
        }

        private void GenerateMap()
        {
            int x_old, y_old, x_new=0, y_new=0;
            int side_start = rand.Next(1, 5);
            int side_finish;
            switch (side_start)
            {
                case 1:
                    side_finish = 3;
                    y_old = 0;
                    x_old = rand.Next(0, fs);
                    break;
                case 2:
                    side_finish = 4;
                    x_old = fs - 1;
                    y_old = rand.Next(0, fs);
                    break;
                case 3:
                    side_finish = 1;
                    y_old = fs - 1;
                    x_old = rand.Next(0, fs);
                    break;
                case 4:
                    side_finish = 2;
                    x_old = 0;
                    y_old = rand.Next(0, fs);
                    break;
            }

            while (y_new != fs)
            {

            }
        }
    }
}
