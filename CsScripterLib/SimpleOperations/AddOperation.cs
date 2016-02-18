
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
					return new EmptyOperation(Value + next.Value, m_varManager);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String + next.Value, m_varManager);
			}
			else if (!string.IsNullOrEmpty(next.String))
			{
				if (!double.IsNaN(Value))
					return new EmptyOperation(Value + next.String, m_varManager);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String + next.String, m_varManager);
			}

			return base.Evaluate(next);
		}
	}
}
