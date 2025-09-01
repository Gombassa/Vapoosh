// In DebugConsole.cs
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // <-- Add this for the new Input System

public class DebugConsole : MonoBehaviour
{
    public TextMeshProUGUI consoleText;
    private string myLog;
    private bool doShow = true;
    private int kChars = 700;

    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }

    void Update() 
    { 
        // Use the new Input System to check for the space key
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) 
        { 
            doShow = !doShow; 
        } 
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        myLog = myLog + "\n" + logString;
        if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }
        if (consoleText != null) { consoleText.text = myLog; }
    }

    void OnGUI()
    {
        if (!doShow) { return; }
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
    }
}