using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.MODULUS)]
	public class ModulusOperation : BaseOperation
	{
		public ModulusOperation(IVarManager varManager)
			: base(varManager)
		{
			Priority = 2;
		}

		public override ISimpleOperation Evaluate(ISimpleOperation next)
		{
			// Compare against types.
			if (!double.IsNaN(next.Value))
			{
				if (!double.IsNaN(Value))
					return new EmptyOperation(Value % next.Value, m_varManager);
			}
			return base.Evaluate(next);
		}
	}
}
