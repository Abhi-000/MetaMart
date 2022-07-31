using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class typeWriter : MonoBehaviour
{
    private TextMeshProUGUI text;
    public float waitTime = .05f;
    [SerializeField]public string mainString;



    private void Awake()
    {
        mainString = GetComponent<TextMeshProUGUI>().text;
    }
    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        //mainString = GetComponent<TextMeshProUGUI>().text;
        StartCoroutine(startProcess());
    }

    IEnumerator startProcess()
    {
        int len = mainString.Length;
        print(gameObject.name);
        while (!RoomManager.Instance.screenLoaded)
        {
            for (int i = 0; i <= len; i++)
            {
                string s = mainString.Substring(0, i);
                text.text = s;
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
