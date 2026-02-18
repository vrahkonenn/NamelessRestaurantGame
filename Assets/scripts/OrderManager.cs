using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public List<IngredientData> allIngredients;

    void Awake()
    {
        Instance = this;
    }

    public OrderData GenerateOrder()
    {
        OrderData order = new OrderData();

        order.mainCourse = GetRandom(IngredientType.MainCourse);
        order.side = GetRandom(IngredientType.Side);
        order.salad = GetRandom(IngredientType.Salad);
        order.drink = GetRandom(IngredientType.Drink);

        return order;
    }

    IngredientData GetRandom(IngredientType type)
    {
        List<IngredientData> filtered =
            allIngredients.FindAll(i => i.type == type);

        return filtered[Random.Range(0, filtered.Count)];
    }
}
