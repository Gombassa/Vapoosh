// In SingletonChecker.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class SingletonChecker : MonoBehaviour
{
    void Awake()
    {
        // Check for Audio Listeners
        if (GetComponent<AudioListener>() != null)
        {
            AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            if (listeners.Length > 1)
            {
                // If more than one listener exists, disable this one.
                GetComponent<AudioListener>().enabled = false;
            }
        }

        // Check for Event Systems
        if (GetComponent<EventSystem>() != null)
        {
            EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
            if (eventSystems.Length > 1)
            {
                // If more than one event system exists, destroy this one.
                Destroy(gameObject);
            }
        }
    }
}