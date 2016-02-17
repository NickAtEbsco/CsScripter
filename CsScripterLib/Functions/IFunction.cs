using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScripterLib.Results;

namespace CsScripterLib.Functions
{
	public interface IFunction
	{
		Result Evaluate(string data, ref int currentPosition);
	}
}
