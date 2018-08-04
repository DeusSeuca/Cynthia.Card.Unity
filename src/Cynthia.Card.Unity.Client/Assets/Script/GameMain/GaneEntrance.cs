using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GaneEntrance : MonoBehaviour
{
    public GameObject GlobalUI;
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GlobalUI);
    }

    void Update()
    {

    }
}
