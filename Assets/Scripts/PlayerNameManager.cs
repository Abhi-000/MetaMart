using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
	[SerializeField] TMP_InputField usernameInput;
	[SerializeField] GameObject savedPopUP;

	void Start()
	{
		if(PlayerPrefs.HasKey("username"))
		{
			usernameInput.text = PlayerPrefs.GetString("username");
			PhotonNetwork.NickName = PlayerPrefs.GetString("username");
		}
		/*else
		{
			usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
			OnUsernameInputValueChanged();
		}*/
	}

	public void save()
	{
		PhotonNetwork.NickName = usernameInput.text;
		PlayerPrefs.SetString("username", usernameInput.text);
		savedPopUP.GetComponent<Animation>().Play("saved");
	}
}
