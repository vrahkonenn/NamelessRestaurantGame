using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public bool DestroyItem(PickupItem item)
    {
        if (item == null)
            return false;

        Destroy(item.gameObject);
        return true;
    }
}
