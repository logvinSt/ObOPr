using System;
using System.Collections.Generic;
using System.Numerics;

Engine engine = new Engine();
engine.Game();

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

    VOID = 0,

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

        Types.Add(TypeDecoder.VOID, new Tuple<char, ConsoleColor>('0', ConsoleColor.Black));

        Types.Add(TypeDecoder.PLAYER_LAND_1, new Tuple<char, ConsoleColor>('1', ConsoleColor.DarkCyan));
        Types.Add(TypeDecoder.PLAYER_LAND_2, new Tuple<char, ConsoleColor>('2', ConsoleColor.DarkMagenta));
        Types.Add(TypeDecoder.PLAYER_LAND_3, new Tuple<char, ConsoleColor>('3', ConsoleColor.Cyan));
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
        ElementType = (int)TypeDecoder.VOID;
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
        ElementType = (int)TypeDecoder.VOID;
    }
    public Units(ref Cursor coordinates, int power_value, int units_type, ref World map)
    {
        switch (units_type) {
            case (int)TypeDecoder.WARRIORS:
                map.UnitsMap[coordinates.X, coordinates.Y] = new Warriors();
                break;
            case (int)TypeDecoder.SHIELDERS:
                map.UnitsMap[coordinates.X, coordinates.Y] = new Shielders();
                break;
            case (int)TypeDecoder.ARCHERS:
                map.UnitsMap[coordinates.X, coordinates.Y] = new Archers();
                break;
            case (int)TypeDecoder.SHIPS:
                map.UnitsMap[coordinates.X, coordinates.Y] = new Ships();
                break;

        }

        map.UnitsMap[coordinates.X, coordinates.Y].Power = power_value;
        map.UnitsMap[coordinates.X, coordinates.Y].ElementType = units_type;
        map.UnitsMap[coordinates.X, coordinates.Y].TheirCountry = map.CountriesLand[coordinates.X, coordinates.Y].ElementType;
        Charge = true;
    }

    public static bool CanArchersAttack(Cursor UsingUnit, Cursor NowPoint, ref World map)
    {
        if(map.Terra[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.FOREST)
        {
            return false;
        }
        if (map.Terra[UsingUnit.X, UsingUnit.Y].ElementType == (int)TypeDecoder.MOUNTAINS && Math.Abs(NowPoint.X - UsingUnit.X) < 4 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 4)
        {
            return true;
        }
        else if(map.Terra[UsingUnit.X, UsingUnit.Y].ElementType != (int)TypeDecoder.FOREST && Math.Abs(NowPoint.X - UsingUnit.X) < 3 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 3)
        {
            return true;
        }
        return false;
    }
    public virtual void Move(Cursor UsingUnit, Cursor  NowPoint, ref World map, int countryWichMove)
    {
        switch (map.UnitsMap[UsingUnit.X, UsingUnit.Y].ElementType)
        {
            case (int)TypeDecoder.WARRIORS:
                map.UnitsMap[NowPoint.X, NowPoint.Y] = new Warriors();
                break;
            case (int)TypeDecoder.SHIELDERS:
                map.UnitsMap[NowPoint.X, NowPoint.Y] = new Shielders();
                break;
            case (int)TypeDecoder.ARCHERS:
                map.UnitsMap[NowPoint.X, NowPoint.Y] = new Archers();
                break;
            case (int)TypeDecoder.SHIPS:
                map.UnitsMap[NowPoint.X, NowPoint.Y] = new Ships();
                break;

        }

        if (map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.VOID)
        {
            map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Income -= 10;
        }
        if (map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType != map.UnitsMap[UsingUnit.X, UsingUnit.Y].TheirCountry)
        {
            map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Income += 10;
        }

        map.UnitsMap[NowPoint.X, NowPoint.Y].Power = map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power;
        map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType = map.UnitsMap[UsingUnit.X, UsingUnit.Y].ElementType;
        map.UnitsMap[NowPoint.X, NowPoint.Y].TheirCountry = map.UnitsMap[UsingUnit.X, UsingUnit.Y].TheirCountry;
        map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType = map.UnitsMap[UsingUnit.X, UsingUnit.Y].TheirCountry;
        map.UnitsMap[NowPoint.X, NowPoint.Y].Charge = false;

        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.MOUNTAINS)
        {    
            map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[1].Add(new Element(NowPoint.X, NowPoint.Y));
        }
        else
        {
            map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Add(new Element(NowPoint.X, NowPoint.Y));
        }

        map.UnitsMap[UsingUnit.X, UsingUnit.Y].ElementType = (int)TypeDecoder.VOID;
    }
}

