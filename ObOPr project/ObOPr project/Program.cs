using System.Collections.Generic;

Engine engine = new Engine();
engine.Player_Move();

enum Type_decoder
{
    CURSORE = 1,
    CASTLE = 101,
    PORT = 102,
    WARRIORS = 103,
    SHIELDERS = 104,
    ARCHERS = 105,
    SHIPS = 106,

    FOREST = 201,
    WATER = 202,
    MOUNTAINS = 203,

    VOID_LAND = 500,

    PLAYER_LAND_1 = 701,
    PLAYER_LAND_2 = 702,
    PLAYER_LAND_3 = 703,
    PLAYER_LAND_4 = 704,
    PLAYER_LAND_5 = 705,
    PLAYER_LAND_6 = 706,
    PLAYER_LAND_7 = 707,
    PLAYER_LAND_8 = 708,
    PLAYER_LAND_9 = 709
}

class Type
{
    public SortedDictionary<Type_decoder, Tuple<char, ConsoleColor>> Types;

    public Type()
    {
        this.Types = new SortedDictionary<Type_decoder, Tuple<char, ConsoleColor>>();

        Types.Add(Type_decoder.CURSORE,  new Tuple<char, ConsoleColor>('@', ConsoleColor.DarkYellow));

        Types.Add(Type_decoder.CASTLE, new Tuple<char, ConsoleColor>('C', ConsoleColor.DarkGray));

        Types.Add(Type_decoder.PORT, new Tuple<char, ConsoleColor>('P', ConsoleColor.DarkGray));

        Types.Add(Type_decoder.WARRIORS, new Tuple<char, ConsoleColor>('w', ConsoleColor.DarkRed));

        Types.Add(Type_decoder.SHIELDERS, new Tuple<char, ConsoleColor>('s', ConsoleColor.DarkRed));

        Types.Add(Type_decoder.ARCHERS, new Tuple<char, ConsoleColor>('a', ConsoleColor.DarkRed));

        Types.Add(Type_decoder.SHIPS, new Tuple<char, ConsoleColor>('S', ConsoleColor.DarkRed));

        Types.Add(Type_decoder.FOREST, new Tuple<char, ConsoleColor>('F', ConsoleColor.Green));

        Types.Add(Type_decoder.WATER, new Tuple<char, ConsoleColor>('W', ConsoleColor.Blue));

        Types.Add(Type_decoder.MOUNTAINS, new Tuple<char, ConsoleColor>('M', ConsoleColor.DarkGreen));

        Types.Add(Type_decoder.VOID_LAND, new Tuple<char, ConsoleColor>('0', ConsoleColor.Black));

        Types.Add(Type_decoder.PLAYER_LAND_1, new Tuple<char, ConsoleColor>('1', ConsoleColor.DarkBlue));
        Types.Add(Type_decoder.PLAYER_LAND_2, new Tuple<char, ConsoleColor>('2', ConsoleColor.DarkMagenta));
        Types.Add(Type_decoder.PLAYER_LAND_3, new Tuple<char, ConsoleColor>('3', ConsoleColor.DarkCyan));
        Types.Add(Type_decoder.PLAYER_LAND_4, new Tuple<char, ConsoleColor>('4', ConsoleColor.Magenta));
        Types.Add(Type_decoder.PLAYER_LAND_5, new Tuple<char, ConsoleColor>('5', ConsoleColor.Red));
        Types.Add(Type_decoder.PLAYER_LAND_6, new Tuple<char, ConsoleColor>('6', ConsoleColor.Yellow));
        Types.Add(Type_decoder.PLAYER_LAND_7, new Tuple<char, ConsoleColor>('7', ConsoleColor.Cyan));
        Types.Add(Type_decoder.PLAYER_LAND_8, new Tuple<char, ConsoleColor>('8', ConsoleColor.Black));
        Types.Add(Type_decoder.PLAYER_LAND_9, new Tuple<char, ConsoleColor>('9', ConsoleColor.White));
    }
}

