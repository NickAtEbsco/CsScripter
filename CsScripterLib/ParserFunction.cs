using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsScripterLib.Functions;
using CsScripterLib.Results;
using CsScripterLib.SimpleOperations;
using Microsoft.Practices.Unity;
using ResolverOverride = Microsoft.Practices.Unity.ResolverOverride;

namespace CsScripterLib
{
	public class ParserFunction : IParserFunction
	{
		Dictionary<string, IFunction> m_allFunctions = new Dictionary<string, IFunction>();

		Dictionary<char, ISimpleOperation> m_singleCharCommands = new Dictionary<char, ISimpleOperation>();

		public void AddFunction(string name, IFunction function)
		{
			m_allFunctions.Add(name, function);
		}

		public void AddSingleCommandFunctions(char command, ISimpleOperation function)
		{
			m_singleCharCommands.Add(command, function);
		}

		public Result ParseAndExecuteNextLine(string data, ref int currentPosition, char[] to)
		{
			if (currentPosition >= data.Length || to.Contains(data[currentPosition]))
				return new Result();

			StringBuilder currentItem = new StringBuilder();
			List<ISimpleOperation> simpleOperations = new List<ISimpleOperation>();
			char previous = Constants.NULL_CHAR;
			char current;

			do
			{
				// Get the current character
				current = data[currentPosition++];

				// If this is the start of a quote, we want to grab everyhting in the quote without checking for if it matches a function
				if (current == Constants.QUOTE && previous == Constants.NULL_CHAR)
				{
					currentItem.Append(current);
					previous = current;
					continue;
				}

				// Check if we have a match
				if (KeepParsingLine(current, previous, to))
				{
					currentItem.Append(current);

					if (previous == Constants.QUOTE && current == Constants.QUOTE)
						previous = Constants.NULL_CHAR;

					if (currentPosition < data.Length)
						continue;
				}

				//// Check if a valid end was found.
				//if (to.Length > 0 && currentPosition < data.Length && !to.Contains(current))
				//{
				//	return new ErrorResult("Unexpected end of string found...Looking for: " + (from c in to
				//																select c.ToString()).Aggregate((a, b) => a + "," + b));
				//}

				var itemToParse = currentItem.ToString();

				if (itemToParse.Length == 0)
					return new Result();

				// Check if the previous string is a valid method.
				var function = (from fcn in m_allFunctions
							 where fcn.Key == itemToParse
							 select fcn.Value).FirstOrDefault();

				if (function != null)
				{
					var fcnResult = function.Evaluate(data, ref currentPosition);

					if (fcnResult.IsError)
						return fcnResult;

					// Do something with the result.


					// Keep looking?
					return fcnResult;
				}

				// Test to see if its a string or value, because the Single Line Commands will need it if it is so.
				var parseStrRes = ParseString(itemToParse);
				if (parseStrRes.IsError)
					return parseStrRes;

				// Check if its a single line cmd.
				var singleLineCmd = GetNewSimpleOperation(current);

				if (singleLineCmd != null)
				{
					singleLineCmd.StoreValue(parseStrRes.Object);
					singleLineCmd.StoreVariable(parseStrRes.VarName);

					simpleOperations.Add(singleLineCmd);

					// Reset this.
					currentItem.Clear();
					continue;
				}

				// This should be a variable, string, etc. but we'll check what it is later.
				if (current == Constants.END_LINE || currentPosition == data.Length || to.Contains(current))
				{
					var empty = BootStrapper.UnityContainer.Resolve<EmptyOperation>();
					empty.StoreValue(parseStrRes.Object);
					empty.StoreVariable(parseStrRes.VarName);

					simpleOperations.Add(empty);

					// Reset this.
					currentItem.Clear();
					continue;
				}

				// See if it matches anything.
			} while (currentPosition < data.Length && !to.Contains(current));

			// Now process all the Simple Operations if any exist
			if (simpleOperations.Count > 0)
			{
				var startCell = simpleOperations[0];
				int startIndex = 1;

				var mergeResult = Merge(startCell, ref startIndex, simpleOperations);
				return new Result(mergeResult.Object);
			}

			return new Result();
		}

