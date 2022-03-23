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
        static ConsoleColor SnakeColor = ConsoleColor.Blue;   

        //basic food item(coin) variable declaration section
        static int FoodX;
        static int FoodY;
        static int OldFoodX;
        static int OldFoodY;

        //Food that make the snake crawl variable
        const double CRAWLFOODINTERVAL = 20;   //Interval in second of witch a crawl food spawn
        const double CRAWLDURATION = 5;        //amount of time the snake will crawl
        static bool CrawlFood; //is a crawl food currently spawned
        static int CrawlFoodX;
        static int CrawlFoodY;
        static int OldCrawlFoodX;
        static int OldCrawlFoodY;
        static double CrawlStop;

        //Food To make the screeen biger declaration section
        const int WINDOWFOODINTERVAL = 10; //interval in second at witch this food spawn
        const int WINDOWFOODFACTOR = 4;      //speed at wich the food move the lower it is the slower it will go
        const double WINDOWFOODDURATION = 999999999;  //time that the food stay on the screen 
        const double WINDOWSIZEGAIN = 1.15; //size gain of the window when the sake eat it 
        static bool WindowFood;
        static bool WindowMaxSize;   //becomes true if the max size of the windwo is hitted
        static double WindowFoodStop;   //time at witch the window food will disapear
        static int WindowFoodX;
        static int WindowFoodY;
        static int OldWindowFoodX;
        static int OldWindowFoodY;
        static char WindowFoodDirX;
        static char WindowFoodDirY;
        static ConsoleColor WindowFoodColor = ConsoleColor.Red;

        //Movement variable declaration section
        static bool GlideOn = false;
        static bool NewKeyStroke = false;   //Make the program only move the player if he it a key if he isn't in glide mode

        //score variables declaration section
        static int Score;

        //Overall game variables declaration section
        const int SCOREZONEY = 4;    //size (Y) of the section for the stats
        static int WINDOWX;     
        static int GAMEY;   //the y size of the playing zone
        static int WINDOWY;   
        static ConsoleColor Background = ConsoleColor.DarkGray; 
        static ConsoleColor FontColor = ConsoleColor.White;
        static double GameSpeed;    
        static TimeSpan gametimer;  


        static void Main(string[] args)
        {
            int choice = 0;
            do
            {
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
            ConsoleColor selectedcolor = ConsoleColor.Red;
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
            message[2] = "3.   Settings";
            message[3] = "4.   Exit";
            message[4] = "Use the arrow keys to change your selection and press enter to confirm it";
            menuX = logo[0].Length;     //all the lines are of the same lenght
            SetBackgroundColor(Background);
            Console.Clear();
            Console.SetWindowSize(menuX, MENUY);
            Console.CursorVisible = false;
            Console.ForegroundColor = FontColor;
            for (int i = 0; i < logo.Length; i++)
                Console.WriteLine(logo[i]);
            do
            {
                for (int i = 0; i < textY.Length; i++)
                {
                    if (choice == i + 1)
                        Console.ForegroundColor = selectedcolor;
                    CenterText(message[i], menuX, textY[i]);
                    Console.ForegroundColor = FontColor;
                }
                confirm = MenuInput(ref choice, MAXCHOICE);
            } while (!confirm);
            return choice;
        }
        public static int DisplayInstructions()
        {
            //function that draw the instruction of the game on the screen
            const int WINDOWBUFFERX = 16;   //how much free space we want from the text on the X axis   
            int windowX = 0;
            int windowY = 16;
            int textY;
            string[] instruction = new string[9];
            instruction[0] = "This is a modified version of the snake game. This game is played through";
            instruction[1] = "the console. Use the arrow keys to move. The basic food item is the green";
            instruction[2] = "dollard bill. Every time you eat it your snake get bigger, the game get";
            instruction[3] = "slightly faster and your score goes up of 10 points. In this modified version,";
            instruction[4] = "you can go through walls, every time you do so you loose 10 points of your ";
            instruction[5] = "score. You loose when your head touches your tail or when your scores goes";
            instruction[6] = "down to 0.";
            instruction[7] = " ";
            instruction[8] = "Press any key to go back to the main menu";
            textY = (windowY - instruction.Length) / 2;
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
            ConsoleColor selectedcolor = ConsoleColor.Red;
            bool confirm = false;   //was the user choice confirmed
            int menuX;             //Size of the menu window on the X axis 
            int choice = MINCHOICE;
            int[] textY = { 10, 12, 14 };     //Y coordonate of the text in the menu
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
                Console.WriteLine(logo[i]);
            do
            {
                for (int i = 0; i < textY.Length; i++)
                {
                    if (choice == i + 1)
                        Console.ForegroundColor = selectedcolor;
                    CenterText(message[i], menuX, textY[i]);
                    Console.ForegroundColor = FontColor;
                }
                confirm = MenuInput(ref choice, MAXCHOICE);
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
        public static void CenterText(string message, int windowX, int TextY)
        {
            //function to center text horizontally on the screen. the first parameter is
            //the message we want to display, the second one is the width of the console
            //and the third one is the y position of the text
            int textX;
            textX = (windowX - message.Length) / 2;
            Console.SetCursorPosition(textX, TextY);
            Console.Write(message);
        }
        public static bool MenuInput(ref int choice, int maxchoice)
        {
            //this function change the user choice based on their input. the first parameter is the 
            //variable we want to change and the second one is the max value we want it to be
            bool confirm = false;   //the the user confirmed his choice
            ConsoleKey k;
            k = ConsoleKey.NoName;
            if (Console.KeyAvailable)
            {
                //was there a key interupt
                k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.UpArrow && choice > 1)
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
            DateTime starttime = DateTime.Now;
            bool gameover = false;
            int framecount = 0;
            GlideOn = true;
            CrawlFood = false;
            WindowFood = false;
            WindowMaxSize = false;
            SnakeDir = 'W';
            WINDOWX = 62;
            GAMEY = 35;
            WINDOWY = GAMEY + 4;
            Score = 9999999;
            GameSpeed = 50;
            Console.SetWindowSize(WINDOWX, WINDOWY);
            Console.CursorVisible = false;
            SetBackgroundColor(Background);
            SpawnSnake(20);
            Spawn(out FoodX, out FoodY);
            Spawn(out WindowFoodX, out WindowFoodY);
            RandomDirWindowFood();
            WindowFood = true;
            WindowFoodStop = gametimer.TotalSeconds + WINDOWFOODDURATION;
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
                    GlideOn = true;
                /*if (Math.Round(gametimer.TotalSeconds) % WINDOWFOODINTERVAL == 0 && !WindowFood && !WindowMaxSize)
                {
                    //spawn the food when the wanted interval passed
                    Spawn(out WindowFoodX, out WindowFoodY);
                    RandomDirWindowFood();
                    WindowFood = true;
                    WindowFoodStop = gametimer.TotalSeconds + WINDOWFOODDURATION;
                }*/
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
                if (k == ConsoleKey.D && SnakeDir != 'W')
                {
                    SnakeDir = 'E';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.A && SnakeDir != 'E')
                {
                    SnakeDir = 'W';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.W && SnakeDir != 'S')
                {
                    SnakeDir = 'N';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.S && SnakeDir != 'N')
                {
                    SnakeDir = 'S';
                    NewKeyStroke = true;
                }
                else if (k == ConsoleKey.E)
                {
                    if (!GlideOn)
                        GlideOn = true;
                    else
                        GlideOn = false; ;
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
            int TIMERX = WINDOWX - 28;
            int SCOREY = WINDOWY - 2;
            int TIMERY = WINDOWY - 2;
            string foodchar = "$";
            ConsoleColor foodbackground = ConsoleColor.DarkGreen;
            ConsoleColor crawlfoodcolor = ConsoleColor.Yellow;
            ConsoleColor bordercolor = FontColor;
            Console.BackgroundColor = bordercolor;
            for (int i = 0; i < WINDOWX; i++)
            {
                //Draw the border
                Console.SetCursorPosition(i, GAMEY);
                Console.Write(" ");
            }
            // Draw the stats and delete old things from the screen
            Console.BackgroundColor = Background;
            Console.SetCursorPosition(SCOREX, SCOREY);
            Console.Write("Score : " + Score + " ");
            Console.SetCursorPosition(TIMERX, TIMERY);
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
                Console.Write(" ");
            }
            Console.BackgroundColor = Background;       //should this be ELSE where
        }
        public static void Limits(int scoreloss)
        {
            //function that set the limits of the games to the border of the window
            //when the player is supposed to be out of the screen he reappears on the other side
            //the parameter "scoreloss" is how much we want the score to drop when the player go through a wall
            if (SnakeX[0] < 0)
            {
                SnakeX[0] = WINDOWX - 1;
                Score -= scoreloss;
            }
            else if (SnakeX[0] >= WINDOWX)
            {
                SnakeX[0] = 0;
                Score -= scoreloss;
            }
            else if (SnakeY[0] < 0)
            {
                SnakeY[0] = GAMEY - 1;
                Score -= scoreloss;
            }
            else if (SnakeY[0] >= GAMEY)
            {
                SnakeY[0] = 0;
                Score -= scoreloss;
            }
        }
        public static void Spawn(out int X, out int Y)
        {
            //function to spawn something at a random location in the window based on the window size
            //the function loops trhough the snake coordinate and if it is in the snake will
            //give new random coordinate until it's not in the snake any more
            Random rnd = new Random();
            bool intail = false;
            do
            {
                X = rnd.Next(0, WINDOWX);
                Y = rnd.Next(0, GAMEY);
                for (int i = 0; i < SnakeSize; i++)
                {
                    if (X == SnakeX[i] && Y == SnakeY[i])
                    {
                        //check if it spawn in the snake
                        intail = true;
                        break;
                    }
                    intail = false;
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
            SnakeX[0] = rnd.Next(0 + BUFFER, WINDOWX - SnakeSize);   //make sure the whole snake is spawning in the window
            SnakeY[0] = rnd.Next(0, GAMEY);
            for (int i = 0; i < size - 1; i++)
            {
                SnakeX[i + 1] = SnakeX[i] + 1;
                SnakeY[i + 1] = SnakeY[i];
            }
        }
        /*public static void GetScreenSize(out int X, out int Y)
        {
            //function to get the size of the screen
            X = Console.WindowWidth;
            Y = Console.WindowHeight;
        }*/
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
            const double REDUCESPEED = 2.3;     //how much in ms the game will go faster
            OldFoodX = FoodX;
            OldFoodY = FoodY;
            Score += 10;
            GameSpeed -= REDUCESPEED;
            BiggerSnake(8);
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
            if (SnakeX[0] == WindowFoodX && SnakeY[0] == WindowFoodY && WindowFood)
                EatWindowFood();
        }
        public static void RandomDirWindowFood()
        {
            //function to establish a random starting direction for the window food 
            Random rnd = new Random();
            int Xdir;
            int Ydir;
            Xdir = rnd.Next(1, 3);  
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

            bool onsnake = false;
            if ((WindowFoodX == 0) || (WindowFoodX == FoodX + 1 && WindowFoodY == FoodY) ||
                (WindowFoodX == CrawlFoodX + 1 && WindowFoodY == CrawlFoodY && CrawlFood))
                WindowFoodDirX = 'E';
            else if ((WindowFoodX == WINDOWX - 1) || (WindowFoodX == FoodX - 1 && WindowFoodY == FoodY) ||
                (WindowFoodX == CrawlFoodX - 1 && WindowFoodY == CrawlFoodY && CrawlFood))
                WindowFoodDirX = 'W';
            if ((WindowFoodY == 0) || (WindowFoodX == FoodX && WindowFoodY == FoodY + 1) ||
                (WindowFoodX == CrawlFoodX && WindowFoodY == CrawlFoodY + 1 && CrawlFood))
                WindowFoodDirY = 'S';
            else if ((WindowFoodY == GAMEY - 1) || (WindowFoodX == FoodX && WindowFoodY == FoodY - 1) ||
                (WindowFoodX == CrawlFoodX && WindowFoodY == CrawlFoodY - 1 && CrawlFood))
                WindowFoodDirY = 'N';
            for (int i = 1; i < SnakeSize; i++)
            {
                if (WindowFoodX == SnakeX[i] && WindowFoodY == SnakeY[i] + 1)
                {
                    WindowFoodDirY = 'S';
                    onsnake = true;
                }
                else if (WindowFoodX == SnakeX[i] && WindowFoodY == SnakeY[i] - 1)
                {
                    WindowFoodDirY = 'N';
                    onsnake = true;
                }
                if (WindowFoodX == SnakeX[i] + 1 && WindowFoodY == SnakeY[i])
                {
                    WindowFoodDirX = 'E';
                    onsnake = true;
                }
                else if (WindowFoodX == SnakeX[i] - 1 && WindowFoodY == SnakeY[i])
                {
                    WindowFoodDirX = 'W';
                    onsnake = true;
                }
                if (onsnake)
                    break;
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
        public static void EatWindowFood()
        {
            //function that make the window food disapear and applies all the wanted effect
            //on the game when the snake eat it
            WindowFood = false;
            OldWindowFoodX = WindowFoodX;
            OldWindowFoodY = WindowFoodY;
            WINDOWX = Convert.ToInt32(WINDOWX * WINDOWSIZEGAIN);
            GAMEY = Convert.ToInt32(GAMEY * WINDOWSIZEGAIN);
            WINDOWY = GAMEY + SCOREZONEY;
            try
            {
                Console.SetWindowSize(WINDOWX, WINDOWY);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                WINDOWX = Convert.ToInt32(WINDOWX / WINDOWSIZEGAIN);
                GAMEY = Convert.ToInt32(GAMEY / WINDOWSIZEGAIN);
                WINDOWY = GAMEY + SCOREZONEY;
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