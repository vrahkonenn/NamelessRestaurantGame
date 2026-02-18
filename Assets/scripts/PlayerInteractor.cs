using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;

    public Transform leftHoldPoint;
    public Transform rightHoldPoint;

    private PickupItem leftHeldItem;
    private PickupItem rightHeldItem;
    private Interactable currentHover;

    void Update()
    {
        HandleHover();

        if (Input.GetMouseButtonDown(0))
        {
            HandleHand(ref leftHeldItem, leftHoldPoint, true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleHand(ref rightHeldItem, rightHoldPoint, false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCut();
        }
    }

    void TryCut()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance, interactLayer))
            return;

        CuttingBoard board = hit.collider.GetComponentInParent<CuttingBoard>();
        if (board == null)
            return;

        if (leftHeldItem != null && leftHeldItem.CompareTag("Knife"))
        {
            Knife knife = leftHeldItem.GetComponent<Knife>();
            board.Cut();
            if (knife != null) knife.PlayCutAnimation();
            return;
        }

        if (rightHeldItem != null && rightHeldItem.CompareTag("Knife"))
        {
            Knife knife = rightHeldItem.GetComponent<Knife>();
            board.Cut();
            if (knife != null) knife.PlayCutAnimation();
            return;
        }

        Debug.Log("Tarvitset veitsen leikkaamiseen!");
    }

    void HandleHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != currentHover)
            {
                if (currentHover != null)
                    currentHover.Highlight(false);

                currentHover = interactable;

                if (currentHover != null)
                    currentHover.Highlight(true);
            }
        }
        else
        {
            if (currentHover != null)
            {
                currentHover.Highlight(false);
                currentHover = null;
            }
        }
    }

    void HandleHand(ref PickupItem heldItem, Transform holdPoint, bool isLeftHand)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {

            Customer customer = hit.collider.GetComponentInParent<Customer>();
            if (customer != null)
            {
                if (customer.Interact(heldItem))
                {
                    heldItem = null;
                }
                return;
            }

            if (heldItem != null)
            {
                CuttingBoard board = hit.collider.GetComponentInParent<CuttingBoard>();
                if (board != null)
                {
                    if (board.PlaceItem(heldItem))
                        heldItem = null;
                    return;
                }

                Pan pan = hit.collider.GetComponentInParent<Pan>();
                if (pan != null)
                {
                    if (pan.TryPlaceItem(heldItem))
                        heldItem = null;
                    return;
                }

                TrashBin trash = hit.collider.GetComponentInParent<TrashBin>();
                if (trash != null)
                {
                    if (trash.DestroyItem(heldItem))
                        heldItem = null;
                    return;
                }

                Plate plate = hit.collider.GetComponentInParent<Plate>();
                if (plate != null)
                {
                    if (plate.AddIngredient(heldItem))
                    {
                        Destroy(heldItem.gameObject);
                        heldItem = null;
                    }
                    return;
                }

                Tray tray = hit.collider.GetComponentInParent<Tray>();
                if (tray != null)
                {
                    if (tray.AddItem(heldItem))
                    {
                        if (heldItem.ingredientData.type == IngredientType.Drink)
                            Destroy(heldItem.gameObject);

                        heldItem = null;
                    }
                    return;
                }

                PlaceSurface surface = hit.collider.GetComponentInParent<PlaceSurface>();
                if (surface != null)
                {
                    if (surface.PlaceItem(heldItem, hit.point))
                        heldItem = null;
                    return;
                }
            }

            if (heldItem == null)
            {
                IngredientSource source = hit.collider.GetComponent<IngredientSource>();
                if (source != null)
                {
                    PickupItem newItem = source.SpawnItem();
                    HoldItem(newItem, ref heldItem, holdPoint, isLeftHand);
                    return;
                }

                CuttingBoard board = hit.collider.GetComponentInParent<CuttingBoard>();
                if (board != null)
                {
                    PickupItem itemFromBoard = board.TakeItem();
                    if (itemFromBoard != null)
                    {
                        HoldItem(itemFromBoard, ref heldItem, holdPoint, isLeftHand);
                        return;
                    }
                }

                Pan pan = hit.collider.GetComponentInParent<Pan>();
                if (pan != null)
                {
                    PickupItem itemFromPan = pan.TakeItem();
                    if (itemFromPan != null)
                    {
                        HoldItem(itemFromPan, ref heldItem, holdPoint, isLeftHand);
                        return;
                    }
                }

                PickupItem item = hit.collider.GetComponent<PickupItem>();
                if (item != null)
                {
                    HoldItem(item, ref heldItem, holdPoint, isLeftHand);
                    return;
                }
            }
        }

        if (heldItem != null)
        {
            heldItem.Drop();
            heldItem = null;
        }
    }

    void HoldItem(PickupItem item, ref PickupItem heldItem, Transform holdPoint, bool isLeftHand)
    {
        heldItem = item;
        item.PickUp(holdPoint, isLeftHand);
    }
}
