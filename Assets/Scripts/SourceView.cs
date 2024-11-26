using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SourceView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject sourceObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        sourceObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    { 
        Debug.Log("Exit");
        sourceObject.SetActive(false);
    }
}
