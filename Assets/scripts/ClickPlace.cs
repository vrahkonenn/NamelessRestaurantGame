using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickPlace : MonoBehaviour
{
    public Transform cloneObj;
    void Start()
    {
        
    }

    // Update is called once per frame


    private void OnMouseDown()
    {
        if (gameObject.name == "steak")
        {
            Debug.Log("pihviä klikattiin");
            Instantiate(cloneObj, new Vector3(1.344f, -1.389f, -5.942f), cloneObj.rotation);
        }

        if (gameObject.name == "french_fries")
        {
            Debug.Log("ranskalaisia klikattiin");
            Instantiate(cloneObj, new Vector3(1.668f, -1.376f, -5.733f), cloneObj.rotation);
        }
        
        if (gameObject.name == "tomato")
        {
            Debug.Log("tomaattia klikattiin");
            Instantiate(cloneObj, new Vector3(1.705f, -1.466f, -6.107f), cloneObj.rotation);
        }

        if (gameObject.name == "milk")
        {
            Debug.Log("maitoa klikattiin");
            Instantiate(cloneObj, new Vector3(0.92f, -1.449f, -5.664f), cloneObj.rotation);
        }

        if (gameObject.name == "plate")
        {
            Debug.Log("lautasta klikattiin");
            Instantiate(cloneObj, new Vector3(1.536f, -1.464f, -6.045f), cloneObj.rotation);
        }
        
        if (gameObject.name == "tray")
        {
            Debug.Log("tarjotinta klikattiin");
            Instantiate(cloneObj, new Vector3(1.507f, -1.455f, -6.043f), cloneObj.rotation);
        }
    }
}
