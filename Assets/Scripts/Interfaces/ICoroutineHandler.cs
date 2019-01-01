using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ICoroutineHandler 
{

	Coroutine StartCoroutine(IEnumerator routine);
	void StopCoroutine(Coroutine routine);
}
