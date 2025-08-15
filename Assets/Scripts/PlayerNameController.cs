using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameController : MonoBehaviour
{
    public string playerName;
    public static PlayerNameController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
