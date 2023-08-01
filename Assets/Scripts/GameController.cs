using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType { Normal, WorldTilt }
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ControlType controlType;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void ToggleWorldTilt(bool _tilt)
    {
        if (_tilt)
            controlType = ControlType.WorldTilt;
        else
            controlType = ControlType.Normal;
    }
}
