using System.Collections.Generic;
using System.Numerics;

Engine engine = new Engine();
engine.PlayerMove();

enum TypeDecoder
{
    CURSORE = 1,
    CASTLE = 101,
    PORT = 102,
    WARRIORS = 103,
    SHIELDERS = 104,
    ARCHERS = 105,
    SHIPS = 106,
    VILLAGE = 107,

    FOREST = 201,
    WATER = 202,
    MOUNTAINS = 203,

    VOID_LAND = 0,

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
    public SortedDictionary<TypeDecoder, Tuple<char, ConsoleColor>> Types;

    public Type()
    {
        this.Types = new SortedDictionary<TypeDecoder, Tuple<char, ConsoleColor>>();

        Types.Add(TypeDecoder.CURSORE,  new Tuple<char, ConsoleColor>('@', ConsoleColor.DarkYellow));

        Types.Add(TypeDecoder.CASTLE, new Tuple<char, ConsoleColor>('C', ConsoleColor.DarkGray));

        Types.Add(TypeDecoder.PORT, new Tuple<char, ConsoleColor>('P', ConsoleColor.DarkGray));

        Types.Add(TypeDecoder.WARRIORS, new Tuple<char, ConsoleColor>('w', ConsoleColor.DarkRed));

        Types.Add(TypeDecoder.SHIELDERS, new Tuple<char, ConsoleColor>('s', ConsoleColor.DarkRed));

        Types.Add(TypeDecoder.ARCHERS, new Tuple<char, ConsoleColor>('a', ConsoleColor.DarkRed));

        Types.Add(TypeDecoder.SHIPS, new Tuple<char, ConsoleColor>('S', ConsoleColor.DarkRed));

        Types.Add(TypeDecoder.VILLAGE, new Tuple<char, ConsoleColor>('V', ConsoleColor.DarkYellow));

        Types.Add(TypeDecoder.FOREST, new Tuple<char, ConsoleColor>('F', ConsoleColor.Green));

        Types.Add(TypeDecoder.WATER, new Tuple<char, ConsoleColor>('W', ConsoleColor.Blue));

        Types.Add(TypeDecoder.MOUNTAINS, new Tuple<char, ConsoleColor>('M', ConsoleColor.DarkGreen));

        Types.Add(TypeDecoder.VOID_LAND, new Tuple<char, ConsoleColor>('0', ConsoleColor.Black));

        Types.Add(TypeDecoder.PLAYER_LAND_1, new Tuple<char, ConsoleColor>('1', ConsoleColor.DarkBlue));
        Types.Add(TypeDecoder.PLAYER_LAND_2, new Tuple<char, ConsoleColor>('2', ConsoleColor.DarkMagenta));
        Types.Add(TypeDecoder.PLAYER_LAND_3, new Tuple<char, ConsoleColor>('3', ConsoleColor.DarkCyan));
        Types.Add(TypeDecoder.PLAYER_LAND_4, new Tuple<char, ConsoleColor>('4', ConsoleColor.Magenta));
        Types.Add(TypeDecoder.PLAYER_LAND_5, new Tuple<char, ConsoleColor>('5', ConsoleColor.Red));
        Types.Add(TypeDecoder.PLAYER_LAND_6, new Tuple<char, ConsoleColor>('6', ConsoleColor.Yellow));
        Types.Add(TypeDecoder.PLAYER_LAND_7, new Tuple<char, ConsoleColor>('7', ConsoleColor.Cyan));
        Types.Add(TypeDecoder.PLAYER_LAND_8, new Tuple<char, ConsoleColor>('8', ConsoleColor.Black));
        Types.Add(TypeDecoder.PLAYER_LAND_9, new Tuple<char, ConsoleColor>('9', ConsoleColor.White));
    }
}

class Element
{
    public int X;
    public int Y;
    public int ElementType;
    
