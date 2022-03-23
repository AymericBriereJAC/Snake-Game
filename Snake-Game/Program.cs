using System;

namespace Snake_Game
{
    class Program
    {
        //Snake variable declaration section
        static int SnakeSize;
        static int[] SnakeX;
        static int[] SnakeY;
        static char SnakeDir; //Direction in witch the player is going (west, east, north, south)
        static int OldSnakeX;
        static int OldSnakeY;

        //basic food item(coin) variable declaration section
        static int FoodX;
        static int FoodY;
        static int OldFoodX;
        static int OldFoodY;

        //Food that make the snake crawl variable
        const double CRAWLDURATION = 5;    //amount of time the snake will crawl
        static bool CrawlFood;            //is a crawl food currently spawned
        static int CrawlFoodX;
        static int CrawlFoodY;
        static int OldCrawlFoodX;
        static int OldCrawlFoodY;
        static double CrawlStop;

        //Food To make the screeen biger declaration section
        const int WINDOWFOODSIZE = 2;       //lenght of this food item
        static bool WindowFood;           //Is a window food curently spawned
        static bool WindowMaxSize;       //becomes true if the max size of the windwo is hitted
        static int WindowFoodX;
        static int WindowFoodY;
        static int OldWindowFoodX;
        static int OldWindowFoodY;
        static char WindowFoodDirX;         //Direction of the food on the X axis
        static char WindowFoodDirY;        //Direction of the food on the Y axis

        //Movement variable declaration section
        static bool GlideOn = false;
        static bool NewKeyStroke = false;   //Make the program only move the player if he it a key if he isn't in glide mode

        //Overall game variables declaration section
        const int SCOREZONEY = 4;     //size (Y) of the section for the stats
        static int WindowX;          //X size of the window
        static int WindowY;         //Y size of the window
        static int GameY;          //the y size of the playing zone
        static int Score;
        static double BaseSpeed = 65;   //By default set to "normal" speed
        static double GameSpeed;
        static ConsoleColor Background = ConsoleColor.DarkGray;
        static ConsoleColor FontColor = ConsoleColor.White;
        static TimeSpan gametimer;

