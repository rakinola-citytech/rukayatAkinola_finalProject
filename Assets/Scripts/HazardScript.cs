using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HazardScript : MonoBehaviour
{
    // Start is called before the first frame update

    //explosion animation & previous sprite change
    public Animator bombAnim;
    public GameObject ground;

    //SpriteRenderer sr;
    //public Sprite explosionGif;

    //audioSources
    //AudioSource playAudio;
    public GameManager gm;
    //public AudioClip explosionAudio;

    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        //playAudio = GetComponent<AudioSource>();
        bombAnim.SetBool("explode", false);
        


    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //explode
        if (collision.gameObject == ground)
        {
            //playAudio.PlayOneShot(explosionAudio);
            bombAnim.SetBool("explode", true);
          
            //sr.sprite = explosionGif;
            gm.GameOver();
        }
    }
}
