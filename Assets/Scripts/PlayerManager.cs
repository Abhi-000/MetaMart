using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using ReadyPlayerMe;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] string link;
    PhotonView PV;
    private AvatarLoader avatarLoader;
    GameObject controller,map;
    [SerializeField]Texture2D tex;
    [SerializeField]Shader shader;
    int kills;
    int deaths;

    void Awake()
    {
        avatarLoader = new AvatarLoader();
        map = GameObject.Find("map");
        PV = GetComponent<PhotonView>();
        link = WebAvatarLoader.instance.link;
        Debug.Log(link);
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        //enableMapColliders();
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        Debug.Log(controller.name);
        //controller.SetActive(false);
        //Debug.Log(WebAvatarLoader.AvatarURL);
        if (RoomManager.Instance.isTest)
        {
            //Debug.Log("is test");
            PV.RPC("SendString", RpcTarget.All, link);
            avatarLoader.LoadAvatar(link, OnAvatarImported, OnAvatarLoaded);
        }
        else
        {
            PV.RPC("SendString", RpcTarget.All, WebAvatarLoader.AvatarURL);
            avatarLoader.LoadAvatar(WebAvatarLoader.AvatarURL, OnAvatarImported, OnAvatarLoaded);
        }
        //if (avatar) Destroy(avatar);
        //controller.GetComponent<MeshRenderer>().
    }
    void enableMapColliders()
    {
        MeshRenderer[] allCollider = map.transform.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < allCollider.Length; i++)
        {
            if (allCollider[i].CompareTag("Collide"))
                allCollider[i].gameObject.AddComponent<MeshCollider>().convex = true;
        }
    }
    [PunRPC]
    void SendString(string url)
    {
        if(!PV.IsMine)
        avatarLoader.LoadAvatar(url, OnAvatarImported, onOtherPlayerAvatarLoaded);
        //Debug.Log(url);
        //String recieved_string = doodookaka;
    
    }

    private void onOtherPlayerAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        foreach (var view in photonViews)
        {
            if(view.transform.childCount>0)
            {
                if(!view.IsMine && view.name!="RoomManager")
                {
                    avatar.transform.parent = view.transform;
                    Debug.Log(view.name);
                    view.GetComponent<ThirdPersonMovement>().enabled = false;
                    avatar.transform.localPosition = new Vector3(0, -2, 0);
                    avatar.transform.localEulerAngles = Vector3.zero;
                    avatar.transform.localScale = new Vector3(3, 3, 3);
                    avatar.transform.SetAsFirstSibling();
                    avatar.transform.parent.name = "Player"+PV.ViewID;
                    Transform go = Instantiate(avatar.transform.GetChild(0), avatar.transform);
                    Material newMat = new  Material(shader);
                    newMat.SetTexture("_MainTex",tex);
                    newMat.SetFloat("_Cutoff", 1);
                    newMat.mainTextureScale = new Vector2(1, 0.25f);
                    newMat.mainTextureOffset =  new Vector2(0,-0.5f);
                    go.GetComponent<SkinnedMeshRenderer>().material = newMat;
                    go.GetComponent<SkinnedMeshRenderer>().sortingOrder = 10;
                   
                }
            }
        }
         RoomManager.Instance.screenLoaded = true;
          RoomManager.Instance.loadingPanel.GetComponent<Animation>().Play("fadeOut");
		 RoomManager.Instance.loadingPanel.SetActive(false);
    }
    private void OnAvatarImported(GameObject avatar)
    {
        //Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        avatar.transform.parent = controller.transform;
        avatar.transform.localPosition = new Vector3(0, -2, 0);
        avatar.transform.localEulerAngles = Vector3.zero;
        avatar.transform.localScale = new Vector3(3, 3,3);
        avatar.tag = "Player";
        avatar.transform.parent.name = "Player"+PV.ViewID;
        //avatar.AddComponent<BoxCollider>().isTrigger = true;
        avatar.transform.SetAsFirstSibling();
        Transform go = Instantiate(avatar.transform.GetChild(0), avatar.transform);
        mat.mainTextureScale = new Vector2(1, 0.25f);
        mat.mainTextureOffset =  new Vector2(0,-0.5f);
        go.GetComponent<SkinnedMeshRenderer>().material = mat;
        go.GetComponent<SkinnedMeshRenderer>().sortingOrder = 10;
        controller.SetActive(true);
        GameObject cam = GameObject.Find("TPPCam");
        cam.transform.GetComponent<CinemachineFreeLook>().LookAt = controller.transform;
        cam.transform.GetComponent<CinemachineFreeLook>().Follow = controller.transform;
        //Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
        ThirdPersonMovement[] allGo = FindObjectsOfType<ThirdPersonMovement>();
        for (int i = 0; i < allGo.Length; i++)
        {
            if (allGo[i].enabled)
            {
                Debug.Log(allGo[i].transform.localPosition);
                allGo[i].playerSpawned = true;
            }
        }
        RoomManager.Instance.screenLoaded = true;
         RoomManager.Instance.loadingPanel.GetComponent<Animation>().Play("fadeOut");
		 RoomManager.Instance.loadingPanel.SetActive(false);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
    {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}