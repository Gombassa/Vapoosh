// In AudioListenerChecker.cs
using UnityEngine;

public class AudioListenerChecker : MonoBehaviour
{
    void Awake()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1) { GetComponent<AudioListener>().enabled = false; }
    }
}
