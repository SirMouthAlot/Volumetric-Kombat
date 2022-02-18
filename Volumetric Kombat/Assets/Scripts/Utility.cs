using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;

public class Utility
{
    static GameObject _gameManagerObj;
    static GameManager _gameManager;
    static MarkerManager _markerManager;
    static bool _foundGameManagerObj = false;
    static bool _foundGameManager = false;
    static bool _foundMarkerManager = false;

    public static GameObject GetGameManagerObj()
    {
        if (!_foundGameManagerObj)
        {
            GameObject eventSystem = GameObject.FindGameObjectWithTag("GameController");

            _gameManagerObj = eventSystem;
            _foundGameManagerObj = true;
        }
        
        return _gameManagerObj;
    }

    public static GameManager GetGameManager()
    {
        if (!_foundGameManager)
        {
            _gameManager = GetGameManagerObj().GetComponent<GameManager>();
            _foundGameManager = true;
        }

        return _gameManager;
    }

    public static MarkerManager GetMarkerManager()
    {
        if (!_foundMarkerManager)
        {
            _markerManager = GetGameManagerObj().GetComponent<MarkerManager>();
            _foundMarkerManager = true;
        }

        return _markerManager;
    }

    public static ToolControls GetToolControlsAsset()
    {
        return GetGameManager()._tc;
    }

    public static RuntimeTransformHandle GetTransformHandleAsset()
    {
        return GetGameManager()._transformHandle;
    }
}
