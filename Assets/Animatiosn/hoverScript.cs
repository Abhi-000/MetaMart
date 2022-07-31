using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hoverScript : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]Animator mainBtn;
    [SerializeField] Animation playBtn;
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        mainBtn.SetBool("popIn",true);
        if(playBtn!=null)
        playBtn.Play("popInBtn");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        if (eventData.pointerCurrentRaycast.gameObject.name != "playBtn")
        {
            mainBtn.SetBool("popIn", false);
            if (playBtn != null)
                playBtn.Play("popOutBtn");
        }
    }
}
