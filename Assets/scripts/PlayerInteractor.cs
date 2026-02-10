using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;

    public Transform leftHoldPoint;
    public Transform rightHoldPoint;

    private PickupItem leftHeldItem;
    private PickupItem rightHeldItem;

    void Update()
    {
        // VASEN KÄSI (Mouse0)
        if (Input.GetMouseButtonDown(0))
        {
            HandleHand(ref leftHeldItem, leftHoldPoint, true);
        }

        // OIKEA KÄSI (Mouse1)
        if (Input.GetMouseButtonDown(1))
        {
            HandleHand(ref rightHeldItem, rightHoldPoint, false);
        }
    }
   
    void HandleHand(ref PickupItem heldItem, Transform holdPoint, bool isLeftHand)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 🔥 1. Jos kädessä on item ja osutaan lautaseen → lisää ingredient
            if (heldItem != null)
            {
                Plate plate = hit.collider.GetComponentInParent<Plate>();
                if (plate != null)
                {
                    Debug.Log("Osui lautaseen");
                    bool success = plate.AddIngredient(heldItem);

                    if (success)
                    {
                        Destroy(heldItem.gameObject);
                        heldItem = null;
                    }
                    return;
                }

                Tray tray = hit.collider.GetComponentInParent<Tray>();
                if (tray != null)
                {
                    bool success = tray.AddItem(heldItem);
                    if (success)
                    {
                        if (heldItem.ingredientData.type == IngredientType.Drink)
                        {
                            Destroy(heldItem.gameObject);
                        }

                        heldItem = null;
                    }
                    return;
                }

            }

            // 🔥 2. Jos käsi on tyhjä → pickup
            if (heldItem == null)
            {
                IngredientSource source = hit.collider.GetComponent<IngredientSource>();
                if (source != null)
                {
                    PickupItem newItem = source.SpawnItem();
                    HoldItem(newItem, ref heldItem, holdPoint, isLeftHand);
                    return;
                }

                PickupItem item = hit.collider.GetComponent<PickupItem>();
                if (item != null)
                {
                    HoldItem(item, ref heldItem, holdPoint, isLeftHand);
                    return;
                }
            }
        }

        // 🔥 3. Jos ei osuttu lautaseen → droppaa
        if (heldItem != null)
        {
            heldItem.Drop();
            heldItem = null;
        }
    }


    void TryPickup(ref PickupItem heldItem, Transform holdPoint, bool isLeftHand)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 🔥 1. Tarkista lautanen ensin
            Plate plate = hit.collider.GetComponent<Plate>();
            if (plate != null && heldItem != null)
            {
                Debug.Log("osui lautaseen");
                plate.AddIngredient(heldItem);
                heldItem = null;
                return;
            }

            // 🔥 2. Spawn source
            IngredientSource source = hit.collider.GetComponent<IngredientSource>();
            if (source != null && heldItem == null)
            {
                PickupItem newItem = source.SpawnItem();
                HoldItem(newItem, ref heldItem, holdPoint, isLeftHand);
                return;
            }

            // 🔥 3. Pickup world item
            PickupItem item = hit.collider.GetComponent<PickupItem>();
            if (item != null && heldItem == null)
            {
                HoldItem(item, ref heldItem, holdPoint, isLeftHand);
            }
        }
    }


    void HoldItem(PickupItem item, ref PickupItem heldItem, Transform holdPoint, bool isLeftHand)
    {
        heldItem = item;
        item.PickUp(holdPoint, isLeftHand);
    }
}
