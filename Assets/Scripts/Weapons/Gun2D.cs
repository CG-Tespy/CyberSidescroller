using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;

public class Gun2D : CSS_MonoBehaviour2D, IPausable
{
	// IPausable
	public UnityEvent Paused 					{ get; protected set; }
	public UnityEvent Unpaused 					{ get; protected set; }
	public bool isPaused 						{ get; protected set; }

	// Other Fields and properties
	[SerializeField] Bullet2D bulletPrefab;
	[SerializeField] public float shotsPerSecond = 	1;
	float timer = 								0;
	bool canShoot = 							true;
	float shotDist = 							0.5f;
	
	public MonoBehaviour user 					{ get; protected set; }
	Player player;
	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		Paused = 				new UnityEvent();
		Unpaused = 				new UnityEvent();
		isPaused = 				false;
		player = 				GameObject.FindObjectOfType<Player>();
	}

	public void Init(MonoBehaviour user)
	{
		this.user = 		user;

		if (user is SidescrollerCharacter)
		{
			// Have the gun avoid colliding with any allies
			int userLayer = 		user.gameObject.layer;
			this.gameObject.layer = userLayer;
		}

		
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
		
		if (!isPaused && timer > 0)
		{
			timer -= 			Time.deltaTime;

			if (timer <= 0)
				canShoot = 		true;
		}

        if (player.GetComponent<SpriteRenderer>().flipX) {
            transform.localPosition = new Vector2(-0.8f,transform.localPosition.y);
        } else if (!player.GetComponent<SpriteRenderer>().flipX)
        {
            transform.localPosition = new Vector2(0.8f, transform.localPosition.y);
        }
    }

	public void Fire(Vector3 direction)
	{
		// Create bullet prefab, have the bullet move in the given direction
		if (canShoot)
		{
            //player.animator.SetTrigger("Shoot");
			//player.animator.SetBool("Shooting", true);

			Vector3 originPos  = 				Vector3.zero;
			//Make sure that when shooting up, the bullet starts above the player.
			if (direction == Vector3.up) 
				originPos = user.transform.position; 
			
			else 
				originPos = transform.position;

			Vector3 spawnPos = 					originPos + 
												(direction.normalized * shotDist);
			GameObject bulletGO = 				Instantiate<GameObject>(bulletPrefab.gameObject, 
																		spawnPos, 
																		Quaternion.identity);

            

			Bullet2D bullet = 					bulletGO.GetComponent<Bullet2D>();

			bullet.direction = 					direction;

            float tilt = 0;
            int yDirection = 0;

            // Used for the way the gun can shoot diagonally
            if (Input.GetAxis("Vertical") == 0) {
                tilt = 0;
            } 
			else if (Input.GetAxis("Vertical") > 0)
            {
                tilt = 0.3f;
                yDirection = 1;
            } 
			else if (Input.GetAxis("Vertical") < 0)
            {
                tilt = 0.3f;
                yDirection = -1;
            }


            if (direction == Vector3.right) 
			{
                bullet.velX = Mathf.Sqrt(Mathf.Pow(bullet.speed,2) * (1 - Mathf.Abs(tilt)))  * 1;
                bullet.vely = Mathf.Sqrt(Mathf.Pow(bullet.speed, 2) * Mathf.Abs(tilt)) * yDirection;
            } else if (direction == Vector3.left)
            {
                bullet.velX = Mathf.Sqrt(Mathf.Pow(bullet.speed, 2) * (1 - Mathf.Abs(tilt))) * -1;
                bullet.vely = Mathf.Sqrt(Mathf.Pow(bullet.speed, 2) * Mathf.Abs(tilt)) * yDirection;
            }
			else if (direction == Vector3.up)
			{
				bullet.vely = Mathf.Sqrt(Mathf.Pow(bullet.speed, 2) * Mathf.Abs(tilt)) * yDirection;
				bullet.velX = 0;
			}


            // Set the timer and disable shooting
            timer = 							1 / shotsPerSecond;
			canShoot = 							false;

			// Put the bullet on the right layer so it doesn't collide with whatever/whoever
			// shot it
			//bulletGO.layer = 					gameObject.layer;// Just set the physics dude
		}


	}

	public void Pause()
	{
		// Keep the gun from shooting
		isPaused = 				true;
		canShoot = 				false;
		Paused.Invoke();
	}

	public void Unpause()
	{
		// Let the gun shoot again
		isPaused = 				false;
		Unpaused.Invoke();
	}

}
