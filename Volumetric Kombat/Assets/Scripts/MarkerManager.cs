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
        _tc.Default.GrabItem.canceled += ctx => ReleaseObject();

        //Spawn marker
        _tc.Default.SpawnMarker.performed += ctx => SpawnMarker();

        //Read in mouse position value
        _tc.Default.Drag.performed += ctx => DragObject(ctx.ReadValue<Vector2>());
        //Read in mouse scroll wheel value
        _tc.Default.DragZ.performed += ctx => DragObjectZ(ctx.ReadValue<Vector2>());
    
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

        if (Physics.Raycast(cast, out hit, 1000.0f, handleCast))
        {
            DragMarker markerAxis;
            
            if (hit.collider.gameObject.TryGetComponent<DragMarker>(out markerAxis))
            {
                _isHeld = true;
                _activePortionOfMarker = markerAxis;
            }
        }
        else if (Physics.Raycast(cast, out hit, 1000.0f, inactiveHandleCast))
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

    private void ReleaseObject()
    {
        _isHeld = false;
        _activePortionOfMarker = null;
    }

    private void DragObject(Vector2 mousePosition)
    {
        //if currently holding an object
        if (_isHeld && _activePortionOfMarker != null)
        {
            //Drag around the object
            _activePortionOfMarker.Drag(mousePosition);
        }
    }

    private void DragObjectZ(Vector2 scrollMovement)
    {
        //if currently holding an object
        if (_isHeld && _activePortionOfMarker._movementAxis == Axis.Z)
        {
            //Drag around the object
            _activePortionOfMarker.DragZ(scrollMovement.y);
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
