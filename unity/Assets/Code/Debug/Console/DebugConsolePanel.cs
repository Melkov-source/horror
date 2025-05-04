namespace Code.Debug.Console
{
	[DebugContract]
	public class DebugConsolePanel
	{
		[DebugMember] 
		private string _id = "1";
		
		[DebugMember(name = "test_method")]
		public void TestMethod()
		{
			
		}
	}
}