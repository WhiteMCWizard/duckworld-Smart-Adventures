using SLAM.InputSystem;

public static class SLAMInput
{
	public enum Button
	{
		UpOrAction = 0,
		Down = 1,
		Left = 2,
		Right = 3,
		Action = 4,
		Up = 5
	}

	public static IInputProvider Provider { get; private set; }

	static SLAMInput()
	{
		Provider = new DesktopInputProvider();
	}
}
