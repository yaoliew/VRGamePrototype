using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignableObject : MonoBehaviour, ISignable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sign(string signName) {
        Debug.Log(signName);
    }
}
