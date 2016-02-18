
using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	// Used for End of Line, end of Parens etc.
	//[Operation(Constants.END_LINE)]
	public class EmptyOperation : BaseOperation
	{
		public EmptyOperation(IVarManager varManager)
			: base(varManager)
		{
			Priority = 0;
		}

		public EmptyOperation(string str, IVarManager varManager)
			: base(varManager)
		{
			String = str;
			Priority = 0;
		}

		public EmptyOperation(double value, IVarManager varManager)
			: base(varManager)
		{
			Value = value;
			Priority = 0;
		}

		public EmptyOperation(double value, string str, IVarManager varManager)
			: base(varManager)
		{
			Value = value;
			String = str;
			Priority = 0;
		}

		public EmptyOperation(double value, string str, string varName, IVarManager varManager)
			: base(varManager)
		{
			Value = value;
			String = str;
			VarName = varName;
			Priority = 0;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// This shouldn't be here?
			return base.Evaluate(next);
		}
	}
}
