using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class Collider2DExtensions 
{
	public static float Height(this Collider2D coll)
	{
		return coll.bounds.size.y;
	}

	public static float Width(this Collider2D coll)
	{
		return coll.bounds.size.x;
	}

	/// <summary>
	/// Returns the x coordinate aligned with this collider's left edge.
	/// </summary>
	public static float LeftEdge(this Collider2D coll)
	{
		return coll.transform.position.x - (coll.Width() / 2);
	}

	/// <summary>
	/// Returns the x coordinate aligned with this collider's right edge.
	/// </summary>
	public static float RightEdge(this Collider2D coll)
	{
		return coll.transform.position.x + (coll.Width() / 2);
	}

	/// <summary>
	/// Returns the y coordinate aligned with this collider's upper edge.
	/// </summary>
	public static float UpperEdge(this Collider2D coll)
	{
		return coll.transform.position.y + (coll.Height() / 2);
	}

	/// <summary>
	/// Returns the y coordinate aligned with this collider's lower edge.
	/// </summary>
	public static float LowerEdge(this Collider2D coll)
	{
		return coll.transform.position.y - (coll.Height() / 2);
	}

}
