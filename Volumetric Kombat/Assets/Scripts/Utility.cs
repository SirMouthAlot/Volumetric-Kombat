using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    static GameObject _gameControllerObj;
    static GameController _gameController;
    static MarkerManager _markerManager;
    static bool _foundGameControllerObj = false;
    static bool _foundGameController = false;
    static bool _foundMarkerManager = false;

    public static GameObject GetGameControllerObj()
    {
        if (!_foundGameControllerObj)
        {
            GameObject eventSystem = GameObject.FindGameObjectWithTag("GameController");

            _gameControllerObj = eventSystem;
            _foundGameControllerObj = true;
        }
        
        return _gameControllerObj;
    }

    public static GameController GetGameController()
    {
        if (!_foundGameController)
        {
            _gameController = GetGameControllerObj().GetComponent<GameController>();
            _foundGameController = true;
        }

        return _gameController;
    }

    public static MarkerManager GetMarkerManager()
    {
        if (!_foundMarkerManager)
        {
            _markerManager = GetGameControllerObj().GetComponent<MarkerManager>();
            _foundMarkerManager = true;
        }

        return _markerManager;
    }

    public static ToolControls GetToolControlsAsset()
    {
        return GetGameController()._tc;
    }
}
