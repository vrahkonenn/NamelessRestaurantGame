using UnityEngine;

public class Tray : MonoBehaviour
{
    public Transform plateSlot;
    public Transform drinkSlot;

    private IngredientData mainCourse;
    private IngredientData side;
    private IngredientData salad;
    private IngredientData drink;

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
                return false;

            Plate plate = item.GetComponent<Plate>();
            if (plate == null)
                return false;

            // 🔥 KOPIOIDAAN DATA LAUTASELTA
            mainCourse = plate.GetIngredient(IngredientType.MainCourse);
            side = plate.GetIngredient(IngredientType.Side);
            salad = plate.GetIngredient(IngredientType.Salad);

            hasPlate = true;

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
                return false;

            hasDrink = true;
            drink = item.ingredientData;

            if (item.trayVisualPrefab != null && drinkSlot != null)
                Instantiate(item.trayVisualPrefab, drinkSlot);

            return true;
        }

        return false;
    }

    // 🔎 Getterit OrderDataa varten

    public string GetMainCourseID()
        => mainCourse != null ? mainCourse.id : null;

    public string GetSideID()
        => side != null ? side.id : null;

    public string GetSaladID()
        => salad != null ? salad.id : null;

    public string GetDrinkID()
        => drink != null ? drink.id : null;

    public void ClearTray()
    {
        mainCourse = null;
        side = null;
        salad = null;
        drink = null;
        hasPlate = false;
        hasDrink = false;
    }
}
