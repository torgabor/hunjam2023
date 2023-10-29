using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class POIArrow : MonoBehaviour
{
    [SerializeField] Image POIImage;

    [SerializeField] Sprite POIArrowRegular, POIArrowFlipped;

    private void Start()
    {
        POIImage.sprite = POIArrowRegular;
    }

    private void Update()
    {
        if (Mathf.FloorToInt(Time.timeSinceLevelLoad % 2) == 1)
        {
            POIImage.sprite = POIArrowFlipped;
        }
        else
        {
            POIImage.sprite = POIArrowRegular;
        }
        POIImage.rectTransform.position += Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * 8f) * Time.deltaTime * 0.4f;
    }
}
