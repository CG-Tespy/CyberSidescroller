using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rigidbody2DSaveState : ISaveState<Rigidbody2D>
{
	public RigidbodyType2D bodyType;
	public float gravityScale, drag, angularDrag, angularVelocity, mass, inertia, rotation;

	public bool freezeRotation;

	public Vector2 velocity, centerOfMass, position;

	public RigidbodyConstraints2D constraints;
	public RigidbodyInterpolation2D interpolation;

	// Methods
	public Rigidbody2DSaveState(Rigidbody2D rb)
	{
		SetFrom(rb);
	}

	/// <summary>
	/// Applies this save state's properties to the passed object.
	/// </summary>
	public void ApplyTo(Rigidbody2D rb)
	{
		rb.bodyType = 					bodyType;
		rb.gravityScale = 				gravityScale;
		rb.drag = 						drag;
		rb.angularDrag = 				angularDrag;
		rb.angularVelocity = 			angularVelocity;
		rb.mass = 						mass;
		rb.inertia = 					inertia;
		rb.rotation = 					rotation;

		rb.freezeRotation = 			freezeRotation;
		rb.velocity = 					velocity;
		rb.centerOfMass = 				centerOfMass;

		rb.constraints = 				constraints;
		rb.interpolation = 				interpolation;
		rb.position = 					position;
	}

	/// <summary>
	/// Sets the properties of this save state from those of the passed object.
	/// </summary>
	public void SetFrom(Rigidbody2D rb)
	{
		bodyType = 					rb.bodyType;
		gravityScale = 				rb.gravityScale;
		drag = 						rb.drag;
		angularDrag = 				rb.angularDrag;
		angularVelocity = 			rb.angularVelocity;
		mass = 						rb.mass;
		inertia = 					rb.inertia;
		rotation = 					rb.rotation;

		freezeRotation = 			rb.freezeRotation;
		velocity = 					rb.velocity;
		centerOfMass = 				rb.centerOfMass;

		constraints = 				rb.constraints;
		interpolation = 			rb.interpolation;
		position = 					rb.position;
	}
	
	public static Rigidbody2DSaveState CreateFrom(Rigidbody2D rb)
	{
		return new Rigidbody2DSaveState(rb);
	}

	
}
