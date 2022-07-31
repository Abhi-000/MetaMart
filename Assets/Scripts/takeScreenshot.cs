using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class takeScreenshot : MonoBehaviour
{
    public Material photo;
    public Texture2D tempSs;
    Camera snapCam;
    int resWidth = 256;
    int resHeight = 256;
    // Start is called before the first frame update
    void Start()
    {
        snapCam = GetComponent<Camera>();
        resWidth = snapCam.targetTexture.width;
        resHeight = snapCam.targetTexture.height;
    }
        
    public void takeSnapshot()
    {
            Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            //photo.mainTexture = snapshot;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            byte[] bytes = snapshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath +  "tempSs.png", bytes);
            //AssetDatabase.Refresh();
        byte[] fileData = File.ReadAllBytes(Application.dataPath + "tempSs.png");
        tempSs.LoadImage(fileData);
        photo.mainTexture = tempSs;
        Debug.Log("Snap taken");
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
