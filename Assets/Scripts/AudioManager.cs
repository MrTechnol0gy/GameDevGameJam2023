using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Tracks")]
    [SerializeField] AudioClip[] muggerSpawn;
    [SerializeField] AudioClip[] muggerRegretClips;
    [SerializeField] AudioClip grandmaMugged;
    [SerializeField] AudioClip[] grandmaShops;
    [SerializeField] AudioClip[] civilians;
    private AudioSource mainAudioSource;

    private void Awake()
    {
        // Check if there is an instance of the AudioManager
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;
        }
        else
        {
            // If there is, destroy this object
            Destroy(gameObject);
        }

        // Make sure this object persists between scenes
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
    }
    
    public void MuggerSpawn()
    {
        if (isGrandmaMuggedPlaying())
        {
            return;
        }
        else if (!isGrandmaMuggedPlaying())
        {
            // Select a random index within the array length
            int randomIndex = Random.Range(0, muggerSpawn.Length);

            // Get the randomly selected AudioClip
            AudioClip randomClip = muggerSpawn[randomIndex];

            // Play the selected clip
            mainAudioSource.clip = randomClip;
            mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
            mainAudioSource.Play();
        }
    }

    public void MuggerCaught()
    {
        if (isGrandmaMuggedPlaying())
        {
            return;
        }
        else if (!isGrandmaMuggedPlaying())
        {
            // Select a random index within the array length
            int randomIndex = Random.Range(0, muggerRegretClips.Length);

            // Get the randomly selected AudioClip
            AudioClip randomClip = muggerRegretClips[randomIndex];

            // Play the selected clip
            mainAudioSource.clip = randomClip;
            mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
            mainAudioSource.Play();
        }
    }

    public void GrandmaMugged()
    {
        mainAudioSource.clip = grandmaMugged;
        mainAudioSource.pitch = 1f;
        mainAudioSource.Play();
    }

    public void GrandmaShops()
    { 
        if (isGrandmaMuggedPlaying())
        {
            return;
        }
        else if (!isGrandmaMuggedPlaying())
        {
            // Select a random index within the array length
            int randomIndex = Random.Range(0, grandmaShops.Length);

            // Get the randomly selected AudioClip
            AudioClip randomClip = grandmaShops[randomIndex];

            // Play the selected clip
            mainAudioSource.clip = randomClip;
            mainAudioSource.pitch = 1f;
            mainAudioSource.Play();
        }
    }

    public void CivilianClips()
    {
        Debug.Log("CivilianClip");
        if (isGrandmaMuggedPlaying())
        {
            return;
        }
        else if (!isGrandmaMuggedPlaying())
        {
            // Select a random index within the array length
            int randomIndex = Random.Range(0, grandmaShops.Length);

            // Get the randomly selected AudioClip
            AudioClip randomClip = civilians[randomIndex];

            // Play the selected clip
            mainAudioSource.clip = randomClip;
            mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
            mainAudioSource.Play();
        }
    }

    private bool isGrandmaMuggedPlaying()
    {
        if (mainAudioSource.clip == grandmaMugged && mainAudioSource.isPlaying)
        {
            return true;
        }
        else
        return false;
    }
}
