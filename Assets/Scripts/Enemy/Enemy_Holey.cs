using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Holey : EnemyController
{
    [SerializeField]
    private Sprite standSprite, downSprite;
    public float attackRate = 1.5f;
    [SerializeField]
    private GameObject bullet;
    GameObject EB1, EB2;
    Coroutine shooting;
    bool killSequence = false;

    [SerializeField] float sightRange =         10;
    [SerializeField] float sightRangeOffset =   5;
    Player player { get { return GameController.S.player; } }
    
    // Use this for initialization
    protected override void Awake() 
    {
        base.Awake();
        attacking =             false;
    }

	
	// Update is called once per frame
	protected override void Update () 
    {
        base.Update();

        if (isPaused)
            return;

        if (!dead)
        {
            HandleAttacking();
        }

        // If the holey is dead, start the kill sequence if it hasn't started already
        else if (!killSequence)
        {
            StopAllCoroutines();
            StartCoroutine(Kill());
            killSequence =      true;
        }

    }


    void ScatterShot() 
    {
        Debug.Log("Holey Shoot");
        int direction =                 1;
        if (!spriteRenderer.flipX)
            direction =                 1;
        
        else 
            direction =                 -1;
        
        EB1 =  Instantiate(bullet, this.gameObject.transform);
        EB1.GetComponent<EnemyBullet>().velX = -15.0f * direction;
        EB1.GetComponent<EnemyBullet>().velY = 0.0f;
        EB2 = Instantiate(bullet, this.gameObject.transform);
        EB2.GetComponent<EnemyBullet>().velX = -10.6f * direction;
        EB2.GetComponent<EnemyBullet>().velY = 10.6f;

    }

    public IEnumerator Kill() {
        
        color =                 Color.black;
        yield return new WaitForSeconds(0.5f);
        
        //drop item
        Destroy(this.gameObject);

    }

    IEnumerator shoot() {
        while (attacking) {
            yield return new WaitForSeconds(attackRate);
            if (attacking)
            {
                ScatterShot();
            }
        }
        
        StopCoroutine(shooting);
        shooting = null;
        yield return null;
    }

    void HandleAttacking()
    {
        // When the player is within certain distances from this, stand up and shoot at the player
        float playerX =             player.transform.position.x;
        float thisX =               transform.position.x;
        float distFromPlayer =      Mathf.Abs(playerX - thisX);
        bool withinSightRange =     distFromPlayer <= sightRange;
        bool attackPlayer =         withinSightRange && (distFromPlayer >= sightRangeOffset);

        if (attackPlayer)
        {
            sprite =                    standSprite;

            // Face towards the player. Note that the sprite faces left by default
            spriteRenderer.flipX =      playerX > thisX;
            if (!attacking)
            {
                attacking =             true;
                shooting =              StartCoroutine(shoot());
            }
            gameObject.GetComponent<Collider2D>().enabled = true;

        }
 
        // While not looking to attack the player, hide in the hole and be ignored by most everything. 
        // And be invincible.
        else 
        {
            
            attacking =                 false;
            spriteRenderer.sprite =     downSprite;
            gameObject.GetComponent<Collider2D>().enabled =     false;
        }
    }
}
