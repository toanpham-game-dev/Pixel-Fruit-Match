using System.Collections.Generic;
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
        Candy prefab = GetRandomValidCandy(x, y);

        Vector3 worldPosition = GetWorldPosition(x, y);

        Candy candy = Instantiate(prefab, worldPosition, Quaternion.identity, transform);

        candy.Bind(m_cells[x, y]);
    }

    private Candy GetRandomValidCandy(int x, int y)
    {
        List<Candy> candidates = new();

        foreach (Candy prefab in AvailableCandies)
        {
            if (!CreatesMatch(x, y, prefab))
            {
                candidates.Add(prefab);
            }
        }

        int index = Random.Range(0, candidates.Count);

        return candidates[index];
    }

    private bool CreatesMatch(int x, int y, Candy prefab)
    {
        // Check horizontal
        if (x >= 2)
        {
            Candy left1 = m_cells[x - 1, y].CurrentCandy;
            Candy left2 = m_cells[x - 2, y].CurrentCandy;

            if (left1 != null &&
                left2 != null &&
                left1.Type == prefab.Type &&
                left2.Type == prefab.Type)
            {
                return true;
            }
        }

        // Check vertical
        if (y >= 2)
        {
            Candy down1 = m_cells[x, y - 1].CurrentCandy;
            Candy down2 = m_cells[x, y - 2].CurrentCandy;

            if (down1 != null &&
                down2 != null &&
                down1.Type == prefab.Type &&
                down2.Type == prefab.Type)
            {
                return true;
            }
        }

        return false;
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
