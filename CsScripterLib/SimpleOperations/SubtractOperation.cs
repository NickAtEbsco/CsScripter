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
			if (!double.IsNaN(next.Value))
			{
				if (!double.IsNaN(Value))
				{
					next.StoreValue(Value - next.Value);
					return next;
				}
				if (!string.IsNullOrWhiteSpace(String))
				{
					next.StoreString(String.Replace(next.Value.ToString(CultureInfo.InvariantCulture), ""));
					return next;
				}
			}
			else if (!string.IsNullOrEmpty(next.String))
			{
				if (!double.IsNaN(Value))
				{
					next.StoreString(Value.ToString(CultureInfo.InvariantCulture).Replace(next.String, ""));
					return next;
				}
				if (!string.IsNullOrWhiteSpace(String))
				{
					next.StoreString(String.Replace(next.String, ""));
					return next;
				}
			}

			return base.Evaluate(next);
		}
	}
}
