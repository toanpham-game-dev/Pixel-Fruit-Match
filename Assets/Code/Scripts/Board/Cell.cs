public class Cell
{
    public int X { get; }
    public int Y { get; }

    public Candy CurrentCandy { get; set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsEmpty => CurrentCandy == null;
}