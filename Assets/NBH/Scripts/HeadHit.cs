using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadHit : MonoBehaviour
{
    Image image;
    Color alpha;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        alpha = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        alpha.a = Mathf.Lerp(alpha.a, 0, 5 * Time.deltaTime);
        image.color = alpha;
        if (image.color.a < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
