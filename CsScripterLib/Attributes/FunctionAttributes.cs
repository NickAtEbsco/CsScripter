using System;

namespace CsScripterLib.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FunctionAttributes : Attribute
	{
		public FunctionAttributes(string theFunction)
		{
			Function = theFunction;
		}

		public string Function { get; private set; }
	}
}
