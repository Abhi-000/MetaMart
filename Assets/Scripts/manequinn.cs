using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class manequinn : MonoBehaviour
{
    PhotonView PV;
    public void Awake() {
        //PV = GetComponent<PhotonView>();
    }
    
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player"))
        {
            PV = other.transform.GetComponent<PhotonView>();
            Debug.Log("PLayer entered");
            if (Input.GetKeyDown(KeyCode.E))
            { Debug.Log("E preseed");
                //if(PV.IsMine){
                //Debug.Log("PV mine");
                other.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTextureOffset =
                     new Vector2(0, int.Parse(transform.name) * .25f);
                //mat.mainTextureOffset = new Vector2(0, int.Parse(other.transform.name) * .25f);
                //}
            }
        }
    }
}
