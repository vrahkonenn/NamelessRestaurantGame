using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // default pois
    }

    public void Highlight(bool state)
    {
        outline.enabled = state;
    }
}
