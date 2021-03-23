using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace event_task
{
    class Program
    {
        static int i = 1;
        static int maxEnergy = 2000;
        static int minEnergy = 100;
   
        class Game
        {
            

            private static void DisplayMessage(string message, int i)
            {
                Console.SetCursorPosition(0, y+1);

                Console.Write(new String(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, y + 1);

                Console.Write(i+". "+message);
            }

            private static void DisplayRedMessage(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, y + 2);
                Console.WriteLine(message);
            }


            static Walls walls;
            static List <Fish> fishes = new List<Fish>();
            static Cat cat;

            static readonly int x = 80;
            static readonly int y = 26;

            static void Main()
            {

                Console.CursorVisible = false;
                
                walls = new Walls(x, y, '#');
                cat = new Cat(x / 2, y / 2);
                for (int i = 0; i < 50; i++)
                {
                    Fish f = new Fish(x, y, '@');
                    f.CreateFish();
                    fishes.Add(f);
                }
                cat.Spend += DisplayMessage;
                cat.Earn += DisplayMessage;
                cat.Deficit += DisplayRedMessage;
                cat.Plenty += DisplayRedMessage;
                bool game = true;
                while (game)
                {
                    ConsoleKeyInfo cki;
                    cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.UpArrow:
                            Point p = (cat.cat.x, cat.cat.y - 1, '#');
                            if (!walls.notFree(p) && cat.Energy >= minEnergy && cat.Energy <= maxEnergy)
                            {
                                cat.spendEnergy();
                                cat.Move(cat.cat.x, cat.cat.y - 1);
                                foreach (Fish f in fishes)
                                    if (f.fish == cat.cat)
                                        cat.Eat(f);
                                
                            }
                            else if (cat.Energy < minEnergy || cat.Energy > maxEnergy)
                            {
                                game = false;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            p = (cat.cat.x, cat.cat.y + 1, '#');
                            if (!walls.notFree(p) && cat.Energy >= minEnergy && cat.Energy <= maxEnergy)
                            {
                                cat.spendEnergy();
                                cat.Move(cat.cat.x, cat.cat.y + 1);

                                foreach (Fish f in fishes)
                                    if (f.fish == cat.cat)
                                        cat.Eat(f);
                            }
                            else if (cat.Energy < minEnergy || cat.Energy > maxEnergy)
                            {
                                game = false;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            p = (cat.cat.x - 1, cat.cat.y, '#');
                            if (!walls.notFree(p) && cat.Energy >= minEnergy && cat.Energy <= maxEnergy)
                            {
                                cat.spendEnergy();
                                cat.Move(cat.cat.x-1, cat.cat.y);

                                foreach (Fish f in fishes)
                                    if (f.fish == cat.cat)
                                        cat.Eat(f);

                            }
                            else if (cat.Energy < minEnergy || cat.Energy > maxEnergy)
                            {
                                game = false;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            p = (cat.cat.x + 1, cat.cat.y, '#');
                            if (!walls.notFree(p) && cat.Energy >= minEnergy && cat.Energy <= maxEnergy)
                            {
                                cat.spendEnergy();
                                cat.Move(cat.cat.x + 1, cat.cat.y);

                                foreach (Fish f in fishes)
                                    if (f.fish == cat.cat)
                                        cat.Eat(f);
                            }
                            else if (cat.Energy < minEnergy || cat.Energy > maxEnergy)
                            {
                                game = false;
                            }
                            break;
                        default: break;
                    }

                }// Main()

            }// class Game

            struct Point
            {
                public int x { get; set; }
                public int y { get; set; }
                public char ch { get; set; }

                public static implicit operator Point((int, int, char) value) =>
                    new Point { x = value.Item1, y = value.Item2, ch = value.Item3 };

                public static bool operator ==(Point a, Point b) =>
                    (a.x == b.x && a.y == b.y) ? true : false;
                public static bool operator !=(Point a, Point b) =>
                    (a.x != b.x || a.y != b.y) ? true : false;

                public void Draw()
                {
                    DrawPoint(ch);
                }
                public void Clear()
                {
                    DrawPoint(' ');
                }
                private void DrawPoint(char _ch)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(_ch);
                }
            }
            class Walls
            {
                private char ch;
                private List<Point> wall = new List<Point>();

                public Walls(int x, int y, char ch)
                {
                    this.ch = ch;
                    DrawHorizontal(x, 0);
                    DrawHorizontal(x, y);
                    DrawVertical(0, y);
                    DrawVertical(x, y);
                }

                private void DrawHorizontal(int x, int y)
                {
                    for (int i = 0; i < x; i++)
                    {
                        Point p = (i, y, ch);
                        p.Draw();
                        wall.Add(p);
                    }
                }
                private void DrawVertical(int x, int y)
                {
                    for (int i = 0; i < y; i++)
                    {
                        Point p = (x, i, ch);
                        p.Draw();
                        wall.Add(p);
                    }
                }
                public bool notFree(Point p)
                {
                    foreach (var w in wall)
                    {
                        if (p == w)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }// class Walls

            class Fish
            {
                int x;
                int y;
                char ch;
                private int energy;
                public Point fish { get; private set; }

                Random random = new Random();

                public Fish(int x, int y, char ch)
                {
                    this.x = x;
                    this.y = y;
                    this.ch = ch;
                    energy = 500;
                }

                public int Energy
                {
                    get { return this.energy; }
                    set
                    {
                        this.energy = value;
                    }
                }
                public void CreateFish()
                {
                    fish = (random.Next(2, x - 2), random.Next(2, y - 2), ch);
                    fish.Draw();
                }
            }//class Fish


            class Cat
            {
                public delegate void EndHandler(string message);
                public event EndHandler Plenty;
                public event EndHandler Deficit;

                public delegate void GameHandler(string message, int i);
                public event GameHandler Spend;
                public event GameHandler Earn;

                private int step_energy;
                public Point cat;
                private int energy;
                public Cat(int x, int y)
                {
                    Point p = (x, y, '*');
                    cat = p;
                    p.Draw();
                    energy = 1000;
                    step_energy = 100;
                }

                public bool notFree(Point p)
                {
                    if (cat == p) return true;
                    return false;
                }

                public int Energy
                {
                    get { return energy; }
                    set { energy = value; }
                }
                public void spendEnergy()
                {
                    this.Energy = this.Energy - step_energy;
                    if (this.Energy == 0)
                        Deficit.Invoke("У вас не осталось энергии!");
                }

                public void Move(int x, int y)
                {
                    cat.Clear();
                    cat.x = x;
                    cat.y = y;
                    cat.Draw();
                    Spend.Invoke($"За ход вы потратили {step_energy} энергии, всего осталось {this.Energy}", i++);
                }
                public void Eat(Fish f)
                {
                    this.Energy += 500;
                    Earn.Invoke($"Вы скушали рыбку, прибавилось {f.Energy} энергии, всего осталось {this.Energy}", i++);
                    if (this.Energy > maxEnergy)
                        Plenty.Invoke("Кошка заболела из-за избытка энергии");
                }

            }//class Cat

        }
    }
}
