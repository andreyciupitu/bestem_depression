using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour {

    public void PlaySound()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

}
