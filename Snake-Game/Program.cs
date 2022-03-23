using System;

namespace Snake_Game
{
    class Program
    {
        //Snake variable declaration section
        static int SnakeSize = 10;
        static int[] SnakeX = new int[SnakeSize];
        static int[] SnakeY = new int[SnakeSize];
        static char SnakeDir = 'W'; //Direction in witch the player is going (west, east, north, south)
        static int OldSnakeX;
        static int OldSnakeY;
        static ConsoleColor SnakeColor = ConsoleColor.Blue;

        //basic food item(coin) variable declaration section
        static int FoodX;
        static int FoodY;
        static int OldFoodX;
        static int OldFoodY;

        //sketch mode variable declaration section
        static bool SketchOn = false;
        static char SketchChar = 'O';

        //Movement variable declaration section
        static bool GlideOn = true;
        static bool NewKeyStroke = false;   //Make the program only move the player if he it a key if he isn't in glide mode

        //score variables declaration section
        static int Score = 10;

        //Overall game variables declaration section
        const int WINDOWX = 62;   
        const int WINDOWY = 35;    
        static ConsoleColor Background = ConsoleColor.DarkGray; 
        static double GameSpeed = 60;

        static void Main(string[] args)
        {
            //main function of the game calls other fucntion. Egine of the game
            bool gameover = false;
            Console.SetWindowSize(WINDOWX, WINDOWY);
            Console.CursorVisible = false;
            SetBackgroundColor(Background);
            SpawnSnake(SnakeX, SnakeY, SnakeSize);
            Spawn(out FoodX, out FoodY);
            while (!gameover)
            {
                GetkeyStroke();
                if (GlideOn || NewKeyStroke)
                    //only move the snake if there is a new keystroke or the glidemode is on
                    Move();
                if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
                    EatFood();
                Limits(10);
                Draw();
                Wait(GameSpeed);
                gameover = GameOver();
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
                else if (k == ConsoleKey.Q)
                {
                    if (!SketchOn)
                        SketchOn = true;
                    else
                        SketchOn = false;
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
            const int SCOREX = 1;
            const int SCOREY = 1;
            string foodchar = "$";
            ConsoleColor foodbackground = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(SCOREX, SCOREY);
            Console.Write("Score: " + Score + " ");   //the blank space after the score is to clear any remaining digit if the number of digit of the score goes down
            Console.SetCursorPosition(FoodX, FoodY);
            Console.BackgroundColor = foodbackground;
            Console.Write(foodchar);
            for (int i = 0; i < SnakeSize; i++)
            {
                //go through the array and draw every part of the snake 
                Console.SetCursorPosition(SnakeX[i], SnakeY[i]);
                Console.BackgroundColor = SnakeColor;
                Console.Write(" ");
            }
            Console.BackgroundColor = Background;
            Console.SetCursorPosition(OldSnakeX, OldSnakeY);
            if (!SketchOn)
                //delete old player position if the player isnt in sketch mode  
                Console.Write(" ");
            else
                //draw the sketch character if the mode is on
                Console.Write(SketchChar);
            if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
            {
                //make the old food disapear if the player ate it
                Console.SetCursorPosition(OldFoodX, OldFoodY);
                Console.Write(" ");
            }
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
                SnakeY[0] = WINDOWY - 1;
                Score -= scoreloss;
            }
            else if (SnakeY[0] >= WINDOWY)
            {
                SnakeY[0] = 0;
                Score -= scoreloss;
            }
        }
        public static void Spawn(out int X, out int Y)
        {
            //function to spawn something at aerandom location in the window based on the window size
            //the function check after spawning the item if it is located in the snake it will
            //give new random coordinate until it's not in the snake any more
            Random rnd = new Random();
            bool intail = false;
            do
            {
                X = rnd.Next(0, WINDOWX);
                Y = rnd.Next(0, WINDOWY);
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
        public static void SpawnSnake(int[] X, int[] Y, int size)
        {
            //Spawn a snake of a given lenght on the X
            const int BUFFER = 15;      //the minimum distance the snake can spawn from the border he is going in
            Random rnd = new Random();
            X[0] = rnd.Next(0 + BUFFER, WINDOWX - size);   //make sure the whole snake is spawning in the window
            Y[0] = rnd.Next(0, WINDOWY);
            for (int i = 0; i < size - 1; i++)
            {
                X[i + 1] = X[i] + 1;
                Y[i + 1] = Y[i];
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
            //function to delay the game a given amount of time
            DateTime start = DateTime.Now;
            DateTime stop = DateTime.Now.AddMilliseconds(waittime);
            while (start < stop)
                start = DateTime.Now;
        }
        public static void EatFood()
        {
            //function to make a new food spawn and make the score go up if the player ate it
            ///add arguments
            const double REDUCESPEED = 2.2;
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
            //the values in the new array
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
        public static bool GameOver()
        {
            //Check if the snake head is colliding with the tail
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
            //function to set the whole background of the console to a color
            Console.BackgroundColor = consoleColor;
            Console.Clear();
        }

    }
}