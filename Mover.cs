using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected private BoxCollider2D boxCollider;
    protected private Vector3 moveDelta;
    protected private RaycastHit2D hit;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        moveDelta = input;

        // swap sprite direction
        if (moveDelta.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }

        //Add push vector
        moveDelta += pushDirection;
        //reduce push force every frame
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // dont move into obstacle
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
            new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime),
            LayerMask.GetMask("Actor", "Blocking"));
        // if hit eq null - we can move
        if (hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * ySpeed * Time.deltaTime, 0);
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
            new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime),
            LayerMask.GetMask("Actor", "Blocking"));
        // if hit eq null - we can move
        if (hit.collider == null)
        {
            transform.Translate(moveDelta.x * xSpeed * Time.deltaTime, 0, 0);
        }
    }


}
