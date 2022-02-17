using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkerToggler : MonoBehaviour
{
    public GameObject _activeAxisObject;
    public Collider _inactiveCollider; 

    public void SetSelected()
    {
        _activeAxisObject.SetActive(true);
        _inactiveCollider.enabled = false;
    }

    public void SetUnselected()
    {
        _activeAxisObject.SetActive(false);
        _inactiveCollider.enabled = true;
    }
}
