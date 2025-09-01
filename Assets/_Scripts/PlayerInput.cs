// In PlayerInput.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Camera mainCamera;

    void Start() { mainCamera = Camera.main; }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsCurrentPlayerAI()) return;
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;
        if (GameManager.Instance.currentState != GameManager.GameState.WaitingForMove) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debugging: Log what object was hit
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            if (hit.collider.TryGetComponent<Tile>(out Tile hitTile))
            {
                // This will log if a tile was successfully identified
                Debug.Log($"Successfully found Tile component at ({hitTile.x},{hitTile.y})");
                GameManager.Instance.ProcessPlayerMove(hitTile.x, hitTile.y);
            }
            else
            {
                // This will log if the object hit did NOT have a Tile script
                Debug.LogWarning("The hit object does not have a Tile component.");
            }
        }
    }
}