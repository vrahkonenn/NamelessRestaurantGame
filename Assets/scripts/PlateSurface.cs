using UnityEngine;

public class PlaceSurface : MonoBehaviour
{
    public Transform surfaceParent; // mihin item parentoidaan

    public bool PlaceItem(PickupItem item, Vector3 hitPoint)
    {
        if (item == null)
            return false;

        item.Drop(); // irrota kädestä

        item.transform.SetParent(surfaceParent != null ? surfaceParent : transform);

        // Aseta tarkasti raycast-osumakohtaan
        item.transform.position = hitPoint + Vector3.up * 0.02f;
        item.transform.rotation = Quaternion.identity;


        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        return true;
    }
}
