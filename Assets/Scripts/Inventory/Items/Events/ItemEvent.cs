﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberSidescroller.Items
{
	public class Event<T> : UnityEvent<T> where T: CSSItem {}
}
