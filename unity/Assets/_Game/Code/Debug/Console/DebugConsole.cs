namespace Code.Debug.Console
{
	/*
	 * call -c DebugConsolePanel -m TestMethod -a "hello world"
	 * call -c Debug ConsolePanel -m TestMethod -a "test" -i 1
	 *
	 * set -c DebugConsolePanel -m id -v "new value"
	 * get -c DebugConsolePanel -m id
	 *
	 * -c -> Class contract
	 * -m -> Class member
	 * -a -> Arguments/Parameters for method
	 * -i -> identity instance
	 * -v -> field/property (<T>back_field) value
	 *
	 * call -> invoke method
	 * set/get -> set/get value in members: field/property
	 * list -m -> list members in contract class
	 * list -c -> list instances contracts
	 */
	public class DebugConsole
	{
		public static void Initialize()
		{
			var console = new DebugConsole();

			console.Start();
		}

		private void Start()
		{
		}
	}
}