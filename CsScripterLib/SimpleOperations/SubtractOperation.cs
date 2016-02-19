using System;
using System.Globalization;
using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.SUBTRACT)]
	public class SubtractOperation : BaseOperation
	{
		public SubtractOperation(IVarManager varManager)
			: base(varManager)
		{
			Priority = 1;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// Compare against types.
			if (next.Value.HasValue)
			{
				if (Value.HasValue)
				{
					next.StoreValue(Value.Value - next.Value.Value);
					return next;
				}
				if (String != null)
				{
					next.StoreValue(String.Replace(next.Value.Value.ToString(CultureInfo.InvariantCulture), ""));
					return next;
				}
				if (Boolean.HasValue)
					throw new ArgumentException("Can't subtract two booleans together.");
			}
			else if (next.String != null)
			{
				if (Value.HasValue)
				{
					next.StoreValue(Value.Value.ToString(CultureInfo.InvariantCulture).Replace(next.String, ""));
					return next;
				}
				if (String != null)
				{
					next.StoreValue(String.Replace(next.String, ""));
					return next;
				}
				if (Boolean.HasValue)
					throw new ArgumentException("Can't subtract two booleans together.");
			}
			else if (next.Boolean.HasValue)
				throw new ArgumentException("Can't subtract two booleans together.");

			return base.Evaluate(next);
		}
	}
}
