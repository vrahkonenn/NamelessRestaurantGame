[System.Serializable]
public class OrderData
{
    public IngredientData mainCourse;
    public IngredientData side;
    public IngredientData salad;
    public IngredientData drink;

    public bool Matches(Tray tray)
    {
        if (tray == null)
            return false;

        if (tray.GetMainCourseID() != mainCourse.id)
            return false;

        if (tray.GetSideID() != side.id)
            return false;

        if (tray.GetSaladID() != salad.id)
            return false;

        if (tray.GetDrinkID() != drink.id)
            return false;

        return true;
    }

}
