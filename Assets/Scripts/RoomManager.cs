using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;
	public bool isTest = false;
	public GameObject loadingPanel;
	internal bool screenLoaded = false;
	void Awake()
	{
		/*int clientIndex = PanettoneGames.MultiPlay.Utils.GetCurrentClientIndex();
		if (clientIndex == 0) Debug.Log("MultiPlay is running on: Main/Server");
		else Debug.Log($"MultiPlay is running on Client: {clientIndex}");*/
#if UNITY_EDITOR
		isTest = true;
#endif
		if (Instance)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		Instance = this;

	}

	public override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if(scene.buildIndex == 1) // We're in the game scene
		{
			Debug.Log("Game scene loaded");
			//StartCoroutine(activateLoadingPanel());
			//screenLoaded = true;
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}

	public IEnumerator activateLoadingPanel(GameObject toLoad = null, GameObject toDeactivate=null)
    {
		screenLoaded = false;
		Debug.Log("Loading");
		loadingPanel.SetActive(true);
		loadingPanel.GetComponent<Animation>().Play("fadeIn");
		if (toLoad != null)
		toLoad.SetActive(true);
		
		while (!screenLoaded) {
			Debug.Log(screenLoaded);
			yield return new WaitForSeconds(1f);
		}
		loadingPanel.GetComponent<Animation>().Play("fadeOut");
		if (toDeactivate != null)
			toDeactivate.SetActive(false);

		loadingPanel.SetActive(false);
		FindObjectOfType<takeScreenshot>().takeSnapshot();

	}
}