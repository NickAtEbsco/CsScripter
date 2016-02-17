using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsScripterLib.Functions;
using CsScripterLib.Results;
using CsScripterLib.SimpleOperations;

namespace CsScripterLib
{
	public class Interpreter : IInterpreter
	{
		IParserFunction m_parser;
		StringBuilder m_outputString;

		int m_currentChar;

		public Interpreter(IParserFunction parser)
		{
			m_parser = parser;
		}

		public void Initialize()
		{
			m_parser.AddSingleCommandFunctions(Constants.ADD, new AddOperation());
			m_parser.AddSingleCommandFunctions(Constants.SUBTRACT, new SubtractOperation());
			m_parser.AddSingleCommandFunctions(Constants.MULTIPLY, new MultiplyOperation());
			m_parser.AddSingleCommandFunctions(Constants.DIVIDE, new DivideOperator());
			m_parser.AddSingleCommandFunctions(Constants.END_LINE, new EmptyOperation());

			m_parser.AddFunction(Constants.SET, new SetFunction(m_parser));
			m_parser.AddFunction(Constants.PRINT, new PrintFunction(m_parser, this));
		}

		public void Process(string script)
		{
			m_outputString = new StringBuilder();

			// Pre parse data to remove comments and other items here.
			var preProcessingResult = CleanupTextForProcessing(script);

			// An Error occured.
			if (preProcessingResult.IsError)
			{
				AppendOutputLine("Script Parsing Error:\n" + preProcessingResult.String);
				return;
			}

			// Use the clean script now
			var cleanScript = preProcessingResult.String;

			m_currentChar = 0;

			while (m_currentChar < cleanScript.Length)
			{
				var result = m_parser.ParseAndExecuteNextLine(cleanScript, ref m_currentChar, new[] { '\n' });
			}
		}

		Result CleanupTextForProcessing(string script)
		{
			var result = new StringBuilder(script.Length);
			bool inQuote = false;
			bool inComments = false;

			int lineNumber = 0;
			int parenthesis = 0;

			for (int i = 0; i < script.Length; i++)
			{
				var current = script[i];

				switch (current)
				{
					case Constants.COMMENT:
						inComments = true;
						break;
					case Constants.START_ARG:
						if (!inComments && !inQuote)
							parenthesis++;
						break;
					case Constants.END_ARG:
						if (!inComments && !inQuote)
							parenthesis--;
						break;
					case Constants.END_LINE:
						var errorMsg = CheckLineForErrorMessage(parenthesis, inQuote, lineNumber);
						if (errorMsg != null)
							return errorMsg;

						lineNumber++;

						if (inComments)
						{
							inComments = false;
							continue;
						}

						break;
					case ' ':
						if (inQuote)
							result.Append(current);
						continue;
					case '\"':
						if (!inComments)
							inQuote = !inQuote;
						break;
				}

				if (!inComments)
					result.Append(current);
			}

			var lastLineErrorMsg = CheckLineForErrorMessage(parenthesis, inQuote, lineNumber);
			if (lastLineErrorMsg != null)
				return lastLineErrorMsg;

			return new Result(double.NaN, result.ToString());
		}

		Result CheckLineForErrorMessage(int parenthesis, bool inQuote, int lineNumber)
		{
			if (inQuote)
				return new ErrorResult("Missing closing quote on line: " + lineNumber);

			if (parenthesis < 0)
				return new ErrorResult("Missing open parenthesis on line: " + lineNumber);

			if (parenthesis > 0)
				return new ErrorResult("Missing closing parenthesis on line: " + lineNumber);

			return null;
		}

		public void AppendOutputLine(string outputLine)
		{
			m_outputString.AppendLine(outputLine);
		}

		public string Output
		{
			get { return m_outputString.ToString(); }
		}
	}
}
