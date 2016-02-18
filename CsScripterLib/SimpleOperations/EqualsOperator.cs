using System;
using System.Runtime.InteropServices;
using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.EQUALS)]
	public class EqualsOperator : BaseOperation
	{
		public EqualsOperator(IVarManager varManager) 
			: base(varManager)
		{
			Priority = -100;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// Its a variable name. Store it
			if (double.IsNaN(Value) && string.IsNullOrEmpty(String) && !string.IsNullOrEmpty(VarName))
			{
				m_varManager.UpdateOrCreateVar(VarName, next.Value, next.String);
				return new EmptyOperation(double.NaN, null, VarName, m_varManager);
			}

			// If statement? Currently not implemented
			throw new NotImplementedException("Currently don't support boolean checks.");
		}
	}
}
