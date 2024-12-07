using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Object rendering code based on Dave Carlile's "Create a GameObject Image Using Render Textures" post
// Link: http://crappycoding.com/2014/12/create-gameobject-image-using-render-textures/

/// <summary>
/// Takes snapshot images of prefabs and GameObject instances, and provides methods to save them as PNG files.
/// </summary>
public class SnapshotCamera : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        { // capture screen shot on left mouse button down

            string folderPath = "Assets/Screenshots/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created

            var screenshotName =
                                    "Screenshot_" +
                                    System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
                                    ".png"; // put youre favorite data format here
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName); // You get instant feedback in the console
        }
    }
}