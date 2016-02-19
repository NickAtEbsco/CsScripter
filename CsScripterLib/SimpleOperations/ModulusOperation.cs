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
			if (next.Value.HasValue)
			{
				if (Value.HasValue)
				{
					next.StoreValue(Value.Value % next.Value.Value);
					return next;
				}
			}
			return base.Evaluate(next);
		}
	}
}