        static void Main(string[] args)
        {
            // function that calls menu by default it calls the main menu
            int choice = 0;
            do
            {
                //loops until the user choose the choice 4 wich is to quit the game
                switch (choice)
                {
                    case 0:
                        choice = MainMenu();
                        break;
                    case 1:
                        Gameloop();
                        choice = GameOverMenu();
                        break;
                    case 2:
                        choice = DisplayInstructions();
                        break;
                    case 3:
                        choice = DifficultyMenu();
                        break;
                }
            } while (choice != 4);
        }
        public static int MainMenu()
        {
            //main menu of the game. I want the game logo to be centered horizontally and at the top of the console
            //return the choice of the user after he confirmed it
            //ascii text font generated using the web site: https://fsymbols.com/generators/carty/
            const int MINCHOICE = 1;    //minimum value of the menu choice
            const int MAXCHOICE = 4;   //maximum value of the menu choice 
            const int MENUY = 20;     //size of the menu window on the Y axis  
            ConsoleColor selectedcolor = ConsoleColor.Red;      //color of the selected option
            bool confirm = false;   //was the user choice confirmed
            int menuX;             //Size of the menu window on the X axis 
            int choice = MINCHOICE;
            int[] textY = { 8, 10, 12, 14, 17 };     //Y coordonate of the text in the menu
            string[] message = new string[5];       //the messages we want outputted to the console
            string[] logo = new string[4];         //number of lines the logo has
            logo[0] = "█████████████████████████████████████████████████████████████████████████████";
            logo[1] = "█─▄▄▄─█─▄▄─█▄─▀█▄─▄█─▄▄▄▄█─▄▄─█▄─▄███▄─▄▄─███─▄▄▄▄█▄─▀█▄─▄██▀▄─██▄─█─▄█▄─▄▄─█";
            logo[2] = "█─███▀█─██─██─█▄▀─██▄▄▄▄─█─██─██─██▀██─▄█▀███▄▄▄▄─██─█▄▀─███─▀─███─▄▀███─▄█▀█";
            logo[3] = "▀▄▄▄▄▄▀▄▄▄▄▀▄▄▄▀▀▄▄▀▄▄▄▄▄▀▄▄▄▄▀▄▄▄▄▄▀▄▄▄▄▄▀▀▀▄▄▄▄▄▀▄▄▄▀▀▄▄▀▄▄▀▄▄▀▄▄▀▄▄▀▄▄▄▄▄▀";
            message[0] = "1.   Play!";
            message[1] = "2.   Intsructions";
            message[2] = "3.   Difficulty";
            message[3] = "4.   Exit";
            message[4] = "Use the arrow keys to change your selection and press enter to confirm it";
            menuX = logo[0].Length;     //all the lines are of the same lenght
            SetBackgroundColor(Background);
            Console.Clear();
            Console.SetWindowSize(menuX, MENUY);
            Console.CursorVisible = false;
            Console.ForegroundColor = FontColor;
            for (int i = 0; i < logo.Length; i++)   //draw the logo
                Console.WriteLine(logo[i]);
            do
            {
                //loops until the user confirm his choice
                for (int i = 0; i < textY.Length; i++)
                {
                    //draw the text
                    if (choice == i + 1)
                        //if we are drawing the choice of the user change the font color
                        Console.ForegroundColor = selectedcolor;
                    CenterText(message[i], menuX, textY[i]);
                    Console.ForegroundColor = FontColor;
                }
                confirm = MenuInput(ref choice, MINCHOICE, MAXCHOICE);
            } while (!confirm);
            return choice;
        }
        public static int DisplayInstructions()
        {
            //function that draw the instruction of the game on the screen.
            const int WINDOWBUFFERX = 20;   //how much free space we want between the text and the window border on the X axis 
            int windowX = 0;     //size of the window on the X axis
            int windowY = 17;   //size of the window on the Y axis
            int textY;         //y coordonate of the text
            string[] instruction = new string[13];
            instruction[0] = "This is a modified version of the snake game. This game is played through";
            instruction[1] = "the console. Use the arrow keys or W A S D to move. The basic food item is the";
            instruction[2] = "green dollar bill, every time you eat it your snake gets bigger, the game gets";
            instruction[3] = "slightly faster and your score goes up by 10 points. In this modified version,";
            instruction[4] = "you can go through walls, every time you do so you loose 10 points, and the snake";
            instruction[5] = "head reappears at a random location on the screen. There is also an item that spawns";
            instruction[6] = "every 20 seconds that when is eaten stops the snake from gliding and makes it 'crawl'";
            instruction[7] = "for 5 seconds. Every 35 seconds a red square spawns when eaten the console gets";
            instruction[8] = "bigger. It disappears if it's not eaten in 6.5 seconds and stops spawning after the";
            instruction[9] = "console reached the max size. You loose when the head of the snake touches the tail";
            instruction[10] = "or when your score goes down to 0";
            instruction[11] = " ";
            instruction[12] = "Press any key to go back to the main menu";
            textY = (windowY - instruction.Length) / 2;         //determining the size of the screen
            for (int i = 0; i < instruction.Length; i++)
            {
                if (instruction[i].Length > windowX)
                    windowX = instruction[i].Length + WINDOWBUFFERX;
            }
            Console.Clear();
            Console.SetWindowSize(windowX, windowY);
            Console.CursorVisible = false;
            for (int i = 0; i < instruction.Length; i++)
            {
                //draw the instruction on the screen
                CenterText(instruction[i], windowX, textY);
                textY++;
            }
            Console.ReadKey();
            return 0;
        }
        public static int GameOverMenu()
        {
            //Menu that is displayed when the player loose he has 3 choice: go bakc to
            //the main menu, start a new game or quit the game
            //ascii font generated using https://fsymbols.com/generators/carty/
            const int MINCHOICE = 1;    //minimum value of the menu choice
            const int MAXCHOICE = 3;   //maximum value of the menu choice 
            const int MENUY = 18;     //size of the menu window on the Y axis  
            ConsoleColor selectedcolor = ConsoleColor.Red;      //color of the selected option
            bool confirm = false;   //was the user choice confirmed
            int menuX;             //Size of the menu window on the X axis 
            int choice = MINCHOICE;
            int[] textY = { 10, 12, 14 };            //Y coordonate of the text in the menu
            string[] message = new string[3];       //the messages we want outputted to the console
            string[] logo = new string[8];         //number of lines the logo has
            logo[0] = "                                                                           ";
            logo[1] = "                                                                           ";
            logo[2] = "   ░██████╗░░█████╗░███╗░░░███╗███████╗░█████╗░██╗░░░██╗███████╗██████╗░   ";
            logo[3] = "   ██╔════╝░██╔══██╗████╗░████║██╔════╝██╔══██╗██║░░░██║██╔════╝██╔══██╗   ";
            logo[4] = "   ██║░░██╗░███████║██╔████╔██║█████╗░░██║░░██║╚██╗░██╔╝█████╗░░██████╔╝   ";
            logo[5] = "   ██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░██║░░██║░╚████╔╝░██╔══╝░░██╔══██╗   ";
            logo[6] = "   ╚██████╔╝██║░░██║██║░╚═╝░██║███████╗╚█████╔╝░░╚██╔╝░░███████╗██║░░██║   ";
            logo[7] = "   ░╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝░╚════╝░░░░╚═╝░░░╚══════╝╚═╝░░╚═╝   ";
            message[0] = "1.   Play Again!";
            message[1] = "2.   Main Menu";
            message[2] = "3.   Exit";
            menuX = logo[0].Length;
            Console.Clear();
            Console.SetWindowSize(menuX, MENUY);
            Console.CursorVisible = false;
            Console.ForegroundColor = FontColor;
            for (int i = 0; i < logo.Length; i++)
                Console.WriteLine(logo[i]);         //draw the logo
            do
            {
                //loops until the user confirm his choice
                for (int i = 0; i < textY.Length; i++)
                {
                    //draw the text
                    if (choice == i + 1)
                        //if the text we are drawing is the selected choice change the font color
                        Console.ForegroundColor = selectedcolor;
                    CenterText(message[i], menuX, textY[i]);
                    Console.ForegroundColor = FontColor;
                }
                confirm = MenuInput(ref choice, MINCHOICE, MAXCHOICE);
            } while (!confirm);
            switch (choice)
            {
                //switch the value of the choice variable to fit to the value in main
                case 2:
                    choice = 0;
                    break;
                case 3:
                    choice = 4;
                    break;
            }
            return choice;
        }
        public static int DifficultyMenu()
        {
            //Menu to select the difficulty of the game
            //The Choices are: very easy, easy, normal, hard, very hard
            //ascii font generated using https://fsymbols.com/generators/carty/
            const int MINCHOICE = 1;    //minimum value of the menu choice
            const int MAXCHOICE = 5;   //maximum value of the menu choice 
            const int MENUY = 19;     //size of the menu window on the Y axis  
            ConsoleColor selectedcolor = ConsoleColor.Red;      //color of the selected option
            bool confirm = false;   //was the user choice confirmed
            int menuX;             //Size of the menu window on the X axis 
            int choice = 3;
            int[] textY = { 5, 7, 9, 11, 13, 16 };     //Y coordonate of the text in the menu
            string[] message = new string[6];       //the messages we want outputted to the console
            string[] logo = new string[3];         //number of lines the logo has
            logo[0] = "█▄─▄▄▀█▄─▄█▄─▄▄─█▄─▄▄─█▄─▄█─▄▄▄─█▄─██─▄█▄─▄███─▄─▄─█▄─█─▄█";
            logo[1] = "██─██─██─███─▄████─▄████─██─███▀██─██─███─██▀███─████▄─▄██";
            logo[2] = "▀▄▄▄▄▀▀▄▄▄▀▄▄▄▀▀▀▄▄▄▀▀▀▄▄▄▀▄▄▄▄▄▀▀▄▄▄▄▀▀▄▄▄▄▄▀▀▄▄▄▀▀▀▄▄▄▀▀";
            message[0] = "Very Hard";
            message[1] = "Hard";
            message[2] = "Normal";
            message[3] = "Easy";
            message[4] = "Very Easy";
            message[5] = "Select a Difficulty to go back to the main menu";
            menuX = logo[0].Length;
            Console.Clear();
            Console.SetWindowSize(menuX, MENUY);
            Console.CursorVisible = false;
            Console.ForegroundColor = FontColor;
            for (int i = 0; i < logo.Length; i++)
                Console.WriteLine(logo[i]);         //draw the logo
            do
            {
                //loops until the user confirm his choice
                for (int i = 0; i < textY.Length; i++)
                {
                    if (choice == i + 1)
                        //if we are drawing the user choice, change the font color
                        Console.ForegroundColor = selectedcolor;
                    CenterText(message[i], menuX, textY[i]);
                    Console.ForegroundColor = FontColor;
                }
                confirm = MenuInput(ref choice, MINCHOICE, MAXCHOICE);
            } while (!confirm);
            switch (choice)
            {
                //change the base gamespeed base on the difficulty choice of the user
                case 1:
                    BaseSpeed = 35;
                    break;
                case 2:
                    BaseSpeed = 50;
                    break;
                case 3:
                    BaseSpeed = 65;
                    break;
                case 4:
                    BaseSpeed = 80;
                    break;
                case 5:
                    BaseSpeed = 100;
                    break;
            }
            return 0;
        }
        public static void CenterText(string message, int windowX, int textY)
        {
            //function to center text horizontally on the screen. the first parameter is
            //the message we want to display, the second one is the width of the console
            //and the third one is the y position of the text
            int textX;
            textX = (windowX - message.Length) / 2;
            Console.SetCursorPosition(textX, textY);
            Console.Write(message);
        }
        public static bool MenuInput(ref int choice, int minchoice, int maxchoice)
        {
            //this function change the user choice based on their input. the first parameter is the variable 
            //we want to change and the second one is the minimum value the choice can be and the third one is
            //the maximum value the choice can be. return true if the choice was confirmed else it return false
            bool confirm = false;   //is the choice confirmed
            ConsoleKey k;
            k = ConsoleKey.NoName;
            if (Console.KeyAvailable)
            {
                //was there a key interupt
                k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.UpArrow && choice > minchoice)
                    choice--;
                else if (k == ConsoleKey.DownArrow && choice < maxchoice)
                    choice++;
                else if (k == ConsoleKey.Enter)
                    confirm = true;
            }
            return confirm;
        }
        public static void Gameloop()
        {
            //main function of the game calls other fucntion. Egine of the game
            const double CRAWLFOODINTERVAL = 20;   //Interval in second of witch a crawl food spawn
            const int WINDOWFOODINTERVAL = 35;    //interval in second at witch this food spawn
            const int WINDOWFOODFACTOR = 4;      //speed at wich the food move the lower it is the slower it will go
            const double WINDOWFOODDURATION = 6.5;  //time that the food stay on the screen 
            bool gameover = false;
            int framecount = 0;
            double WindowFoodStop = 0;   //time at witch the window food will disapear
            DateTime starttime = DateTime.Now;
            GlideOn = true;
            CrawlFood = false;
            WindowFood = false;
            WindowMaxSize = false;
            SnakeDir = 'W';
            WindowX = 62;
            GameY = 35;
            WindowY = GameY + SCOREZONEY;
            Score = 10;
            GameSpeed = BaseSpeed;
            Console.SetWindowSize(WindowX, WindowY);
            Console.CursorVisible = false;
            SetBackgroundColor(Background);
            SpawnSnake(15);
            Spawn(out FoodX, out FoodY);
            while (!gameover)
            {
                //loops until the game is over
                GetkeyStroke();
                if (GlideOn || NewKeyStroke)
                    //only move the snake if there is a new keystroke or the glidemode is on
                    Move();
                Limits(10);
                IsSnakeEating();
                if (Math.Round(gametimer.TotalSeconds) % CRAWLFOODINTERVAL == 0 && !CrawlFood && Math.Round(gametimer.TotalSeconds) != 0 && GlideOn)
                {
                    //spawn a crawl food when the wanted interval passed
                    Spawn(out CrawlFoodX, out CrawlFoodY);
                    CrawlFood = true;
                }
                if (gametimer.TotalSeconds >= CrawlStop)    //reactivate glide mode after the wanted amount of time
                {
                    GlideOn = true;
                }
                if (Math.Round(gametimer.TotalSeconds) % WINDOWFOODINTERVAL == 0 && Math.Round(gametimer.TotalSeconds) != 0 && !WindowFood && !WindowMaxSize)
                {
                    //spawn the food when the wanted interval passed
                    Spawn(out WindowFoodX, out WindowFoodY);
                    RandomDirWindowFood();
                    WindowFood = true;
                    WindowFoodStop = gametimer.TotalSeconds + WINDOWFOODDURATION;
                }
                if (gametimer.TotalSeconds >= WindowFoodStop)   //make the window food disapear when the wanted interval passed
                    WindowFoodDisapear();
                if (WindowFood && framecount % WINDOWFOODFACTOR == 0)
                {
                    //move and check the limits of the window food
                    WindowFoodLimits();
                    MoveWindowFood();
                }
                gametimer = UpDateGameTimer(starttime);
                Draw();
                Wait(GameSpeed);
                gameover = IsGameOver();
                framecount++;
            }
        }
        public static void GetkeyStroke()
        {
            //function to get key stroke and change the player direction based on his input
            //also check if the player is trying to go over himself and if he is his direction won't change
            ConsoleKey k;
            k = ConsoleKey.NoName;
            if (Console.KeyAvailable)
            {
                //was there a key interupt
                k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.RightArrow || k == ConsoleKey.D && SnakeDir != 'W')
                {
                    SnakeDir = 'E';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.LeftArrow || k == ConsoleKey.A && SnakeDir != 'E')
                {
                    SnakeDir = 'W';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.UpArrow || k == ConsoleKey.W && SnakeDir != 'S')
                {
                    SnakeDir = 'N';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.DownArrow || k == ConsoleKey.S && SnakeDir != 'N')
                {
                    SnakeDir = 'S';
                    NewKeyStroke = true;
                }
            }
        }
        public static void Move()
        {
            //function to move the whole snake
            //move each part of the snake to the position of the part in front of it in the array
            //Shift the whole array and only move the the head of the snake based on user input
            NewKeyStroke = false;
            OldSnakeX = SnakeX[SnakeSize - 1];
            OldSnakeY = SnakeY[SnakeSize - 1];
            for (int i = SnakeSize - 1; i > 0; i--)
            {
                SnakeX[i] = SnakeX[i - 1];
                SnakeY[i] = SnakeY[i - 1];
            }
            switch (SnakeDir)
            {
                case 'N':
                    SnakeY[0]--;
                    break;
                case 'S':
                    SnakeY[0]++;
                    break;
                case 'W':
                    SnakeX[0]--;
                    break;
                case 'E':
                    SnakeX[0]++;
                    break;

            }
        }
        public static void Draw()
        {
            //function to update the screen and draw all the elements on the screen        
            const int SCOREX = 2;
            int timerX = WindowX - 28;
            int scoreY = WindowY - Convert.ToInt32(SCOREZONEY / 2);
            int timerY = scoreY;
            string foodchar = "$";
            ConsoleColor SnakeColor = ConsoleColor.Blue;
            ConsoleColor foodbackground = ConsoleColor.DarkGreen;
            ConsoleColor crawlfoodcolor = ConsoleColor.Yellow;
            ConsoleColor WindowFoodColor = ConsoleColor.Red;
            ConsoleColor bordercolor = FontColor;
            Console.BackgroundColor = bordercolor;
            for (int i = 0; i < WindowX; i++)
            {
                //Draw the border
                Console.SetCursorPosition(i, GameY);
                Console.Write(" ");
            }
            // Draw the stats and delete old things from the screen
            Console.BackgroundColor = Background;
            Console.SetCursorPosition(SCOREX, scoreY);
            Console.Write("Score : " + Score + " ");
            Console.SetCursorPosition(timerX, timerY);
            Console.Write("Time : {0} minutes {1} seconds", gametimer.Minutes, gametimer.Seconds);
            Console.SetCursorPosition(OldSnakeX, OldSnakeY);
            Console.Write(" ");
            Console.BackgroundColor = Background;
            Console.SetCursorPosition(OldWindowFoodX, OldWindowFoodY);
            Console.Write("  ");
            if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
            {
                //old food disapear if the player ate it
                Console.SetCursorPosition(OldFoodX, OldFoodY);
                Console.Write(" ");
            }
            if (SnakeX[0] == CrawlFoodX && SnakeY[0] == CrawlFoodY)
            {
                //make the old crawl food disapear
                Console.SetCursorPosition(OldCrawlFoodX, OldCrawlFoodY);
                Console.Write(" ");
            }
            Console.BackgroundColor = SnakeColor;
            for (int i = 0; i < SnakeSize; i++)
            {
                //go through the array and draw every part of the snake 
                Console.SetCursorPosition(SnakeX[i], SnakeY[i]);
                Console.Write(" ");
            }
            //Draw the food items (if they are on)
            Console.SetCursorPosition(FoodX, FoodY);
            Console.BackgroundColor = foodbackground;
            Console.Write(foodchar);
            if (CrawlFood)
            {
                Console.SetCursorPosition(CrawlFoodX, CrawlFoodY);
                Console.BackgroundColor = crawlfoodcolor;
                Console.Write(" ");
            }
            if (WindowFood)
            {
                Console.BackgroundColor = WindowFoodColor;
                Console.SetCursorPosition(WindowFoodX, WindowFoodY);
                for (int i = 0; i < WINDOWFOODSIZE; i++)
                    Console.Write(" ");     //draw this food item in the wanted lenght
            }
            Console.BackgroundColor = Background;
        }
        public static void Limits(int scoreloss)
        {
            //function that set the limits of the games to the border of the window
            //when the player is supposed to be out of the screen the snake head reapears at a random location in the screen
            //the parameter "scoreloss" is how much we want the score to drop when the player go through a wall
            if (SnakeX[0] < 0)
            {
                Spawn(out SnakeX[0], out SnakeY[0]);
                Score -= scoreloss;
            }
            else if (SnakeX[0] >= WindowX)
            {
                Spawn(out SnakeX[0], out SnakeY[0]);
                Score -= scoreloss;
            }
            else if (SnakeY[0] < 0)
            {
                Spawn(out SnakeX[0], out SnakeY[0]);
                Score -= scoreloss;
            }
            else if (SnakeY[0] >= GameY)
            {
                Spawn(out SnakeX[0], out SnakeY[0]);
                Score -= scoreloss;
            }
        }
        public static void Spawn(out int X, out int Y)
        {
            //function to spawn something at a random location in the window based on the window size
            //the function loops trhough the snake coordinate and if it is in the snake will
            //give new random coordinate until it's not in the snake any more
            Random rnd = new Random();
            bool intail;
            do
            {
                X = rnd.Next(0, WindowX);
                Y = rnd.Next(0, GameY);
                intail = false;
                for (int i = 1; i < SnakeSize; i++)
                {
                    if (X == SnakeX[i] && Y == SnakeY[i])
                    {
                        //check if it spawn in the snake
                        intail = true;
                        break;
                    }
                }
            } while (intail);
        }
        public static void SpawnSnake(int size)
        {
            //Spawn a snake of a given lenght determined by the argument the snake spawn on X axis
            const int BUFFER = 15;      //minimum distance the snake can spawn from the border he is going in
            Random rnd = new Random();
            SnakeSize = size;
            SnakeX = new int[SnakeSize];
            SnakeY = new int[SnakeSize];
            SnakeX[0] = rnd.Next(0 + BUFFER, WindowX - SnakeSize);   //make sure the whole snake is spawning in the window
            SnakeY[0] = rnd.Next(0, GameY);
            for (int i = 0; i < size - 1; i++)
            {
                SnakeX[i + 1] = SnakeX[i] + 1;
                SnakeY[i + 1] = SnakeY[i];
            }
        }
        public static void Wait(double waittime)
        {
            //function to delay the game a given amount of time wich is determined by the argument
            DateTime start = DateTime.Now;
            DateTime stop = DateTime.Now.AddMilliseconds(waittime);
            while (start < stop)        //loops until the wanted amount of time has elapsed
                start = DateTime.Now;
        }
        public static void EatFood()
        {
            //function to make a new food spawn, make the score go up, the game going faster and the snake bigger
            const double REDUCESPEED = 2.65;     //how much in ms the game will go faster
            OldFoodX = FoodX;
            OldFoodY = FoodY;
            Score += 10;
            GameSpeed -= REDUCESPEED;
            BiggerSnake(10);
            Spawn(out FoodX, out FoodY);
        }
        public static void BiggerSnake(int sizegain)
        {
            //function to make the snake bigger. Make a new arrays for the snake position that is
            //1 slot bigger than the whole one, store the old one in a temporary array and then 
            //use the value of the temporary array and the position of the old snake end to assign
            //the values in the new array the parameter is how much biger we want the snake to be
            for (int j = 0; j < sizegain; j++)
            {
                //loops until the snake is bigger of the wanted size
                int[] tempX = new int[SnakeSize];
                int[] tempY = new int[SnakeSize];
                for (int i = 0; i < SnakeSize; i++)
                {
                    tempX[i] = SnakeX[i];
                    tempY[i] = SnakeY[i];
                }
                SnakeSize++;
                SnakeX = new int[SnakeSize];
                SnakeY = new int[SnakeSize];
                for (int i = 0; i < SnakeSize - 1; i++)
                {
                    SnakeX[i] = tempX[i];
                    SnakeY[i] = tempY[i];
                }
                SnakeX[SnakeSize - 1] = OldSnakeX;
                SnakeY[SnakeSize - 1] = OldSnakeY;
            }
        }
        public static bool IsGameOver()
        {
            //check if the game (head colling with tail or score == 0) if it is it return true  else it return false
            bool gameover = false;
            for (int i = 1; i < SnakeSize; i++)
            {
                if (SnakeX[0] == SnakeX[i] && SnakeY[0] == SnakeY[i])
                    gameover = true;
            }
            if (Score <= 0)
                gameover = true;
            return gameover;
        }
        public static void SetBackgroundColor(ConsoleColor consoleColor)
        {
            //function to set the whole background of the console to a color. the color is the argument
            Console.BackgroundColor = consoleColor;
            Console.Clear();
        }
        public static TimeSpan UpDateGameTimer(DateTime starttime)
        {
            //function to update the in gametimer. the argument is the timer we want to update
            //we assume the timer was alreaddy initialised 
            TimeSpan timer;
            DateTime now = DateTime.Now;
            timer = now - starttime;
            return timer;
        }
        public static void EatCrawlFood()
        {
            //Function to make the current crawl food disapear and disable gliding
            OldCrawlFoodX = CrawlFoodX;
            OldCrawlFoodY = CrawlFoodY;
            GlideOn = false;
            CrawlFood = false;
            CrawlStop = gametimer.TotalSeconds + CRAWLDURATION;
        }
        public static void IsSnakeEating()
        {
            //function that check if the snake ate some thing(headXY == foodXY) and if so call the eat function of the food eated
            if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
                EatFood();
            if (SnakeX[0] == CrawlFoodX && SnakeY[0] == CrawlFoodY && CrawlFood)
                EatCrawlFood();
            if (SnakeX[0] >= WindowFoodX && SnakeX[0] <= WindowFoodX + WINDOWFOODSIZE - 1 && SnakeY[0] == WindowFoodY && WindowFood)
                EatWindowFood(1.15);
        }
        public static void RandomDirWindowFood()
        {
            //function to establish a random starting direction for the window food 
            Random rnd = new Random();
            int Xdir;
            int Ydir;
            Xdir = rnd.Next(1, 3);  //double check if 3 is really excluded and 1 included
            Ydir = rnd.Next(1, 3);
            if (Xdir == 1)
                WindowFoodDirX = 'W';
            else
                WindowFoodDirX = 'E';
            if (Ydir == 1)
                WindowFoodDirY = 'N';
            else
                WindowFoodDirY = 'S';
        }
        public static void WindowFoodLimits()
        {
            //function to invert the food direction in function of wich wall he hitted
            if ((WindowFoodX == 0) || (WindowFoodX == FoodX + 1 && WindowFoodY == FoodY) || (WindowFoodX == CrawlFoodX + 1 && WindowFoodY == CrawlFoodY && CrawlFood))
                WindowFoodDirX = 'E';
            else if ((WindowFoodX + WINDOWFOODSIZE - 1 == WindowX - 1) || (WindowFoodX + WINDOWFOODSIZE - 1 == FoodX - 1 && WindowFoodY == FoodY) || (WindowFoodX + WINDOWFOODSIZE - 1 == CrawlFoodX - 1 && WindowFoodY == CrawlFoodY && CrawlFood))
                WindowFoodDirX = 'W';
            if ((WindowFoodY == 0) || (WindowFoodX == FoodX && WindowFoodY == FoodY + 1) || (WindowFoodX == CrawlFoodX && WindowFoodY == CrawlFoodY + 1 && CrawlFood))
                WindowFoodDirY = 'S';
            else if ((WindowFoodY == GameY - 1) || (WindowFoodX == FoodX && WindowFoodY == FoodY - 1) || (WindowFoodX == CrawlFoodX && WindowFoodY == CrawlFoodY - 1 && CrawlFood))
                WindowFoodDirY = 'N';
            for (int i = 1; i < SnakeSize; i++)
            {
                //check if touching the snake tail and if so change teh position accordingly
                //cant use break becasue some time the item touches multiple "position" of the snake tail
                //and the direction need to change according to this
                if (WindowFoodX == SnakeX[i] && WindowFoodY == SnakeY[i] + 1)
                    WindowFoodDirY = 'S';
                else if (WindowFoodX == SnakeX[i] && WindowFoodY == SnakeY[i] - 1)
                    WindowFoodDirY = 'N';
                if (WindowFoodX == SnakeX[i] + 1 && WindowFoodY == SnakeY[i])
                    WindowFoodDirX = 'E';
                else if (WindowFoodX + WINDOWFOODSIZE - 1 == SnakeX[i] - 1 && WindowFoodY == SnakeY[i])
                    WindowFoodDirX = 'W';
            }
        }
        public static void MoveWindowFood()
        {
            //function to move the position of the window food 
            OldWindowFoodX = WindowFoodX;
            OldWindowFoodY = WindowFoodY;
            if (WindowFoodDirX == 'W')
                WindowFoodX--;
            else
                WindowFoodX++;
            if (WindowFoodDirY == 'N')
                WindowFoodY--;
            else
                WindowFoodY++;
        }
        public static void EatWindowFood(double sizegain)
        {
            //function that make the window food disapear and applies all the wanted effect
            //on the game when the snake eat it.
            WindowFood = false;
            OldWindowFoodX = WindowFoodX;
            OldWindowFoodY = WindowFoodY;
            WindowX = Convert.ToInt32(WindowX * sizegain);
            GameY = Convert.ToInt32(GameY * sizegain);
            WindowY = GameY + SCOREZONEY;
            try
            {
                //tries making the screen bigger
                Console.SetWindowSize(WindowX, WindowY);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //if it can't, revert back to the old window size
                WindowX = Convert.ToInt32(WindowX / sizegain);
                GameY = Convert.ToInt32(GameY / sizegain);
                WindowY = GameY + SCOREZONEY;
                WindowMaxSize = true;
            }
            Console.Clear();
        }
        public static void WindowFoodDisapear()
        {
            //function that make the food disapear without the effect of eating it
            WindowFood = false;
            OldWindowFoodX = WindowFoodX;
            OldWindowFoodY = WindowFoodY;
        }
    }
}