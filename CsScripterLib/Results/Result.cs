using System.Collections.Generic;

namespace CsScripterLib.Results
{
	public class Result
	{
		public Result(double dRes = double.NaN, string sRes = null, IEnumerable<Result> manyResults = null)
		{
			Value = dRes;
			String = sRes;
			Tuple = manyResults;
		}

		public double Value { get; protected set; }

		public string String { get; protected set; }

		public IEnumerable<Result> Tuple { get; protected set; }

		public bool IsError { get; protected set; }
	}
}
