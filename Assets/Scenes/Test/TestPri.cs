using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPri : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"0.0001f < -float.MaxValue = {0.00001f < -float.MaxValue}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
