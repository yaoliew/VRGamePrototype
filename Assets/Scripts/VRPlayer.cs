using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<ISignable> objects = new List<ISignable>();
    
    public Transform playerTransf;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<ISignable>() != null) {
            objects.Add(other.transform.GetComponent<ISignable>());
        }
    }

    public void Signed(string signName) {
        Debug.Log(signName);
        switch (signName) {
            case "gun":
                foreach (ISignable obj in objects) {
                    obj.Sign("gun");
                }
                break;
            case "open":
                foreach (ISignable obj in objects) {
                    obj.Sign("open");
                }
                break;
            case "door":
                foreach (ISignable obj in objects) {
                    obj.Sign("door");
                }
                break;
            case "go":
                playerTransf.position = playerTransf.position + new Vector3(transform.forward.x / 20f, 0f, transform.forward.z / 20f);
                break;
            default:
                Debug.Log("Absolutely Unreal");
                break;
        }
    }
}