using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType { Normal, WorldTilt }
public enum GameType { Normal, SpeedRun }
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ControlType controlType;
    public GameType gameType;
   
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

    // Toggle between world tilt on or off
    public void ToggleWorldTilt(bool _tilt)
    {
        if (_tilt)
            controlType = ControlType.WorldTilt;
        else
            controlType = ControlType.Normal;
    }

    //Sets the game type from our selections for speed run
    public void SetGameType(GameType _gameType)
    {
        gameType = _gameType;
    }

    //To toggle between speedrun on or off
    public void ToggleSpeedRun(bool _speedRun)
    {
        if (_speedRun)
            SetGameType(GameType.SpeedRun);
        else
            SetGameType(GameType.Normal);
    }

}
