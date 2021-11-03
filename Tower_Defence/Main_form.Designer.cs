namespace Tower_Defence
{
    partial class Main_form
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_form));
			this.Screen_PB = new System.Windows.Forms.PictureBox();
			this.GameAnimation_timer = new System.Windows.Forms.Timer(this.components);
			this.EnemySpawn_timer = new System.Windows.Forms.Timer(this.components);
			this.Credits_timer = new System.Windows.Forms.Timer(this.components);
			this.Intro_timer = new System.Windows.Forms.Timer(this.components);
			this.Help_timer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.Screen_PB)).BeginInit();
			this.SuspendLayout();
			// 
			// Screen_PB
			// 
			this.Screen_PB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Screen_PB.BackColor = System.Drawing.Color.Magenta;
			this.Screen_PB.Location = new System.Drawing.Point(0, 0);
			this.Screen_PB.Margin = new System.Windows.Forms.Padding(0);
			this.Screen_PB.Name = "Screen_PB";
			this.Screen_PB.Size = new System.Drawing.Size(690, 630);
			this.Screen_PB.TabIndex = 0;
			this.Screen_PB.TabStop = false;
			this.Screen_PB.Paint += new System.Windows.Forms.PaintEventHandler(this.Screen_PB_Paint);
			this.Screen_PB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Screen_PB_MouseClick);
			this.Screen_PB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Screen_PB_MouseMove);
			// 
			// GameAnimation_timer
			// 
			this.GameAnimation_timer.Interval = 42;
			this.GameAnimation_timer.Tick += new System.EventHandler(this.GameAnimation_timer_Tick);
			// 
			// EnemySpawn_timer
			// 
			this.EnemySpawn_timer.Interval = 1000;
			this.EnemySpawn_timer.Tick += new System.EventHandler(this.EnemySpawn_timer_Tick);
			// 
			// Credits_timer
			// 
			this.Credits_timer.Interval = 42;
			this.Credits_timer.Tick += new System.EventHandler(this.Credits_timer_Tick);
			// 
			// Intro_timer
			// 
			this.Intro_timer.Interval = 42;
			this.Intro_timer.Tick += new System.EventHandler(this.Intro_timer_Tick);
			// 
			// Help_timer
			// 
			this.Help_timer.Interval = 42;
			this.Help_timer.Tick += new System.EventHandler(this.Help_timer_Tick);
			// 
			// Main_form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(690, 630);
			this.Controls.Add(this.Screen_PB);
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Main_form";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Age of Clash of War of Clans";
			((System.ComponentModel.ISupportInitialize)(this.Screen_PB)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Screen_PB;
		private System.Windows.Forms.Timer GameAnimation_timer;
		private System.Windows.Forms.Timer EnemySpawn_timer;
		private System.Windows.Forms.Timer Credits_timer;
		private System.Windows.Forms.Timer Intro_timer;
		private System.Windows.Forms.Timer Help_timer;
	}
}

