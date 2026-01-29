using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{

    public float interactDistance = 2f;
    public InteractableObject currentInteractable = null;
    public InputSystem_Actions inputActions;
    public Transform playerDetectTransform;
    public LayerMask interactLayer;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += Interact;
    }
    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Interact.performed -= Interact;
    }

    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(playerDetectTransform.position, playerDetectTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            currentInteractable = interactable;
            UIManager.Instance.ShowInteract();
        }
        else
        {
            currentInteractable = null;
            UIManager.Instance.HideInteract();
        }
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interacted();
        }
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(playerDetectTransform.position, playerDetectTransform.forward);
        Vector3 rayEnd = ray.origin + ray.direction * interactDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(ray.origin, rayEnd);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rayEnd, 0.2f);

        if (currentInteractable != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(currentInteractable.transform.position, 0.3f);
        }
    }
}
