namespace CsScripterLib.SimpleOperations
{
	public interface ISimpleOperation
	{
		void StoreValue(double value);
		void StoreString(string str);
		void StoreVariable(string varName);

		ISimpleOperation Evaluate(ISimpleOperation next);

		/// <summary>
		/// Order of operations priority
		/// </summary>
		int Priority { get; }
		double Value { get; }
		string String { get; }
		string VarName { get; }
	}
}
