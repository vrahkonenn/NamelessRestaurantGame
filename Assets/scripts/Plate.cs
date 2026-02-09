using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform ingredientParent;

    private List<IngredientData> ingredients = new List<IngredientData>();

    public void AddIngredient(PickupItem item)
    {   
        Debug.Log("Add ingredientissä");
        if (item.ingredientData == null)
        {
            Debug.Log("IngredientData puuttuu!");
            return;
        }

        ingredients.Add(item.ingredientData);

        if (item.plateVisualPrefab != null && ingredientParent != null)
        {
            Instantiate(item.plateVisualPrefab, ingredientParent);
        }
        else
        {
            Debug.Log("PlateVisualPrefab tai IngredientParent puuttuu!");
        }

        Destroy(item.gameObject);
    }
}
