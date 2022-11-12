
	using UnityEngine;

	public interface CommandExecuter
	{
		public ExecuterType GetExecuterType();
		public Transform GetExecuterTransform();
	}

	public enum ExecuterType
	{
		Player,
		Enemy
	}
	
