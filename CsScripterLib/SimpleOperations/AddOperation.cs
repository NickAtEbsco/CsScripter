namespace CsScripterLib.SimpleOperations
{
	public class AddOperation : BaseOperation
	{
		public AddOperation()
		{
			Priority = 1;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// Compare against types.
			if (!double.IsNaN(next.Value))
			{
				if (!double.IsNaN(Value))
					return new EmptyOperation(Value + next.Value);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String + next.Value);
			}
			else if (!string.IsNullOrEmpty(next.String))
			{
				if (!double.IsNaN(Value))
					return new EmptyOperation(Value + next.String);
				if (!string.IsNullOrWhiteSpace(String))
					return new EmptyOperation(String + next.String);
			}

			return base.Evaluate(next);
		}
	}
}
