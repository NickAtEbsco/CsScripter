
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

		public EmptyOperation(object value, string varName, IVarManager varManager)
			: base(varManager)
		{
			StoreValue(value);
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
