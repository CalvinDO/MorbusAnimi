using UnityEngine;

public class MAInteractable : MonoBehaviour
{

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
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void removeHover()
    {
        this.meshRenderer.material = standardMaterial;
    }

    public void setHover()
    {
        this.meshRenderer.material = MAPlayerMovement2.highlightMaterial;
    }

}
