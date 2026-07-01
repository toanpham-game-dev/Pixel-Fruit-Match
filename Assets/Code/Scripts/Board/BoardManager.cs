using UnityEngine;

/// <summary>
/// Manages board creation, candy spawning and match operations.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardConfig m_boardConfig;

    private Cell[,] m_cells;

    public int Width => m_boardConfig.Width;
    public int Height => m_boardConfig.Height;
    public Candy[] AvailableCandies => m_boardConfig.CandyPrefabs;

    private void Start()
    {
        CreateBoard();
        SpawnInitialCandies();
    }

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
}
