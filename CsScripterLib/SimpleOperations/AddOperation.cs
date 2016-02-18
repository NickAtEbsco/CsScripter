
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
			if (!double.IsNaN(next.Value))
			{
				if (!double.IsNaN(Value))
				{
					next.StoreValue(Value + next.Value);
					return next;
				}
				if (!string.IsNullOrWhiteSpace(String))
				{
					next.StoreString(String + next.Value);
					return next;
				}
			}
			else if (!string.IsNullOrEmpty(next.String))
			{
				if (!double.IsNaN(Value))
				{
					next.StoreString(Value + next.String);
					return next;
				}
				if (!string.IsNullOrWhiteSpace(String))
				{
					next.StoreString(String + next.String);
					return next;
				}
			}

			return base.Evaluate(next);
		}
	}
}
