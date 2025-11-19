using UnityEngine;
using UnityEngine.UI;

public class S_CustomHover : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<Image>().sprite.texture.isReadable)
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
