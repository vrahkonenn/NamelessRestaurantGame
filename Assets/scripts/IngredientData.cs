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
    public string ingredientName;
    public IngredientType type;
    public Vector3 plateOffset;
}
