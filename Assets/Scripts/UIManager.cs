using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI interactText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowInteract()
    {
        interactText.gameObject.SetActive(true);
    }

    public void HideInteract()
    {
        interactText.gameObject.SetActive(false);
    }
}
