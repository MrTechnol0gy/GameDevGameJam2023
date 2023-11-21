using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Tracks")]
    [SerializeField] AudioClip[] muggerSpawn;
    [SerializeField] AudioClip[] muggerRegretClips;
    [SerializeField] AudioClip[] cultistSpawn;
    [SerializeField] AudioClip[] cultistDefeatClips;
    [SerializeField] AudioClip[] balloonClownSpawn;
    [SerializeField] AudioClip[] clownDefeatClips;
    [SerializeField] AudioClip grandmaMugged;
    [SerializeField] AudioClip[] grandmaShops;
    [SerializeField] AudioClip[] civilians;
    [SerializeField] AudioClip villainSpottedClips;
    [SerializeField] AudioClip[] wrestlerSpawn;
    [SerializeField] AudioClip sniperShotClip;
    [SerializeField] AudioClip balloonPop;
    private AudioSource mainAudioSource;
    [Header("Audio Sliders")]
    [SerializeField] Slider masterVolumeSlider;

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
        // Select a random index within the array length
        int randomIndex = Random.Range(0, muggerSpawn.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = muggerSpawn[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);       
    }

    public void CultistSpawn()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, cultistSpawn.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = cultistSpawn[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void BalloonClownSpawned()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, balloonClownSpawn.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = balloonClownSpawn[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void MuggerCaught()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, muggerRegretClips.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = muggerRegretClips[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void CultistCaught()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, cultistDefeatClips.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = cultistDefeatClips[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }
    
    public void ClownCaught()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, clownDefeatClips.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = clownDefeatClips[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void GrandmaMugged()
    {
        mainAudioSource.pitch = 1f;
        mainAudioSource.PlayOneShot(grandmaMugged);
    }

    public void GrandmaShops()
    { 
        // Select a random index within the array length
        int randomIndex = Random.Range(0, grandmaShops.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = grandmaShops[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = 1f;
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void VillainSpotted()
    {
        mainAudioSource.pitch = 1f;
        mainAudioSource.PlayOneShot(villainSpottedClips);
    }

    public void CivilianClips()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, grandmaShops.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = civilians[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void WrestlerSpawn()
    {
        // Select a random index within the array length
        int randomIndex = Random.Range(0, wrestlerSpawn.Length);

        // Get the randomly selected AudioClip
        AudioClip randomClip = wrestlerSpawn[randomIndex];

        // Play the selected clip
        mainAudioSource.clip = randomClip;
        mainAudioSource.pitch = Random.Range(0.8f, 1.2f);
        mainAudioSource.PlayOneShot(randomClip);
    }

    public void SniperShot()
    {
        mainAudioSource.pitch = 1f;
        mainAudioSource.PlayOneShot(sniperShotClip);
    }

    public void BalloonPop()
    {
        mainAudioSource.pitch = 1f;
        mainAudioSource.PlayOneShot(balloonPop);
    }

    public void ChangeVolumes()
    {
        if (masterVolumeSlider == null)
        {
            Debug.Log("master volume slider is null");
            return;
        }
        Debug.Log("master volume is " + mainAudioSource.volume);
        mainAudioSource.volume = masterVolumeSlider.value;
        Debug.Log("master volume is " + mainAudioSource.volume);
    }
}
