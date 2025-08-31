// In EventSystemChecker.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
    void Awake()
    {
        // Use the new, recommended method to find objects.
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventSystems.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
