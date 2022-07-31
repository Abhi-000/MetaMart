using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;
	private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Debug.Log("Connecting to Master");
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		
		Debug.Log("Joined Lobby");
	}
	public void showTitleMenu()
	{
		MenuManager.Instance.OpenMenu("title");
	}
	public void CreateRoom()
	{
	// 		RoomInfo temp;
	// 	foreach (KeyValuePair<string , RoomInfo> keyValue in cachedRoomList)
	// 	{
	// 		string key = keyValue.Key;
	// 		RoomInfo value = keyValue.Value;
	// 		Debug.Log("KEy:" + key);
	// 		Debug.Log("RoomINFo:" + value);
	// 		if(key == "ShoppingMall1")
	// 			JoinRoom(value);
	// 		else
	// 			PhotonNetwork.CreateRoom("ShoppingMall1", new RoomOptions { IsVisible = true });
	// 	}
	// 	Debug.Log(cachedRoomList.Count);
	// 	if(cachedRoomList.Count == 0)
	// 		PhotonNetwork.CreateRoom("ShoppingMall1", new RoomOptions { IsVisible = true });


	// }
		//RoomManager.Instance.screenLoaded = false;
		//StartCoroutine(RoomManager.Instance.activateLoadingPanel());
		Debug.Log(roomNameInputField.text);
		if (string.IsNullOrEmpty(roomNameInputField.text))
		{
			return;
		}
		PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { IsVisible = true });
		StartCoroutine(RoomManager.Instance.activateLoadingPanel());
		//MenuManager.Instance.OpenMenu("loading");
	}
	private void UpdateCachedRoomList(List<RoomInfo> roomList)
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			RoomInfo info = roomList[i];
			if (info.RemovedFromList)
			{
				cachedRoomList.Remove(info.Name);
			}
			else
			{
				//cachedRoomList.Add(info.Name, info);
				cachedRoomList[info.Name] = info;
			}
			Debug.Log(cachedRoomList[info.Name]);
		}
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("Created room");
		MenuManager.Instance.OpenMenu("room");
		RoomManager.Instance.screenLoaded  =true;
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach(Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for(int i = 0; i < players.Count(); i++)
		{
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
		//StopCoroutine(RoomManager.Instance.activateLoadingPanel());
		//Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		//StartGame();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		MenuManager.Instance.OpenMenu("error");
	}

	public void StartGame()
	{
		StartCoroutine(RoomManager.Instance.activateLoadingPanel());
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		StartCoroutine(RoomManager.Instance.activateLoadingPanel());
		//MenuManager.Instance.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		MenuManager.Instance.OpenMenu("title");
		
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		//Debug.Log("room list updated");
		//UpdateCachedRoomList(roomList);
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for(int i = 0; i < roomList.Count; i++)
		{
			if(roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}