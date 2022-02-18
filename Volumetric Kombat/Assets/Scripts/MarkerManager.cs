using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MarkerManager : MonoBehaviour
{
    //Tool controls object
    private ToolControls _tc;

    //Marker tool to Spawn
    public GameObject _markerToSpawn = null;
    //Where to spawn and parent marker
    public GameObject _markerSpawnPoint = null;
    //List of spawned markers
    [System.NonSerialized]
    public List<GameObject> _spawnedMarkers = new List<GameObject>();

    //Actively selected marker
    MarkerToggler _activeSelectedMarker;
    //Actively selected object
    DragMarker _activePortionOfMarker;
    //Currently clicking?
    bool _isHeld;

    private void Start()
    {
        _tc = Utility.GetToolControlsAsset();
        //Initialize actions for this tool
        InitInputActions();
    }

    private void InitInputActions()
    {
        //Grab items
        _tc.Default.GrabItem.started += ctx => CheckIfGrabbed();

        //Spawn marker
        _tc.Default.SpawnMarker.performed += ctx => SpawnMarker();
    }

    public void UnselectCurrentMarker()
    {
        //Unselect current marker
        if (_activeSelectedMarker != null)
        {
            MarkerToggler disabler = _activeSelectedMarker.GetComponent<MarkerToggler>();
            disabler.SetUnselected();
            _activeSelectedMarker = null;
        }
    }

    public void SelectNewMarker(MarkerToggler newMarker)
    {
        newMarker.SetSelected();
        _activeSelectedMarker = newMarker;
    }

    private void SpawnMarker()
    {
        //Unselect current marker
        UnselectCurrentMarker();

        //Keeps track of spawned markers
        _spawnedMarkers.Add(Instantiate(_markerToSpawn, _markerSpawnPoint.transform));

        //Select new marker
        SelectNewMarker(_spawnedMarkers[_spawnedMarkers.Count-1].GetComponent<MarkerToggler>());
    }

    private void CheckIfGrabbed()
    {
        Camera mainCam = Camera.main;
        Mouse mouse = Mouse.current;

        int handleCast = LayerMask.GetMask("HandleCast");
        int inactiveHandleCast = LayerMask.GetMask("InactiveHandleCast");

        Ray cast = mainCam.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(cast, out hit, 1000.0f, inactiveHandleCast))
        {
            InactiveMarker inactiveMarker;
            if (hit.collider.gameObject.TryGetComponent<InactiveMarker>(out inactiveMarker))
            {
                MarkerToggler newMarkerToggler = inactiveMarker._targetObject.GetComponent<MarkerToggler>();

                UnselectCurrentMarker();
                SelectNewMarker(newMarkerToggler);
            }
        }
    }

    public void SaveMarkers()
    {
        string localPath = "Assets/Prefabs/Markers.prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        UnselectCurrentMarker();
        
        PrefabUtility.SaveAsPrefabAsset(_markerSpawnPoint, localPath);
    }
}
