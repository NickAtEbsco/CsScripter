namespace CsScripterLib.SimpleOperations
{
	/// <summary>
	/// Base class for re-using code.
	/// </summary>
	public abstract class BaseOperation : ISimpleOperation
	{
		protected IVarManager m_varManager;
		int m_iPriority;
		double m_value;
		string m_string;

		protected BaseOperation(IVarManager varManager)
		{
			m_varManager = varManager;
			VarName = null;
		}

		public void StoreValue(double value)
		{
			Value = value;
		}

		public void StoreString(string str)
		{
			String = str;
		}

		public void StoreVariable(string varName)
		{
			VarName = varName;
		}

		public virtual ISimpleOperation Evaluate(ISimpleOperation next)
		{
			Priority = next.Priority;

			return next;
		}

		public int Priority { get { return m_iPriority; } protected set { m_iPriority = value; } }

		public double Value
		{
			get
			{
				// If its a variable, we need to get the variable's value instead of the direct value
				if (string.IsNullOrEmpty(VarName))
					return m_value;

				var variable = m_varManager.GetVar(VarName);
				if (variable is double)
					return (double)variable;

				return double.NaN;
			}
			protected set
			{
				// If its a variable, we need to get the variable's value instead of the direct value
				if (string.IsNullOrEmpty(VarName))
				{
					m_value = value;
					return;
				}

				m_varManager.UpdateOrCreateVar(VarName, value, null);
			}
		}

		public string String
		{
			get
			{
				// If its a variable, we need to get the variable's value instead.
				if (string.IsNullOrEmpty(VarName))
					return m_string;

				var variable = m_varManager.GetVar(VarName);
				if (variable is string)
					return (string)variable;

				return null;
			}
			protected set
			{
				// If its a variable, we need to get the variable's value instead.
				if (string.IsNullOrEmpty(VarName))
				{
					m_string = value;
					return;
				}

				m_varManager.UpdateOrCreateVar(VarName, double.NaN, value);
			}
		}

		public string VarName { get; protected set; }
	}
}
