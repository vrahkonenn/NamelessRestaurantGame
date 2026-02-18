using UnityEngine;
using System.Collections;


public class CuttingBoard : MonoBehaviour
{
    public Transform itemPoint;   // Mihin vihannes spawnaa
    private PickupItem currentItem;
    public int requiredCuts = 10;

    private int currentCuts = 0;
    private Vector3 originalLocalPos;
    private bool isShaking = false;

    void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    public void PlayShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;

        float duration = 0.1f;
        float magnitude = 0.01f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalLocalPos + new Vector3(x, 0f, z);

            yield return null;
        }

        transform.localPosition = originalLocalPos;
        isShaking = false;
    }

    public bool PlaceItem(PickupItem item)
    {
        if (currentItem != null)
        {
            Debug.Log("Leikkuulauta on jo varattu!");
            return false;
        }

        if (item.ingredientData == null)
        {
            Debug.Log("Itemillä ei ole IngredientDataa!");
            return false;
        }

        // ✅ HYVÄKSYTÄÄN VAIN SALAD
        if (item.ingredientData.type != IngredientType.Salad)
        {
            Debug.Log("Leikkuulauta hyväksyy vain Salad-tyypin itemit!");
            return false;
        }

        currentItem = item;

        // Irrotetaan kädestä
        item.Drop();

        item.transform.SetParent(itemPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("Salad asetettu leikkuulaudalle");

        return true;
    }

    public void Cut()
    {
        if (currentItem == null)
            return;

        currentCuts++;

        Debug.Log(currentCuts + "/" + requiredCuts + " leikattu");
        PlayShake();

        if (currentCuts >= requiredCuts)
        {
            FinishCutting();
        }
    }

    public PickupItem TakeItem()
    {
        if (currentItem == null)
            return null;

        PickupItem item = currentItem;

        item.transform.SetParent(null);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        currentItem = null;
        currentCuts = 0;

        return item;
    }

    void FinishCutting()
    {
        Debug.Log("Leikkaus valmis!");

        if (currentItem.ingredientData.cookedPrefab == null)
        {
            Debug.LogWarning("Cooked prefab puuttuu IngredientDatasta!");
            return;
        }

        Vector3 pos = itemPoint.position;
        Quaternion rot = itemPoint.rotation;

        Destroy(currentItem.gameObject);

        PickupItem newItem = Instantiate(
            currentItem.ingredientData.cookedPrefab,
            pos,
            rot
        );

        newItem.transform.SetParent(itemPoint);

        Rigidbody rb = newItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = newItem.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        currentItem = newItem;
        currentCuts = 0;
    }


}
