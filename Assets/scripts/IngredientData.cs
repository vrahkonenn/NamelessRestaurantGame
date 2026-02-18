using UnityEngine;

public enum IngredientType
{
    MainCourse,
    Side,
    Salad,
    Drink,
    Plate,
    Other
}


[CreateAssetMenu(menuName = "Restaurant/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string id;
    public string ingredientName;
    public PickupItem cookedPrefab;
    public IngredientType type;
    public Vector3 plateOffset;
}
