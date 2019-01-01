using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Contains fields and such we want all of our custom 2D Monobehaviours in this game to have.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CSS_MonoBehaviour2D : MonoBehaviour 
{
	public SpriteRenderer spriteRenderer 		{ get; protected set; }
	new public Collider2D collider 				{ get; protected set; }
	new public Rigidbody2D rigidbody 			{ get; protected set; }
	public ContactEvents2D contactEvents 		{ get; protected set; }
	public float height 						{ get { return collider.bounds.size.y; } }
	public float width 							{ get { return collider.bounds.size.x; } }
	public Sprite sprite
	{
		get { return spriteRenderer.sprite; }
		set { spriteRenderer.sprite = value; }
	}
	public Color color 
	{
		get { return spriteRenderer.color; }
		set { spriteRenderer.color = value; }
	}
	[SerializeField] List<Collider2D> _collidersTouching;
	public List<Collider2D> collidersTouching 	
	{ 
		get { return _collidersTouching; }
		protected set { _collidersTouching = value; } 
	}
	public Vector3 pos 							
	{ 
		get { return transform.position; } 
		set { transform.position = value; }
	}
	
	// Use this for initialization
	protected virtual void Awake () 
	{
		spriteRenderer = 					GetComponent<SpriteRenderer>();
		collider = 							GetComponent<Collider2D>();
		rigidbody = 						GetComponent<Rigidbody2D>();
		contactEvents = 					new ContactEvents2D();
		collidersTouching = 				new List<Collider2D>();
	}

	protected virtual void Update()
	{
		collidersTouching.RemoveAll((Collider2D coll) => coll == null);
	}

	// Contact event handlers
	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (!collidersTouching.Contains(other.collider))
			collidersTouching.Add(other.collider);

		contactEvents.CollisionEnter2D.Invoke(other);
	}

	protected virtual void OnCollisionStay2D(Collision2D other)
	{
		contactEvents.CollisionStay2D.Invoke(other);

		if (!collidersTouching.Contains(other.collider))
			collidersTouching.Add(other.collider);
	}

	protected virtual void OnCollisionExit2D(Collision2D other)
	{
		collidersTouching.Remove(other.collider);
		contactEvents.CollisionExit2D.Invoke(other);
	}

	protected virtual void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (!collidersTouching.Contains(otherCollider))
			collidersTouching.Add(otherCollider);
		contactEvents.TriggerEnter2D.Invoke(otherCollider);
	}

	protected virtual void OnTriggerStay2D(Collider2D otherCollider)
	{
		contactEvents.TriggerStay2D.Invoke(otherCollider);

		// Keep items from being removed from collidersTouching just because one 
		// collider in a child object left it.
		if (!collidersTouching.Contains(otherCollider))
			collidersTouching.Add(otherCollider);
	}

	protected virtual void OnTriggerExit2D(Collider2D otherCollider)
	{
		collidersTouching.Remove(otherCollider);
		contactEvents.TriggerExit2D.Invoke(otherCollider);
	}


}
