using System;

namespace Snake_Game
{
    class Program
    {
        static int SnakeSize = 3;
        static int[] SnakeX = new int[SnakeSize];
        static int[] SnakeY = new int[SnakeSize];
        static char SnakeDir; //Direction in witch the player is going (west, east, north, south)
        static int OldSnakeX;
        static int OldSnakeY;
        static char SnakeChar = 'X';

        static int FoodX;
        static int FoodY;
        static int OldFoodX;
        static int OldFoodY;
        static char FoodChar = '$';

        static bool SketchOn = false;
        static char SketchChar = 'O';

        static bool GlideOn = false;
        static bool NewKeyStroke = false;   //Make the program only move the player if he it an array if he isnt in glide mode

        static int Score = 10;
        static int ScoreX = 1;  
        static int ScoreY = 1;  

        static void Main(string[] args)
        {
            //main function of the game calls other fucntion. Egine of the game
            bool gameover = false;
            Console.Clear();
            Console.CursorVisible = false;
            SpawnSnake(SnakeX, SnakeY, SnakeSize);
            Spawn(out FoodX, out FoodY);
            while (!gameover)
            {
                GetkeyStroke();
                if (GlideOn || NewKeyStroke)
                    Move();
                if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
                    EatFood();
                Limits();
                Draw();
                Wait(150);
                gameover = GameOver();
            }
        }
        public static void GetkeyStroke()
        {
            //function to get key stroke and change value of variable based on user input
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
            Console.SetCursorPosition(ScoreX, ScoreY);
            Console.Write("Score: " + Score);
            Console.SetCursorPosition(FoodX, FoodY);
            Console.Write(FoodChar);
            for (int i = 0; i < SnakeSize; i++)
            {
                Console.SetCursorPosition(SnakeX[i], SnakeY[i]);
                Console.Write(SnakeChar);
            }
            Console.SetCursorPosition(OldSnakeX, OldSnakeY);
            if (!SketchOn)
                //delete old player position if the player isnt in sketch mode
                Console.Write(" ");
            else
                //draw the sketch character if the mode is on
                Console.Write(SketchChar);
            if (SnakeX[0] == FoodX && SnakeY[0] == FoodY)
            { //make the old food disapear if the player ate it
                Console.SetCursorPosition(OldFoodX, OldFoodY);
                Console.Write(" ");
            }
        }
        public static void Limits()
        {
            //function that set the limits of the games to the border of the window
            //when the player is supposed to be out of the screen he reappears on the other side
            int width;
            int height;
            GetScreenSize(out width, out height);
            for (int i = 0; i < SnakeSize; i++)
            {
                if (SnakeX[i] < 0)
                    SnakeX[i] = width - 1;
                else if (SnakeX[i] >= width)
                    SnakeX[i] = 0;
                else if (SnakeY[i] < 0)
                    SnakeY[i] = height - 1;
                else if (SnakeY[i] >= height)
                    SnakeY[i] = 0;
            }
        }
        public static void Spawn(out int X, out int Y)
        {
            //funciton to spawn something at random location in the window based on the window size
            Random rnd = new Random();
            int width;
            int lenght;
            GetScreenSize(out width, out lenght);
            X = rnd.Next(0, width);
            Y = rnd.Next(0, lenght);
        }
        public static void SpawnSnake(int[] X, int[] Y, int size)
        {
            //Spawn a snake of a given lenght on the X axis\
            Random rnd = new Random();
            int width;
            int lenght;
            GetScreenSize(out width, out lenght);
            X[0] = rnd.Next(0, width);
            Y[0] = rnd.Next(0, lenght);
            for (int i = 0; i < size - 1; i++)
            {
                X[i + 1] = X[i] + 1;
                Y[i + 1] = Y[i];
            }
        }
        public static void GetScreenSize(out int X, out int Y)
        {
            //function to get the size of the screen
            X = Console.WindowWidth;
            Y = Console.WindowHeight;
        }
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
            OldFoodX = FoodX;
            OldFoodY = FoodY;
            Score += 10;
            Spawn(out FoodX, out FoodY);
            BiggerSnake();
        }
        public static void BiggerSnake()
        {
            //function to make the snake bigger. basicly make new arrays for the snake position
            //and put the new position at the position od the old snake end
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
    }
}