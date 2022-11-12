using System;
using UnityEngine;

public abstract class ICommand:ScriptableObject
{
	#region Public methods

	public abstract bool DoCommand(); // return success state

	#endregion
}
