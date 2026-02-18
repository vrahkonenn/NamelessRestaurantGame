using UnityEngine;

public class IngredientSource : MonoBehaviour
{
    public PickupItem itemPrefab;

    void Awake()
    {

        if (GetComponent<Outline>() == null)
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.enabled = false;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 7;
        }

        if (GetComponent<Interactable>() == null)
        {
            gameObject.AddComponent<Interactable>();
        }
    }
    public PickupItem SpawnItem()
    {
        PickupItem newItem = Instantiate(itemPrefab);
        return newItem;
    }
}
