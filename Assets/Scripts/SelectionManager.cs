using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SelectionManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField]private GameObject crosshair,equipHover,arHover,qrImage;
    public void Awake()
    {
        PV = transform.GetComponent<PhotonView>();
    }
    //[SerializeField]Material mat;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("man"))
        {
            equipHover.SetActive(true);
            arHover.SetActive(true);
            Debug.Log(other.transform.name);
            if (Input.GetKeyDown(KeyCode.E))
            { 
            string str = transform.name+"|"+other.transform.name;
            Debug.Log(str);
            PV.RPC("SendNameWithMan", RpcTarget.All,str );
            // Debug.Log(other.transform.GetChild(0));
            // Debug.Log(other.transform.GetChild(0).GetChild(2));
                transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTextureOffset =
                     new Vector2(0, int.Parse(other.transform.name) * .25f);
                
            }
            if (Input.GetKeyDown(KeyCode.X))
                { 
              qrImage.SetActive(true);
                }
        } 
        if(other.CompareTag("arObj"))
            {   
                arHover.SetActive(true);
                if (Input.GetKeyDown(KeyCode.X))
                { 
                    Debug.Log("E preseed");
                    qrImage.SetActive(true);
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("man"))
        // {
            equipHover.SetActive(false);
            arHover.SetActive(false);
            qrImage.SetActive(false);
        //}
    }
    [PunRPC]
    void SendNameWithMan(string str)
    {
        string[] splitArray =  str.Split(char.Parse("|")); 
        Debug.Log(str + "Recceiver");
        if(!PV.IsMine)
         GameObject.Find(splitArray[0]).transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTextureOffset =
                     new Vector2(0, int.Parse(splitArray[1]) * .25f);
        //Debug.Log(url);
        //String recieved_string = doodookaka;
    
    }
}
