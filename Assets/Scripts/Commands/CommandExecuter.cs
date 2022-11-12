
	public interface CommandExecuter
	{
		public ExecuterType GetExecuterType();
	}

	public enum ExecuterType
	{
		Player,
		Enemy
	}
	
