using System;
using System.Timers;

namespace Snake_Game
{
    class Program
    {
        static int MeX;
        static int MeY;
        static char MeDir;  //Direction in witch the player is going (west, east, north, south)
        static int OldMeX;
        static int OldMeY;
        static char MeChar = 'X';

        static bool SketchOn = false;
        static char SketchChar = 'O';

        static bool GlideOn = false;
        static bool NewKeyStroke = false;

        static void Main(string[] args)
        {
            //main function of the game calls other fucntion. Egine of the game
            bool gameover = false;
            Console.CursorVisible = false;
            Spawn(out MeX, out MeY);
            while (!gameover)
            {
                GetkeyStroke();
                Move();
                Limits();
                Draw();
                Wait(50);
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
                switch (k)
                {
                    case ConsoleKey.D:
                        MeDir = 'E';
                        NewKeyStroke = true;
                        break;
                    case ConsoleKey.A:
                        MeDir = 'W';
                        NewKeyStroke = true;
                        break;
                    case ConsoleKey.W:
                        MeDir = 'N';
                        NewKeyStroke = true;
                        break;
                    case ConsoleKey.S:
                        MeDir = 'S';
                        NewKeyStroke = true;
                        break;
                    case ConsoleKey.Q:
                        if (!SketchOn)
                            SketchOn = true;
                        else
                            SketchOn = false;
                        break;
                    case ConsoleKey.E:
                        if (!GlideOn)
                            GlideOn = true;
                        else
                            GlideOn = false; ;
                        break;
                }
            }
        }
        public static void Move()
        {
            //function to move to player (either glide or move a spot)
            if (GlideOn || NewKeyStroke)
            {
                NewKeyStroke = false;
                OldMeX = MeX;
                OldMeY = MeY;
                switch (MeDir)
                {
                    case 'N':
                        MeY--;
                        break;
                    case 'S':
                        MeY++;
                        break;
                    case 'W':
                        MeX--;
                        break;
                    case 'E':
                        MeX++;
                        break;
                }
            }
        }
        public static void Draw()
        {
            //function to update the screen and draw all the elements on the screen
            Console.SetCursorPosition(MeX, MeY);
            Console.Write(MeChar);
            Console.SetCursorPosition(OldMeX, OldMeY);
            if (!SketchOn)
                Console.Write(" ");
            else
                Console.Write(SketchChar);
        }
        public static void Limits()
        {
            //function that set the limits of the games to the border of the window
            //when the player is supposed to be out of the screen he reappears on the other side
            int width;
            int height;
            GetScreenSize(out width, out height);
            if (MeX < 0)
                MeX = width - 1;
            else if (MeX >= width)
                MeX = 0;
            else if (MeY < 0)
                MeY = height - 1;
            else if (MeY >= height)
                MeY = 0;
        }
        public static void Spawn(out int X, out int Y)
        {
            //funciton to spawn something at  random location in the window based on the window size
            Random rnd = new Random();
            int width;
            int lenght;
            GetScreenSize(out width, out lenght);
            X = rnd.Next(0, width);
            Y = rnd.Next(0, lenght);
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
    }
}
