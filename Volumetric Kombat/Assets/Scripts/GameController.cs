using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Input controls for tool
    public ToolControls _tc;

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
}
