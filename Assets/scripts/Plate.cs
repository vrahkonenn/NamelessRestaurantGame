using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform ingredientParent;
    public List<IngredientType> allowedTypes;

    // Käytetään tätä estämään duplikaatit
    private HashSet<IngredientType> usedTypes = new HashSet<IngredientType>();

    public bool AddIngredient(PickupItem item)
    {
        if (item.ingredientData == null)
            return false;

        IngredientType type = item.ingredientData.type;

        // Onko tyyppi sallittu?
        if (!allowedTypes.Contains(type))
        {
            Debug.Log("Tätä ei voi laittaa lautaselle!");
            return false;
        }

        // 🔒 Onko samaa tyyppiä jo lautasella?
        if (usedTypes.Contains(type))
        {
            Debug.Log("Tätä tyyppiä on jo lautasella!");
            return false;
        }

        // Lisää tyyppi käytetyksi
        usedTypes.Add(type);

        // 🔥 Spawn täsmälleen samalla tavalla kuin ennen
        if (item.plateVisualPrefab != null && ingredientParent != null)
        {
            Instantiate(item.plateVisualPrefab, ingredientParent);
        }
        else
        {
            Debug.Log("PlateVisualPrefab tai IngredientParent puuttuu!");
        }

        return true;
    }
}
