using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    public Transform singleGrip;
    public Transform leftGrip;
    public Transform rightGrip;
    public IngredientData ingredientData;
    public GameObject plateVisualPrefab;
    public GameObject trayVisualPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

public void PickUp(Transform holdPoint, bool isLeftHand)
{
    rb.isKinematic = true;
    col.enabled = false;

    transform.SetParent(holdPoint);
    transform.localScale = Vector3.one;

    Transform chosenGrip = null;

    if (isLeftHand && leftGrip != null)
        chosenGrip = leftGrip;
    else if (!isLeftHand && rightGrip != null)
        chosenGrip = rightGrip;
    else if (singleGrip != null)
        chosenGrip = singleGrip;

    if (chosenGrip != null)
    {
        transform.localPosition = -chosenGrip.localPosition;
        transform.localRotation = Quaternion.Inverse(chosenGrip.localRotation);
    }
    else
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}


    public void Drop()
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        rb.isKinematic = false;
        col.enabled = true;
    }
}
