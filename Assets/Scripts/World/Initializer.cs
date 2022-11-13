using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private List<ScriptableObjectSystemBase> soList;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < soList.Count; i++)
        {
            var current = soList[i];
            current.Init();
        }
    }
}
