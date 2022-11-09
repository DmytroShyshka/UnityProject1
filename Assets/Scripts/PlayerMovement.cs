using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float moveX;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpforce = 12f;

    [SerializeField] private LayerMask canJump;
    

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D colider;

    private enum AnimationState { idle, running, jumping, falling }
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        colider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()    
    {
        moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
           rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }

       AnimationUpdate();
    }

    private void AnimationUpdate()
    {
        AnimationState state;
           
        if (moveX > 0f)
        {
            state = AnimationState.running;
            sprite.flipX = false;
        }
        else if (moveX < 0f)
        {
            state = AnimationState.running;
            sprite.flipX = true;
        }
        else
        {
            state = AnimationState.idle;
        }


        if (rb.velocity.y > .1f)
        {
            state = AnimationState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = AnimationState.falling;
        }

        animator.SetInteger("state", (int)state);
    }
    
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(colider.bounds.center, colider.bounds.size, 0f, Vector2.down, .1f, canJump);
        
    }
}
