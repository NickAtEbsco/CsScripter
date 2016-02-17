using System;

namespace CsScripterLib.SimpleOperations
{
	public class DivideOperator : BaseOperation
	{
		public DivideOperator()
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
					return new EmptyOperation(Value / next.Value);
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

			return new EmptyOperation();
		}
	}
}
