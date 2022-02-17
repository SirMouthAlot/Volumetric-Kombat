using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkerController : MonoBehaviour
{
    //Input controls for tool
    public ToolControls _tc;

    //Marker tool to Spawn
    public GameObject _markerToSpawn = null;

    //Actively selected object
    DragMarker _activeObject;
    //Currently clicking?
    bool _isHeld;
    //Start position for the drag
    Vector3 _dragStartPosition;

    private void Awake()
    {
        //Store new tool controls object
        _tc = new ToolControls();

        //Initialize actions for this tool
        InitInputActions();
    }

    private void OnEnable()
    {
        //Enable tool controls
        _tc.Enable();
    }

    private void OnDisable()
    {
        //Disable tool controls
        _tc.Disable();
    }

    private void InitInputActions()
    {
        //Grab items
        _tc.Default.Leftclick.started += ctx => CheckIfGrabbed();
        _tc.Default.Leftclick.canceled += ctx => ReleaseObject();

        //Spawn marker
        _tc.Default.Rightclick.performed += ctx => SpawnMarker();

        //Read in mouse delta value
        _tc.Default.Drag.performed += ctx => DragObject(ctx.ReadValue<Vector2>());
    }

    private void SpawnMarker()
    {
        Instantiate(_markerToSpawn);
    }

    private void CheckIfGrabbed()
    {
        Camera mainCam = Camera.main;
        Mouse mouse = Mouse.current;

        int layerMask = LayerMask.GetMask("ToolControlsCast");

        Ray cast = mainCam.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(cast, out hit, Mathf.Infinity, layerMask))
        {
            DragMarker marker;
            if (hit.collider.gameObject.TryGetComponent<DragMarker>(out marker))
            {
                _isHeld = true;
                _activeObject = marker;
                float zDistToObj = _activeObject._movementTarget.transform.position.z - Camera.main.transform.position.z;

                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector3 mousePosWithZ = new Vector3(mousePosition.x, mousePosition.y, zDistToObj);
                _dragStartPosition = Camera.main.ScreenToWorldPoint(mousePosWithZ);
            }
            else
            {
                //Do nothing
                return;
            }
        }
        else
        {
            //Do nothing
            return;
        }
    }

    private void ReleaseObject()
    {
        _isHeld = false;
        _activeObject = null;
        _dragStartPosition = new Vector2();
    }

    private void DragObject(Vector2 mousePosition)
    {
        //if currently holding an object
        if (_isHeld && _activeObject != null)
        {
            float zDistToObj = _activeObject._movementTarget.transform.position.z - Camera.main.transform.position.z;

            Vector3 mousePosWithZ = new Vector3(mousePosition.x, mousePosition.y, zDistToObj);
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosWithZ);

            //Drag around the object
            _activeObject.Drag(mousePosWorld);

            Debug.Log("Mouse Position: " + mousePosWorld);
            Debug.Log("Start Position: " + _dragStartPosition);
        }
    }
}
