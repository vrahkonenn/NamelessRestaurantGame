using UnityEngine;

public class IngredientSource : MonoBehaviour
{
    public PickupItem itemPrefab;

    public PickupItem SpawnItem()
    {
        PickupItem newItem = Instantiate(itemPrefab);
        return newItem;
    }
}