class Warriors : Units
{
    public override void Move(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VOID && map.Terra[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else if(map.Terra[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            this.Attack(UsingUnit, NowPoint, ref map, countryWichMove);
        }
    }
    private void Attack(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {

        int newPower;
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.ARCHERS)
        {
            newPower = map.UnitsMap[NowPoint.X, NowPoint.Y].Power - map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power * 2;
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power -= map.UnitsMap[NowPoint.X, NowPoint.Y].Power / 2;
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power = newPower;

        }
        else
        {
            newPower = map.UnitsMap[NowPoint.X, NowPoint.Y].Power - map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power * 2;
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power -= map.UnitsMap[NowPoint.X, NowPoint.Y].Power;
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power = newPower;
        }
        if (map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power <= 0)
        {
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].ElementType = (int)TypeDecoder.VOID;
        }
        if(map.UnitsMap[NowPoint.X, NowPoint.Y].Power <= 0)
        {
            if(map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.CASTLE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].NumberCastles--;
            }
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VILLAGE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Income -= 10;
            }
            map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType = (int)TypeDecoder.VOID;
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else
        {
            this.Charge = false;
            map.Countries[map.CountriesLand[UsingUnit.X, UsingUnit.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Add(new Element(NowPoint.X, NowPoint.Y));
        }
    }
}
class Shielders : Units
{
    public override void Move(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VOID && map.Terra[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else if (map.Terra[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            this.Attack(UsingUnit, NowPoint, ref map, countryWichMove);
        }
    }
    private void Attack(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {

        int newPower;
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.ARCHERS)
        {
            newPower = map.UnitsMap[NowPoint.X, NowPoint.Y].Power - map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power;
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power -= map.UnitsMap[NowPoint.X, NowPoint.Y].Power / 4;
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power = newPower;

        }
        else
        {
            newPower = map.UnitsMap[NowPoint.X, NowPoint.Y].Power - map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power;
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power -= map.UnitsMap[NowPoint.X, NowPoint.Y].Power / 2;
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power = newPower;
        }
        if (map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power <= 0)
        {
            map.UnitsMap[UsingUnit.X, UsingUnit.Y].ElementType = (int)TypeDecoder.VOID;
        }
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].Power <= 0)
        {
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.CASTLE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].NumberCastles--;
            }
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VILLAGE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Income -= 10;
            }
            map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType = (int)TypeDecoder.VOID;
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else
        {
            this.Charge = false;
            map.Countries[map.CountriesLand[UsingUnit.X, UsingUnit.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Add(new Element(NowPoint.X, NowPoint.Y));
        }
    }
}
class Archers : Units
{
    public override void Move(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {
        bool canAttack = Units.CanArchersAttack(UsingUnit, NowPoint, ref map);
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VOID && map.Terra[NowPoint.X, NowPoint.Y].ElementType != (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else if (canAttack && this.Charge)
        {
            this.Attack(UsingUnit, NowPoint, ref map);
        }
    }
    private void Attack(Cursor UsingUnit, Cursor NowPoint, ref World map)
    {
        if(map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.SHIELDERS)
        {
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power -= map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power / 2;
        }
        else
        {
            map.UnitsMap[NowPoint.X, NowPoint.Y].Power -= map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power * 2;
        }

        if (map.UnitsMap[NowPoint.X, NowPoint.Y].Power <= 0)
        {
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.CASTLE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].NumberCastles--;
            }
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VILLAGE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Income -= 10;
            }
            map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType = (int)TypeDecoder.VOID;
        }
        this.Charge = false;
        map.Countries[map.CountriesLand[UsingUnit.X, UsingUnit.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Add(new Element(NowPoint.X, NowPoint.Y));
    }
}
class Ships : Units
{
    public override void Move(Cursor UsingUnit, Cursor NowPoint, ref World map, int countryWichMove)
    {
        bool canAttack = Units.CanArchersAttack(UsingUnit, NowPoint, ref map);
        if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VOID && map.Terra[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.WATER && Math.Abs(NowPoint.X - UsingUnit.X) < 2 && Math.Abs(NowPoint.Y - UsingUnit.Y) < 2 && this.Charge)
        {
            base.Move(UsingUnit, NowPoint, ref map, countryWichMove);
        }
        else if (canAttack && this.Charge)
        {
            this.Attack(UsingUnit, NowPoint, ref map);
        }
    }
    private void Attack(Cursor UsingUnit, Cursor NowPoint, ref World map)
    {
        map.UnitsMap[NowPoint.X, NowPoint.Y].Power -= map.UnitsMap[UsingUnit.X, UsingUnit.Y].Power * 2;

        if (map.UnitsMap[NowPoint.X, NowPoint.Y].Power <= 0)
        {
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.CASTLE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].NumberCastles--;
            }
            if (map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType == (int)TypeDecoder.VILLAGE)
            {
                map.Countries[map.CountriesLand[NowPoint.X, NowPoint.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Income -= 10;
            }
            map.UnitsMap[NowPoint.X, NowPoint.Y].ElementType = (int)TypeDecoder.VOID;
        }
        this.Charge = false;
        map.Countries[map.CountriesLand[UsingUnit.X, UsingUnit.Y].ElementType - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Add(new Element(NowPoint.X, NowPoint.Y));
    }
}

class Country
{
    public int Money;
    public int Income;
    public int NumberCastles;
    public List<Element>[] Recharge;
    public Country()
    {
        this.Money = 100;
        this.Income = 90;
        this.NumberCastles = 1;
        Recharge = new List<Element>[2];
        Recharge[0] = new List<Element>();
        Recharge[1] = new List<Element>();
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
    public Cursor(int x, int y)
    {
        this.ElementType = 1;
        this.X = x;
        this.Y = y;
    }
}

class TerraPoint : Element
{
    public TerraPoint()
    {
        this.ElementType = (int)TypeDecoder.VOID;
    }
    public TerraPoint(int x_start, int y_start, int x_size, int y_size, int spawning_change, int terra_type, ref TerraPoint[,] map, ref Element[,] country)
    {
        map[x_start, y_start].ElementType = terra_type;
        Random rundom = new Random();
        if(x_start > 0 && map[x_start - 1, y_start].ElementType == (int)TypeDecoder.VOID && country[x_start - 1, y_start].ElementType == (int)TypeDecoder.VOID && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start - 1, y_start, x_size, y_size, spawning_change, terra_type, ref map, ref country);
        }
        if (y_start > 0 && map[x_start, y_start - 1].ElementType == (int)TypeDecoder.VOID && country[x_start, y_start - 1].ElementType == (int)TypeDecoder.VOID && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start, y_start - 1, x_size, y_size, spawning_change, terra_type, ref map, ref country);
        }
        if (y_start < y_size - 1 && map[x_start, y_start + 1].ElementType == (int)TypeDecoder.VOID && country[x_start, y_start + 1].ElementType == (int)TypeDecoder.VOID && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start, y_start + 1, x_size, y_size, spawning_change, terra_type, ref map, ref country);
        }
        if (x_start < x_size - 1 && map[x_start + 1, y_start].ElementType == (int)TypeDecoder.VOID && country[x_start + 1, y_start].ElementType == (int)TypeDecoder.VOID && rundom.Next(0, 99) < spawning_change)
        {
            new TerraPoint(x_start + 1, y_start, x_size, y_size, spawning_change, terra_type, ref map, ref country);
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
                Castles.Add(new Element(XRandom, YRandom));
                Countries[spawnedCountries - 1] = new Country();
                UnitsMap[XRandom, YRandom].ElementType = (int)TypeDecoder.CASTLE;
                UnitsMap[XRandom, YRandom].Power = 1000;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        this.CountriesLand[XRandom - 1 + i, YRandom - 1 + j].ElementType = spawnedCountries + 700;
                    }
                }
            }
            else
            {
                spawnedCountries--;
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
            while (this.Terra[XRandom, YRandom].ElementType != (int)TypeDecoder.VOID && this.CountriesLand[XRandom, YRandom].ElementType == (int)TypeDecoder.VOID && spawnTries < 10000);
            if (spawnTries < 10000)
            {
                TerraPoint Spawn_terra_type = new TerraPoint(XRandom, YRandom, XSize, YSize, SpawnChange, TerraType, ref this.Terra, ref this.CountriesLand);
            }
        }
    }
}

