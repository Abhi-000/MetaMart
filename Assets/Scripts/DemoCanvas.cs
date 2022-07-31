using UnityEngine;

public class DemoCanvas : MonoBehaviour
{
    public GameObject LoginCanvas,homePanel;
    public void OnCreateAvatar()
    {
        Debug.Log("Button CLicked");
        if (!RoomManager.Instance.isTest)
            WebInterface.SetIFrameVisibility(true);

        else
        {
            StartCoroutine(RoomManager.Instance.activateLoadingPanel(LoginCanvas, homePanel));
        }

    }
}
