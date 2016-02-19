namespace CsScripterLib.SimpleOperations
{
	public interface ISimpleOperation
	{
		void StoreValue(object value);
		void StoreVariable(string varName);

		ISimpleOperation Evaluate(ISimpleOperation next);

		/// <summary>
		/// Order of operations priority
		/// </summary>
		int Priority { get; }
		double? Value { get; }
		string String { get; }
		bool? Boolean { get; }
		object Object { get; }
		string VarName { get; }
	}
}
