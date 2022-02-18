using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;

public class GameManager : MonoBehaviour
{
    //Input controls for tool
    public ToolControls _tc;
    //Runtime handle
    public RuntimeTransformHandle _transformHandle;


    // Start is called before the first frame update
    void Awake()
    {
        //Store new tool controls object
        _tc = new ToolControls();
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

    private void Update()
    {
        HideTransformHandle();
    }

    private void HideTransformHandle()
    {
        if (_transformHandle != null)
        {
            if (_transformHandle.target == null)
            {
                _transformHandle.Clear();
            }
            else
            {
                _transformHandle.CreateHandles();
            }
        }
    }
}
