var tanaHouse = new House("棚はうす");
var umbreon = new Umbreon();
var tana = new Tana();

umbreon.Into(tanaHouse);
tana.Into(tanaHouse);
Task sleeping = tana.SleepAsync();
umbreon.Play();
sleeping.Wait();



enum Location
{
    InBed,
    UnderTable,
    OutsideOfHouse,
}

interface IResident
{
    Location Location { get; }
}

class House
{
    private readonly List<IResident> _residents = new();
    private readonly string _name;

    public House(string name)
        => _name = name;

    public void Into(IResident resident)
    {
        if (_residents.Contains(resident)) { return; }
        _residents.Add(resident);
    }

    public void Remove(IResident resident)
        => _residents.Remove(resident);

    public bool Exists<T>(Location location)
        => _residents.Any(r => (r is T) && (r.Location == location));
}

class Umbreon : IResident
{
    private House? _home;
    
    public Location Location { get; private set; } = Location.OutsideOfHouse;

    public void Into(House house)
    {
        _home = house;
        _home.Into(this);
        Location = Location.UnderTable;
    }

    public void Play()
    {
        if (_home is null) { return; }

        var random = new Random();
        Location = random.Next(197) switch
        {
            < 1 => Location.InBed,
            < 97 => Location.UnderTable,
            _ => Location.OutsideOfHouse,
        };

        if (Location == Location.OutsideOfHouse)
        {
            _home.Remove(this);
        }
    }
}

class Tana : IResident
{
    private House? _home;

    public Location Location { get; private set; } = Location.OutsideOfHouse;

    public void Into(House house)
    {
        _home = house;
        _home.Into(this);
        Location = Location.UnderTable;
    }

    public async Task SleepAsync()
    {
        Location = Location.InBed;

        Console.WriteLine("Tana: 寝る！");
        await Task.Run(() => Thread.Sleep(1970));
        
        OnWakeUp();
        return;
    }

    private void OnWakeUp()
    {
        if (_home is null) { throw new Exception(); }

        Console.WriteLine("Tana: 起きた！");
        if (_home.Exists<Umbreon>(Location.InBed))
        {
            Console.WriteLine("Tana: ブラッキーがベッドに居る！　ぐへへ……プラッキーぺろぺろ");
            return;
        }

        Console.WriteLine("Tana: ブラッキー居ないなぁ……どこだろう");
        if (_home.Exists<Umbreon>(Location.UnderTable))
        {
            Console.WriteLine("Tana: あっ！　机の下に居た！　うひひひひ、おはようのブラッキーぺろぺろ");
            return;
        }

        Console.WriteLine("Tana: 棚はうすに居ない……(´・ω・`)");
    }
}
