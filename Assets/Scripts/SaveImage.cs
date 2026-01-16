using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveImage : MonoBehaviour
{
    private IEnumerator TakeScreenshotandSave()
    {
        yield return new WaitForEndOfFrame();
        

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); 
        ss.ReadPixels( new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Save the screenshot to Gallery/Photos
        NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        //To avoid memory leaks\
        Destroy(ss);
    }

    public void TakeandSaveScreenshotButton()
    {
        StartCoroutine(TakeScreenshotandSave());
    }
}
