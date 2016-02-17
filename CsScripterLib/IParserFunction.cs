using CsScripterLib.Functions;
using CsScripterLib.Results;
using CsScripterLib.SimpleOperations;

namespace CsScripterLib
{
	public interface IParserFunction
	{
		void AddFunction(string name, IFunction function);
		void AddSingleCommandFunctions(char command, ISimpleOperation function);

		Result ParseAndExecuteNextLine(string data, ref int currentPosition, char[] to);
	}
}
