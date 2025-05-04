using System;

namespace Code.Debug.Console
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DebugContractAttribute : Attribute
	{
		public string name { get; set; }
	}
}