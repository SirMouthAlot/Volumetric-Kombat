using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RuntimeHandle;

public class MarkerToggler : MonoBehaviour
{
    RuntimeTransformHandle _handle;

    private void Awake()
    {
        _handle = Utility.GetTransformHandleAsset();
    }

    public void SetSelected()
    {
        _handle.target = transform;
    }

    public void SetUnselected()
    {
        _handle.target = null;
    }
}
