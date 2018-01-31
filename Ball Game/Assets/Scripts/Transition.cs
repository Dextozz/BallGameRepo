using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

    Image panelImage;

    [HideInInspector]
    public static float alpha;

    void Start()
    {
        panelImage = GetComponent<Image>();
        alpha = 0;
    }

    public IEnumerator DoTransition()
    {
        yield return new WaitForSeconds(1.0f);

        while (alpha < 1)
        {

            yield return new WaitForSeconds(0.01f);

            alpha += 0.042f;

            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
        }

        alpha = 1.0f;
    }
}
