using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrepareAndPlay());
    }

    private IEnumerator PrepareAndPlay()
    {
        // Prepare the video (load it into memory)
        videoPlayer.Prepare();

        // Wait until preparation is complete
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Set the videoPlayer to play the video clip
        videoPlayer.Play();

        // Wait until the video finishes playing
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // Load the main menu scene
        LevelLoadManager.instance.LoadMainMenuFromIntro();
    }
}