using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform holeFolder;
    public Transform camTransform;

    //player turn & player choice
    bool playerTurn = true;
    Vector2 playerPosition;

    //computer's choice
    GameObject computer;
    Vector3 compHit;
    Vector2 compPosition;
    bool compChoiceMade;

    //holePrefab starting position
    public Transform startGrid;

    //public player life
    int life = 0;
    public GameObject life_one;
    public GameObject life_two;

    //prefabs 
    public GameObject holePrefab;
    public GameObject glovePrefab;

    float holeSize_x = 8;
    float holeSize_y = 16;

    int columns = 3;
    int rows = 5;

    int xPosition = 0;
    int yPosition;

    //holegrids
    public GameObject[,] holeGrid;
    //int arrayLength = 0;

    //change hole sprites ...
    public Sprite moleInHoleSprite;
    public Sprite moleHitSprite;

    //spriteRender controls
    SpriteRenderer sr;

    //TextMeshPro
    public TextMeshPro roundChange;
    

    //public float camMovementDuration = 2;
    public float cameraSpeed = 1;
    float timeElapsed;
    public float cameraMoveDelay = 2;

    //audioSource
    AudioSource hitAudio;
    public AudioClip hitClip;
    public AudioClip safeClip;

    //game over, reload scene
    string currentSceneName;
    public TextMeshPro win;
    public TextMeshPro lose;
    float wait = 60;
    float move = 0;

    void Start()
    {
        holeGrid = new GameObject[rows, columns];

        holeSize_x *= (holePrefab.GetComponent<CircleCollider2D>().radius);
        holeSize_y *= (holePrefab.GetComponent<CircleCollider2D>().radius);

        hitAudio = GetComponent<AudioSource>();

        currentSceneName = SceneManager.GetActiveScene().name;

        


        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {

                Vector2 pos = new Vector2(startGrid.position.x + (holeSize_x * c), startGrid.position.y + holeSize_y * r) ;
                GameObject g = Instantiate(holePrefab, pos, Quaternion.identity, holeFolder);
                holeGrid[r, c] = g;
                g.name = "Hole [ " + r + " , " + c + "]";    
                //Debug.Log(holeGrid[r, c].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (playerTurn)
        {
            compChoiceMade = false;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
               

                if (hit.collider != null)
                {
                    if (xPosition < 4)
                    {
                        xPosition += 1;
                    }
                    else
                    {
                        GameOver();
           
                    }
                    playerPosition = hit.transform.position;

                    //get spriterender for collision.gameObject &
                    //change sprite of collision.gameObject
                    sr = hit.collider.GetComponent<SpriteRenderer>();
                    sr.sprite = moleInHoleSprite;

                    //Debug.Log("CLICKED " + hit.collider.name);
                   
                    playerTurn = false;
                   
                }

            }

        }
        else {

            if (!compChoiceMade)
            {
                MakeCompChoice();
                compChoiceMade = true;
            }

            if (timeElapsed < cameraMoveDelay)
            {
                timeElapsed += Time.deltaTime;
            }
            else
            {
                MoveCamera(xPosition);

            }

      
        }
        roundChange.text = "ROUND:" + (xPosition+1).ToString();
        if (life == 1)
        {
            Destroy(life_one);

        }
        else if (life == 2)
        {
            Destroy(life_two);
        }


    }

    private void MoveCamera(int end)
    {
       // if (end == 0) end = 1;
        //Vector3 startPos = new Vector3(holeGrid[start, 1].transform.position.x, holeGrid[start,1].transform.position.y, camTransform.position.z);
        Vector3 endPos= new Vector3(holeGrid[end, 1].transform.position.x, holeGrid[end, 1].transform.position.y, camTransform.position.z);

        float buffer = 0.07f;

        if (camTransform.position.y <= endPos.y - buffer)
        {
            //timeElapsed += Time.deltaTime;
            //camTransform.position = Vector3.Lerp(startPos, endPos, timeElapsed/ camMovementDuration );
            camTransform.position = Vector3.Lerp(camTransform.position, endPos, Time.deltaTime * cameraSpeed);

        }
        else
        {
            camTransform.position = endPos;
            timeElapsed = 0;
            playerTurn = true;
 

        }
       

    }

    void MakeCompChoice()
    {
        compPosition = compChoice().GetComponent<Transform>().position;
        computer = Instantiate(glovePrefab, compPosition + new Vector2(0, 3), Quaternion.identity);


        if (Vector2.Distance(playerPosition, compPosition) < 0.05f)
        {
            // hit and consequences
            Debug.Log("Hit");
            life += 1;
            hitAudio.PlayOneShot(hitClip);


            //failed lerping attempt for glovePrefab
            //compHit = computer.GetComponent<Transform>().position;
            //computer.transform.position = Vector3.Lerp((compHit + new Vector2 (0,1)), (playerPosition + new Vector2(0,1)), timeElapsed / camMovementDuration);
            //computer.transform.position = new Vector3(compHit.x, compHit.y + 1, compHit.z);
            //computer.transform.position = new Vector3(compHit.x, compHit.y - 0.8f, compHit.z);

            sr.sprite = moleHitSprite;
        }
        else
        {
            hitAudio.PlayOneShot(safeClip);
        }

    }

    GameObject compChoice()
    {
        yPosition = Random.Range(0, 3);
        Debug.Log("yPosition = " + yPosition);
        Debug.Log("xPosition = " + xPosition);
        return (holeGrid[xPosition-1, yPosition]);
    }


    public void GameOver()
    {
        Debug.Log("Game Over");
        if (life < 2)
        {
            win.gameObject.SetActive(true);
            //while (move < wait)
            //{
            //    move += Time.deltaTime;
            //}
        }
        else
        {
            lose.gameObject.SetActive(true);
            //while(move < wait)
            //{
            //    move += Time.deltaTime;
            //}
   
        }

        Invoke("RestartScene", 3);
       
    }

    void RestartScene()
    {
        SceneManager.LoadScene(currentSceneName);

    }

   
}