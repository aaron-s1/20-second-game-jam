using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;


public class VideoPlayerManager : MonoBehaviour
{
    [HideInInspector] public static VideoPlayerManager Instance { get; private set; }

    [SerializeField] GameObject videoRendererObj;

    Renderer renderer;

    Coroutine fadeInHomieLander;    

    VideoPlayer videoPlayer;
    
    Animation videoPlayerAnim;

    Color videoPlayerColor;
    
    public int currentClipNumber;

    float videoRendererAlpha;


    // for easier testing.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
            GameManager.Instance.IncrementScore(8);
    }


    void Awake() {
        Instance = this;
        videoPlayer = videoRendererObj.GetComponent<VideoPlayer>();
    }

    void Start() {
        videoPlayerColor = videoRendererObj.GetComponent<MeshRenderer>().material.color;
        renderer = videoRendererObj.GetComponent<MeshRenderer>();
        videoPlayerAnim = videoRendererObj.GetComponent<Animation>();
    }


    // Call from elsewhere.
    public void AssignAndPlayClip()
    {    
        if (currentClipNumber == 0) {            
            currentClipNumber++;
            AssignNewClipURL(currentClipNumber);
        }
        else if (currentClipNumber == 1) {
            currentClipNumber++;
            AssignNewClipURL(currentClipNumber);
        }
        else if (currentClipNumber == 2)
            return;

        if (fadeInHomieLander == null)
            fadeInHomieLander = StartCoroutine(FadeInHomieLander());
        else Debug.Log("moo :(");
    }


    IEnumerator FadeInHomieLander()
    {
        renderer.enabled = true;
        videoPlayer.Play();

        // increases alpha towards 1
        videoPlayerAnim.Play();


        while (true) 
        {
            if (!videoPlayerAnim.isPlaying)
            {
                videoPlayerAnim.Stop();
                renderer.enabled = false;

                StopCoroutine(fadeInHomieLander);
                fadeInHomieLander = null;

                yield return null;
            }
            
            yield return null;
        }
    }

    void AssignNewClipURL(int clipNumber) =>
        videoPlayer.url = Application.streamingAssetsPath + "/" + "homelander drinking " + clipNumber + ".mp4";        
}
