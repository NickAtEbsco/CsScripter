
namespace CsScripterLib.Results
{
	public class Result
	{
		object m_value;

		public Result(object dRes = null, string varName = null)
		{
			m_value = dRes;
			VarName = varName;
		}

		public double? Value
		{
			get
			{
				return m_value as double?;
			}
			protected set
			{
				m_value = value;
			}
		}

		public string String
		{
			get
			{
				return m_value as string;
			}
			protected set
			{
				m_value = value;
			}
		}

		public bool? Boolean
		{
			get
			{
				return m_value as bool?;
			}
		}

		public object Object { get { return m_value; } }

		public string VarName { get; protected set; }

		public bool IsError { get; protected set; }
	}
}
