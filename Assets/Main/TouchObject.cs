using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchObject : MonoBehaviour, IPointerDownHandler
{
    public Action<Vector3> Touching;
    public void OnPointerDown(PointerEventData eventData)
    {      
        Vector3 localContactPosition = transform.InverseTransformPoint(eventData.pointerPressRaycast.worldPosition);
        if(Touching != null)
        Touching.Invoke(localContactPosition);     
    }
}
