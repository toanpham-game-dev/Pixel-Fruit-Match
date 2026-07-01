using UnityEngine;

/// <summary>
/// Manages board creation, candy spawning and match operations.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig m_boardConfig;

    private Cell[,] m_cells;

    private Candy m_selectedCandy;

    public int Width => m_boardConfig.Width;
    public int Height => m_boardConfig.Height;
    public Candy[] AvailableCandies => m_boardConfig.CandyPrefabs;

    private void Start()
    {
        CreateBoard();
        SpawnInitialCandies();
    }

    #region Board generation
    private void CreateBoard()
    {
        m_cells = new Cell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                m_cells[x, y] = new Cell(x, y);
            }
        }
    }

    private void SpawnInitialCandies()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SpawnCandy(x, y);
            }
        }
    }

    private void SpawnCandy(int x, int y)
    {
        int index = Random.Range(0, AvailableCandies.Length);
        Candy prefab = AvailableCandies[index];

        Vector3 worldPosition = GetWorldPosition(x, y);

        Candy candy = Instantiate(prefab, worldPosition, Quaternion.identity, transform);

        candy.Bind(m_cells[x, y]);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        float offsetX = (Width - 1) / 2f;
        float offsetY = (Height - 1) / 2f;

        return new Vector3(
            x - offsetX,
            y - offsetY,
            0f);
    }
    #endregion

    #region Candy swap
    public void SelectCandy(Candy candy)
    {
        if (m_selectedCandy == null)
        {
            m_selectedCandy = candy;
            return;
        }

        if (m_selectedCandy == candy)
        {
            m_selectedCandy = null;
            return;
        }

        TrySwap(m_selectedCandy, candy);

        m_selectedCandy = null;
    }

    private void TrySwap(Candy first, Candy second)
    {
        if (!AreAdjacent(first, second))
            return;

        Swap(first, second);
    }

    private bool AreAdjacent(Candy first, Candy second)
    {
        int dx = Mathf.Abs(
            first.Cell.X - second.Cell.X);

        int dy = Mathf.Abs(
            first.Cell.Y - second.Cell.Y);

        return dx + dy == 1;
    }

    private void Swap(Candy first, Candy second)
    {
        Cell firstCell = first.Cell;
        Cell secondCell = second.Cell;

        Vector3 firstPosition = first.transform.position;
        Vector3 secondPosition = second.transform.position;

        first.Bind(secondCell);
        second.Bind(firstCell);

        first.MoveTo(secondPosition);
        second.MoveTo(firstPosition);
    }
    #endregion
}