class Drawing
{
    public static void DrawMap(ref World Map, ref Cursor Player, Cursor playerSecondCursor, int CountryNowMove)
    {
        Console.Clear();
        Console.Write("Now move: ");
        Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)CountryNowMove].Item2;
        Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)CountryNowMove].Item2;
        Console.WriteLine(Element.TypeMap.Types[(TypeDecoder)CountryNowMove].Item1);

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Money: ");
        Console.WriteLine(Map.Countries[CountryNowMove - (int)TypeDecoder.PLAYER_LAND_1].Money);
        Console.Write("Income money: ");
        Console.WriteLine(Map.Countries[CountryNowMove - (int)TypeDecoder.PLAYER_LAND_1].Income);
        for (int y = 0; y < Map.YSize; y++)
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
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Now you choose:");
        Console.Write("Type of units: ");
        Console.WriteLine((TypeDecoder)Map.UnitsMap[Player.X, Player.Y].ElementType);
        if((TypeDecoder)Map.UnitsMap[Player.X, Player.Y].ElementType != TypeDecoder.VOID)
        {
            Console.Write("Power of units: ");
            Console.WriteLine(Map.UnitsMap[Player.X, Player.Y].Power);
            if(Map.UnitsMap[Player.X, Player.Y].Charge)
            {
                Console.WriteLine("Units is ready to do something");
            }
            else
            {
                Console.WriteLine("Units isn't ready to do something");
            }
        }
        Console.Write("Country: ");
        Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[Player.X, Player.Y].ElementType].Item2;
        Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[Player.X, Player.Y].ElementType].Item2;
        Console.WriteLine(Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[Player.X, Player.Y].ElementType].Item1);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Type of terra: ");
        Console.WriteLine((TypeDecoder)Map.Terra[Player.X, Player.Y].ElementType);

        Console.WriteLine();

        if (playerSecondCursor.X != -1)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You selected:");
            Console.Write("Type of units: ");
            Console.WriteLine((TypeDecoder)Map.UnitsMap[playerSecondCursor.X, playerSecondCursor.Y].ElementType);
            if ((TypeDecoder)Map.UnitsMap[playerSecondCursor.X, playerSecondCursor.Y].ElementType != TypeDecoder.VOID)
            {
                Console.Write("Power of units: ");
                Console.WriteLine(Map.UnitsMap[playerSecondCursor.X, playerSecondCursor.Y].Power);
                if (Map.UnitsMap[playerSecondCursor.X, playerSecondCursor.Y].Charge)
                {
                    Console.WriteLine("Units is ready to do something");
                }
                else
                {
                    Console.WriteLine("Units isn't ready to do something");
                }
            }
            Console.Write("Country: ");
            Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[playerSecondCursor.X, playerSecondCursor.Y].ElementType].Item2;
            Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[playerSecondCursor.X, playerSecondCursor.Y].ElementType].Item2;
            Console.WriteLine(Element.TypeMap.Types[(TypeDecoder)Map.CountriesLand[playerSecondCursor.X, playerSecondCursor.Y].ElementType].Item1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Type of terra: ");
            Console.WriteLine((TypeDecoder)Map.Terra[playerSecondCursor.X, playerSecondCursor.Y].ElementType);
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
    private Cursor playerSecondCursor;
    private int countryWichMove;


    public Engine():this(30, 20, 2)
    { }
    public Engine(int X, int Y, int CountryQuantity)
    {
        this.player = new Cursor();
        this.Map = new World(X, Y, CountryQuantity);
        this.Store = new UnitsStore();
        playerSecondCursor = new Cursor(-1, 0);

        countryWichMove = (int)TypeDecoder.PLAYER_LAND_1;
    }
    public void Game()
    {
        bool finishGame = false;
        int liveCountry, numberOfLiveCountry;
        while (!finishGame)
        {
            liveCountry = 0;
            for (int i = 0; i < Map.Countries.Length; i++)
            {
                if (Map.Countries[i].NumberCastles > 0)
                {
                    liveCountry++;
                    numberOfLiveCountry = i;
                }
            }
            if (liveCountry <= 1)
            {
                break;
            }
            else
            {
                this.PlayerMove();
            }

            Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money += Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Income;

            liveCountry = 0;
            int startCountry = countryWichMove;
            do
            {
                countryWichMove++;
                if (countryWichMove - (int)TypeDecoder.PLAYER_LAND_1 >= Map.Countries.Length)
                {
                    countryWichMove = (int)TypeDecoder.PLAYER_LAND_1;
                }
                if (Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].NumberCastles > 0)
                {
                    liveCountry++;
                }
            }
            while (liveCountry == 0 && countryWichMove != startCountry);
            for(int i = 0; i < Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0].Count(); i++)
            {
                Map.UnitsMap[Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0][i].X, Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0][i].Y].Charge = true;
            }
            Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[0] = Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[1];
            Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Recharge[1] = new List<Element>();

        }

        Console.Clear();
        Console.Write("The winer is ");
        Console.BackgroundColor = Element.TypeMap.Types[(TypeDecoder)countryWichMove].Item2;
        Console.ForegroundColor = Element.TypeMap.Types[(TypeDecoder)countryWichMove].Item2;
        Console.Write(Element.TypeMap.Types[(TypeDecoder)countryWichMove].Item1);
        Console.WriteLine();

    }
    private void PlayerMove()
    {
        bool storeOpen = false;
        bool finished = false;
        do
        {
            if (storeOpen)
            {
                Drawing.DrawMap(ref this.Map, ref this.playerSecondCursor, new Cursor(-1, 0), countryWichMove);
                Drawing.DrawStore(this.player);
            }
            else
            {
                Drawing.DrawMap(ref this.Map, ref this.player, playerSecondCursor, countryWichMove);
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
                playerSecondCursor = player;
                player = new Cursor();
            }
            else
            {
                storeOpen = false;
                this.player.X = playerSecondCursor.X;
                this.player.Y = playerSecondCursor.Y;
                playerSecondCursor.X = -1;
            }
        }
        else if (input == ConsoleKey.Enter)
        {
            if (storeOpen)
            {
                Store.UnitWichBuy = player.X + (int)TypeDecoder.CASTLE;
                storeOpen = false;
                this.player.X = playerSecondCursor.X;
                this.player.Y = playerSecondCursor.Y;
                playerSecondCursor.X = -1;
            }
            else if (Store.UnitWichBuy != 0)
            {
                BuyUnits();
                Store.UnitWichBuy = 0;
            }
            else if (Map.UnitsMap[player.X, player.Y].ElementType >= (int)TypeDecoder.WARRIORS && Map.UnitsMap[player.X, player.Y].ElementType <= (int)TypeDecoder.SHIPS && Map.UnitsMap[player.X, player.Y].TheirCountry == countryWichMove && playerSecondCursor.X == -1)
            {
                playerSecondCursor.X = player.X;
                playerSecondCursor.Y = player.Y;
            }
            else if(playerSecondCursor.X != -1)
            {
                Map.UnitsMap[playerSecondCursor.X, playerSecondCursor.Y].Move(playerSecondCursor, player, ref Map, countryWichMove);
                playerSecondCursor.X = -1;
            }
        }
        else if(input == ConsoleKey.Escape || input == ConsoleKey.Backspace)
        {
            if (storeOpen)
            {
                storeOpen = false;
                this.player.X = playerSecondCursor.X;
                this.player.Y = playerSecondCursor.Y;
                playerSecondCursor.X = -1;
            }
            return true;
        }
        return false;
    }
    private void BuyUnits()
    {
        if (Map.UnitsMap[player.X, player.Y].ElementType == 0 && Map.CountriesLand[player.X, player.Y].ElementType == countryWichMove)
        {
            if (Store.UnitWichBuy == (int)TypeDecoder.CASTLE && Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money >= 50)
            {
                bool spawned = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if(Map.XSize > player.X + i && player.X + i >= 0 && Map.YSize > player.Y + j && player.Y + j >= 0 && Map.UnitsMap[player.X + i, player.Y + j].Power >= 1000)
                        {
                            spawned = true;
                        }
                    }
                }
                if (spawned)
                {
                    new Units(ref player, 1000, Store.UnitWichBuy, ref Map);
                    Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money -= 50;
                }
            }
            else if (Store.UnitWichBuy == (int)TypeDecoder.PORT && Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money >= 25)
            {
                bool spawned = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (Map.XSize > player.X + i && player.X + i >= 0 && Map.YSize > player.Y + j && player.Y + j >= 0 && Map.UnitsMap[player.X + i, player.Y + j].Power >= 250)
                        {
                            spawned = true;
                        }
                    }
                }
                if (spawned)
                {
                    new Units(ref player, 250, Store.UnitWichBuy, ref Map);
                    Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money -= 25;
                }
            }
            else if (Store.UnitWichBuy == (int)TypeDecoder.VILLAGE && Map.Terra[player.X, player.Y].ElementType == (int)TypeDecoder.VOID && Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money >= 15)
            {
                new Units(ref player, 10, Store.UnitWichBuy, ref Map);
                Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money -= 15;
                Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Income += 10;
            }
            else if (Store.UnitWichBuy == (int)TypeDecoder.SHIPS && Map.Terra[player.X, player.Y].ElementType == (int)TypeDecoder.WATER)
            {
                bool spawned = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (Map.XSize > player.X + i && player.X + i >= 0 && Map.YSize > player.Y + j && player.Y + j >= 0 && Map.UnitsMap[player.X + i, player.Y + j].ElementType == (int)TypeDecoder.PORT)
                        {
                            spawned = true;
                        }
                    }
                }
                int wantedPower;
                wantedPower = WantedPowerOfUnits();
                if (wantedPower / 10 <= Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money && spawned)
                {
                    new Units(ref player, wantedPower, Store.UnitWichBuy, ref Map);
                    Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money -= wantedPower / 10;
                }
            }
            else
            {
                int wantedPower;
                wantedPower = WantedPowerOfUnits();
                if (wantedPower / 10 <= Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money)
                {
                    new Units(ref player, wantedPower, Store.UnitWichBuy, ref Map);
                    Map.Countries[countryWichMove - (int)TypeDecoder.PLAYER_LAND_1].Money -= wantedPower / 10;
                }
            }
        }
    }

    private int WantedPowerOfUnits()
    {
        int wantedPower = 0;
        bool sended = false;
        while (!sended) {
            Console.Clear();
            Console.WriteLine("How much power do you want? You can buy only in dozens!");
            Console.WriteLine(wantedPower);
            Console.Write("How you need money:");
            Console.WriteLine(wantedPower/10);
            ConsoleKey input = Console.ReadKey().Key;
            switch (input)
            {
                case ConsoleKey.Enter:
                    sended = true;
                    break;
                case ConsoleKey.Backspace:
                    wantedPower = wantedPower / 10;
                    break;
                case ConsoleKey.D0:
                    wantedPower = wantedPower * 10;
                    break;
                case ConsoleKey.D1:
                    wantedPower = wantedPower * 10 + 1;
                    break;
                case ConsoleKey.D2:
                    wantedPower = wantedPower * 10 + 2;
                    break;
                case ConsoleKey.D3:
                    wantedPower = wantedPower * 10 + 3;
                    break;
                case ConsoleKey.D4:
                    wantedPower = wantedPower * 10 + 4;
                    break;
                case ConsoleKey.D5:
                    wantedPower = wantedPower * 10 + 5;
                    break;
                case ConsoleKey.D6:
                    wantedPower = wantedPower * 10 + 6;
                    break;
                case ConsoleKey.D7:
                    wantedPower = wantedPower * 10 + 7;
                    break;
                case ConsoleKey.D8:
                    wantedPower = wantedPower * 10 + 8;
                    break;
                case ConsoleKey.D9:
                    wantedPower = wantedPower * 10 + 9;
                    break;
            }
        }
        wantedPower -= wantedPower % 10; 
        return wantedPower;
    }
}

