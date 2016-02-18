using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScripterLib.Attributes;
using CsScripterLib.Results;
using CsScripterLib.SimpleOperations;
using Microsoft.Practices.Unity;

namespace CsScripterLib.Functions
{
	//[FunctionAttributes(Constants.VARIABLE)]
	//public class VarFunction : BaseOperation
	//{
	//	IParserFunction m_parserFunction;
	//	IVarManager m_varManager;

	//	public VarFunction()
	//	{
	//		m_parserFunction = BootStrapper.UnityContainer.Resolve<IParserFunction>();
	//		m_varManager = BootStrapper.UnityContainer.Resolve<IVarManager>(); ;

	//		Priority = -100;
	//	}

	//	//public Result Evaluate(string data, ref int currentPosition)
	//	//{
	//	//	// Create the list of all keys to stop at for variables
	//	//	var varEnd = new List<Char>(new[] { Constants.END_ARG, Constants.END_LINE });
	//	//	varEnd.AddRange(m_parserFunction.SimpleOperations.Keys);

	//	//	var result = m_parserFunction.ParseAndExecuteNextLine(data, ref currentPosition, new[] { Constants.END_ARG });

	//	//	//if (result.IsError)
	//	//	//	return result;

	//	//	//return new VarResult();

	//	//	return result;
	//	//}

	//	public override ISimpleOperation Evaluate(ISimpleOperation next)
	//	{
	//		if (next == null)
	//		{
	//			var var = m_varManager.GetVar(VarName);
	//		}
	//		else
	//		{
	//			m_varManager.UpdateOrCreateVar(VarName, next.Value, next.String);
	//		}

	//		return new EmptyOperation();
	//	}

	//	public string VarName { get; set; }
	//}
}
