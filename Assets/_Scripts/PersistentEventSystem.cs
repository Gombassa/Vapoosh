// In PersistentEventSystem.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class PersistentEventSystem : MonoBehaviour
{
    public static PersistentEventSystem Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}