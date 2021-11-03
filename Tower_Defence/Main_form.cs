using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Tower_Defence
{
	public partial class Main_form : Form
	{
		Random rand = new Random();     //подношение Богу Рандома
										//const double EPS = 0.000001;	//подношение Богу Сравнения чисел с плавающей точкой

		List<Point> Path = new List<Point>();               //список точек, задающих путь
		SolidBrush bru = new SolidBrush(Color.Lime);        //обыкновенная кисть для заливки
		System.Drawing.Drawing2D.HatchBrush Hbru =          //необыкновенная кисть для красивой заливки
			new System.Drawing.Drawing2D.HatchBrush(
			System.Drawing.Drawing2D.HatchStyle.Percent50,
			Color.Lime, Color.Magenta);

		const int roadWidth = 50;   //ширина пути
		int gameZone;               //ширина и высота игрового поля (без интерфейса)
		int selTwrID;               //номер выделенной башни
		int helpPage;               //текущая страница Помощи
		int helpTime;               //время показа Помощи
		int secsRemaining;          //количество секунд для продолжения игры с помощью жетона
		int coinTime;               //время подбрасывания монетки
		int creditsPage;            //номер текущей страницы титров
		int creditsTime;            //время показа страницы титров
		int introTime;              //время показа вступительного ролика
		int GlobalWave;             //индекс текущей волны
		int GlobalScore;            //количество очков игрока
		int GlobalLives;            //количество жизней игрока
		int GlobalGold;             //количество золота игрока
		bool GlobalVictory;         //маркер победы
		bool GlobalDeffeat;         //маркер поражения
		bool GlobalPause;           //маркер паузы игры
		bool ESTpaused;             //маркер паузы спавна противников
		bool needHelp;              //маркер внутриигровой помощи (в меню паузы)
		bool twrIsHeld;             //маркер удержания башни мышкой (при её установке (башни, не мышки))
		bool wrongPlace;            //маркер недопустимого места установки башни
		string twrForInfo;          //тип башни для отображения информации
		string screenCondition;     //наименование текущего состояния экрана
		string yourName;            //имя игрока
		string[,] Records;          //матрица рекордов

		List<CustomButton> BtnsList =   //список кнопок интерфейса: 
			new List<CustomButton>();
		CustomButton NewGame_btn;       //Новая игра
		CustomButton Leaderboard_btn;   //Рекорды
		CustomButton Help_btn;          //Помощь
		CustomButton Credits_btn;       //Титры
		CustomButton Exit_btn;          //Выход
		CustomButton Back_btn;          //Назад
		CustomButton Pause_btn;         //Пауза
		CustomButton NextWave_btn;      //Следующая волна
		CustomButton Archer_btn;        //башня Лучника
		CustomButton Mushroom_btn;      //башня-Гриб
		CustomButton Mage_btn;          //башня Мага
		CustomButton Eye_btn;           //башня-Око
		CustomButton Sell_btn;          //Продать башню
		CustomButton PrevLetter_1_btn;  //Предыдущая первая буква
		CustomButton PrevLetter_2_btn;  //Предыдущая вторая буква
		CustomButton PrevLetter_3_btn;  //Предыдущая третья буква
		CustomButton NextLetter_1_btn;  //Следующая первая буква
		CustomButton NextLetter_2_btn;  //Следующая вторая буква
		CustomButton NextLetter_3_btn;  //Следующая третья буква

		//Bitmap MissingTexture_bmp;  //текстура Отсутствующей текстуры (была добавлена в ресурсы программы)
		Bitmap MainBackground_bmp;  //текстура фона главного меню
		Bitmap Background_bmp;      //основная текстура фона
		Bitmap EnterYourName_bmp;   //текстура фона при вводе имени
		Bitmap Leaderboard_bmp;     //текстура фона Рекордов
		List<Bitmap> Help_bmps =    //текстуры фона Помощи
			new List<Bitmap>();
		Bitmap Victory_bmp;         //текстура спрайта победы
		Bitmap Deffeat_bmp;         //текстура спрайта поражения
		Bitmap Lives_bmp;           //текстура иконки здоровья
		Bitmap Gold_bmp;            //текстура иконки золота
		Bitmap Range_bmp;           //текстура иконки радиуса атаки башни
		Bitmap Damage_bmp;          //текстура иконки урона башни
		Bitmap Reload_bmp;          //текстура иконки перезарядки башни
		Bitmap Coin_bmp;            //текстура монетки
		List<Bitmap> Credits_bmps = //текстуры титров
			new List<Bitmap>();
		Bitmap Logo_bmp;            //текстура логотипа
		Bitmap Crutch_bmp;          //текстура костыля
		Bitmap Bicycle_bmp;         //текстура велосипеда

		List<Enemy> CurrentEnemyList = new List<Enemy>();   //список противников на поле
		List<Bitmap> Stickman_bmps = new List<Bitmap>();    //список текстур анимации Крупье
		List<Bitmap> Soldier_bmps = new List<Bitmap>();     //список текстур анимации Копчёной селёдки
		List<Bitmap> Dragon_bmps = new List<Bitmap>();      //список текстур анимации Дуэньи
		List<Enemy> HelpEnemyList = new List<Enemy>();      //список противников в Помощи

		List<Tower> CurrentTowerList =  //список башен на поле
			new List<Tower>();
		Bitmap Archer_bmp;              //текстура башни Лучника
		Bitmap ArcherShot_bmp;          //текстура выстрела башни Лучника
		Bitmap Mushroom_bmp;            //текстура башни-Гриба
		Bitmap MushroomShot_bmp;        //текстура выстрела башни-Гриба
		Bitmap Mage_bmp;                //текстура башни Мага
		Bitmap MageShot_bmp;            //текстура выстрела башни Мага
		Bitmap Eye_bmp;                 //текстура башни-Ока
		Bitmap EyeShot_bmp;             //текстура выстрела башни-Ока
		List<Tower> HelpTowerList =     //список башен в Помощи
			new List<Tower>();

		Point mouseLocation;    //точка с координатами курсора
		Tower hTwr;             //башня, удерживаемая мышкой при её установке (опять же, башни, а не мышки)
		Bitmap HeldTower_bmp;   //текстура устанавливаемой башни

		List<Wave> WavesList = new List<Wave>();    //список волн

		List<Bitmap> Alphabet = new List<Bitmap>(); //список текстур используемых текстовых символов

		Point Crutch_pt;
		Point Bicycle_pt;

		public Main_form()
		{
			InitializeComponent();

			//this.Text = "Age of Clash of War of Clans" + " (v1.22.20170905)";

			Records = new string[10, 2];
			LoadRecords();
			gameZone = Screen_PB.Height;

			//---Подгрузка необходимых текстур---

			//MissingTexture_bmp = (File.Exists("Textures/MissingTexture.png")) ? new Bitmap("Textures/MissingTexture.png") :  Properties.Resources.MissingTexture;
			MainBackground_bmp = (File.Exists("Textures/Backgrounds/MainBackground.png")) ? new Bitmap("Textures/Backgrounds/MainBackground.png") : Properties.Resources.MissingTexture;
			Background_bmp = (File.Exists("Textures/Backgrounds/Background.png")) ? new Bitmap("Textures/Backgrounds/Background.png") : Properties.Resources.MissingTexture;
			EnterYourName_bmp = (File.Exists("Textures/Backgrounds/EnterYourName.png")) ? new Bitmap("Textures/Backgrounds/EnterYourName.png") : Properties.Resources.MissingTexture;
			Leaderboard_bmp = (File.Exists("Textures/Backgrounds/Leaderboard.png")) ? new Bitmap("Textures/Backgrounds/Leaderboard.png") : Properties.Resources.MissingTexture;
			Help_bmps.Add((File.Exists("Textures/Backgrounds/Instructions_1.png")) ? new Bitmap("Textures/Backgrounds/Instructions_1.png") : Properties.Resources.MissingTexture);
			Help_bmps.Add((File.Exists("Textures/Backgrounds/Instructions_2.png")) ? new Bitmap("Textures/Backgrounds/Instructions_2.png") : Properties.Resources.MissingTexture);
			Help_bmps.Add((File.Exists("Textures/Backgrounds/Instructions_3.png")) ? new Bitmap("Textures/Backgrounds/Instructions_3.png") : Properties.Resources.MissingTexture);
			Help_bmps.Add((File.Exists("Textures/Backgrounds/Instructions_4.png")) ? new Bitmap("Textures/Backgrounds/Instructions_4.png") : Properties.Resources.MissingTexture);
			Victory_bmp = (File.Exists("Textures/GUI/End_Vict.png")) ? new Bitmap("Textures/GUI/End_Vict.png") : Properties.Resources.MissingTexture;
			Deffeat_bmp = (File.Exists("Textures/GUI/End_Deff.png")) ? new Bitmap("Textures/GUI/End_Deff.png") : Properties.Resources.MissingTexture;
			Lives_bmp = (File.Exists("Textures/GUI/Ico_Hrt.png")) ? new Bitmap("Textures/GUI/Ico_Hrt.png") : Properties.Resources.MissingTexture;
			Gold_bmp = (File.Exists("Textures/GUI/Ico_Gld.png")) ? new Bitmap("Textures/GUI/Ico_Gld.png") : Properties.Resources.MissingTexture;
			Range_bmp = (File.Exists("Textures/GUI/Ico_Rng.png")) ? new Bitmap("Textures/GUI/Ico_Rng.png") : Properties.Resources.MissingTexture;
			Damage_bmp = (File.Exists("Textures/GUI/Ico_Dmg.png")) ? new Bitmap("Textures/GUI/Ico_Dmg.png") : Properties.Resources.MissingTexture;
			Reload_bmp = (File.Exists("Textures/GUI/Ico_Rld.png")) ? new Bitmap("Textures/GUI/Ico_Rld.png") : Properties.Resources.MissingTexture;
			Coin_bmp = (File.Exists("Textures/GUI/End_Coin.png")) ? new Bitmap("Textures/GUI/End_Coin.png") : Properties.Resources.MissingTexture;
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_1.png")) ? new Bitmap("Textures/Credits/Credits_1.png") : Properties.Resources.MissingTexture);
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_2.png")) ? new Bitmap("Textures/Credits/Credits_2.png") : Properties.Resources.MissingTexture);
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_3.png")) ? new Bitmap("Textures/Credits/Credits_3.png") : Properties.Resources.MissingTexture);
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_4.png")) ? new Bitmap("Textures/Credits/Credits_4.png") : Properties.Resources.MissingTexture);
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_5.png")) ? new Bitmap("Textures/Credits/Credits_5.png") : Properties.Resources.MissingTexture);
			Credits_bmps.Add((File.Exists("Textures/Credits/Credits_6.png")) ? new Bitmap("Textures/Credits/Credits_6.png") : Properties.Resources.MissingTexture);
			Logo_bmp = (File.Exists("Textures/Intro/Logo_KostylGames.png")) ? new Bitmap("Textures/Intro/Logo_KostylGames.png") : Properties.Resources.MissingTexture;
			Crutch_bmp = (File.Exists("Textures/Intro/Crutch.png")) ? new Bitmap("Textures/Intro/Crutch.png") : Properties.Resources.MissingTexture;
			Bicycle_bmp = (File.Exists("Textures/Intro/Bicycle.png")) ? new Bitmap("Textures/Intro/Bicycle.png") : Properties.Resources.MissingTexture;

			Stickman_bmps.Add((File.Exists("Textures/Enemies/Stickman_1.png")) ? new Bitmap("Textures/Enemies/Stickman_1.png") : Properties.Resources.MissingTexture);
			Stickman_bmps.Add((File.Exists("Textures/Enemies/Stickman_2.png")) ? new Bitmap("Textures/Enemies/Stickman_2.png") : Properties.Resources.MissingTexture);
			Soldier_bmps.Add((File.Exists("Textures/Enemies/Soldier_1.png")) ? new Bitmap("Textures/Enemies/Soldier_1.png") : Properties.Resources.MissingTexture);
			Soldier_bmps.Add((File.Exists("Textures/Enemies/Soldier_2.png")) ? new Bitmap("Textures/Enemies/Soldier_2.png") : Properties.Resources.MissingTexture);
			Dragon_bmps.Add((File.Exists("Textures/Enemies/Dragon_1.png")) ? new Bitmap("Textures/Enemies/Dragon_1.png") : Properties.Resources.MissingTexture);
			Dragon_bmps.Add((File.Exists("Textures/Enemies/Dragon_2.png")) ? new Bitmap("Textures/Enemies/Dragon_2.png") : Properties.Resources.MissingTexture);
			HelpEnemyList.Add(new Stickman("of help", new Rectangle(30, 80, 64, 64), Stickman_bmps));
			HelpEnemyList.Add(new Soldier("of help", new Rectangle(30, 220, 64, 64), Soldier_bmps));
			HelpEnemyList.Add(new Stickman("of help" + "*", new Rectangle(45, 395, 40, 40), new List<Bitmap> { Stickman_bmps[0], Properties.Resources.MissingTexture }, 25, 0, 100.0, 50, 50));
			HelpEnemyList.Add(new Dragon("of help", new Rectangle(12, 518, 100, 100), Dragon_bmps));

			Archer_bmp = (File.Exists("Textures/Towers/Twr_Arch.png")) ? new Bitmap("Textures/Towers/Twr_Arch.png") : Properties.Resources.MissingTexture;
			ArcherShot_bmp = (File.Exists("Textures/Towers/Pew_Arch.png")) ? new Bitmap("Textures/Towers/Pew_Arch.png") : Properties.Resources.MissingTexture;
			Mushroom_bmp = (File.Exists("Textures/Towers/Twr_Mush.png")) ? new Bitmap("Textures/Towers/Twr_Mush.png") : Properties.Resources.MissingTexture;
			MushroomShot_bmp = (File.Exists("Textures/Towers/Pew_Mush.png")) ? new Bitmap("Textures/Towers/Pew_Mush.png") : Properties.Resources.MissingTexture;
			Mage_bmp = (File.Exists("Textures/Towers/Twr_Mage.png")) ? new Bitmap("Textures/Towers/Twr_Mage.png") : Properties.Resources.MissingTexture;
			MageShot_bmp = (File.Exists("Textures/Towers/Pew_Mage.png")) ? new Bitmap("Textures/Towers/Pew_Mage.png") : Properties.Resources.MissingTexture;
			Eye_bmp = (File.Exists("Textures/Towers/Twr_Eye.png")) ? new Bitmap("Textures/Towers/Twr_Eye.png") : Properties.Resources.MissingTexture;
			EyeShot_bmp = (File.Exists("Textures/Towers/Pew_Eye.png")) ? new Bitmap("Textures/Towers/Pew_Eye.png") : Properties.Resources.MissingTexture;
			HelpTowerList.Add(new Archer(new Point(53, 150), Archer_bmp, ArcherShot_bmp));
			HelpTowerList.Add(new Mushroom(new Point(55, 270), Mushroom_bmp, MushroomShot_bmp));
			HelpTowerList.Add(new Mage(new Point(54, 400), Mage_bmp, MageShot_bmp));
			HelpTowerList.Add(new Eye(new Point(56, 550), Eye_bmp, EyeShot_bmp));

			NewGame_btn = new CustomButton("NewGame_btn", new Rectangle(465, 15, 150, 50), (File.Exists("Textures/GUI/Btn_NewG.png")) ? new Bitmap("Textures/GUI/Btn_NewG.png") : Properties.Resources.MissingTexture);
			Leaderboard_btn = new CustomButton("Leaderboard_btn", new Rectangle(465, 80, 150, 50), (File.Exists("Textures/GUI/Btn_Lead.png")) ? new Bitmap("Textures/GUI/Btn_Lead.png") : Properties.Resources.MissingTexture);
			Help_btn = new CustomButton("Help_btn", new Rectangle(465, 145, 150, 50), (File.Exists("Textures/GUI/Btn_Help.png")) ? new Bitmap("Textures/GUI/Btn_Help.png") : Properties.Resources.MissingTexture);
			Credits_btn = new CustomButton("Credits_btn", new Rectangle(465, 210, 150, 50), (File.Exists("Textures/GUI/Btn_Cred.png")) ? new Bitmap("Textures/GUI/Btn_Cred.png") : Properties.Resources.MissingTexture);
			Exit_btn = new CustomButton("Exit_btn", new Rectangle(465, 275, 150, 50), (File.Exists("Textures/GUI/Btn_Exit.png")) ? new Bitmap("Textures/GUI/Btn_Exit.png") : Properties.Resources.MissingTexture);
			Back_btn = new CustomButton("Back_btn", new Rectangle(635, 600, 50, 25), (File.Exists("Textures/GUI/Btn_Back.png")) ? new Bitmap("Textures/GUI/Btn_Back.png") : Properties.Resources.MissingTexture);
			Pause_btn = new CustomButton("Pause_btn", new Rectangle(635, 600, 50, 25), (File.Exists("Textures/GUI/Btn_Twix.png")) ? new Bitmap("Textures/GUI/Btn_Twix.png") : Properties.Resources.MissingTexture);
			NextWave_btn = new CustomButton("NextWave_btn", new Rectangle(310, 503, 50, 50), (File.Exists("Textures/GUI/Btn_NxtW.png")) ? new Bitmap("Textures/GUI/Btn_NxtW.png") : Properties.Resources.MissingTexture);
			Archer_btn = new CustomButton("Archer_btn", new Rectangle(gameZone + (Screen_PB.Width - gameZone - Archer_bmp.Width) / 2, 165, Archer_bmp.Width, Archer_bmp.Height), (File.Exists("Textures/Towers/Twr_Arch.png")) ? new Bitmap("Textures/Towers/Twr_Arch.png") : Properties.Resources.MissingTexture);                                              //Лень считать координаты? Забей. Программа сама посчитает.
			Mushroom_btn = new CustomButton("Mushroom_btn", new Rectangle(gameZone + (Screen_PB.Width - gameZone - Mushroom_bmp.Width) / 2, 190 + Archer_bmp.Height, Mushroom_bmp.Width, Mushroom_bmp.Height), (File.Exists("Textures/Towers/Twr_Mush.png")) ? new Bitmap("Textures/Towers/Twr_Mush.png") : Properties.Resources.MissingTexture);
			Mage_btn = new CustomButton("Mage_btn", new Rectangle(gameZone + (Screen_PB.Width - gameZone - Mage_bmp.Width) / 2, 194 + Archer_bmp.Height + Mushroom_bmp.Height, Mage_bmp.Width, Mage_bmp.Height), (File.Exists("Textures/Towers/Twr_Mage.png")) ? new Bitmap("Textures/Towers/Twr_Mage.png") : Properties.Resources.MissingTexture);
			Eye_btn = new CustomButton("Eye_btn", new Rectangle(gameZone + (Screen_PB.Width - gameZone - Eye_bmp.Width) / 2, 216 + Archer_bmp.Height + Mushroom_bmp.Height + Mage_bmp.Height, Eye_bmp.Width, Eye_bmp.Height), (File.Exists("Textures/Towers/Twr_Eye.png")) ? new Bitmap("Textures/Towers/Twr_Eye.png") : Properties.Resources.MissingTexture);
			Sell_btn = new CustomButton("Sell_btn", new Rectangle(321, 567, 25, 25), MakeOtherARGB((File.Exists("Textures/MissingTexture.png")) ? new Bitmap("Textures/GUI/Alphabet/_Dollar Sign.png") : Properties.Resources.MissingTexture, Color.Gold));
			PrevLetter_1_btn = new CustomButton("PrevLetter_1_btn", new Rectangle(75, 60, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Less-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Less-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			PrevLetter_2_btn = new CustomButton("PrevLetter_2_btn", new Rectangle(265, 60, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Less-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Less-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			PrevLetter_3_btn = new CustomButton("PrevLetter_3_btn", new Rectangle(455, 60, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Less-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Less-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			NextLetter_1_btn = new CustomButton("NextLetter_1_btn", new Rectangle(75, 435, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Greater-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Greater-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			NextLetter_2_btn = new CustomButton("NextLetter_2_btn", new Rectangle(265, 435, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Greater-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Greater-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			NextLetter_3_btn = new CustomButton("NextLetter_3_btn", new Rectangle(455, 435, 100, 100), MakeOtherARGB((File.Exists("Textures/GUI/Alphabet/_Greater-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Greater-Than Sign.png") : Properties.Resources.MissingTexture, Color.LightSlateGray));
			PrevLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			PrevLetter_2_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			PrevLetter_3_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			NextLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			NextLetter_2_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			NextLetter_3_btn.Texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
			BtnsList.Add(NewGame_btn);
			BtnsList.Add(Leaderboard_btn);
			BtnsList.Add(Help_btn);
			BtnsList.Add(Credits_btn);
			BtnsList.Add(Exit_btn);
			BtnsList.Add(Back_btn);
			BtnsList.Add(Pause_btn);
			BtnsList.Add(NextWave_btn);
			BtnsList.Add(Archer_btn);
			BtnsList.Add(Mushroom_btn);
			BtnsList.Add(Mage_btn);
			BtnsList.Add(Eye_btn);
			BtnsList.Add(Sell_btn);
			BtnsList.Add(PrevLetter_1_btn);
			BtnsList.Add(PrevLetter_2_btn);
			BtnsList.Add(PrevLetter_3_btn);
			BtnsList.Add(NextLetter_1_btn);
			BtnsList.Add(NextLetter_2_btn);
			BtnsList.Add(NextLetter_3_btn);

			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Space.png")) ? new Bitmap("Textures/GUI/Alphabet/_Space.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Exclamation Mark.png")) ? new Bitmap("Textures/GUI/Alphabet/_Exclamation Mark.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Quotation Mark.png")) ? new Bitmap("Textures/GUI/Alphabet/_Quotation Mark.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Number Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Number Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Dollar Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Dollar Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Percent Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Percent Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Ampersand.png")) ? new Bitmap("Textures/GUI/Alphabet/_Ampersand.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Apostrophe.png")) ? new Bitmap("Textures/GUI/Alphabet/_Apostrophe.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Left Parenthesis.png")) ? new Bitmap("Textures/GUI/Alphabet/_Left Parenthesis.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Right Parenthesis.png")) ? new Bitmap("Textures/GUI/Alphabet/_Right Parenthesis.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Asterisk.png")) ? new Bitmap("Textures/GUI/Alphabet/_Asterisk.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Plus Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Plus Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Comma.png")) ? new Bitmap("Textures/GUI/Alphabet/_Comma.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Hyphen-Minus.png")) ? new Bitmap("Textures/GUI/Alphabet/_Hyphen-Minus.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Full Stop.png")) ? new Bitmap("Textures/GUI/Alphabet/_Full Stop.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Solidus.png")) ? new Bitmap("Textures/GUI/Alphabet/_Solidus.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/0.png")) ? new Bitmap("Textures/GUI/Alphabet/0.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/1.png")) ? new Bitmap("Textures/GUI/Alphabet/1.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/2.png")) ? new Bitmap("Textures/GUI/Alphabet/2.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/3.png")) ? new Bitmap("Textures/GUI/Alphabet/3.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/4.png")) ? new Bitmap("Textures/GUI/Alphabet/4.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/5.png")) ? new Bitmap("Textures/GUI/Alphabet/5.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/6.png")) ? new Bitmap("Textures/GUI/Alphabet/6.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/7.png")) ? new Bitmap("Textures/GUI/Alphabet/7.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/8.png")) ? new Bitmap("Textures/GUI/Alphabet/8.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/9.png")) ? new Bitmap("Textures/GUI/Alphabet/9.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Colon.png")) ? new Bitmap("Textures/GUI/Alphabet/_Colon.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Semicolon.png")) ? new Bitmap("Textures/GUI/Alphabet/_Semicolon.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Less-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Less-Than Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Equals Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Equals Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Greater-Than Sign.png")) ? new Bitmap("Textures/GUI/Alphabet/_Greater-Than Sign.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Question Mark.png")) ? new Bitmap("Textures/GUI/Alphabet/_Question Mark.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Commercial At.png")) ? new Bitmap("Textures/GUI/Alphabet/_Commercial At.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital A.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital A.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital B.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital B.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital C.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital C.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital D.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital D.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital E.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital E.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital F.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital F.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital G.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital G.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital H.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital H.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital I.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital I.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital J.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital J.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital K.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital K.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital L.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital L.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital M.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital M.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital N.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital N.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital O.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital O.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital P.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital P.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital Q.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital Q.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital R.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital R.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital S.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital S.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital T.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital T.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital U.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital U.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital V.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital V.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital W.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital W.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital X.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital X.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital Y.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital Y.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Capital Z.png")) ? new Bitmap("Textures/GUI/Alphabet/Capital Z.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Left Square Bracket.png")) ? new Bitmap("Textures/GUI/Alphabet/_Left Square Bracket.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Reverse Solidus.png")) ? new Bitmap("Textures/GUI/Alphabet/_Reverse Solidus.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Right Square Bracket.png")) ? new Bitmap("Textures/GUI/Alphabet/_Right Square Bracket.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Circumflex Accent.png")) ? new Bitmap("Textures/GUI/Alphabet/_Circumflex Accent.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Low Line.png")) ? new Bitmap("Textures/GUI/Alphabet/_Low Line.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Grave Accent.png")) ? new Bitmap("Textures/GUI/Alphabet/_Grave Accent.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small A.png")) ? new Bitmap("Textures/GUI/Alphabet/Small A.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small B.png")) ? new Bitmap("Textures/GUI/Alphabet/Small B.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small C.png")) ? new Bitmap("Textures/GUI/Alphabet/Small C.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small D.png")) ? new Bitmap("Textures/GUI/Alphabet/Small D.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small E.png")) ? new Bitmap("Textures/GUI/Alphabet/Small E.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small F.png")) ? new Bitmap("Textures/GUI/Alphabet/Small F.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small G.png")) ? new Bitmap("Textures/GUI/Alphabet/Small G.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small H.png")) ? new Bitmap("Textures/GUI/Alphabet/Small H.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small I.png")) ? new Bitmap("Textures/GUI/Alphabet/Small I.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small J.png")) ? new Bitmap("Textures/GUI/Alphabet/Small J.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small K.png")) ? new Bitmap("Textures/GUI/Alphabet/Small K.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small L.png")) ? new Bitmap("Textures/GUI/Alphabet/Small L.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small M.png")) ? new Bitmap("Textures/GUI/Alphabet/Small M.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small N.png")) ? new Bitmap("Textures/GUI/Alphabet/Small N.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small O.png")) ? new Bitmap("Textures/GUI/Alphabet/Small O.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small P.png")) ? new Bitmap("Textures/GUI/Alphabet/Small P.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small Q.png")) ? new Bitmap("Textures/GUI/Alphabet/Small Q.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small R.png")) ? new Bitmap("Textures/GUI/Alphabet/Small R.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small S.png")) ? new Bitmap("Textures/GUI/Alphabet/Small S.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small T.png")) ? new Bitmap("Textures/GUI/Alphabet/Small T.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small U.png")) ? new Bitmap("Textures/GUI/Alphabet/Small U.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small V.png")) ? new Bitmap("Textures/GUI/Alphabet/Small V.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small W.png")) ? new Bitmap("Textures/GUI/Alphabet/Small W.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small X.png")) ? new Bitmap("Textures/GUI/Alphabet/Small X.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small Y.png")) ? new Bitmap("Textures/GUI/Alphabet/Small Y.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/Small Z.png")) ? new Bitmap("Textures/GUI/Alphabet/Small Z.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Left Curly Bracket.png")) ? new Bitmap("Textures/GUI/Alphabet/_Left Curly Bracket.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Vertical Line.png")) ? new Bitmap("Textures/GUI/Alphabet/_Vertical Line.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Right Curly Bracket.png")) ? new Bitmap("Textures/GUI/Alphabet/_Right Curly Bracket.png") : Properties.Resources.MissingTexture);
			Alphabet.Add((File.Exists("Textures/GUI/Alphabet/_Tilde.png")) ? new Bitmap("Textures/GUI/Alphabet/_Tilde.png") : Properties.Resources.MissingTexture);
			Alphabet.Add(Properties.Resources.MissingTexture);

			SetScreenTo("Intro");
		}

		private void SaveRecords() //сохраняет рекорды в файл
		{
			string toFile = "";
			for (int i = 0; i < Records.GetLength(0); i++)
			{
				string line = Records[i, 0] + "№" + Records[i, 1];
				toFile += line + "\r\n";
			}
			File.WriteAllText("records.td", toFile);
		}

		private void LoadRecords() //загружает рекорды из файла
		{
			if (File.Exists("records.td"))
			{
				var lines = File.ReadAllLines("records.td").ToList();
				for (int i = 0; i < lines.Count; i++)
				{
					var arr = lines[i].Split('№');
					Records[i, 0] = arr[0];
					Records[i, 1] = arr[1];
				}
			}
		}

		private void SetScreenTo(string cond) //изменяет состояние экрана
		{
			switch (cond)
			{
				case "Intro":
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = false;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = false;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = false;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					introTime = 0;
					Crutch_pt = new Point(387, -96);
					Bicycle_pt = new Point(-108, 327);
					Screen_PB.BackgroundImage = null;
					Screen_PB.BackColor = Color.White;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Stop();
					Intro_timer.Start();
					Help_timer.Stop();
					break;
				case "Main menu":
					Leaderboard_btn.ChangePos(465, 80);
					Help_btn.ChangePos(465, 145);
					Exit_btn.ChangePos(465, 275);
					NewGame_btn.Visible = true;
					Leaderboard_btn.Visible = true;
					Help_btn.Visible = true;
					Credits_btn.Visible = true;
					Exit_btn.Visible = true;
					Back_btn.Visible = false;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = false;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					GlobalDeffeat = false;
					Screen_PB.BackgroundImage = MainBackground_bmp;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Stop();
					Intro_timer.Stop();
					Help_timer.Stop();
					break;
				case "Enter your name":
					Leaderboard_btn.ChangePos(240, 550);
					PrevLetter_1_btn.Shape = new Rectangle(75, 60, 100, 100);
					NextLetter_1_btn.Shape = new Rectangle(75, 435, 100, 100);
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = true;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = false;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = true;
					PrevLetter_2_btn.Visible = true;
					PrevLetter_3_btn.Visible = true;
					NextLetter_1_btn.Visible = true;
					NextLetter_2_btn.Visible = true;
					NextLetter_3_btn.Visible = true;
					yourName = "AAA";
					Screen_PB.BackgroundImage = EnterYourName_bmp;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Stop();
					Intro_timer.Stop();
					Help_timer.Stop();
					break;
				case "Leaderboard":
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = false;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = true;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = false;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					Screen_PB.BackgroundImage = Leaderboard_bmp;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Stop();
					Intro_timer.Stop();
					Help_timer.Stop();
					break;
				case "Help":
					PrevLetter_1_btn.Shape = new Rectangle(10, 10, 50, 50);
					NextLetter_1_btn.Shape = new Rectangle(gameZone - 60, 10, 50, 50);
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = false;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = true;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = true;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					helpPage = 1;
					Screen_PB.BackgroundImage = Help_bmps[0];
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Stop();
					Intro_timer.Stop();
					Help_timer.Start();
					break;
				case "Credits":
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = false;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = true;
					Pause_btn.Visible = false;
					NextWave_btn.Visible = false;
					Archer_btn.Visible = false;
					Mushroom_btn.Visible = false;
					Mage_btn.Visible = false;
					Eye_btn.Visible = false;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = false;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					creditsPage = 1;
					creditsTime = 0;
					Screen_PB.BackgroundImage = null;
					Screen_PB.BackColor = Color.Black;
					Credits_timer.Interval = 42;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Stop();
					Credits_timer.Start();
					Intro_timer.Stop();
					Help_timer.Stop();
					break;
				case "Game":
					Help_btn.ChangePos(240, 252);
					Exit_btn.ChangePos(240, 328);
					PrevLetter_1_btn.Shape = new Rectangle(10, 10, 50, 50);
					NextLetter_1_btn.Shape = new Rectangle(gameZone - 60, 10, 50, 50);
					NewGame_btn.Visible = false;
					Leaderboard_btn.Visible = false;
					Help_btn.Visible = false;
					Credits_btn.Visible = false;
					Exit_btn.Visible = false;
					Back_btn.Visible = false;
					Pause_btn.Visible = true;
					NextWave_btn.Visible = true;
					Archer_btn.Visible = true;
					Mushroom_btn.Visible = true;
					Mage_btn.Visible = true;
					Eye_btn.Visible = true;
					Sell_btn.Visible = false;
					PrevLetter_1_btn.Visible = false;
					PrevLetter_2_btn.Visible = false;
					PrevLetter_3_btn.Visible = false;
					NextLetter_1_btn.Visible = false;
					NextLetter_2_btn.Visible = false;
					NextLetter_3_btn.Visible = false;
					Screen_PB.BackgroundImage = null;
					Screen_PB.BackColor = Color.ForestGreen;
					selTwrID = -1;
					helpPage = 1;
					secsRemaining = 10;
					coinTime = 0;
					GlobalWave = -1;
					GlobalScore = 0;
					GlobalLives = 10;
					GlobalGold = 30;
					GlobalVictory = false;
					GlobalDeffeat = false;
					GlobalPause = false;
					ESTpaused = false;
					needHelp = false;
					twrIsHeld = false;
					wrongPlace = false;
					twrForInfo = null;
					yourName = null;
					Path.Clear();
					CurrentEnemyList.Clear();
					CurrentTowerList.Clear();
					WavesList.Clear();
					GeneratePath();
					GenerateWaves();
					EnemySpawn_timer.Interval = 1000;
					GameAnimation_timer.Interval = 42;
					EnemySpawn_timer.Stop();
					GameAnimation_timer.Start();
					Credits_timer.Stop();
					Intro_timer.Stop();
					Help_timer.Stop();
					break;
				default:
					Screen_PB.BackgroundImage = Properties.Resources.MissingTexture;
					break;
			}
			screenCondition = cond;
			Screen_PB.Refresh();
		}

		private double Cos3Points(Point p, Point q, Point r) //возвращает косинус угла между векторами QP и QR 
		{
			return ((p.X - q.X) * (r.X - q.X) + (p.Y - q.Y) * (r.Y - q.Y)) / Math.Sqrt((p.X - q.X) * (p.X - q.X) + (p.Y - q.Y) * (p.Y - q.Y)) / Math.Sqrt((r.X - q.X) * (r.X - q.X) + (r.Y - q.Y) * (r.Y - q.Y));
		}

		private double MinDistanceNormal(int x, int y) //возвращает расстояние до ближайшего отрезка пути
		{
			int A, B, C, x1, y1, x2, y2;
			double distMin = Double.MaxValue, dist, norm, cosA, cosB, p, c1, c2;
			for (int i = 0; i < Path.Count - 1; i++)
			{
				x1 = Path[i].X; //(x1;y1) и (x2;y2) -- точки, задающие прямую (отрезок пути)
				y1 = Path[i].Y;
				x2 = Path[i + 1].X;
				y2 = Path[i + 1].Y;

				A = y2 - y1; //A, B и C -- коэффициенты общего уравнения прямой, заданной точками (x1;y1) и (x2;y2)
				B = x1 - x2;
				C = -x1 * A - y1 * B;

				norm = 1 / Math.Sqrt(A * A + B * B); //нормирующий множитель для приведения общего уравнения прямой к нормальному виду
				if (C > 0)
					norm = -norm;
				cosA = A * norm; //cosA, cosB и p -- коэффициенты нормального уравнения прямой, заданной точками (x1;y1) и (x2;y2)
				cosB = B * norm;
				p = C * norm;

				c1 = Cos3Points(Path[i + 1], Path[i], new Point(x, y));
				c2 = Cos3Points(Path[i], Path[i + 1], new Point(x, y));
				if (c1 < 0.0)
				{
					dist = Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1)); //расстояние от точки (x;y) до точки Path[i]
				}
				else
				{
					if (c2 < 0.0)
					{
						dist = Math.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2)); //расстояние от точки (x;y) до точки Path[i + 1]
					}
					else
					{
						dist = Math.Abs(cosA * x + cosB * y + p); //расстояние от точки (x;y) до отрезка пути
					}
				}
				if (dist < distMin)
					distMin = dist;
			}
			return distMin;
		}

		private void GeneratePath() //генерирует путь. Неожиданно, да?
		{
			int x = 0, y = 0, x1, y1, x2 = 0, y2 = 0; //(x;y) -- текущая точка, (x1;y1) -- предыдущая точка пути, (x2;y2) -- предпредыдущая точка пути
			int side_start = rand.Next(1, 4 + 1), side_finish = 0;
			int steps = 9;
			double c;

			switch (side_start)
			{
				case 1:
					side_finish = 3;
					y = 0;
					x = rand.Next(roadWidth, gameZone - roadWidth);
					NextWave_btn.ChangePos(x - NextWave_btn.Shape.Width / 2, y);
					break;
				case 2:
					side_finish = 4;
					x = gameZone - 1;
					y = rand.Next(roadWidth, gameZone - roadWidth);
					NextWave_btn.ChangePos(x - NextWave_btn.Shape.Width, y - NextWave_btn.Shape.Height / 2);
					break;
				case 3:
					side_finish = 1;
					y = gameZone - 1;
					x = rand.Next(roadWidth, gameZone - roadWidth);
					NextWave_btn.ChangePos(x - NextWave_btn.Shape.Width / 2, y - NextWave_btn.Shape.Height);
					break;
				case 4:
					side_finish = 2;
					x = 0;
					y = rand.Next(roadWidth, gameZone - roadWidth);
					NextWave_btn.ChangePos(x, y - NextWave_btn.Shape.Height / 2);
					break;
			}
			Path.Add(new Point(x, y));
			x1 = x;
			y1 = y;

			int k = 0;
			for (int i = 1; i < steps; i++)
			{
				x = rand.Next(roadWidth, gameZone - roadWidth);
				y = rand.Next(roadWidth, gameZone - roadWidth);
				c = Cos3Points(new Point(x2, y2), new Point(x1, y1), new Point(x, y));

				if ((Path.Count == 1) || (c < 0.0) && (MinDistanceNormal(x, y) > 2.0 * roadWidth))
				{
					Path.Add(new Point(x, y));
					x2 = x1;
					y2 = y1;
					x1 = x;
					y1 = y;
				}
				else
				{
					i--;
				}

				if (k++ > steps * steps) //обнуление прогресса генерации при слишком долгом её времени
				{
					k = 0;
					i = 0;
					Point tp = Path[0];
					Path.Clear();
					Path.Add(tp);
				}
			}

			double cLim = 0.0; //верхний предел для косинуса угла между последним и предпоследним отрезками пути
			do
			{
				switch (side_finish)
				{
					case 1:
						y = 0;
						x = rand.Next(roadWidth, gameZone - roadWidth);
						break;
					case 2:
						x = gameZone - 1;
						y = rand.Next(roadWidth, gameZone - roadWidth);
						break;
					case 3:
						y = gameZone - 1;
						x = rand.Next(roadWidth, gameZone - roadWidth);
						break;
					case 4:
						x = 0;
						y = rand.Next(roadWidth, gameZone - roadWidth);
						break;
				}
				c = Cos3Points(new Point(x2, y2), new Point(x1, y1), new Point(x, y));
				cLim += 0.0078125;
			} while ((cLim < 1.0) && !((c < cLim) && (MinDistanceNormal(x, y) > 2.0 * roadWidth)));
			Path.Add(new Point(x, y));
		}

		private bool IsAngleRight(Point p, Point q, Point r) //определяет, образуют ли вектора PQ и QR вращение против часовой стрелки (поворот дороги налево)
		{
			double c = Cos3Points(new Point(gameZone + 1, p.Y), p, q), s = Math.Sqrt(1.0 - c * c), y, y_1;

			if (q.Y - p.Y < 0)
				s = -s;

			y = -q.X * s + q.Y * c;
			y_1 = -r.X * s + r.Y * c;

			if (y_1 - y < 0.0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void Screen_PB_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			Point[] PathArr = Path.ToArray();

			Color firstColour = Color.SandyBrown;
			Color secondColour = Color.SaddleBrown;
			int frames = 1;
			int dAlpha = 255;

			switch (screenCondition)
			{
				case "Intro":
					frames = Convert.ToInt32(1000.0 / Intro_timer.Interval);
					dAlpha = 255 / frames;
					if (introTime < frames * 2)
					{
						g.DrawImage(Crutch_bmp, Crutch_pt);
						g.DrawImage(Bicycle_bmp, Bicycle_pt);
					}
					else if (introTime < frames * 2 + frames)
					{
						g.DrawImage(Crutch_bmp, Crutch_pt);
						g.DrawImage(MakeOtherARGB(Bicycle_bmp, 255 - (introTime - frames * 2) * dAlpha, 255, 255, 255), Bicycle_pt);
						g.DrawImage(MakeOtherARGB(Logo_bmp, (introTime - frames * 2) * dAlpha, 255, 255, 255), 0, 0);
					}
					else if (introTime < frames * 2 + frames + frames)
					{
						g.DrawImage(Logo_bmp, 0, 0);
					}
					else if (introTime < frames * 2 + frames + frames + frames)
					{
						g.DrawImage(Logo_bmp, 0, 0);
						bru.Color = Color.FromArgb((introTime - frames * 2 - frames - frames) * dAlpha, Screen_PB.BackColor);
						g.FillRectangle(bru, Screen_PB.Bounds);
					}
					break;
				case "Main menu":
					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.HorizontalBrick, Color.DarkSlateGray, Color.DarkGray);
					g.FillRectangle(Hbru, gameZone, 0, Screen_PB.Width - gameZone, gameZone);
					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);
					break;
				case "Enter your name":
					DrawText(g, yourName[0].ToString(), Color.White, 50, 135, 150, 300);
					DrawText(g, yourName[1].ToString(), Color.White, 240, 135, 150, 300);
					DrawText(g, yourName[2].ToString(), Color.White, 430, 135, 150, 300);

					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.HorizontalBrick, Color.DarkSlateGray, Color.DarkGray);
					g.FillRectangle(Hbru, gameZone, 0, Screen_PB.Width - gameZone, gameZone);
					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);
					break;
				case "Leaderboard":
					DrawText(g, Records[0, 0], Color.Gold, 215, 70, 30);
					DrawText(g, Records[0, 1], Color.Gold, 340, 70, 30);
					DrawText(g, Records[1, 0], Color.Silver, 215, 120, 30);
					DrawText(g, Records[1, 1], Color.Silver, 340, 120, 30);
					DrawText(g, Records[2, 0], Color.FromArgb(184, 115, 51), 215, 170, 30);
					DrawText(g, Records[2, 1], Color.FromArgb(184, 115, 51), 340, 170, 30);
					for (int i = 3; i < Records.GetLength(0); i++)
					{
						DrawText(g, Records[i, 0], Color.Black, 215, 70 + 50 * i, 30);
						DrawText(g, Records[i, 1], Color.Black, 340, 70 + 50 * i, 30);
					}
					DrawText(g, Convert.ToString(GlobalScore), Color.White, 222, gameZone - 52, 30);

					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.HorizontalBrick, Color.DarkSlateGray, Color.DarkGray);
					g.FillRectangle(Hbru, gameZone, 0, Screen_PB.Width - gameZone, gameZone);
					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);
					break;
				case "Help":
					Screen_PB.BackgroundImage = Help_bmps[helpPage - 1];

					switch (helpPage)
					{
						case 2:
							DrawInfo(g, twrForInfo, 30, 80);
							break;
						case 3:
							foreach (Tower twr in HelpTowerList)
								DrawTower(g, twr);
							break;
						case 4:
							foreach (Enemy foe in HelpEnemyList)
								DrawEnemy(g, foe);
							break;
					}

					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.HorizontalBrick, Color.DarkSlateGray, Color.DarkGray);
					g.FillRectangle(Hbru, gameZone, 0, Screen_PB.Width - gameZone, gameZone);
					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);
					break;
				case "Credits":
					frames = Convert.ToInt32(1000.0 / Credits_timer.Interval);
					dAlpha = 255 / frames;

					if (creditsPage - 1 < Credits_bmps.Count)
					{
						if (creditsTime < frames)
						{
							g.DrawImage(MakeOtherARGB(Credits_bmps[creditsPage - 1], creditsTime * dAlpha, 255, 255, 255), 0, 0);
						}
						else if (creditsTime < frames + frames * 8)
						{
							g.DrawImage(Credits_bmps[creditsPage - 1], 0, 0);
						}
						else if (creditsTime < frames + frames * 8 + frames)
						{
							g.DrawImage(MakeOtherARGB(Credits_bmps[creditsPage - 1], 255 - (creditsTime - frames - frames * 8) * dAlpha, 255, 255, 255), 0, 0);
						}
					}

					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);
					break;
				case "Game":
					//---Отрисовка фона игрового поля---

					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Divot, Color.LawnGreen, Color.ForestGreen);
					g.FillRectangle(Hbru, 0, 0, gameZone, gameZone);

					//---Отрисовка первого участка пути---

					int lastID = PathArr.Length - 1;
					int x1 = 0, y1 = 0, x = PathArr[0].X, y = PathArr[0].Y, x_1 = PathArr[1].X, y_1 = PathArr[1].Y;
					double sweepAngle, startAngle = 0;

					if (y == 0)
					{
						x1 = x;
						y1 = -1;
						startAngle = 0.0;
					}
					else if (x == gameZone - 1)
					{
						x1 = gameZone;
						y1 = y;
						startAngle = 90.0;
					}
					else if (y == gameZone - 1)
					{
						x1 = x;
						y1 = gameZone;
						startAngle = 180.0;
					}
					else if (x == 0)
					{
						x1 = -1;
						y1 = y;
						startAngle = 270.0;
					}

					sweepAngle = 180.0 - 180.0 / Math.PI * Math.Acos(Cos3Points(new Point(x1, y1), PathArr[0], PathArr[1]));
					if (IsAngleRight(new Point(x1, y1), PathArr[0], PathArr[1]))
					{
						startAngle = startAngle + 180.0 - sweepAngle;
					}
					startAngle -= 10.0;
					sweepAngle += 20.0;
					g.DrawCurve(new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(PathArr[0], PathArr[1], firstColour, secondColour), roadWidth), PathArr, 0, 1, 0.0f);//0.15F);
					bru.Color = firstColour;
					g.FillPie(bru, x - roadWidth / 2, y - roadWidth / 2, roadWidth, roadWidth, Convert.ToSingle(startAngle), Convert.ToSingle(sweepAngle));

					//---Отрисовка остальных участков и секторов пути---

					for (int i = 1; i < lastID; i++)
					{
						x1 = PathArr[i - 1].X;
						y1 = PathArr[i - 1].Y;
						x = PathArr[i].X;
						y = PathArr[i].Y;
						x_1 = PathArr[i + 1].X;
						y_1 = PathArr[i + 1].Y;

						sweepAngle = 180.0 - 180.0 / Math.PI * Math.Acos(Cos3Points(PathArr[i - 1], PathArr[i], PathArr[i + 1]));
						startAngle = 180.0 / Math.PI * Math.Acos(Cos3Points(new Point(gameZone, y1), PathArr[i - 1], PathArr[i])) - 90;
						if (y < y1)
						{
							startAngle = 180.0 - startAngle;
						}
						if (IsAngleRight(PathArr[i - 1], PathArr[i], PathArr[i + 1]))
						{
							startAngle = startAngle + 180.0 - sweepAngle;
						}
						startAngle -= 10.0;
						sweepAngle += 20.0;

						if (i % 2 == 0)
						{
							g.DrawCurve(new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(PathArr[i], PathArr[i + 1], firstColour, secondColour), roadWidth), PathArr, i, 1, 0.0f);//0.15F);
							bru.Color = firstColour;
						}
						else
						{
							g.DrawCurve(new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(PathArr[i], PathArr[i + 1], secondColour, firstColour), roadWidth), PathArr, i, 1, 0.0f);//0.15F);
							bru.Color = secondColour;
						}
						g.FillPie(bru, x - roadWidth / 2, y - roadWidth / 2, roadWidth, roadWidth, Convert.ToSingle(startAngle), Convert.ToSingle(sweepAngle));
					}

					//---Отрисовка последнего сектора пути---

					x1 = PathArr[lastID - 1].X;
					y1 = PathArr[lastID - 1].Y;
					x = PathArr[lastID].X;
					y = PathArr[lastID].Y;
					if (y == 0)
					{
						x_1 = x;
						y_1 = -1;
						startAngle = 0.0;
					}
					else if (x == gameZone - 1)
					{
						x_1 = gameZone;
						y_1 = y;
						startAngle = 90.0;
					}
					else if (y == gameZone - 1)
					{
						x_1 = x;
						y_1 = gameZone;
						startAngle = 180.0;
					}
					else if (x == 0)
					{
						x_1 = -1;
						y_1 = y;
						startAngle = 270.0;
					}

					sweepAngle = 180.0 - 180.0 / Math.PI * Math.Acos(Cos3Points(PathArr[lastID - 1], PathArr[lastID], new Point(x_1, y_1)));
					if (!IsAngleRight(PathArr[lastID - 1], PathArr[lastID], new Point(x_1, y_1)))
					{
						startAngle = startAngle + 180.0 - sweepAngle;
					}
					startAngle -= 10.0;
					sweepAngle += 20.0;
					if (PathArr.Length % 2 != 0)
					{
						bru.Color = firstColour;
					}
					else
					{
						bru.Color = secondColour;
					}
					g.FillPie(bru, x - roadWidth / 2, y - roadWidth / 2, roadWidth, roadWidth, Convert.ToSingle(startAngle), Convert.ToSingle(sweepAngle));

					//pen.Color = Color.Black;
					//pen.Width = 1;
					//g.DrawCurve(pen, PathArr, 0.0F);

					//---Отрисовка противников---

					foreach (Enemy foe in CurrentEnemyList)
					{
						DrawEnemy(g, foe);
					}

					//---Отрисовка башен---

					foreach (Tower twr in CurrentTowerList)
					{
						DrawTower(g, twr);
					}

					//---Отрисовка информации о выделенной башне---

					if ((selTwrID > -1) && (CurrentTowerList.Count > 0))
					{
						Tower selTwr = CurrentTowerList[selTwrID];
						bru.Color = Color.FromArgb(127, 100, 149, 237);
						g.FillEllipse(bru, new Rectangle(selTwr.Location.X - selTwr.Range, selTwr.Location.Y - selTwr.Range, selTwr.Range * 2, selTwr.Range * 2));
						g.DrawEllipse(new Pen(Color.FromArgb(127, 65, 105, 225)), new Rectangle(selTwr.Location.X - selTwr.Range, selTwr.Location.Y - selTwr.Range, selTwr.Range * 2, selTwr.Range * 2));
					}

					//---Отрисовка устанавливаемой башни---

					if (twrIsHeld)
					{
						g.DrawImage(HeldTower_bmp, new Point(mouseLocation.X - hTwr.Texture.Width / 2, mouseLocation.Y - hTwr.Texture.Height + hTwr.Size / 2));
						if (wrongPlace)
							g.DrawEllipse(new Pen(Color.FromArgb(127, 220, 20, 60)), new Rectangle(mouseLocation.X - hTwr.Range, mouseLocation.Y - hTwr.Range, hTwr.Range * 2, hTwr.Range * 2));
						else
							g.DrawEllipse(new Pen(Color.FromArgb(127, 124, 252, 0)), new Rectangle(mouseLocation.X - hTwr.Range, mouseLocation.Y - hTwr.Range, hTwr.Range * 2, hTwr.Range * 2));
					}

					//---Отрисовка интерфейса---

					Hbru = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.HorizontalBrick, Color.DarkSlateGray, Color.DarkGray);
					g.FillRectangle(Hbru, gameZone, 0, Screen_PB.Width - gameZone, gameZone);

					if (GlobalPause && !GlobalDeffeat && !GlobalVictory)
					{
						g.FillRectangle(Hbru, (gameZone - 200) / 2, (gameZone - 175) / 2, 200, 175);
						if (needHelp)
						{
							g.DrawImage(Help_bmps[helpPage - 1], 0, 0, gameZone, gameZone);
							switch (helpPage)
							{
								case 2:
									DrawInfo(g, twrForInfo, 30, 80);
									break;
								case 3:
									foreach (Tower twr in HelpTowerList)
										DrawTower(g, twr);
									break;
								case 4:
									foreach (Enemy foe in HelpEnemyList)
										DrawEnemy(g, foe);
									break;
							}
						}
					}

					foreach (CustomButton btn in BtnsList)
						if (btn.Visible)
							g.DrawImage(btn.Texture, btn.Shape);

					//g.DrawImage(, gameZone, 10, 15, 15);
					if (GlobalScore / 1000 > 0)
						DrawText(g, Convert.ToString(GlobalScore), Color.White, gameZone + 15, 10, 45, 15);
					else
						DrawText(g, Convert.ToString(GlobalScore), Color.White, gameZone + 15, 10, 15);

					g.DrawImage(Lives_bmp, gameZone, 35, 15, 15);
					if (GlobalLives / 1000 > 0)
						DrawText(g, Convert.ToString(GlobalLives), Color.Red, gameZone + 15, 35, 45, 15);
					else
						DrawText(g, Convert.ToString(GlobalLives), Color.Red, gameZone + 15, 35, 15);

					g.DrawImage(Gold_bmp, gameZone, 60, 15, 15);
					if (GlobalGold / 1000 > 0)
						DrawText(g, Convert.ToString(GlobalGold), Color.Gold, gameZone + 15, 60, 45, 15);
					else
						DrawText(g, Convert.ToString(GlobalGold), Color.Gold, gameZone + 15, 60, 15);

					if (GlobalWave + 1 / 10 > 0)
						DrawText(g, Convert.ToString(GlobalWave + 1), Color.White, gameZone, 80, 60, 60);
					else
						DrawText(g, Convert.ToString(GlobalWave + 1), Color.White, gameZone, 80, 60);

					if (!GlobalPause)
					{
						DrawInfo(g, twrForInfo);
					}

					if (GlobalDeffeat)
					{
						bru.Color = Color.FromArgb(63, Color.Black);
						g.FillRectangle(bru, 0, 0, gameZone, gameZone);
						g.DrawImage(Deffeat_bmp, new Point((gameZone - Deffeat_bmp.Width) / 2, (gameZone - Deffeat_bmp.Height) / 2));
						//if (secsRemaining > -1)
						DrawText(g, Convert.ToString(secsRemaining), Color.FromArgb(255 - secsRemaining * 25, secsRemaining * 25, 0), (gameZone - Convert.ToString(secsRemaining).Length * 64) / 2, 495, 64);
						g.DrawImage(Coin_bmp, (gameZone - 50) / 2, 400 - 175 * (float)(Math.Abs(Math.Sin(coinTime * GameAnimation_timer.Interval / 1000.0 * Math.PI / 2))), 60, 60);
					}
					else if (GlobalVictory)
					{
						bru.Color = Color.FromArgb(31, Color.Black);
						g.FillRectangle(bru, 0, 0, gameZone, gameZone);
						g.DrawImage(Victory_bmp, new Point((gameZone - Victory_bmp.Width) / 2, (gameZone - Victory_bmp.Height) / 2));
					}

					break;
			}
		}

		private void DrawEnemy(Graphics g, Enemy foe) //отрисовывает противника
		{
			int texID = foe.AnimationTicks / (250 / (int)foe.Speed);
			if (!needHelp && screenCondition != "Help")
			{
				if (Path[foe.PathNumber].X > Path[foe.PathNumber - 1].X)
					g.DrawImage(foe.TextureList[texID], foe.Shape);
				else
				{
					foe.TextureList[texID].RotateFlip(RotateFlipType.RotateNoneFlipX);
					g.DrawImage(foe.TextureList[texID], foe.Shape);
					foe.TextureList[texID].RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
				g.FillRectangle(Brushes.Red, foe.Shape.X, foe.Shape.Y - foe.Shape.Height / 5, foe.Shape.Width, foe.Shape.Height / 10);
				g.FillRectangle(Brushes.Blue, foe.Shape.X, foe.Shape.Y - foe.Shape.Height / 5, foe.Shape.Width * foe.Health / foe.MaxHealth, foe.Shape.Height / 10);
			}
			else
			{
				g.DrawImage(foe.TextureList[texID], foe.Shape);
			}
		}

		private void DrawTower(Graphics g, Tower twr) //отрисовывает башню
		{
			g.DrawImage(twr.Texture, new Rectangle(twr.Location.X - twr.Texture.Width / 2, twr.Location.Y - twr.Texture.Height + twr.Size / 2, twr.Texture.Width, twr.Texture.Height));
			if (twr.IsShot)
				g.DrawImage(twr.ShotTexture, new Point(twr.Location.X - twr.Texture.Width / 2, twr.Location.Y - twr.Texture.Height + twr.Size / 2));
		}

		private void DrawText(Graphics g, string text, Color col, int x, int y) //отрисовывает строку текста
		{
			if (text == null)
			{
				g.DrawImage(Alphabet[0], x, y);
			}
			else
			{
				int wid = Alphabet[0].Width;
				char ch;
				for (int i = 0; i < text.Length; i++)
				{
					ch = text[i];
					if ((ch - ' ' >= 0) && (ch - ' ' < Alphabet.Count))
					{
						if (col != Color.Black)
							g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], Color.Black), x + wid * i, y - 1, wid + wid / 15, wid + wid / 15);
						g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], col), x + wid * i, y);
					}
					else
					{
						g.DrawImage(MakeOtherARGB(Properties.Resources.MissingTexture, col), x + wid * i, y, wid, wid);
					}
				}
			}
		}

		private void DrawText(Graphics g, string text, Color col, int x, int y, int size) //отрисовывает строку текста заданной высоты
		{
			if (text == null)
			{
				g.DrawImage(Alphabet[0], x, y, size, size);
			}
			else
			{
				int s = 15;
				if (size <= s)
					s /= 2;
				char ch;
				for (int i = 0; i < text.Length; i++)
				{
					ch = text[i];
					if ((ch - ' ' >= 0) && (ch - ' ' < Alphabet.Count))
					{
						if (col != Color.Black)
							g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], Color.Black), x + size * i, y - 1, size + size / s, size + size / s);
						g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], col), x + size * i, y, size, size);
					}
					else
					{
						g.DrawImage(MakeOtherARGB(Properties.Resources.MissingTexture, col), x + size * i, y, size, size);
					}
				}
			}
		}

		private void DrawText(Graphics g, string text, Color col, int x, int y, int width, int height) //отрисовывает строку текста заданной ширины и высоты
		{
			if (text == null)
			{
				g.DrawImage(Alphabet[0], x, y, width, height);
			}
			else
			{
				int sw = 15, sh = 15;
				if (width <= sw)
					sw /= 2;
				if (height <= sh)
					sh /= 2;
				int kw = width / text.Length * Alphabet[0].Width;
				int kh = height / text.Length * Alphabet[0].Height;
				char ch;
				for (int i = 0; i < text.Length; i++)
				{
					ch = text[i];
					if ((ch - ' ' >= 0) && (ch - ' ' < Alphabet.Count))
					{
						if (col != Color.Black)
							g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], Color.Black), x + width / text.Length * i, y - 1, width / text.Length + width / text.Length / sw, height + height / sh);
						g.DrawImage(MakeOtherARGB(Alphabet[ch - ' '], col), x + width / text.Length * i, y, width / text.Length, height);
					}
					else
					{
						g.DrawImage(MakeOtherARGB(Properties.Resources.MissingTexture, col), x + width / text.Length * i, y, width / text.Length, height);
					}
				}
			}
		}

		private void DrawInfo(Graphics g, string twrName) //отрисовывает информацию о башне
		{
			bru.Color = Color.FromArgb(191, Color.DarkSlateGray);
			Tower tTwr;
			switch (twrName)
			{
				case "Archer":
					tTwr = new Archer();
					g.FillRectangle(bru, gameZone - 70, Archer_btn.Shape.Bottom - 85, 70, 85);
					g.DrawImage(Gold_bmp, gameZone - 65, Archer_btn.Shape.Bottom - 80);
					DrawText(g, Convert.ToString(tTwr.Cost), Color.Gold, gameZone - 50, Archer_btn.Shape.Bottom - 80, 15);
					g.DrawImage(Range_bmp, gameZone - 65, Archer_btn.Shape.Bottom - 60);
					DrawText(g, Convert.ToString(tTwr.Range), Color.CornflowerBlue, gameZone - 50, Archer_btn.Shape.Bottom - 60, 15);
					g.DrawImage(Damage_bmp, gameZone - 65, Archer_btn.Shape.Bottom - 40);
					DrawText(g, Convert.ToString(tTwr.Damage), Color.Tomato, gameZone - 50, Archer_btn.Shape.Bottom - 40, 15);
					g.DrawImage(Reload_bmp, gameZone - 65, Archer_btn.Shape.Bottom - 20);
					DrawText(g, Convert.ToString(tTwr.Reload), Color.LimeGreen, gameZone - 50, Archer_btn.Shape.Bottom - 20, 15);
					break;
				case "Mushroom":
					tTwr = new Mushroom();
					g.FillRectangle(bru, gameZone - 70, Mushroom_btn.Shape.Bottom - 85, 70, 85);
					g.DrawImage(Gold_bmp, gameZone - 65, Mushroom_btn.Shape.Bottom - 80);
					DrawText(g, Convert.ToString(tTwr.Cost), Color.Gold, gameZone - 50, Mushroom_btn.Shape.Bottom - 80, 15);
					g.DrawImage(Range_bmp, gameZone - 65, Mushroom_btn.Shape.Bottom - 60);
					DrawText(g, Convert.ToString(tTwr.Range), Color.CornflowerBlue, gameZone - 50, Mushroom_btn.Shape.Bottom - 60, 15);
					g.DrawImage(Damage_bmp, gameZone - 65, Mushroom_btn.Shape.Bottom - 40);
					DrawText(g, Convert.ToString(tTwr.Damage), Color.Tomato, gameZone - 50, Mushroom_btn.Shape.Bottom - 40, 15);
					g.DrawImage(Reload_bmp, gameZone - 65, Mushroom_btn.Shape.Bottom - 20);
					DrawText(g, Convert.ToString(tTwr.Reload), Color.LimeGreen, gameZone - 50, Mushroom_btn.Shape.Bottom - 20, 15);
					break;
				case "Mage":
					tTwr = new Mage();
					g.FillRectangle(bru, gameZone - 70, Mage_btn.Shape.Bottom - 85, 70, 85);
					g.DrawImage(Gold_bmp, gameZone - 65, Mage_btn.Shape.Bottom - 80);
					DrawText(g, Convert.ToString(tTwr.Cost), Color.Gold, gameZone - 50, Mage_btn.Shape.Bottom - 80, 15);
					g.DrawImage(Range_bmp, gameZone - 65, Mage_btn.Shape.Bottom - 60);
					DrawText(g, Convert.ToString(tTwr.Range), Color.CornflowerBlue, gameZone - 50, Mage_btn.Shape.Bottom - 60, 15);
					g.DrawImage(Damage_bmp, gameZone - 65, Mage_btn.Shape.Bottom - 40);
					DrawText(g, Convert.ToString(tTwr.Damage), Color.Tomato, gameZone - 50, Mage_btn.Shape.Bottom - 40, 15);
					g.DrawImage(Reload_bmp, gameZone - 65, Mage_btn.Shape.Bottom - 20);
					DrawText(g, Convert.ToString(tTwr.Reload), Color.LimeGreen, gameZone - 50, Mage_btn.Shape.Bottom - 20, 15);
					break;
				case "Eye":
					tTwr = new Eye();
					g.FillRectangle(bru, gameZone - 70, Eye_btn.Shape.Bottom - 85, 70, 85);
					g.DrawImage(Gold_bmp, gameZone - 65, Eye_btn.Shape.Bottom - 80);
					DrawText(g, Convert.ToString(tTwr.Cost), Color.Gold, gameZone - 50, Eye_btn.Shape.Bottom - 80, 15);
					g.DrawImage(Range_bmp, gameZone - 65, Eye_btn.Shape.Bottom - 60);
					DrawText(g, Convert.ToString(tTwr.Range), Color.CornflowerBlue, gameZone - 50, Eye_btn.Shape.Bottom - 60, 15);
					g.DrawImage(Damage_bmp, gameZone - 65, Eye_btn.Shape.Bottom - 40);
					DrawText(g, Convert.ToString(tTwr.Damage), Color.Tomato, gameZone - 50, Eye_btn.Shape.Bottom - 40, 15);
					g.DrawImage(Reload_bmp, gameZone - 65, Eye_btn.Shape.Bottom - 20);
					DrawText(g, Convert.ToString(tTwr.Reload), Color.LimeGreen, gameZone - 50, Eye_btn.Shape.Bottom - 20, 15);
					break;
			}
		}

		private void DrawInfo(Graphics g, string twrName, int x, int y) //отрисовывает информацию о башне по указанным координатам
		{
			bru.Color = Color.FromArgb(191, Color.DarkSlateGray);
			Tower tTwr = new Archer();
			switch (twrName)
			{
				case "Archer":
					tTwr = new Archer();
					break;
				case "Mushroom":
					tTwr = new Mushroom();
					break;
				case "Mage":
					tTwr = new Mage();
					break;
				case "Eye":
					tTwr = new Eye();
					break;
			}
			g.FillRectangle(bru, x, y, 70, 85);
			g.DrawImage(Gold_bmp, x + 5, y + 5);
			DrawText(g, Convert.ToString(tTwr.Cost), Color.Gold, x + 20, y + 5, 15);
			g.DrawImage(Range_bmp, x + 5, y + 25);
			DrawText(g, Convert.ToString(tTwr.Range), Color.CornflowerBlue, x + 20, y + 25, 15);
			g.DrawImage(Damage_bmp, x + 5, y + 45);
			DrawText(g, Convert.ToString(tTwr.Damage), Color.Tomato, x + 20, y + 45, 15);
			g.DrawImage(Reload_bmp, x + 5, y + 65);
			DrawText(g, Convert.ToString(tTwr.Reload), Color.LimeGreen, x + 20, y + 65, 15);
		}

		private bool IsFarFromTowers(Tower cTwr, Point pnt) //определяет, не накладывается ли заданная башня в заданной точке на башни, уже находящиеся на поле
		{
			Tower tTwr = cTwr;
			int x = pnt.X, y = pnt.Y;
			double distMin = Double.MaxValue, dist;
			foreach (Tower twr in CurrentTowerList)
			{
				int tX = twr.Location.X, tY = twr.Location.Y;
				dist = Math.Sqrt((x - tX) * (x - tX) + (y - tY) * (y - tY));
				if (dist < distMin)
				{
					distMin = dist;
					tTwr = twr;
				}
			}
			if (distMin > cTwr.Size / 2 + tTwr.Size / 2)
				return true;
			else
				return false;
		}

		private void Screen_PB_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				switch (screenCondition)
				{
					case "Intro":
						SetScreenTo("Main menu");
						break;
					case "Main menu":
						if (NewGame_btn.IsClicked(e))
						{
							SetScreenTo("Game");
						}
						else if (Leaderboard_btn.IsClicked(e))
						{
							SetScreenTo("Leaderboard");
						}
						else if (Help_btn.IsClicked(e))
						{
							PrevLetter_1_btn.Texture = MakeOtherARGB(PrevLetter_1_btn.Texture, Color.Black);
							PrevLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate270FlipNone);
							NextLetter_1_btn.Texture = MakeOtherARGB(NextLetter_1_btn.Texture, Color.Black);
							NextLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate270FlipNone);
							SetScreenTo("Help");
						}
						else if (Credits_btn.IsClicked(e))
						{
							SetScreenTo("Credits");
						}
						else if (Exit_btn.IsClicked(e))
						{
							this.Close();
						}
						break;
					case "Enter your name":
						if (Leaderboard_btn.IsClicked(e))
						{
							int tpResult = 0;
							int.TryParse(Records[0, 1], out tpResult);
							int i = 0;
							while ((i < Records.GetLength(0)) && (GlobalScore <= tpResult))
							{
								i++;
								int.TryParse(Records[i, 1], out tpResult);
							}
							if (i != Records.GetLength(0) - 1)
							{
								for (int j = Records.GetLength(0) - 1; j > i; j--)
								{
									Records[j, 0] = Records[j - 1, 0];
									Records[j, 1] = Records[j - 1, 1];
								}
							}
							Records[i, 0] = yourName;
							Records[i, 1] = Convert.ToString(GlobalScore);
							SaveRecords();
							SetScreenTo("Leaderboard");
						}
						else if (PrevLetter_1_btn.IsClicked(e))
						{
							char ch = yourName[0];
							if (ch == ' ')
								ch = '~';
							else
								ch--;
							yourName = ch.ToString() + yourName[1] + yourName[2];
							Screen_PB.Refresh();
						}
						else if (PrevLetter_2_btn.IsClicked(e))
						{
							char ch = yourName[1];
							if (ch == ' ')
								ch = '~';
							else
								ch--;
							yourName = yourName[0].ToString() + ch + yourName[2];
							Screen_PB.Refresh();
						}
						else if (PrevLetter_3_btn.IsClicked(e))
						{
							char ch = yourName[2];
							if (ch == ' ')
								ch = '~';
							else
								ch--;
							yourName = yourName[0].ToString() + yourName[1] + ch;
							Screen_PB.Refresh();
						}
						else if (NextLetter_1_btn.IsClicked(e))
						{
							char ch = yourName[0];
							if (ch == '~')
								ch = ' ';
							else
								ch++;
							yourName = ch.ToString() + yourName[1] + yourName[2];
							Screen_PB.Refresh();
						}
						else if (NextLetter_2_btn.IsClicked(e))
						{
							char ch = yourName[1];
							if (ch == '~')
								ch = ' ';
							else
								ch++;
							yourName = yourName[0].ToString() + ch + yourName[2];
							Screen_PB.Refresh();
						}
						else if (NextLetter_3_btn.IsClicked(e))
						{
							char ch = yourName[2];
							if (ch == '~')
								ch = ' ';
							else
								ch++;
							yourName = yourName[0].ToString() + yourName[1] + ch;
							Screen_PB.Refresh();
						}
						break;
					case "Leaderboard":
						if (Back_btn.IsClicked(e))
						{
							SetScreenTo("Main menu");
						}
						break;
					case "Help":
						if (PrevLetter_1_btn.IsClicked(e))
						{
							helpPage--;
							NextLetter_1_btn.Visible = true;
							if (helpPage == 1)
								PrevLetter_1_btn.Visible = false;
							Screen_PB.Refresh();
						}
						else if (NextLetter_1_btn.IsClicked(e))
						{
							helpPage++;
							PrevLetter_1_btn.Visible = true;
							if (helpPage == Help_bmps.Count)
								NextLetter_1_btn.Visible = false;
							Screen_PB.Refresh();
						}
						else if (Back_btn.IsClicked(e))
						{
							PrevLetter_1_btn.Texture = new Bitmap(PrevLetter_2_btn.Texture);
							NextLetter_1_btn.Texture = new Bitmap(NextLetter_2_btn.Texture);
							SetScreenTo("Main menu");
						}
						break;
					case "Credits":
						if (Back_btn.IsClicked(e))
						{
							if (GlobalDeffeat)
								SetScreenTo("Leaderboard");
							else
								SetScreenTo("Main menu");
						}
						break;
					case "Game":
						if (GlobalDeffeat)
						{
							SetScreenTo("Credits");
						}
						else if (GlobalVictory)
						{
							if ((Records[9, 1] == "") || (GlobalScore > Convert.ToInt32(Records[9, 1])))
							{
								SetScreenTo("Enter your name");
							}
							else
							{
								SetScreenTo("Leaderboard");
							}
						}
						else if (Help_btn.IsClicked(e))
						{
							needHelp = true;
							Help_btn.Visible = false;
							Exit_btn.Visible = false;
							NextWave_btn.Visible = false;
							if (helpPage != 1)
								PrevLetter_1_btn.Visible = true;
							PrevLetter_1_btn.Texture = MakeOtherARGB(PrevLetter_1_btn.Texture, Color.Black);
							PrevLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate270FlipNone);
							if (helpPage != Help_bmps.Count)
								NextLetter_1_btn.Visible = true;
							NextLetter_1_btn.Texture = MakeOtherARGB(NextLetter_1_btn.Texture, Color.Black);
							NextLetter_1_btn.Texture.RotateFlip(RotateFlipType.Rotate270FlipNone);
							Help_timer.Start();
							Screen_PB.Refresh();
						}
						else if (PrevLetter_1_btn.IsClicked(e))
						{
							helpPage--;
							NextLetter_1_btn.Visible = true;
							if (helpPage == 1)
								PrevLetter_1_btn.Visible = false;
							Screen_PB.Refresh();
						}
						else if (NextLetter_1_btn.IsClicked(e))
						{
							helpPage++;
							PrevLetter_1_btn.Visible = true;
							if (helpPage == Help_bmps.Count)
								NextLetter_1_btn.Visible = false;
							Screen_PB.Refresh();
						}
						else if (Exit_btn.IsClicked(e))
						{
							SetScreenTo("Main menu");
						}
						else if (Back_btn.IsClicked(e))
						{
							if (needHelp)
							{
								needHelp = false;
								Help_btn.Visible = true;
								Exit_btn.Visible = true;
								NextWave_btn.Visible = true;
								PrevLetter_1_btn.Visible = false;
								PrevLetter_1_btn.Texture = new Bitmap(PrevLetter_2_btn.Texture);
								NextLetter_1_btn.Visible = false;
								NextLetter_1_btn.Texture = new Bitmap(NextLetter_2_btn.Texture);
								Help_timer.Stop();
								Screen_PB.Refresh();
							}
							else
							{
								GlobalPause = false;
								if (ESTpaused)
								{
									EnemySpawn_timer.Start();
									ESTpaused = false;
								}
								GameAnimation_timer.Start();
								Help_btn.Visible = false;
								Exit_btn.Visible = false;
								Back_btn.Visible = false;
								Pause_btn.Visible = true;
								Screen_PB.Refresh();
							}
						}
						else if (!GlobalPause && !GlobalDeffeat && !GlobalVictory)
						{
							if (Pause_btn.IsClicked(e))
							{
								GlobalPause = true;
								if (EnemySpawn_timer.Enabled)
								{
									EnemySpawn_timer.Stop();
									ESTpaused = true;
								}
								GameAnimation_timer.Stop();
								Help_btn.Visible = true;
								Exit_btn.Visible = true;
								Back_btn.Visible = true;
								Pause_btn.Visible = false;
								Screen_PB.Refresh();
							}
							else if (Archer_btn.IsClicked(e))
							{
								hTwr = new Archer(e.Location, Archer_bmp, ArcherShot_bmp);
								twrIsHeld = true;
							}
							else if (Mushroom_btn.IsClicked(e))
							{
								hTwr = new Mushroom(e.Location, Mushroom_bmp, MushroomShot_bmp);
								twrIsHeld = true;
							}
							else if (Mage_btn.IsClicked(e))
							{
								hTwr = new Mage(e.Location, Mage_bmp, MageShot_bmp);
								twrIsHeld = true;
							}
							else if (Eye_btn.IsClicked(e))
							{
								hTwr = new Eye(e.Location, Eye_bmp, EyeShot_bmp);
								twrIsHeld = true;
							}
							else if (twrIsHeld && !wrongPlace)
							{
								if (GlobalGold >= hTwr.Cost)
								{
									GlobalGold -= hTwr.Cost;
									hTwr.Location = e.Location;
									CurrentTowerList.Add(hTwr);
									CurrentTowerList.Sort(delegate (Tower twr1, Tower twr2)
									{
										return twr1.Location.Y.CompareTo(twr2.Location.Y);
									});
									twrIsHeld = false;
									selTwrID = -1;
									Sell_btn.Visible = false;
								}
							}
							else if (NextWave_btn.IsClicked(e))
							{
								NextWave_btn.Visible = false;
								GlobalWave++;
								if (GlobalWave == WavesList.Count)
								{
									int dragonHealth = 64;
									foreach (Tower twr in CurrentTowerList)
									{
										switch (twr.Name)
										{
											case "Archer":
												dragonHealth += 8;
												break;
											case "Mushroom":
												dragonHealth += 4;
												break;
											case "Mage":
												dragonHealth += 16;
												break;
											case "Eye":
												dragonHealth += 32;
												break;
										}
									}
									WavesList.Add(new Wave("Волна №" + Convert.ToString(GlobalWave + 1), new List<Enemy> { new Dragon("1", new Rectangle(Path[0].X - 50 / 2, Path[0].Y - 50 / 2, 50, 50), Dragon_bmps, dragonHealth) }));
								}
								EnemySpawn_timer.Start();
							}
							else if (Sell_btn.IsClicked(e))
							{
								GlobalGold += CurrentTowerList[selTwrID].Cost / 2;
								CurrentTowerList.RemoveAt(selTwrID);
								selTwrID = -1;
								Sell_btn.Visible = false;
							}
							else
							{
								selTwrID = FindSelectedTowerID(e);
								if (selTwrID > -1)
								{
									if (gameZone - CurrentTowerList[selTwrID].Location.Y - CurrentTowerList[selTwrID].Size / 2 > Sell_btn.Shape.Height + 5)
										Sell_btn.ChangePos(CurrentTowerList[selTwrID].Location.X - Sell_btn.Shape.Width / 2, CurrentTowerList[selTwrID].Location.Y + CurrentTowerList[selTwrID].Size / 2 + 5);
									else
										Sell_btn.ChangePos(CurrentTowerList[selTwrID].Location.X - Sell_btn.Shape.Width / 2, CurrentTowerList[selTwrID].Location.Y + CurrentTowerList[selTwrID].Size / 2 - CurrentTowerList[selTwrID].Texture.Height - Sell_btn.Shape.Height - 5);
									Sell_btn.Visible = true;
								}
								else
								{
									Sell_btn.Visible = false;
								}
							}
						}
						break;
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				if (twrIsHeld)
					twrIsHeld = false;
				else
				{
					selTwrID = -1;
					Sell_btn.Visible = false;
				}
			}
		}

		private int FindSelectedTowerID(MouseEventArgs e) //находит номер выделенной башни в списке башен на поле
		{
			int id = -1;
			Tower twr;

			//for (int i = 0; (id < 0) && (i < CurrentTowerList.Count); i++)
			for (int i = CurrentTowerList.Count - 1; (id < 0) && (i >= 0); i--)
			{
				twr = CurrentTowerList[i];
				if ((e.X >= twr.Location.X - twr.Texture.Width / 2) && (e.X < twr.Location.X + twr.Texture.Width / 2) && (e.Y >= twr.Location.Y - twr.Texture.Height + twr.Size / 2) && (e.Y < twr.Location.Y + twr.Size / 2))
					id = i;
			}

			return id;
		}

		private static Bitmap MakeOtherARGB(Bitmap bmOld, int A, int R, int G, int B) //модифицирует значения каналов текстуры к заданным
		{
			//Создаём пустой Bitmap такого же размера, как и старый
			Bitmap bmNew = new Bitmap(bmOld.Width, bmOld.Height);
			//Получаем объект Graphics из нового изображения
			Graphics g = Graphics.FromImage(bmNew);

			//Создаём матрицу цветов
			ColorMatrix colorMx = new ColorMatrix(
				new float[][]
				{
					new float[] {R / 255f, 0, 0, 0, 0},
					new float[] {0, G / 255f, 0, 0, 0},
					new float[] {0, 0, B / 255f, 0, 0},
					new float[] {0, 0, 0, A / 255f, 0},
					new float[] {0, 0, 0, 0, 1f}
				});

			//Создаём атрибуты изображения
			ImageAttributes atts = new ImageAttributes();
			//Устанавливаем для атрибутов изображения нужную матрицу цветов
			atts.SetColorMatrix(colorMx);

			//Рисуем старое изображение на новом, используя новые атрибуты
			g.DrawImage(bmOld, new Rectangle(0, 0, bmOld.Width, bmOld.Height), 0, 0, bmOld.Width, bmOld.Height, GraphicsUnit.Pixel, atts);

			g.Dispose();
			return bmNew;
		}

		private static Bitmap MakeOtherARGB(Bitmap bmOld, Color col) //модифицирует значения каналов текстуры к заданному цвету
		{
			//Создаём пустой Bitmap такого же размера, как и старый
			Bitmap bmNew = new Bitmap(bmOld.Width, bmOld.Height);
			//Получаем объект Graphics из нового изображения
			Graphics g = Graphics.FromImage(bmNew);

			//Создаём матрицу цветов
			ColorMatrix colorMx = new ColorMatrix(
				new float[][]
				{
					new float[] {col.R / 255f, 0, 0, 0, 0},
					new float[] {0, col.G / 255f, 0, 0, 0},
					new float[] {0, 0, col.B / 255f, 0, 0},
					new float[] {0, 0, 0, col.A / 255f, 0},
					new float[] {0, 0, 0, 0, 1f}
				});

			//Создаём атрибуты изображения
			ImageAttributes atts = new ImageAttributes();
			//Устанавливаем для атрибутов изображения нужную матрицу цветов
			atts.SetColorMatrix(colorMx);

			//Рисуем старое изображение на новом, используя новые атрибуты
			g.DrawImage(bmOld, new Rectangle(0, 0, bmOld.Width, bmOld.Height), 0, 0, bmOld.Width, bmOld.Height, GraphicsUnit.Pixel, atts);

			g.Dispose();
			return bmNew;
		}

		private void Screen_PB_MouseMove(object sender, MouseEventArgs e)
		{
			mouseLocation = e.Location;
			if (twrIsHeld)
			{
				double dp = MinDistanceNormal(e.X, e.Y);
				if ((dp > hTwr.Size / 2 + roadWidth / 2) && (IsFarFromTowers(hTwr, e.Location)) && (e.X > hTwr.Size / 2) && (e.Y > hTwr.Size / 2) && (e.X < gameZone - hTwr.Size / 2) && (e.Y < gameZone - hTwr.Size / 2))
				{
					HeldTower_bmp = MakeOtherARGB(hTwr.Texture, 127, 255, 255, 255);
					wrongPlace = false;
				}
				else
				{
					HeldTower_bmp = MakeOtherARGB(hTwr.Texture, 127, 255, 63, 63);
					wrongPlace = true;
				}
			}
			else
			{
				if (Archer_btn.IsClicked(e))
				{
					twrForInfo = "Archer";
				}
				else if (Mushroom_btn.IsClicked(e))
				{
					twrForInfo = "Mushroom";
				}
				else if (Mage_btn.IsClicked(e))
				{
					twrForInfo = "Mage";
				}
				else if (Eye_btn.IsClicked(e))
				{
					twrForInfo = "Eye";
				}
				else
				{
					twrForInfo = null;
				}
			}
		}

		private void GenerateWaves() //формирует список волн и состав каждой волны
		{
			int wavesAmt = 10;
			int border = 90;
			for (int i = 0; i < wavesAmt - 1; i++)
			{
				List<Enemy> foeList = new List<Enemy>();
				int enemiesAmt = rand.Next(5 + i, 10 + 2 * i + 1);
				for (int j = 0; j < enemiesAmt; j++)
				{
					int size = 0;
					int enemyType = rand.Next(0, 100 + 1);
					if ((enemyType >= 1) && (enemyType <= border))
					{
						size = 32;
						foeList.Add(new Stickman(Convert.ToString(j + 1), new Rectangle(Path[0].X - size / 2, Path[0].Y - size / 2, size, size), Stickman_bmps, 5 + i));
					}
					else if ((enemyType > border) && (enemyType <= 100))
					{
						size = 32;
						foeList.Add(new Soldier(Convert.ToString(j + 1), new Rectangle(Path[0].X - size / 2, Path[0].Y - size / 2, size, size), Soldier_bmps, 10 + 2 * i));
					}
					else
					{
						size = 20;
						foeList.Add(new Stickman(Convert.ToString(j + 1) + "*", new Rectangle(Path[0].X - size / 2, Path[0].Y - size / 2, size, size), new List<Bitmap> { Stickman_bmps[0], Properties.Resources.MissingTexture }, 25 + 3 * i, 0, 100.0, 50, 50));
					}
				}
				WavesList.Add(new Wave("Волна №" + Convert.ToString(i + 1), foeList));
				border -= 4;
			}
			//WavesList.Add(new Wave("Волна №" + Convert.ToString(wavesAmt), new List<Enemy> { new Dragon("1", new Rectangle(Path[0].X - 50 / 2, Path[0].Y - 50 / 2, 50, 50), Dragon_bmps) }));
		}

		private void EnemySpawn_timer_Tick(object sender, EventArgs e)
		{
			if (GlobalDeffeat)
			{
				if (secsRemaining > 0)
				{
					secsRemaining--;
				}
			}
			else
			{
				if (GlobalWave < WavesList.Count)
				{
					if (WavesList[GlobalWave].SpawnNumber < WavesList[GlobalWave].EnemySpawnQueue.Count)
					{
						CurrentEnemyList.Add(WavesList[GlobalWave].EnemySpawnQueue[WavesList[GlobalWave].SpawnNumber]);
						WavesList[GlobalWave].SpawnNumber++;
					}
					else
					{
						WavesList[GlobalWave].SpawnNumber = 0;
						EnemySpawn_timer.Stop();
					}
				}
			}
		}

		private void GameAnimation_timer_Tick(object sender, EventArgs e)
		{
			//---Движение противников---

			double d = 0.0, s = 0.0;    //расстояние до следующей точки пути; длина шага противника
			Point A, B;                 //центр модели противника; следующая точка пути
			int nX, nY;                 //абсцисса и ордината точки, в которую должен шагнуть противник
			bool finished;              //маркер достижения противником последней точки пути

			for (int i = 0; i < CurrentEnemyList.Count; i++)
			{
				finished = false;
				Enemy foe = CurrentEnemyList[i];

				B = Path[foe.PathNumber];
				A = Path[foe.PathNumber - 1];
				d = Math.Sqrt((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
				s = foe.Speed * GameAnimation_timer.Interval / 1000.0;
				if (d - s * foe.Steps < s)
				{
					if (foe.PathNumber + 1 < Path.Count)
					{
						foe.Steps = 0;
						foe.PathNumber++;
						B = Path[foe.PathNumber];
						A = Path[foe.PathNumber - 1];
						d = Math.Sqrt((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
					}
					else
					{
						finished = true;
					}
				}

				if (finished)
				{
					if (!GlobalDeffeat)
						foe.Finish(ref GlobalLives);
					CurrentEnemyList.Remove(foe);
					if (!GlobalDeffeat && (GlobalLives <= 0))
					{
						GlobalDeffeat = true;
						GlobalPause = true;
						twrIsHeld = false;
						selTwrID = -1;
						Sell_btn.Visible = false;
						EnemySpawn_timer.Start();
						EnemySpawn_timer.Interval *= 2;

						Enemy tFoe;
						for (int j = 0; j < CurrentEnemyList.Count; j++)
						{
							tFoe = CurrentEnemyList[j];
							tFoe.Speed /= 2.0;
							tFoe.Steps *= 2;
							if (WavesList[GlobalWave].EnemySpawnQueue.Contains(tFoe))
							{
								for (int k = 1; k < WavesList[GlobalWave].EnemySpawnQueue.Count; k++)
								{
									tFoe = WavesList[GlobalWave].EnemySpawnQueue[k];
									tFoe.Speed /= 2.0;
									tFoe.Steps *= 2;
								}
								j = CurrentEnemyList.Count;
							}
						}
						foreach (Tower tTwr in CurrentTowerList)
						{
							tTwr.Reload *= 2.0;
						}
					}
				}
				else
				{
					double dX = A.X + (B.X - A.X) / d * s * foe.Steps, dY = A.Y + (B.Y - A.Y) / d * s * foe.Steps;
					nX = Convert.ToInt32(dX);
					nY = Convert.ToInt32(dY);
					foe.Move(nX, nY);
					foe.Steps++;
					if (foe.AnimationTicks < (250 / (int)foe.Speed) * 2 - 1)
						foe.AnimationTicks++;
					else
						foe.AnimationTicks = 0;
				}
			}

			//---Выстрелы башен---

			foreach (Tower twr in CurrentTowerList)
			{
				if (twr.AnimationTicks < twr.Reload / GameAnimation_timer.Interval * 1000)
				{
					twr.AnimationTicks++;

					int animLim = 8;
					if (GlobalDeffeat)
						animLim *= 2;

					if (twr.AnimationTicks >= animLim)
					{
						twr.IsShot = false;
					}
				}
				else
				{
					if (twr.Shoot(CurrentEnemyList, ref GlobalScore, ref GlobalGold))
					{
						twr.IsShot = true;
						twr.AnimationTicks = 0;
						if ((WavesList[GlobalWave].EnemySpawnQueue[0].GetType().Equals(typeof(Dragon))) && (GlobalWave == WavesList.Count - 1) && (CurrentEnemyList.Count == 0))
						{
							Sell_btn.Visible = false;
							selTwrID = -1;
							GlobalScore += 3 * GlobalLives;
							GlobalVictory = true;
							GlobalPause = true;
							twrIsHeld = false;
						}
					}
				}
			}

			//---Определение возможности вызвать следующую волну---

			if (GlobalWave > -1)
			{
				if ((GlobalWave < WavesList.Count - 1) || !(WavesList[GlobalWave].EnemySpawnQueue[0].GetType().Equals(typeof(Dragon))))
				{
					if (WavesList[GlobalWave].SpawnNumber == WavesList[GlobalWave].EnemySpawnQueue.Count)
					{
						NextWave_btn.Visible = true;
					}
				}
				else
				{
					NextWave_btn.Visible = false;
				}
			}

			//---Подбрасывание монетки---

			if (GlobalDeffeat)
			{
				if (secsRemaining > 0)
				{
					coinTime++;
					if (coinTime % 4 == 0)
					{
						Coin_bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
					}
				}
			}

			Screen_PB.Refresh();
		}

		private void Credits_timer_Tick(object sender, EventArgs e)
		{
			int frames = Convert.ToInt32(1000.0 / Credits_timer.Interval);
			if (creditsTime < frames + frames * 8 + frames)
			{
				creditsTime++;
			}
			else
			{
				creditsTime = 0;
				if (creditsPage - 1 < Credits_bmps.Count)
				{
					creditsPage++;
				}
				else
				{
					SetScreenTo("Main menu");
				}
			}

			Screen_PB.Refresh();
		}

		private void Intro_timer_Tick(object sender, EventArgs e)
		{
			int frames = Convert.ToInt32(1000.0 / Credits_timer.Interval);
			if (introTime < frames)
			{
				if (Bicycle_pt.X < 287)
					Bicycle_pt.X = Convert.ToInt32(-108 + 395 * Math.Sin(introTime * Intro_timer.Interval / 1000.0));
				introTime++;
			}
			else if (introTime < frames * 2)
			{
				if (Crutch_pt.Y < 230)
				{
					Crutch_pt.Y = Convert.ToInt32(-96 + 326 * ((introTime - frames) * Intro_timer.Interval / 1000.0) * (introTime * Intro_timer.Interval / 1000.0));
					if (Crutch_pt.Y > 230)
						Crutch_pt.Y = 230;
				}
				if (Bicycle_pt.X < 287)
					Bicycle_pt.X = Convert.ToInt32(-108 + 395 * Math.Sin(introTime * Intro_timer.Interval / 1000.0));
				introTime++;
			}
			else if (introTime < frames * 3 + frames + frames + frames)
			{
				introTime++;
			}
			else
			{
				SetScreenTo("Main menu");
			}

			Screen_PB.Refresh();
		}

		private void Help_timer_Tick(object sender, EventArgs e)
		{
			int frames = Convert.ToInt32(1000.0 / Credits_timer.Interval);
			switch (helpPage)
			{
				case 2:
					if (helpTime < frames * 3 / 2)
						twrForInfo = "Archer";
					else if (helpTime < frames * 3)
						twrForInfo = "Mushroom";
					else if (helpTime < frames * 9 / 2)
						twrForInfo = "Mage";
					else if (helpTime < frames * 6)
						twrForInfo = "Eye";
					else
						helpTime = -1;
					helpTime++;
					break;
				case 3:
					foreach (Tower twr in HelpTowerList)
					{
						if (twr.AnimationTicks < twr.Reload / GameAnimation_timer.Interval * 1000)
						{
							twr.AnimationTicks++;
							int animLim = 8;
							if (twr.AnimationTicks >= animLim)
								twr.IsShot = false;
						}
						else
						{
							twr.IsShot = true;
							twr.AnimationTicks = 0;
						}
					}
					break;
				case 4:
					foreach (Enemy foe in HelpEnemyList)
					{
						if (foe.AnimationTicks < (250 / (int)foe.Speed) * 2 - 1)
							foe.AnimationTicks++;
						else
							foe.AnimationTicks = 0;
					}
					break;
			}

			Screen_PB.Refresh();
		}
	}
}
//Achievement unlocked: Читер
//Посмотреть в конец кода, не читая его полностью
