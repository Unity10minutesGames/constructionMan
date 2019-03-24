using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    public float speed = 250.0f;
    public float jumpForce = 12.0f;
    
	void Start ()
    {
        body = GetComponent<Rigidbody2D> ();
        boxCollider = GetComponent<BoxCollider2D>();
	}
	
	void Update ()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;

        Vector3 max = boxCollider.bounds.max;
        Vector3 min = boxCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;

        if (hit != null)
        {
            grounded = true;
        }

        body.gravityScale = grounded && deltaX == 0 ? 0 : 1;

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
