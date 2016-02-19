
using System;
using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.ADD)]
	public class AddOperation : BaseOperation
	{
		public AddOperation(IVarManager varManager)
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
					next.StoreValue(Value.Value + next.Value.Value);
					return next;
				}
				if (String != null)
				{
					next.StoreValue(String + next.Value.Value);
					return next;
				}
				if (Boolean.HasValue)
					throw new ArgumentException("Can't add two booleans together.");

			}
			else if (next.String != null)
			{
				if (Value.HasValue)
				{
					next.StoreValue(Value.Value + next.String);
					return next;
				}
				if (String != null)
				{
					next.StoreValue(String + next.String);
					return next;
				}
				if (Boolean.HasValue)
					throw new ArgumentException("Can't add two booleans together.");
			}
			else if (next.Boolean.HasValue)
				throw new ArgumentException("Can't add two booleans together.");

			return base.Evaluate(next);
		}
	}
}
