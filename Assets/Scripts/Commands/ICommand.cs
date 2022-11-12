using System;
using UnityEngine;

public abstract class ICommand:ScriptableObject
{
	#region Public methods

	public abstract bool DoCommand(CommandExecuter executer); // return success state

	#endregion
}
