using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Just messing around with the animation system.
/// </summary>
public class AnimationProgressTracker : MonoBehaviour 
{
	SidescrollerCharacter player;
	[SerializeField] float animationLength;
	[SerializeField] float animationProgress;
	Animator animator 					{ get { return player.animator; } }
	AnimatorClipInfo currentClipInfo 	{ get { return animator.GetCurrentAnimatorClipInfo(0)[0]; } }

	// Use this for initialization
	void Awake () 
	{
		player = 					GameObject.FindObjectOfType<SidescrollerCharacter>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Get the length of the current animation that's playing, as well as how far into it 
		// the animation is at the current frame
		AnimatorStateInfo asi = 	animator.GetCurrentAnimatorStateInfo(0);
		
		animationLength = 			asi.length;
		animationProgress = 		asi.normalizedTime;
		

	}

	/// <summary>
	/// Author: edu4hd0 from Unity Community
	/// </summary>
	bool AnimatorIsPlaying()
	{
     return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
  	}

	/// <summary>
	/// Author: edu4hd0 from Unity Community
	/// </summary>
	bool AnimationIsPlaying(string stateName)
	{
		return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
	}
}
