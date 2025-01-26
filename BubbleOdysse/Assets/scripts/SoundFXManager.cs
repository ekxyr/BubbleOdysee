//Assets/Scripts/Managers/SoundFXManager

//New Game Object SoundFXManager

//Code starts here

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

    //assign the audioClip
    audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //length of the SoundFX
        float clipLength = audioSource.clip.length;

        //destroy the SoundFX afterwards
        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        //assign a random index
        int rand = Random.Range(0, audioClip.Length);

        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

    //assign the audioClip
    audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //length of the SoundFX
        float clipLength = audioSource.clip.length;

        //destroy the SoundFX afterwards
        Destroy(audioSource.gameObject, clipLength);

    }
}

//SoundFXObject - Add Audio Source, uncheck Play On Awake ,Then pull it into the Prefabs Folder /Assets/Prefabs
//Assign the SoundFXObject from the Prefabs to the SoundFXManager Object
