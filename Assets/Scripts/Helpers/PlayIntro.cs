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
        StartCoroutine(PlayMovie());
    }

    private IEnumerator PlayMovie()
    { 
        videoPlayer.Prepare();
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
    }
}
