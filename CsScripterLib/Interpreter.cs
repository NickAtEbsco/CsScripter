﻿using System;
using System.Linq;
using System.Text;
using CsScripterLib.Functions;
using CsScripterLib.Results;
using CsScripterLib.SimpleOperations;

namespace CsScripterLib
{
	public class Interpreter : IInterpreter
	{
		IParserFunction m_parser;
		IVarManager m_varManager;
		StringBuilder m_outputString;

		int m_currentChar;

		public Interpreter(IParserFunction parser, IVarManager varManager)
		{
			m_parser = parser;
			m_varManager = varManager;
		}

		public void Initialize()
		{
			try
			{
				//Fill the imports of this object
				var init = BootStrapper.UnityContainer;
				var exportedSimpleOps = init.ResolveAll(typeof(ISimpleOperation)).Cast<ISimpleOperation>();

				// ** Get all the exported Simple Operations
				foreach( var simpleOp in exportedSimpleOps)
				{
					var exportedSymbol = simpleOp.GetExportedSymbol();

					if (exportedSymbol.HasValue)
						m_parser.AddSingleCommandFunctions(exportedSymbol.Value, simpleOp);
				}

				// ** Get all the exported Functions
				var exportedFunctions = init.ResolveAll(typeof(IFunction)).Cast<IFunction>();
				foreach (var function in exportedFunctions)
				{
					var exportedSymbol = function.GetExportedSymbol();

					if (!string.IsNullOrEmpty(exportedSymbol))
						m_parser.AddFunction(exportedSymbol, function);
				}
			}
			catch (Exception ex)
			{
				AppendOutputLine("Error dynamically loading all the parts.\n" + ex);
			}
		}

		public void Process(string script)
		{
			// Clean anything from before.
			m_varManager.Clear();

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

				if (result.IsError)
					return;
			}
		}

		Result CleanupTextForProcessing(string script)
		{
			var result = new StringBuilder(script.Length);
			bool inQuote = false;
			bool inComments = false;

			int lineNumber = 0;
			int column = 0;
			int parenthesis = 0;

			char current = Constants.NULL_CHAR;
			char previous = Constants.NULL_CHAR;

			for (int i = 0; i < script.Length; i++)
			{
				column++;

				previous = current;
				current = script[i];

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
						column = 0;

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
					case Constants.TAB:
						if (previous != Constants.NULL_CHAR && previous != Constants.END_LINE && previous != Constants.TAB)
							return new ErrorResult(string.Format("Tabs aren't allowed in the middle of a statement.\nLine Number: {0}, Column: {1}", lineNumber, column));
						break;
				}

				if (!inComments)
					result.Append(current);
			}

			var lastLineErrorMsg = CheckLineForErrorMessage(parenthesis, inQuote, lineNumber);
			if (lastLineErrorMsg != null)
				return lastLineErrorMsg;

			return new Result(result.ToString());
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
