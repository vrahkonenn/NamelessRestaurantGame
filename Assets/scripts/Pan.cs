using UnityEngine;

public class Pan : MonoBehaviour
{
    public Transform itemPoint;
    public float cookingTime = 5f;

    private PickupItem currentItem;
    private float cookTimer;
    private bool isCooking;

    void Update()
    {
        if (isCooking && currentItem != null)
        {
            cookTimer += Time.deltaTime;

            if (cookTimer >= cookingTime)
            {
                FinishCooking();
            }
        }
    }

    public bool TryPlaceItem(PickupItem item)
    {
        if (currentItem != null)
            return false;

        if (item.ingredientData.type != IngredientType.MainCourse)
            return false;

        currentItem = item;

        item.transform.SetParent(itemPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.GetComponent<Rigidbody>().isKinematic = true;
        item.GetComponent<Collider>().enabled = false;

        cookTimer = 0f;
        isCooking = true;

        return true;
    }

void FinishCooking()
{
    if (currentItem == null)
        return;

    if (currentItem.ingredientData.cookedPrefab == null)
    {
        isCooking = false;
        return;
    }

    // 🔥 Spawn cooked versio
    PickupItem cookedPI = Instantiate(currentItem.ingredientData.cookedPrefab, itemPoint);

    cookedPI.transform.localPosition = Vector3.zero;
    cookedPI.transform.localRotation = Quaternion.identity;

    PickupItem cooked = cookedPI.GetComponent<PickupItem>();

    // 🔒 Lukitse pannuun
    Rigidbody rb = cooked.GetComponent<Rigidbody>();
    Collider col = cooked.GetComponent<Collider>();

    if (rb != null)
        rb.isKinematic = true;

    if (col != null)
        col.enabled = false;

    // Poista raw
    Destroy(currentItem.gameObject);

    currentItem = cooked;
    isCooking = false;

    Debug.Log("Paistaminen valmis – prefab vaihdettu.");
}


    public PickupItem TakeItem()
    {
        if (currentItem == null)
            return null;

        PickupItem item = currentItem;

        currentItem = null;
        cookTimer = 0f;
        isCooking = false;

        return item;
    }
}
