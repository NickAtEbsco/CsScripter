using System;
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
			if (!Value.HasValue && String == null && !string.IsNullOrEmpty(VarName))
			{
				m_varManager.UpdateOrCreateVar(VarName, next.Object);
				return new EmptyOperation(null, VarName, m_varManager);
			}

			// ** If statement
			// Compare Nulls
			var thisValue = Object;
			var nextValue = next.Object;

			// Both nulls
			if (thisValue == null && nextValue == null)
			{
				next.StoreValue(true);
				return next;
			}

			// One null, the other non-null
			if (thisValue == null && nextValue != null || thisValue != null && nextValue == null)
			{
				next.StoreValue(false);
				return next;
			}

			if (thisValue.GetType() == nextValue.GetType())
			{
				if( thisValue is double?)
					next.StoreValue((double?)thisValue == (double?)nextValue);
				else if( thisValue is bool?)
					next.StoreValue((bool?)thisValue == (bool?)nextValue);
				else
					next.StoreValue((string)thisValue == (string)nextValue);

				return next;
			}

			next.StoreValue(thisValue.ToString() == nextValue.ToString());
			return next;
		}
	}
}
