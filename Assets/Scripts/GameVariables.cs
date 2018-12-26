public static class GameVariables
{
    private static int diamonds = 3, stones = 15, tiles = 12, floors = 3;
    private static float time = 300f;
    private static string device = "Cardboard";
    private static bool daydreamsupported = true;

    public static int Diamonds
    {
        get
        {
            return diamonds;
        }
        set
        {
            diamonds = value;
        }
    }

    public static int Stones
    {
        get
        {
            return stones;
        }
        set
        {
            stones = value;
        }
    }

    public static int Tiles
    {
        get
        {
            return tiles;
        }
        set
        {
            tiles = value;
        }
    }

    public static int Floors
    {
        get
        {
            return floors;
        }
        set
        {
            floors = value;
        }
    }

    public static float Time
    {
        get
        {
            return time;
        }
        set
        {
            time = value;
        }
    }

    public static string Device
    {
        get
        {
            return device;
        }
        set
        {
            device = value;
        }
    }

    public static bool DaydreamSupported
    {
        get
        {
            return daydreamsupported;
        }
        set
        {
            daydreamsupported = value;
        }
    }

}
