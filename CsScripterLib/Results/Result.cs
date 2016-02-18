
namespace CsScripterLib.Results
{
	public class Result
	{
		public Result(double dRes = double.NaN, string sRes = null, string varName = null)
		{
			Value = dRes;
			String = sRes;
			VarName = varName;
		}

		public double Value { get; protected set; }

		public string String { get; protected set; }

		public string VarName { get; protected set; }

		public bool IsError { get; protected set; }
	}
}
