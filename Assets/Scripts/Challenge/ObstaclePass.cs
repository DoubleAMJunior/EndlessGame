using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class ObstaclePass : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Random random = new Random();
            int index = random.Next(0, audioClips.Count);
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

   
}
