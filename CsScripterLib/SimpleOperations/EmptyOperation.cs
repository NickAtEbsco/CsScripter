namespace CsScripterLib.SimpleOperations
{
	// Used for End of Line, end of Parens etc.
	public class EmptyOperation : BaseOperation
	{
		public EmptyOperation()
		{
			Priority = 0;
		}

		public EmptyOperation(string str)
		{
			String = str;
			Priority = 0;
		}

		public EmptyOperation(double value)
		{
			Value = value;
			Priority = 0;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// This shouldn't be here?
			return base.Evaluate(next);
		}
	}
}
