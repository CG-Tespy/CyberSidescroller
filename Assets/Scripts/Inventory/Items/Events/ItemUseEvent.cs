using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberSidescroller.Items
{
	/// <summary>
	/// Event revolving around an item being used at a given point in time.
	/// </summary>
	/// <typeparam name="TUsageArgs">Type of UsageArgs</typeparam>
	/// <typeparam name="TCSSItem">Type of CSSItem</typeparam>
	public class UseEvent<TUsageArgs, TCSSItem> : UnityEvent<TUsageArgs> 
	where TUsageArgs: UsageArgs<TCSSItem> 
	where TCSSItem: CSSItem {}
}
