using System;
using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.DIVIDE)]
	public class DivideOperator : BaseOperation
	{
		public DivideOperator(IVarManager varManager)
			: base(varManager)
		{
			Priority = 2;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// Compare against types.
			if (next.Value.HasValue)
			{
				if (Value.HasValue)
				{
					if (next.Value.Value == 0.0)
						throw new DivideByZeroException();
					next.StoreValue(Value.Value / next.Value.Value);
					return next;
				}
				//else if (!string.IsNullOrWhiteSpace(String))
				//	next.StoreString(String.Replace(next.Value.ToString(CultureInfo.InvariantCulture), ""));
			}
			else if(next.Boolean.HasValue)
				throw new ArgumentException("Can't divide two booleans together.");
				
			//else if (!string.IsNullOrEmpty(next.String))
			//{
			//	if (!double.IsNaN(Value))
			//		next.StoreString(Value.ToString(CultureInfo.InvariantCulture).Replace(next.String, ""));
			//	else if (!string.IsNullOrWhiteSpace(String))
			//		next.StoreString(String.Replace(next.String, ""));
			//}

			return next;
		}
	}
}
