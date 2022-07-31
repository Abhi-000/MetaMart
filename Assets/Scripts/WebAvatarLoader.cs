using ReadyPlayerMe;
using UnityEngine;

public class WebAvatarLoader : MonoBehaviour
{
    private GameObject avatar;
    [SerializeField] internal string link;
    public static string AvatarURL = "";
    private AvatarLoader avatarLoader;
    [SerializeField] private GameObject roomFind, avatarParent;
    public static WebAvatarLoader instance;


    private void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        PartnerSO partner = Resources.Load<PartnerSO>("Partner");
        WebInterface.SetupRpmFrame(partner.Subdomain);
        avatarLoader = new AvatarLoader();
    }
    public void OnWebViewAvatarGenerated(string avatarUrl)
    {
        LoadAvatar(avatarUrl);
    }
    private void OnEnable()
    {
        if (RoomManager.Instance.isTest)
        {
            if (avatarLoader == null)
                avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(link, OnAvatarImported, OnAvatarLoaded);
        }
    }
    public void LoadAvatar(string avatarUrl)
    {
        AvatarURL = avatarUrl;
        if (RoomManager.Instance.isTest)
        {
            avatarLoader.LoadAvatar(link, OnAvatarImported, OnAvatarLoaded);
        }
        else
        {
            avatarLoader.LoadAvatar(avatarUrl, OnAvatarImported, OnAvatarLoaded);
        }
        //roomFind.SetActive(true);
        //if (avatar) Destroy(avatar);
    }
    private void OnAvatarImported(GameObject avatar) { }
    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        avatar.transform.parent = avatarParent.transform;
        Destroy(avatar.GetComponent<Transform>());
        avatar.AddComponent<RectTransform>();
        avatar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        avatar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);

        avatar.GetComponent<RectTransform>().localPosition =
     new Vector3(avatar.GetComponent<RectTransform>().localPosition.x,
                 avatar.GetComponent<RectTransform>().localPosition.y,
                 0);
        RoomManager.Instance.screenLoaded = true;
            
    }
}
