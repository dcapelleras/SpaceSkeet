using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource source;
    public List<AudioClip> clips;
    public bool musicOn = true;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        source = GetComponent<AudioSource>();
    }

    public void ShootSound(int weaponIndex)
    {
        if (!musicOn)
        {
            return;
        }
        source.PlayOneShot(clips[weaponIndex]);
        Debug.Log(clips[weaponIndex].name + "was shot");
    }
}