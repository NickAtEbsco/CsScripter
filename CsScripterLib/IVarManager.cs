namespace CsScripterLib
{
	public interface IVarManager
	{
		void UpdateOrCreateVar(string name, object value);
		object GetVar(string name);
		void Clear();
	}
}
