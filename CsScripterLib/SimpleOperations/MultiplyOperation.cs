using CsScripterLib.Attributes;

namespace CsScripterLib.SimpleOperations
{
	[Operation(Constants.MULTIPLY)]
	public class MultiplyOperation : BaseOperation
	{
		public MultiplyOperation(IVarManager varManager)
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
					 return new EmptyOperation(Value * next.Value, m_varManager);
				//else if (!string.IsNullOrWhiteSpace(String))
				//	next.StoreString(String.Replace(next.Value.ToString(CultureInfo.InvariantCulture), ""));
			}
			//else if (!string.IsNullOrEmpty(next.String))
			//{
			//	if (!double.IsNaN(Value))
			//		next.StoreString(Value.ToString(CultureInfo.InvariantCulture).Replace(next.String, ""));
			//	else if (!string.IsNullOrWhiteSpace(String))
			//		next.StoreString(String.Replace(next.String, ""));
			//}

			return new EmptyOperation(m_varManager);
		}
	}
}