class Element
{
    public int X;
    public int Y;
    public int Element_type;
    
    public Element()
    {
        Element_type = 500;
    }

    public static Type Type_map = new Type(); 
}

class Units : Element
{
    public bool Charge = true;
    public int Power;
    public int Their_country;
    public Units()
    {
        Element_type = 500;
    }
    public Units(ref Cursor coordinates, int power_value, int units_type, ref World map)
    {
        map.Units_map[coordinates.X, coordinates.Y].Power = power_value;
        map.Units_map[coordinates.X, coordinates.Y].Element_type = units_type;
        map.Units_map[coordinates.X, coordinates.Y].Their_country = map.Countries_Land[coordinates.X, coordinates.Y].Element_type;
    }
}

class Country
{
    public int Number_Castles;
    public Country()
    {
        this.Number_Castles = 1;
    }
}

class Cursor : Element
{
    public Cursor()
    {
        this.Element_type = 1;
        this.X = 0;
        this.Y = 0;
    }
}

class Terra_point : Element
{
    public Terra_point()
    {
        this.Element_type = 500;
    }
    public Terra_point(int x_start, int y_start, int x_size, int y_size, int spawning_change, int terra_type, ref Terra_point[,] map)
    {
        map[x_start, y_start].Element_type = terra_type;
        Random rundom = new Random();
        if(x_start > 0 && map[x_start - 1, y_start].Element_type == 500 && rundom.Next(0, 99) < spawning_change)
        {
            new Terra_point(x_start - 1, y_start, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (y_start > 0 && map[x_start, y_start - 1].Element_type == 500 && rundom.Next(0, 99) < spawning_change)
        {
            new Terra_point(x_start, y_start - 1, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (y_start < y_size - 1 && map[x_start, y_start + 1].Element_type == 500 && rundom.Next(0, 99) < spawning_change)
        {
            new Terra_point(x_start, y_start + 1, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (x_start < x_size - 1 && map[x_start + 1, y_start].Element_type == 500 && rundom.Next(0, 99) < spawning_change)
        {
            new Terra_point(x_start + 1, y_start, x_size, y_size, spawning_change, terra_type, ref map);
        }
    }
}

class World
{
    public Element[,] Countries_Land;
    public Terra_point [,] Terra;
    public Units[,] Units_map;
    public Country[] Countries;
    public int X_size;
    public int Y_size;

    public World(int X, int Y, int Countries_quantity)
    {
        this.X_size = X;
        this.Y_size = Y;
        this.Countries_Land = new Element[X, Y];
        this.Terra = new Terra_point[X, Y];
        this.Units_map = new Units[X, Y];
        this.Countries = new Country[Countries_quantity];

        for (int y = 0; y < this.Y_size; y++)
        {
            for (int x = 0; x < this.X_size; x++)
            {
                this.Countries_Land[x, y] = new Element();
                this.Terra[x, y] = new Terra_point();
                this.Units_map[x, y] = new Units();
            }
        }


        Spawn_countries(Countries_quantity);

        Spawn_terra(40, 201);

        Spawn_terra(40, 202);

        Spawn_terra(30, 203);

    }

    private void Spawn_countries(int Countries_quantity)
    {
        Random random_coordinates = new Random();
        int X_random;
        int Y_random;

        for (int spawned_countries = 1; spawned_countries <= Countries_quantity; spawned_countries++)
        {
            Countries[spawned_countries - 1] = new Country();
            X_random = random_coordinates.Next(1, X_size - 1);
            Y_random = random_coordinates.Next(1, Y_size - 1);
            Units_map[X_random, Y_random].Element_type = (int)Type_decoder.CASTLE;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.Countries_Land[X_random - 1 + i, Y_random - 1 + j].Element_type = spawned_countries + 700;
                    this.Terra[X_random - 1 + i, Y_random - 1 + j].Element_type = spawned_countries + 700;
                }
            }
        }
    }

    private void Spawn_terra(int Spawn_change, int Terra_type)
    {
        for (int spawn_dotes = 0; spawn_dotes <= X_size / 10 || spawn_dotes < Y_size / 10; spawn_dotes++)
        {
            Random random_coordinates = new Random();
            int X_random;
            int Y_random;

            int spawn_tries = 0;
            do
            {
                X_random = random_coordinates.Next(0, X_size);
                Y_random = random_coordinates.Next(0, Y_size);
                spawn_tries++;
            }
            while (this.Terra[X_random, Y_random].Element_type != 500 && spawn_tries < 10000);
            if (spawn_tries < 10000)
            {
                Terra_point Spawn_terra_type = new Terra_point(X_random, Y_random, X_size, Y_size, Spawn_change, Terra_type, ref this.Terra);
            }
        }
    }

}

class Drawing
{
    public static void Draw_map(ref World Map, ref Cursor Cursore)
    {
        Console.Clear();
        for(int y = 0; y < Map.Y_size; y++)
        {
            for (int x = 0; x < Map.X_size; x++)
            {
                if (Cursore.X == x && Cursore.Y == y)
                {
                    Console.BackgroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                    Console.ForegroundColor = Element.Type_map.Types[(Type_decoder)Cursore.Element_type].Item2;
                    Console.Write(Element.Type_map.Types[(Type_decoder)Cursore.Element_type].Item1);
                }
                else if (Map.Units_map[x, y].Element_type != 500)
                {
                    Console.BackgroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                    Console.ForegroundColor = Element.Type_map.Types[(Type_decoder)Map.Units_map[x, y].Element_type].Item2;
                    Console.Write(Element.Type_map.Types[(Type_decoder)Map.Units_map[x, y].Element_type].Item1);
                }
                else if (Map.Terra[x, y].Element_type != 500)
                {
                    Console.BackgroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                    Console.ForegroundColor = Element.Type_map.Types[(Type_decoder)Map.Terra[x, y].Element_type].Item2;
                    Console.Write(Element.Type_map.Types[(Type_decoder)Map.Terra[x, y].Element_type].Item1);
                }
                else
                {
                    if (Map.Countries_Land[x, y].Element_type == 500)
                    {
                        Console.BackgroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item1);
                    }
                    else
                    {
                        Console.BackgroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                        Console.ForegroundColor = Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item2;
                        Console.Write(Element.Type_map.Types[(Type_decoder)Map.Countries_Land[x, y].Element_type].Item1);
                    }
                }
                if (x == Map.X_size - 1)
                {
                    
                    Console.WriteLine();
                }
            }
        }
    }
}

class Engine
{
    public World map;
    private Cursor player;

    public Engine():this(30, 20, 2)
    { }
    public Engine(int X, int Y, int Country_quantity)
    {
        this.player = new Cursor();
        this.map = new World(X, Y, Country_quantity); 


    }
    public void Player_Move()
    {
        bool finished = false;
        do
        {
            Drawing.Draw_map(ref this.map, ref this.player);
            finished = player_Input();
        } while (!finished);
    }

    private bool player_Input()
    {
        ConsoleKey input = Console.ReadKey().Key;
        if ((input == ConsoleKey.UpArrow || input == ConsoleKey.W) && this.player.Y != 0)
        {
            this.player.Y -= 1;
        }
        else if ((input == ConsoleKey.DownArrow || input == ConsoleKey.S) && this.player.Y != map.Y_size - 1)
        {
            this.player.Y += 1;
        }
        else if ((input == ConsoleKey.LeftArrow || input == ConsoleKey.A) && this.player.X != 0)
        {
            this.player.X -= 1;
        }
        else if ((input == ConsoleKey.RightArrow || input == ConsoleKey.D) && this.player.X != map.X_size - 1)
        {
            this.player.X += 1;
        }else if(input == ConsoleKey.Spacebar) { }
        else if (input == ConsoleKey.Enter) { }
        else if(input == ConsoleKey.Escape || input == ConsoleKey.Backspace)
        {
            return true;
        }
        return false;
    }
}
