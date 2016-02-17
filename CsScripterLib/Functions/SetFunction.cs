using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScripterLib.Results;

namespace CsScripterLib.Functions
{
	public class SetFunction : IFunction
	{
		IParserFunction m_parserFunction;

		public SetFunction(IParserFunction parserFunction)
		{
			m_parserFunction = parserFunction;
		}

		public Result Evaluate(string data, ref int currentPosition)
		{
			throw new NotImplementedException();
		}
	}
}
