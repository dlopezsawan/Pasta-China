using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    public int levelCounter = 2;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(Instance);
        }
    }
}
