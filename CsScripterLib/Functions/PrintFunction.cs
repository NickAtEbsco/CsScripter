using System.Globalization;
using CsScripterLib.Attributes;
using CsScripterLib.Results;

namespace CsScripterLib.Functions
{
	[FunctionAttributes(Constants.PRINT)]
	public class PrintFunction : IFunction
	{
		IParserFunction m_parserFunction;
		IInterpreter m_interpreter;

		public PrintFunction(IParserFunction parserFunction, IInterpreter interpreter)
		{
			m_parserFunction = parserFunction;
			m_interpreter = interpreter;
		}

		public Result Evaluate(string data, ref int currentPosition)
		{
			var result = m_parserFunction.ParseAndExecuteNextLine(data, ref currentPosition, new[] { Constants.END_ARG } );

			if (result.IsError)
				return result;

			if (!double.IsNaN(result.Value))
				m_interpreter.AppendOutputLine(result.Value.ToString(CultureInfo.InvariantCulture));
			else
				m_interpreter.AppendOutputLine(result.String);

			return new Result();
		}
	}
}
