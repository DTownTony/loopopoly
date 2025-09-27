public class Gear
{
    public GearData Data { get; private set; }

    public int Level;

    public Gear(GearData data)
    {
        Data = data;
    }
}
