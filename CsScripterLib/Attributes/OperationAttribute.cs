using System;

namespace CsScripterLib.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class OperationAttribute : Attribute
	{
		public OperationAttribute(char theOperator)
		{
			Operator = theOperator;
		}

		public char Operator { get; private set; }
	}
}
