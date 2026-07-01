using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInput : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private BoardManager m_boardManager;

    [SerializeField] private InputActionReference m_clickAction;

    private void OnEnable()
    {
        m_clickAction.action.Enable();
        m_clickAction.action.performed += OnClick;
    }

    private void OnDisable()
    {
        m_clickAction.action.performed -= OnClick;
        m_clickAction.action.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector2 screenPosition = Mouse.current.position.ReadValue();

        Vector2 worldPosition =
            m_camera.ScreenToWorldPoint(screenPosition);

        Collider2D hit =
            Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
            return;

        Candy candy = hit.GetComponent<Candy>();

        if (candy == null)
            return;
        Debug.Log(candy.name);
        m_boardManager.SelectCandy(candy);
    }
}