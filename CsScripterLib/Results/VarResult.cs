namespace CsScripterLib.Results
{
	public class VarResult : Result
	{
		public VarResult(string varName, double dRes = double.NaN, string sRes = null)
			: base(dRes, sRes, null)
		{
			VarName = varName;
		}

		public string VarName { get; private set; }
	}
}
