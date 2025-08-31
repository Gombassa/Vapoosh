// In PlayerInput.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Camera mainCamera;

    void Start() { mainCamera = Camera.main; }

    void Update()
    {
        // Add a safety check to ensure the GameManager is ready before proceeding.
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.IsCurrentPlayerAI()) return;
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;
        if (GameManager.Instance.currentState != GameManager.GameState.WaitingForMove) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<Tile>(out Tile hitTile))
            {
                GameManager.Instance.ProcessPlayerMove(hitTile.x, hitTile.y);
            }
        }
    }
}