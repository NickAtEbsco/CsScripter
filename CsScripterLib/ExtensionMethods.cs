using System;
using CsScripterLib.Attributes;
using CsScripterLib.Functions;
using CsScripterLib.SimpleOperations;

namespace CsScripterLib
{
	static class ExtensionMethods
	{
		/// <summary>
		/// This will get the exported Char for ISimpleOperation
		/// </summary>
		/// <seealso cref="ISimpleOperation"/>
		/// <returns></returns>
		public static char? GetExportedSymbol(this ISimpleOperation operation)
		{
			return GetOperationSymbol(operation.GetType());
		}

		public static char? GetOperationSymbol(Type type)
		{
			// Get instance of the attribute.
			OperationAttribute theAttribute = (OperationAttribute)Attribute.GetCustomAttribute(type, typeof(OperationAttribute));

			if (theAttribute == null)
				return null;

			return theAttribute.Operator;
		}

		/// <summary>
		/// This will get the exported string for IFunction
		/// </summary>
		/// <seealso cref="IFunction"/>
		/// <returns></returns>
		public static string GetExportedSymbol(this IFunction operation)
		{
			return GetFunctionSymbol(operation.GetType());
		}

		public static string GetFunctionSymbol(Type type)
		{
			// Get instance of the attribute.
			FunctionAttributes theAttribute = (FunctionAttributes)Attribute.GetCustomAttribute(type, typeof(FunctionAttributes));

			if (theAttribute == null)
				return null;

			return theAttribute.Function;
		}
	}
}
