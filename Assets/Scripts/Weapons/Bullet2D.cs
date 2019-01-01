using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet2D : CSS_MonoBehaviour2D, IPausable
{
	// IPausable
	public UnityEvent Paused 					{ get; protected set; }
	public UnityEvent Unpaused 					{ get; protected set; }
	public bool isPaused 						{ get; protected set; }
    public bool wallPassable = false;
    public bool enemyPassable = false;
    public float activeTime = 1.0f;
    public float velX = 5.0f, vely = 0.0f;
    Rigidbody2D rb;
    void Start() 
	{
        Invoke("EraseBullet", activeTime);
        gameObject.GetComponent<SpriteRenderer>().flipX = GameController.S.player.GetComponent<SpriteRenderer>().flipX;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

	// Other fields and properties
	[Tooltip("How fast this bullet moves in meters per second.")]
	[SerializeField] float _speed = 			5;
	[SerializeField] float _damage = 			1;
	[Tooltip("This bullet can damage objects with the tags defined here.")]
	public List<string> damageTags;

	[Tooltip("This bullet can damage objects in the layers defined here.")]
	public List<string> damageLayers;
	Vector3 startingVelocity = 					Vector3.zero;
	Vector3 previousVelocity; // For resuming motion from unpausing


    public Vector3 velocity
	{
		get { return rigidbody.velocity; }
		set 
		{ 
			previousVelocity = 					rigidbody.velocity;
			rigidbody.velocity = 				value; 
		}

	}

	public Vector3 direction
	{
		get { return velocity.normalized; }
		set { velocity = value.normalized * (speed * Time.deltaTime); }
	}

	public float speed 
	{
		get { return _speed; }
		set { _speed = value; }
	}

	public float damage 
	{
		get { return _damage; }
		set { _damage = value; }
	}

	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		Paused = 				new UnityEvent();
		Unpaused = 				new UnityEvent();
		isPaused = 				false;

		//velocity =                              startingVelocity *speed * Time.deltaTime;

        // If the damge tags and/or layers aren't set in the inspector, initialize them
        // here
        if (damageTags == null) 
			damageTags = 						new List<string>();
		if (damageLayers == null)
			damageLayers = 						new List<string>();
		
	}


	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);

		// Check if the other thing is damageable, and see whether this bullet should 
		// damage it
		GameObject otherGO = 				other.gameObject;
		IDamageable<float> toDamage = 		otherGO.GetComponent<IDamageable<float>>();
		
		bool shouldDamage = 				toDamage != null && 
											(damageTags.Contains(other.tag) || 
											damageLayers.Contains(LayerMask.LayerToName(otherGO.layer)));
		if (shouldDamage)
		{
			// Only the player can get invincibility frames from getting hit
			bool damagingPlayer = 			other.CompareTag("Player");
			toDamage.TakeDamage(damage, damagingPlayer);
			//Destroy(this.gameObject);
		}

        if (!enemyPassable && other.gameObject.tag == "Enemy") {
            EraseBullet();
        }
        if (!wallPassable && other.gameObject.tag == "Wall")
        {
            EraseBullet();
        }

    }
    public void EraseBullet() {
        Destroy(this.gameObject);
    }
	protected override void OnCollisionEnter2D(Collision2D other)
	{
        

        // Same thing happens whether this bullet touches a trigger or not, so...
        OnTriggerEnter2D(other.collider);
		if (this.gameObject != null)
			Destroy(this.gameObject);

   

    }

	public void Pause()
	{
		// Keep this from moving or registering collisions
		velocity = 				Vector3.zero;
		rigidbody.Sleep();

		isPaused = 				true;
		Paused.Invoke();
	}

    void FixedUpdate() {
        rb.velocity = new Vector2(velX, vely);
    }

	public void Unpause()
	{
		// Resume previous motion and collision-registering
		velocity = 				previousVelocity;
		previousVelocity = 		velocity;
		// ^ Keep previousVelocity from being 0
		rigidbody.WakeUp();

		isPaused = 				false;
		Unpaused.Invoke();
	}

	
}
