using UnityEngine;

public class MAInteractable : MonoBehaviour
{
    public enum interactType {item, obstacle};
    public interactType currentSelection = interactType.item;
    public float radius = 3f;

    private Material standardMaterial;
    private MeshRenderer meshRenderer;

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
        this.meshRenderer.material = standardMaterial;
    }

    public void setHover()
    {
        this.meshRenderer.material = MAPlayerMovement2.highlightMaterial;
    }

    public virtual void MAInteract()
    {
        // this is meant to be overwritten
    }
}