    public Element()
    {
        ElementType = 0;
    }
    public Element(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public static Type TypeMap = new Type(); 
}

class UnitsStore
{
    public int XSize;
    public int YSize;
    public int UnitWichBuy;
    public UnitsStore()
    {
        UnitWichBuy = 0;
        XSize = 7;
        YSize = 0;
    } 
}

class Units : Element
{
    public bool Charge = true;
    public int Power;
    public int TheirCountry;
    public Units()
    {
        ElementType = 0;
    }
    public Units(ref Cursor coordinates, int power_value, int units_type, ref World map)
    {
        map.UnitsMap[coordinates.X, coordinates.Y].Power = power_value;
        map.UnitsMap[coordinates.X, coordinates.Y].ElementType = units_type;
        map.UnitsMap[coordinates.X, coordinates.Y].TheirCountry = map.CountriesLand[coordinates.X, coordinates.Y].ElementType;
    }
}

class Country
{
    public int Money;
    public int Income;
    public int NumberCastles;
    public Country()
    {
        this.Money = 100;
        this.Income = 80;
        this.NumberCastles = 1;
    }
}

class Cursor : Element
{
    public Cursor()
    {
        this.ElementType = 1;
        this.X = 0;
        this.Y = 0;
    }
}

class TerraPoint : Element
{
    public TerraPoint()
    {
        this.ElementType = 0;
    }
    public TerraPoint(int x_start, int y_start, int x_size, int y_size, int spawning_change, int terra_type, ref TerraPoint[,] map)
    {
        map[x_start, y_start].ElementType = terra_type;
        Random rundom = new Random();
        if(x_start > 0 && map[x_start - 1, y_start].ElementType == 0 && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start - 1, y_start, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (y_start > 0 && map[x_start, y_start - 1].ElementType == 0 && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start, y_start - 1, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (y_start < y_size - 1 && map[x_start, y_start + 1].ElementType == 0 && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start, y_start + 1, x_size, y_size, spawning_change, terra_type, ref map);
        }
        if (x_start < x_size - 1 && map[x_start + 1, y_start].ElementType == 0 && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start + 1, y_start, x_size, y_size, spawning_change, terra_type, ref map);
        }
    }
}

class World
{
    public Element[,] CountriesLand;
    public TerraPoint[,] Terra;
    public Units[,] UnitsMap;
    public Country[] Countries;
    public int XSize;
    public int YSize;

    public World(int X, int Y, int CountriesQuantity)
    {
        this.XSize = X;
        this.YSize = Y;
        this.CountriesLand = new Element[X, Y];
        this.Terra = new TerraPoint[X, Y];
        this.UnitsMap = new Units[X, Y];
        this.Countries = new Country[CountriesQuantity];

        for (int y = 0; y < this.YSize; y++)
        {
            for (int x = 0; x < this.XSize; x++)
            {
                this.CountriesLand[x, y] = new Element();
                this.Terra[x, y] = new TerraPoint();
                this.UnitsMap[x, y] = new Units();
            }
        }


        Spawn_countries(CountriesQuantity);

        Spawn_terra(35, (int)TypeDecoder.FOREST);

        Spawn_terra(40, (int)TypeDecoder.WATER);

        Spawn_terra(35, (int)TypeDecoder.MOUNTAINS);

    }

    private void Spawn_countries(int CountriesQuantity)
    {
        Random randomCoordinates = new Random();
        int XRandom;
        int YRandom;
        List<Element> Castles = new List<Element>();

        for (int spawnedCountries = 1; spawnedCountries <= CountriesQuantity; spawnedCountries++)
        {
            XRandom = randomCoordinates.Next(1, XSize - 1);
            YRandom = randomCoordinates.Next(1, YSize - 1);
            bool spawned = true;
            for (int i = 0; i < Castles.Count; i++)
            {
                if (Math.Abs(Castles[i].X - XRandom) < 4 && Math.Abs(Castles[i].Y - YRandom) < 4)
                {
                    spawned = false;
                    break;
                }
            }
            if (spawned)
            {
                Countries[spawnedCountries - 1] = new Country();
                UnitsMap[XRandom, YRandom].ElementType = (int)TypeDecoder.CASTLE;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        this.CountriesLand[XRandom - 1 + i, YRandom - 1 + j].ElementType = spawnedCountries + 700;
                        this.Terra[XRandom - 1 + i, YRandom - 1 + j].ElementType = spawnedCountries + 700;
                    }
                }
            }
        }
    }

    private void Spawn_terra(int SpawnChange, int TerraType)
    {
        for (int spawnDotes = 0; spawnDotes <= XSize / 10 || spawnDotes < YSize / 10; spawnDotes++)
        {
            Random randomCoordinates = new Random();
            int XRandom;
            int YRandom;

            int spawnTries = 0;
            do
            {
                XRandom = randomCoordinates.Next(0, XSize);
                YRandom = randomCoordinates.Next(0, YSize);
                spawnTries++;
            }
            while (this.Terra[XRandom, YRandom].ElementType != 0 && spawnTries < 10000);
            if (spawnTries < 10000)
            {
                TerraPoint Spawn_terra_type = new TerraPoint(XRandom, YRandom, XSize, YSize, SpawnChange, TerraType, ref this.Terra);
            }
        }
    }
}

