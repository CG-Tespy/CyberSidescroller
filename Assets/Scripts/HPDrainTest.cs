using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class HPDrainTest 
{

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator TestHPBarFluctuation() 
	{
		// Use the Assert class to test conditions.
		// yield to skip a frame

		// Cycle between draining the player's HP to 0 and bringing it back to full
		GameObject playerPrefab = 			Resources.Load<GameObject>("Prefabs/Player");
		GameObject hpBarPrefab = 			Resources.Load<GameObject>("Prefabs/HPBar");

		GameObject playerGO = 				MonoBehaviour.Instantiate<GameObject>(playerPrefab);
		GameObject hpBarGO = 				MonoBehaviour.Instantiate<GameObject>(hpBarPrefab);

		SidescrollerCharacter player = 		playerGO.GetComponent<SidescrollerCharacter>();
		HPBar hpBar = 						hpBarGO.GetComponentInChildren<HPBar>();

		float prevFillAmount = 				hpBar.fillAmount;
		player.TakeDamage(1);

		yield return null;
		float newFillAmount = 				hpBar.fillAmount;
		Assert.AreNotEqual(prevFillAmount, newFillAmount);

		
	}
}
