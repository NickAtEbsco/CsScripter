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
					return new EmptyOperation( Value - next.Value, m_varManager);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String.Replace(next.Value.ToString(CultureInfo.InvariantCulture), ""), m_varManager);
			}
			else if (!string.IsNullOrEmpty(next.String))
			{
				if (!double.IsNaN(Value))
					return new EmptyOperation(Value.ToString(CultureInfo.InvariantCulture).Replace(next.String, ""), m_varManager);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String.Replace(next.String, ""), m_varManager);
			}

			return base.Evaluate(next);
		}
	}
}
