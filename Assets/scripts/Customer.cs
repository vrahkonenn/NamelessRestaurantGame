using UnityEngine;

public class Customer : MonoBehaviour
{
    public OrderData currentOrder;
    private bool hasOrdered = false;

    public bool Interact(PickupItem heldItem)
    {
        // 1️⃣ Jos ei ole vielä tilannut → generoi tilaus
        if (!hasOrdered)
        {
            currentOrder = OrderManager.Instance.GenerateOrder();
            hasOrdered = true;

            Debug.Log("Asiakas tilasi: " 
            + currentOrder.mainCourse.ingredientName + 
            " " + currentOrder.side.ingredientName +
            " " + currentOrder.salad.ingredientName + 
            " " + currentOrder.drink.ingredientName);
            return false;
        }

        // 2️⃣ Jos pelaajalla ei ole mitään kädessä
        if (heldItem == null)
        {
            Debug.Log("Asiakas odottaa tilaustaan.");
            return false;
        }

        // 3️⃣ Jos kädessä ei ole tarjotinta
        Tray tray = heldItem.GetComponent<Tray>();
        if (tray == null)
        {
            Debug.Log("Tarvitset tarjottimen!");
            return false;
        }

        // 4️⃣ Tarkistetaan tilaus
        if (currentOrder.Matches(tray))
        {
            Debug.Log("Tilaus oikein!");
            tray.ClearTray();
            Destroy(heldItem.gameObject);
            hasOrdered = false;
            return true;
        }
        else
        {
            Debug.Log("Väärä tilaus!");
            return false;
        }
    }
}
