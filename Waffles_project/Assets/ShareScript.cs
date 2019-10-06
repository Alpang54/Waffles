﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareScript : MonoBehaviour
{
    


    public void Share()
    {
        Debug.Log("startcoroutine");
        StartCoroutine(TakeSSAndShare());
        Debug.Log("startcoroutine");
    }

    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).SetSubject("SharedImage").Share();
    }
}
