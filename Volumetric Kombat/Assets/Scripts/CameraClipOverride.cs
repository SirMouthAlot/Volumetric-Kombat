using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClipOverride : MonoBehaviour
{
    Camera _main;

    // Start is called before the first frame update
    void Start()
    {
        _main = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _main.farClipPlane = 9000.0f;
    }
}
