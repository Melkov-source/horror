using System;

namespace Code.Debug.Console
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
	public class DebugMemberAttribute : Attribute
	{
		public string name { get; set; }
		public string description { get; set; }
		public string group { get; set; }
	}
}