class Drawing
{
    public static void DrawMap(ref World Map, ref Cursor Player)
    {
        Console.Clear();
        for(int y = 0; y < Map.YSize; y++)
        {
            for (int x = 0; x < Map.XSize; x++)
            {
                if (Player.X == x && Player.Y == y)
                {
                    Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                    Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Player.ElementType].Item2;
                    Console.Write(Element.TypeMap.Types[(TypeDecoder)Player.ElementType].Item1);
                }
                else if (Map.UnitsMap[x, y].ElementType != 0)
                {
                    Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                    Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Map.UnitsMap[x, y].ElementType].Item2;
                    Console.Write(Element.TypeMap.Types[(TypeDecoder)Map.UnitsMap[x, y].ElementType].Item1);
                }
                else if (Map.Terra[x, y].ElementType != 0)
                {
                    Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                    Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Map.Terra[x, y].ElementType].Item2;
                    Console.Write(Element.TypeMap.Types[(TypeDecoder)Map.Terra[x, y].ElementType].Item1);
                }
                else
                {
                    if (Map.CountriesLand[x, y].ElementType == 0)
                    {
                        Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item1);
                    }
                    else
                    {
                        Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                        Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item2;
                        Console.Write(Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[x, y].ElementType].Item1);
                    }
                }
                if (x == Map.XSize - 1)
                {
                    
                    Console.WriteLine();
                }
            }
        }
    }

    public static void DrawStore(Cursor Player)
    {
        Console.WriteLine("Store of units:");
        for (int units = (int)TypeDecoder.CASTLE; units <= (int)TypeDecoder.VILLAGE; units++)
        {
            Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)units].Item2;
            Console.Write(Element.TypeMap.Types[(TypeDecoder)units].Item1);
            Console.Write(' ');
        }
        Console.WriteLine();
        for (int units = (int)TypeDecoder.CASTLE; units <= (int)TypeDecoder.VILLAGE; units++)
        {
            if (Player.X == units - (int)TypeDecoder.CASTLE)
            {
                Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Player.ElementType].Item2;
                Console.Write(Element.TypeMap.Types[(TypeDecoder)Player.ElementType].Item1);
            }
            else
            {
                Console.Write(' ');
                Console.Write(' ');
            }
        }
        Console.WriteLine();

        Console.WriteLine((TypeDecoder)(Player.X + (int)TypeDecoder.CASTLE));
    }
}

class Engine
{
    public World Map;
    public UnitsStore Store;
    private Cursor player;
    private Cursor playerInStore;
    private int countryWichMove;

    public Engine():this(30, 20, 2)
    { }
    public Engine(int X, int Y, int CountryQuantity)
    {
        this.player = new Cursor();
        this.Map = new World(X, Y, CountryQuantity);
        this.Store = new UnitsStore();

        countryWichMove = (int)TypeDecoder.PLAYER_LAND_1;
    }
    public void PlayerMove()
    {
        bool storeOpen = false;
        bool finished = false;
        do
        {
            if (storeOpen)
            {
                Drawing.DrawMap(ref this.Map, ref this.playerInStore);
                Drawing.DrawStore(this.player);
            }
            else
            {
                Drawing.DrawMap(ref this.Map, ref this.player);
            }
            finished = playerInput(ref storeOpen);
        } while (!finished);
    }

    private bool playerInput(ref bool storeOpen)
    {
        int XSize;
        int YSize;
        if (storeOpen)
        {
            XSize = Store.XSize;
            YSize = Store.YSize;
        }
        else
        {
            XSize = Map.XSize;
            YSize = Map.YSize;
        }
        ConsoleKey input = Console.ReadKey().Key;
        if ((input == ConsoleKey.UpArrow || input == ConsoleKey.W) && this.player.Y != 0)
        {
            this.player.Y -= 1;
        }
        else if ((input == ConsoleKey.DownArrow || input == ConsoleKey.S) && this.player.Y != YSize - 1)
        {
            this.player.Y += 1;
        }
        else if ((input == ConsoleKey.LeftArrow || input == ConsoleKey.A) && this.player.X != 0)
        {
            this.player.X -= 1;
        }
        else if ((input == ConsoleKey.RightArrow || input == ConsoleKey.D) && this.player.X != XSize - 1)
        {
            this.player.X += 1;
        }
        else if(input == ConsoleKey.Spacebar)
        {
            if (!storeOpen)
            {
                storeOpen = true;
                playerInStore = player;
                player = new Cursor();
            }
            else
            {
                storeOpen = false;
                player = playerInStore;
            }
        }
        else if (input == ConsoleKey.Enter)
        {
            if (storeOpen)
            {
                Store.UnitWichBuy = player.X + (int)TypeDecoder.CASTLE;
                storeOpen = false;
                player = playerInStore;
            }
            else if(Store.UnitWichBuy != 0)
            {
                BuyUnits();
                Store.UnitWichBuy = 0;
            }else if(Map.UnitsMap[player.X, player.Y].ElementType >= (int)TypeDecoder.WARRIORS && Map.UnitsMap[player.X, player.Y].ElementType <= (int)TypeDecoder.SHIPS)
            {

            }
        }
        else if(input == ConsoleKey.Escape || input == ConsoleKey.Backspace)
        {
            if (storeOpen)
            {
                storeOpen = false;
                player = playerInStore;
            }
            return true;
        }
        return false;
    }
    private void BuyUnits()
    {
        if (Map.UnitsMap[player.X, player.Y].ElementType == 0 && Map.CountriesLand[player.X, player.Y].ElementType == countryWichMove)
        {
            new Units(ref player, 1, Store.UnitWichBuy, ref Map);
        }
    }
}
