namespace CsScripterLib
{
	public interface IVarManager
	{
		void UpdateOrCreateVar(string name, double value, string str);
		object GetVar(string name);
		void Clear();
	}
}
