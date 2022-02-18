using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCameraController : MonoBehaviour
{
    //Tool controls object
    private ToolControls _tc;
    public float _movementSpeed = 2.0f;
    public float _mouseSensitivity = 90.0f;


    //Variables
    bool _cameraControlsActive = false;
    float _rotationOnX = 0.0f;
    float _rotationOnY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Grab tools controller asset from game manager
        _tc = Utility.GetToolControlsAsset();
        //Initialize actions for this tool
        InitInputActions();
    }

    private void InitInputActions()
    {
        _tc.Default.ControlCamera.started += ctx => EnableCameraControls();
        _tc.Default.ControlCamera.canceled += ctx => DisableCameraControls();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraControlsActive)
        {
            CameraRotation();
        }

        CameraMovement(_tc.Default.Movement.ReadValue<Vector2>());
        CameraRiseAndFall(_tc.Default.RiseFall.ReadValue<float>());
    }

    private void CameraRotation()
    {
        //Get mouse delta movement
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * (Time.deltaTime * _mouseSensitivity);

        //Rotate camera up and down
        _rotationOnX -= mouseDelta.y;
        //Rotate camera left and right
        _rotationOnY += mouseDelta.x;
        _rotationOnX = Mathf.Clamp(_rotationOnX, -90.0f, 90.0f);
        transform.localEulerAngles = new Vector3(_rotationOnX, _rotationOnY, 0.0f);
    }

    private void CameraMovement(Vector2 moveInput)
    {
        //Get forward direction
        Vector3 forwardDirection = transform.forward.normalized;

        //Get left direction
        Vector3 leftDirection = new Vector3(1.0f, 0.0f, 0.0f);
        leftDirection = transform.rotation * leftDirection;

        //Use data
        Vector3 forwardMovement = (forwardDirection * moveInput.y) * (_movementSpeed * Time.deltaTime);
        Vector3 sidewaysMovement = (leftDirection * moveInput.x) * (_movementSpeed * Time.deltaTime);

        transform.position += forwardMovement;
        transform.position += sidewaysMovement;
    }

    private void CameraRiseAndFall(float movement)
    {
        //Get direction
        Vector3 upDirection = Vector3.up;

        //use date
        Vector3 upMovement = (upDirection * movement) * (_movementSpeed * Time.deltaTime);
        transform.position += upMovement;
    }

    private void EnableCameraControls()
    {
        _cameraControlsActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void DisableCameraControls()
    {
        _cameraControlsActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


}