		public IDictionary<char, ISimpleOperation> SimpleOperations
		{
			get { return m_singleCharCommands; }
		}

		public IDictionary<string, IFunction> Functions
		{
			get { return m_allFunctions; }
		}

		bool KeepParsingLine(char current, char previous, char[] to)
		{
			if (to.Contains(current))
				return false;

			if (current == Constants.END_LINE || current == Constants.START_ARG || current == Constants.END_ARG)
				return false;

			// We will check all single commands here, because this list should be small
			if (m_singleCharCommands.Any(c => c.Key == current))
				return false;

			return true;
		}

		Result ParseString(string parseItem)
		{
			if (parseItem[0] == Constants.VARIABLE)
				return new Result(null, parseItem);

			// Check if its a string.  
			if (parseItem[0] == Constants.QUOTE && parseItem[parseItem.Length - 1] == Constants.QUOTE)
				return new Result(parseItem.Substring(1, parseItem.Length - 2));

			// Check if its a number.
			double parseNumber;
			if (double.TryParse(parseItem, out parseNumber))
				return new Result(parseNumber);

			return new ErrorResult(string.Format("Unexpected string found: \'{0}\'.  Expected String or variable.", parseItem));
		}

		private ISimpleOperation Merge(ISimpleOperation current, ref int index, List<ISimpleOperation> simpleOperations, bool mergeOneOnly = false)
		{
			// If there's only 1 simple operation and its a variable, we want to return the value of the var instead of the text for it.
			if (simpleOperations.Count == 1 && !string.IsNullOrEmpty(simpleOperations[0].VarName))
			{
				var empty = BootStrapper.UnityContainer.Resolve<EmptyOperation>();
				empty.StoreValue(simpleOperations[0].Object);
				return empty;
			}

			while( index < simpleOperations.Count)
			{
				ISimpleOperation next = simpleOperations[index++];

				while (!CanItemsMerge(current, next))
				{
					// If we cannot merge cells yet, go to the next cell and merge
					// next cells first. E.g. if we have 1+2*3, we first merge next
					// cells, i.e. 2*3, getting 6, and then we can merge 1+6.
					next = Merge(next, ref index, simpleOperations, true);
				}

				current = MergeItems(current, next);

				if (mergeOneOnly)
				{
					break;
				}
			}

			return current;
		}

		private bool CanItemsMerge(ISimpleOperation current, ISimpleOperation next)
		{
			// Equals is a slightly different case.
			if (current is EqualsOperator && next is EmptyOperation)
				return true;

			return current.Priority >= next.Priority;
		}

		private ISimpleOperation MergeItems(ISimpleOperation current, ISimpleOperation next)
		{
			return current.Evaluate(next);
		}

		public Result ParseAndGetNextItem(string data, ref int currentPosition, char[] to)
		{
			return new Result();
		}

		public ISimpleOperation GetNewSimpleOperation(char key)
		{
			var simpleOperation = (from sngl in m_singleCharCommands
							   where sngl.Key == key
							   select sngl.Value).FirstOrDefault();

			if (simpleOperation == null)
				return null;

			return (ISimpleOperation)BootStrapper.UnityContainer.Resolve(simpleOperation.GetType(), "");
		}

		/// <summary>
		/// Ensure the TABs are correct.
		/// </summary>
		/// <param name="currentString"></param>
		/// <param name="currentChar"></param>
		/// <returns></returns>
		bool ValidateTabCorrectness(StringBuilder currentString, char currentChar)
		{
			// If its not a tab, ignore
			if (currentChar != Constants.TAB)
				return true;

			// If its the first Tab, its okay.
			if (currentString.Length == 0 && currentChar == Constants.TAB)
				return true;

			// if the current item is a tab, and the previous is a tab, we're all set.
			if (currentString.Length > 0 && currentString[currentString.Length - 1] == Constants.TAB)
				return true;

			// all other items are not allowed.
			return false;
		}
	}
}
