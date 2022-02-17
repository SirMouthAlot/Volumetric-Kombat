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

    public void Drag(Vector3 moveVal)
    {
        Vector3 vectorMovement = _movementTarget.transform.position;

        switch(_movementAxis)
        {
            case Axis.X:
                vectorMovement[(int)Axis.X] = moveVal[(int)Axis.X] - _holdOffset.x;
                break;
            case Axis.Y:
                vectorMovement[(int)Axis.Y] = moveVal[(int)Axis.Y] - _holdOffset.y;
                break;
            case Axis.Z:
                vectorMovement[(int)Axis.Z] = moveVal[(int)Axis.Y];
                break;
            default:
                break;
        }

        _movementTarget.transform.position = vectorMovement;
    }
}
