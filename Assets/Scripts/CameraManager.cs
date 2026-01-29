using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.SwitchCameraPrevious.performed += ctx => PreviousCamera();
        inputActions.Player.SwitchCameraNext.performed += ctx => NextCamera();
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.SwitchCameraPrevious.performed -= ctx => PreviousCamera();
        inputActions.Player.SwitchCameraNext.performed -= ctx => NextCamera();
    }

    void Start()
    {
        if (cameras.Length > 0)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(i == 0);
            }
        }
    }

    void NextCamera()
    {
        if (cameras.Length == 0) return;

        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        cameras[currentCameraIndex].gameObject.SetActive(true);

        Debug.Log($"Switched to Camera {currentCameraIndex + 1}");
    }

    void PreviousCamera()
    {
        if (cameras.Length == 0) return;

        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length;
        cameras[currentCameraIndex].gameObject.SetActive(true);

        Debug.Log($"Switched to Camera {currentCameraIndex + 1}");
    }
}