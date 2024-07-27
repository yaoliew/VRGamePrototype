using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    public GameObject VRPlayer;
    public GameObject flatscreenPlayer;

    public bool enableFlatscreenMode;
    void Start()
    {
        if (enableFlatscreenMode) {
            VRPlayer.SetActive(false);
            flatscreenPlayer.SetActive(true);
        } else {
            VRPlayer.SetActive(true);
            flatscreenPlayer.SetActive(false);
        }
    }

}
