using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour 
{
    [SerializeField] float _velX =                     5.0f;
    [SerializeField] float _velY =                     0.0f;
    public int lasting =                    1;
    Rigidbody2D rb;
    Coroutine traveling;

    public Vector2 velocity 
    {
        get                                 { return rb.velocity; }
        set                                 { rb.velocity = value; }
    }

    public float velX 
    {
        get                             { return _velX; }
        set 
        {
            _velX =                     value;
            Vector2 newVel =            velocity;
            newVel.x =                  velX;
            velocity =                  newVel;
        }
    }

    public float velY 
    {
        get                             { return _velY; }
        set 
        {
            _velY =                     value;
            Vector2 newVel =            velocity;
            newVel.y =                  velY; 
            velocity =                  newVel;
        }
    }

	// Use this for initialization
	void Awake () 
    {
        rb =                                GetComponent<Rigidbody2D>();
        transform.localPosition =           Vector3.zero;
        traveling =                         StartCoroutine(Travel(lasting));
        velocity =                          new Vector2(velX, velY);
	}
	

    IEnumerator Travel(int waitFor) {
        yield return new WaitForSeconds(waitFor);
        StopCoroutine(traveling);
        Destroy(this.gameObject);
    }

}
