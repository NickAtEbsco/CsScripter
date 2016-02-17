namespace CsScripterLib.Results
{
	public class ErrorResult : Result
	{
		public ErrorResult(string errorMessage) : base(double.NaN, errorMessage)
		{
			IsError = true;
		}
	}
}
