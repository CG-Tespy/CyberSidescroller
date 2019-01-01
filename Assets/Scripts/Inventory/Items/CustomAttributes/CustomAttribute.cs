using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberSidescroller.Items
{
	public abstract class CustomAttribute<T>
	{
		bool _modifiable = 				true;
		T _value;

		public virtual System.Type type 			{ get; protected set; }
		public virtual string asString 				{ get; protected set;  }
		public virtual bool modifiable
		{
			get { return _modifiable; }
			protected set { _modifiable = value; }
		}
		
		public virtual T value 						
		{
			get { return _value; } 
			set { _value = value; } 
		}

		public CustomAttribute()
		{
			// Might as well just cache the values.
			type = 				typeof(T);
			asString = 			value.ToString();
		}

		public CustomAttribute(T value) : base()
		{
			this.value = 		value;
		}
		
	}
}