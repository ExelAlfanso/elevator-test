using UnityEngine;

public class CameraSwitchView : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        // Enable only the first camera
        if (cameras.Length > 0)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(i == 0);
            }
        }
    }

    void Update()
    {
        // Switch to previous camera with Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PreviousCamera();
        }

        // Switch to next camera with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextCamera();
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

