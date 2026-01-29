using UnityEngine;


//By ExelCoeg
/// <summary>
/// Base class for all interactable objects in the scene.
/// </summary>
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    public bool isInteractable;
    Material outline;

    public abstract void Interacted();

    // Make sure that the object has a mesh renderer and a material with the name "Outline" (use shader file from repo) 
    // Make material with "Outline" shader and assign it to the object.
    public virtual void Awake()
    {
        if (TryGetComponent(out MeshRenderer meshRenderer))
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                if (meshRenderer.materials[i].name.Contains("Outline"))
                {
                    outline = meshRenderer.materials[i];
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (!isInteractable && outline.GetFloat("_Scale") > 0)
        {
            outline.SetFloat("_Scale", 0f);
        }
    }
    public void DisableOutline()
    {
        if (outline != null && isInteractable)
        {
            outline.SetFloat("_Scale", 0f);
            // UIManager.Instance.HideInteract();
        }
    }
    public void EnableOutline()
    {
        if (outline != null && isInteractable)
        {
            outline.SetFloat("_Scale", 1.025f);
            // UIManager.Instance.ShowInteract();
        }
    }

    public void EnableInteractable()
    {
        isInteractable = true;
    }
    public void DisableInteractable()
    {
        isInteractable = false;
    }
}