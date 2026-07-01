using DG.Tweening;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private const float k_swapDuration = 0.2f;

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

    public Tween MoveTo(Vector3 targetPosition)
    {
        return transform.DOMove(
            targetPosition,
            k_swapDuration);
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