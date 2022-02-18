using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X,
    Y, 
    Z
}

public class DragMarker : MonoBehaviour
{
    public GameObject _movementTarget;
    public Axis _movementAxis;
    public Vector3 _holdOffset;

    [Range(0.1f, 5.0f)]
    public float _zMoveSpeed = 1.0f;

    public void Drag(Vector2 mousePosition)
    {
        float zDistToObj = _movementTarget.transform.position.z - Camera.main.transform.position.z;

        Vector3 mousePosWithZ = new Vector3(mousePosition.x, mousePosition.y, zDistToObj);
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosWithZ);

        Vector3 vectorMovement = _movementTarget.transform.position;

        switch (_movementAxis)
        {
            case Axis.X:
                vectorMovement[(int)Axis.X] = mousePosWorld[(int)Axis.X] - _holdOffset.x;
                break;
            case Axis.Y:
                vectorMovement[(int)Axis.Y] = mousePosWorld[(int)Axis.Y] - _holdOffset.y;
                break;
            default:
                break;
        }

        _movementTarget.transform.position = vectorMovement;
    }

    public void DragZ(float scrollMovement)
    {
        Vector3 vectorMovement = _movementTarget.transform.position;

        vectorMovement[(int)Axis.Z] += (scrollMovement/120.0f) * (_zMoveSpeed * Time.deltaTime);

        _movementTarget.transform.position = vectorMovement;
    }

    
}
