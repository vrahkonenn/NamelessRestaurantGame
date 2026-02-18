using UnityEngine;
using System.Collections;


public class Knife : MonoBehaviour
{
    private bool isAnimating = false;

    public void PlayCutAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(CutAnimation());
        }
    }

    private IEnumerator CutAnimation()
    {
        Debug.Log("Cut animation");
        isAnimating = true;

        float duration = 0.08f;
        float timer = 0f;

        Quaternion startRot = transform.localRotation;
        Quaternion downRot = startRot * Quaternion.Euler(-35f, 0f, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float f = timer / duration;
            transform.localRotation = Quaternion.Lerp(startRot, downRot, f);
            yield return null;
        }

        timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localRotation = Quaternion.Lerp(downRot, startRot, t);
            yield return null;
        }

        transform.localRotation = startRot;
        isAnimating = false;
    }
}
