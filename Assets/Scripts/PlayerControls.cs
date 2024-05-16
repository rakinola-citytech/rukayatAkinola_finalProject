using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float xMove;
    float yMove;

    public float speed = 10;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //player movement
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");

        //mouse click raycast ... should return hole gameObject

        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("CLICKED " + hit.collider.name);
            }


        }*/
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);
    }

}