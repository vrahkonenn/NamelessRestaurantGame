using UnityEngine;

public class Tray : MonoBehaviour
{
    public Transform plateSlot;
    public Transform drinkSlot;

    private bool hasPlate = false;
    private bool hasDrink = false;

    public bool AddItem(PickupItem item)
    {
        if (item.ingredientData == null)
            return false;

        IngredientType type = item.ingredientData.type;

        // 🍽 PLATE
        if (type == IngredientType.Plate)
        {
            if (hasPlate)
            {
                Debug.Log("Tarjottimella on jo lautanen!");
                return false;
            }

            hasPlate = true;

            // Siirrä oikea lautanen tarjottimelle
            item.transform.SetParent(plateSlot);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            Collider col = item.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            item.enabled = false;

            return true;
        }

        // 🥤 DRINK
        if (type == IngredientType.Drink)
        {
            if (hasDrink)
            {
                Debug.Log("Tarjottimella on jo juoma!");
                return false;
            }

            hasDrink = true;

            if (item.trayVisualPrefab != null && drinkSlot != null)
            {
                Instantiate(item.trayVisualPrefab, drinkSlot);
            }

            return true;
        }

        return false;
    }
}
