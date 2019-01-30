using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBonnie : EnemyController 
{
    public float actionSpeed = 1;
    public Coroutine pattern;
    [SerializeField]
    private GameObject shockBullet;
    [SerializeField]
    private GameObject hitBox;
    [SerializeField]
    private GameObject whip;
    public Animator animator; 
    
    private SpriteRenderer whipRenderer;
    Player player;
    public Vector2 targetPos;
    bool killSequence = false;
    SpriteRenderer bossRenderer;
    protected override void Awake() {
        base.Awake();
        hp.value = 500;
    }
    void Start()
    {
        bossRenderer =          gameObject.GetComponent<SpriteRenderer>();
        whipRenderer =          whip.gameObject.GetComponent<SpriteRenderer>();
        whipRenderer.size =     new Vector2 (0.0f, 0.0f);
        animator =              GetComponent<Animator>();
        player =                GameController.S.player;
        PatternChange(0);

    }

    override protected void Update() {
        base.Update();
        if (!killSequence && hp.value <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Kill());
            killSequence = true;
        }
    }

    public void PatternChange(int goToPattern) 
    {
        //change the coroutine with this
        switch (goToPattern) 
        {
            case 0:
                pattern = StartCoroutine(Wait(3));
                break;
            case 1:
                pattern = StartCoroutine(FlyingAttack());
                break;
            case 2:
                pattern = StartCoroutine(WhipAttack());
                break;
            case 3:
                pattern = StartCoroutine(SpinAttack());
                break;
            default:
                pattern = StartCoroutine(FlyingAttack());
                break;
        }
    }
    public IEnumerator Wait(float waitTime ) 
    {
        animator.SetTrigger("Enter");
        yield return new WaitForSeconds(waitTime);
        StopCoroutine(pattern);
        PatternChange(Random.Range(1,3));

    }
    public IEnumerator FlyingAttack() {
        int choseDirection = Random.Range(-1,2);

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        switch (choseDirection) {
            case 1:
                targetPos = new Vector2(6,4);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                whip.gameObject.transform.localScale = new Vector2(-1, 1);
                whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
                break;
            case -1:
                targetPos = new Vector2(-6, 4);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                whip.gameObject.transform.localScale = new Vector2(1, 1);
                whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
                break;
            default:
                targetPos = new Vector2(6 * choseDirection, 4);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                whip.gameObject.transform.localScale = new Vector2(-1, 1);
                whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
                if (choseDirection < 0) {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    whip.gameObject.transform.localScale = new Vector2(1, 1);
                    whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
                }
                break;
        }

        Vector3 startingPos = transform.position;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, targetPos, t);
            yield return 0;
        }

        switch (choseDirection)
        {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                whip.gameObject.transform.localScale = new Vector2(1, 1);
                whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
                break;
            case -1:
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                whip.gameObject.transform.localScale = new Vector2(-1, 1);
                whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
                break;
            default:
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                whip.gameObject.transform.localScale = new Vector2(1, 1);
                whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
                if (choseDirection < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    whip.gameObject.transform.localScale = new Vector2(-1, 1);
                    whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
                }
                else if (choseDirection == 0) {
                    if (player.transform.position.x > gameObject.transform.position.x)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        whip.gameObject.transform.localScale = new Vector2(-1, 1);
                        whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
                    }
                    else {
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        whip.gameObject.transform.localScale = new Vector2(1, 1);
                        whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
                    }
                }
                break;
        }
        animator.SetBool("Attack",true);
        GameObject EB1, EB2, EB3;

        int direction = 1;
        if (!bossRenderer.flipX)
            direction = -1;

        else
            direction = 1;

        EB1 = Instantiate(shockBullet, this.gameObject.transform);
        EB1.GetComponent<EnemyBullet>().velX = -8.9f * direction;
        EB1.GetComponent<EnemyBullet>().velY = -18;

        yield return new WaitForSeconds(0.6f);

        EB2 = Instantiate(shockBullet, this.gameObject.transform);
        EB2.GetComponent<EnemyBullet>().velX = -20f * direction;
        EB2.GetComponent<EnemyBullet>().velY = -20f;

        yield return new WaitForSeconds(0.6f);

        EB3 = Instantiate(shockBullet, this.gameObject.transform);
        EB3.GetComponent<EnemyBullet>().velX = -18f * direction;
        EB3.GetComponent<EnemyBullet>().velY = -8.9f;

        yield return new WaitForSeconds(0.6f);
        

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds (2);
        StopCoroutine(pattern);
        PatternChange(2);
    }

    public IEnumerator WhipAttack() {
        if (player.transform.position.x > 0)
        {
            targetPos = new Vector2(player.transform.position.x - 2.5f, -4);
        }
        else {
            targetPos = new Vector2(player.transform.position.x + 2.5f, -4);
        }

        if (targetPos.x < transform.position.x) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            whip.gameObject.transform.localScale = new Vector2(1, 1);
            whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
        } else {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            whip.gameObject.transform.localScale = new Vector2(-1, 1);
            whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
        }

        Vector3 startingPos = transform.position;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * 0.5f;
            transform.position = Vector3.Lerp(startingPos, targetPos, t);
            yield return 0;
        }
        
        if (player.transform.position.x > transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            whip.gameObject.transform.localScale = new Vector2(-1, 1);
            whip.gameObject.transform.localPosition = new Vector2(0.5f, 0);
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            whip.gameObject.transform.localScale = new Vector2(1, 1);
            whip.gameObject.transform.localPosition = new Vector2(-0.5f, 0);
        }

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Attack", true);

        while (whipRenderer.size.x < 3) {
            whipRenderer.size = new Vector2 (whipRenderer.size.x + 0.2f, 0.18f);
            yield return new WaitForSeconds(0.01f);
        }

        while (whipRenderer.size.x > 0)
        {
            whipRenderer.size = new Vector2(whipRenderer.size.x - 0.2f, 0.18f);
            yield return new WaitForSeconds(0.01f);
        }

        whipRenderer.size = Vector2.zero;

        animator.SetBool("Attack", false);

        yield return new WaitForSeconds (0.5f);
        
        StopCoroutine(pattern);
        PatternChange(Random.Range(2,4));
    }

    public IEnumerator SpinAttack()
    {
        DamageOnContact2D spinner = hitBox.GetComponent<DamageOnContact2D>();

        spinner.damageDealt = 25;

        float t = 0;
        /*while (transform.position.x != transform.position.x - 3 * whip.transform.localScale.x &&
            transform.position.x != -7 * whip.transform.localScale.x)
        {
            transform.position = new Vector2(transform.position.x - 0.1f, -4);
            yield return new WaitForSeconds(0.01f);
        }*/
        animator.SetBool("Spinning", true);

        if (whip.transform.localScale.x == -1) {
            while (t < 1) {
                transform.position = new Vector2(transform.position.x + 0.1f, -4);
                yield return new WaitForSeconds(0.01f);
                t += 0.01f;
                if (transform.position.x >= 7)
                {
                    t = 1;
                }
            }
        }

        if (whip.transform.localScale.x == 1)
        {
            while (t < 1)
            {
                transform.position = new Vector2(transform.position.x - 0.1f, -4);
                yield return new WaitForSeconds(0.01f);
                t += 0.01f;
                if (transform.position.x <= -7) {
                    t = 1;
                }
            }
        }

        spinner.damageDealt = 10;

        animator.SetBool("Spinning", false);

        yield return new WaitForSeconds(0.2f);
        StopCoroutine(pattern);
        PatternChange(1);
    }

    public IEnumerator Kill()
    {
        GameController.S.GoToScene("TitleScreen", 3.5f);
        color = Color.black;
        yield return new WaitForSeconds(0.5f);

        //drop item
        Destroy(this.gameObject);
    }
}
