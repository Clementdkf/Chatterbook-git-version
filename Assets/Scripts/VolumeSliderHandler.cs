using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeSliderHandler : MonoBehaviour, IPointerUpHandler
{
    public SliderControl sliderControl;

    public void OnPointerUp(PointerEventData eventData)
    {
        sliderControl.OnVolumeSet();
    }
}
