using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HazardScript : MonoBehaviour
{
    // Start is called before the first frame update

    //explosion animation
    SpriteRenderer sr;
    public Sprite explosionGif;

    //audioSources
    AudioSource playAudio;
    public GameManager gm;
    public AudioClip dropAudio;
    public AudioClip explosionAudio;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "ground")
        {
            playAudio.PlayOneShot(dropAudio);
            playAudio.PlayOneShot(explosionAudio);
            sr.sprite = explosionGif;
            gm.GameOver();
        }
    }
}
