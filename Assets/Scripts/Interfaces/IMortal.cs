using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IMortal 
{

	bool Die(bool forceDeath = false);
}
