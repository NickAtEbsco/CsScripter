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
			if (!double.IsNaN(next.Value))
			{
				if (!double.IsNaN(Value))
				{
					if (next.Value == 0.0)
						throw new DivideByZeroException();
					next.StoreValue(Value / next.Value);
					return next;
				}
				//else if (!string.IsNullOrWhiteSpace(String))
				//	next.StoreString(String.Replace(next.Value.ToString(CultureInfo.InvariantCulture), ""));
			}
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
