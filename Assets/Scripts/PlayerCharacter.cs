using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	#region Serialize Fields

	[SerializeField] private List<PlayerInputMap> PlayerInputMaps;

	#endregion

	#region Unity methods

	private void Update()
	{
		for (int i = 0; i < PlayerInputMaps.Count; i++)
		{
			var current = PlayerInputMaps[i];
			if (current.Command == null)
			{
				Debug.LogError("no command set");
				continue;
			}
			switch (current.InputType)
			{
				case InputType.ButtonDown:
					if (Input.GetKeyDown(current.KeyCode))
					{
						current.Command.DoCommand();
					}

					break;
				case InputType.ButtonUp:
					if (Input.GetKeyUp(current.KeyCode))
					{
						current.Command.DoCommand();
					}

					break;
				default:
					Debug.LogError("not supported yet");
					break;
			}
		}
	}

	#endregion
}

[Serializable]
public class PlayerInputMap
{
	#region Public Fields
	
	public ICommand Command;
	public InputType InputType;
	public KeyCode KeyCode;

	#endregion
}

public enum InputType
{
	ButtonDown,
	ButtonUp,
	ButtonHold,
}
