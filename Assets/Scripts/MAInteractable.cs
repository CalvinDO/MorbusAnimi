using UnityEngine;

public class MAInteractable : MonoBehaviour
{
    public enum interactType {item, obstacle};
    public interactType currentSelection = interactType.item;
    public float radius = 3f;
    public GameObject textDisplay;
    public UnityEngine.UI.Text hoverTextObject;

    private Material standardMaterial;
    private MeshRenderer meshRenderer;
    private string newHoverText;

private void Start()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.standardMaterial = this.meshRenderer.material;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (currentSelection == interactType.item)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        } 
        else
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale * radius);
        }
    }

    public void removeHover()
    {
        textDisplay.SetActive(false);
        this.meshRenderer.material = standardMaterial;
    }

    public void setHover()
    {
        switch (currentSelection)
        {
            case interactType.item:
                newHoverText = "pick up [E]";
                break;
            case interactType.obstacle:
                newHoverText = "open [E]";
                break;
            default:
                newHoverText = "interact [E]";
                break;
        }
        textDisplay.SetActive(true);
        hoverTextObject.text = newHoverText;
        this.meshRenderer.material = MAPlayerMovement2.highlightMaterial;
    }

    public virtual void MAInteract()
    {
        // this is overwritten
    }
}
