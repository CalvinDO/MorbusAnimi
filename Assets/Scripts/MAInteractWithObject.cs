using UnityEngine;

public class MAInteractWithObject : MAInteractable
{

    public MAItem item;
    public MAInventory inventory;

    public override void MAInteract()
    {
        inventory = MAInventory.instance;
        base.MAInteract();

        if (this.currentSelection == interactType.item)
        {
            PickUp();
        }
        else
        {
            Open();
        }
    }

    void PickUp()
    {
        Debug.Log("Picked up " + this.item.name);
        MAInventory.instance.Add(this.item);
        Destroy(this.gameObject);
    }

    void Open()
    {
        if (inventory.items.Contains(this.item))
        {
            Debug.Log("opened door.");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("need key.");
        }
    }
}