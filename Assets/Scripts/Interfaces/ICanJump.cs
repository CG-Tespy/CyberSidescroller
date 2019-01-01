
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ICanJump : IGroundable
{
	bool canJump { get; }
	bool isJumping { get; }

}
