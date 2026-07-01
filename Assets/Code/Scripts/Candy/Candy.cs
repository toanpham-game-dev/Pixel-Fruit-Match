using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] private CandyType m_type;

    private Cell m_cell;

    public CandyType Type => m_type;
    public Cell Cell => m_cell;

    public void Bind(Cell cell)
    {
        m_cell = cell;
        cell.CurrentCandy = this;
    }

    public void Unbind()
    {
        if (m_cell != null)
        {
            m_cell.CurrentCandy = null;
            m_cell = null;
        }
    }

    // Draw the bound cell coordinates in the Scene view for debugging.
    private void OnDrawGizmosSelected()
        {
            if (m_cell == null) return;

            Gizmos.color = Color.yellow;

            UnityEditor.Handles.Label(
                transform.position,
                $"{m_cell.X},{m_cell.Y}"
            );
        }
}