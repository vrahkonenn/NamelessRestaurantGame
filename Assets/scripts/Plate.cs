using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform ingredientParent;
    public List<IngredientType> allowedTypes;

    private HashSet<IngredientType> usedTypes = new HashSet<IngredientType>();

    // 🔥 TÄMÄ ON TÄRKEÄ
    private Dictionary<IngredientType, IngredientData> ingredients =
        new Dictionary<IngredientType, IngredientData>();

    public bool AddIngredient(PickupItem item)
    {
        if (item.ingredientData == null)
            return false;

        IngredientType type = item.ingredientData.type;

        if (!allowedTypes.Contains(type))
            return false;

        if (usedTypes.Contains(type))
            return false;

        usedTypes.Add(type);

        // 🔥 TALLENNETAAN OIKEA DATA
        ingredients[type] = item.ingredientData;

        if (item.plateVisualPrefab != null && ingredientParent != null)
            Instantiate(item.plateVisualPrefab, ingredientParent);

        return true;
    }

    // 🔎 Getterit tarjottimelle
    public IngredientData GetIngredient(IngredientType type)
    {
        if (ingredients.ContainsKey(type))
            return ingredients[type];

        return null;
    }

    public void ClearPlate()
    {
        ingredients.Clear();
        usedTypes.Clear();
    }
}
