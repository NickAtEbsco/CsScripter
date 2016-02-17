using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsScripterLib
{
	public interface IInterpreter
	{
		void Initialize();
		void Process(string script);

		void AppendOutputLine(string outputLine);

		string Output { get; }
	}
}
