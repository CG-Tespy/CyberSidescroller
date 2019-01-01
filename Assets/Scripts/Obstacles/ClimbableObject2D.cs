using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Lets the player climb when in contact with this.
/// </summary>
public class ClimbableObject2D : CSS_MonoBehaviour2D, IClimbable2D
{
    private SpriteRenderer ladderRenderer;
    private BoxCollider2D ladderCollider;

    void Start() {
        ladderRenderer = gameObject.GetComponent<SpriteRenderer>();
        ladderCollider = gameObject.GetComponent<BoxCollider2D>();

        //ladderRenderer.size = new Vector2(1, ladderRenderer.size.y);
        //ladderCollider.size = new Vector2(1, ladderRenderer.size.y);
    }

}
