using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_TeppinAI : EnemyController 
{
    // Use this for initialization
    [SerializeField]
    private GameObject ground;
    public float moveSpeed = 1, stopTime = 1;
    SpriteRenderer groundRender;
    Coroutine slidin;
    float groundSize;
    [SerializeField]
    private bool goingRight = true;

    protected override void Awake()
    {
        base.Awake();
        isInvincible =          true;

    }

    IEnumerator Slide() 
    {
        gameObject.GetComponent<Animator>().SetBool("Stop", false);

        while (attacking) 
        {
            // Have this face the direction it's going in. Note the sprite points to the left by default.
            spriteRenderer.flipX =          goingRight;

            // Move based on physics rather than translation
            Vector2 newVel =            rigidbody.velocity;

            if (goingRight)
                newVel.x =              moveSpeed;
                
            else 
                newVel.x =              -moveSpeed;
                
            rigidbody.velocity =        newVel;

            // Have this change direction when reaching an edge of the ground it's moving on
            bool reachedRightEdge =     transform.position.x > 
                                        ground.transform.position.x + (groundSize / 2 - 0.5f);
            bool reachedLeftEdge =      transform.position.x < 
                                        ground.transform.position.x - (groundSize / 2 - 0.5f);

            if (reachedRightEdge)
                goingRight =            false;
            
            else if (reachedLeftEdge)
                goingRight =            true;
            
            yield return new WaitForFixedUpdate();
        }

        attacking =                     false;
        yield return null;
    }

    IEnumerator Stopped() 
    {
        gameObject.GetComponent<Animator>().SetBool("Stop", true);
        rigidbody.velocity =                Vector2.zero;
        yield return new WaitForSeconds(stopTime);
        StopCoroutine(slidin);
        slidin = null;
        slidin = StartCoroutine(Slide());
        gameObject.layer = 9;
    }
    
    
    protected override void OnCollisionEnter2D (Collision2D collision) 
    {
        base.OnCollisionEnter2D(collision);

        // When this touches ground, register details about it and start sliding around on it
        if (collision.gameObject.tag == "Walkable") 
        {
            ground =                collision.gameObject;
            //gameObject.transform.SetParent(ground.GetComponentInParent<Transform>());
            groundRender =          ground.GetComponentInParent<SpriteRenderer>();
            groundSize =            groundRender.size.x;
            attacking =             true;
            slidin =                StartCoroutine(Slide());
        }
    }

    protected override void OnCollisionExit2D(Collision2D other)
    {
        base.OnCollisionExit2D(other);

        // When this leaves ground, unregister it and stop doing anything
        if (other.gameObject.tag == "Walkable")
        {
            attacking =             false;
            ground =                null;
            groundRender =          null;
            groundSize =            0;
            StopCoroutine(slidin);
            slidin =                null;
            slidin =                StartCoroutine(Stopped());
            //gameObject.GetComponent<Animator>().SetBool("Stop", true);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) 
    {
        base.OnTriggerEnter2D(other);

        // When this is hit by a player's attack, just stop sliding for a moment
        
        if (other.gameObject.tag == "PAttack") 
        {
            StopCoroutine(slidin);
            slidin =            null;
            slidin =            StartCoroutine(Stopped());
            gameObject.layer = 10;

        }
        
    }

}
