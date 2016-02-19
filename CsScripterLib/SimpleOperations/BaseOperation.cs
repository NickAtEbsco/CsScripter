namespace CsScripterLib.SimpleOperations
{
	/// <summary>
	/// Base class for re-using code.
	/// </summary>
	public abstract class BaseOperation : ISimpleOperation
	{
		protected IVarManager m_varManager;
		int m_iPriority;

		object m_value;

		protected BaseOperation(IVarManager varManager)
		{
			m_varManager = varManager;
			VarName = null;
		}

		public void StoreValue(object value)
		{
			var d = value as double?;
			if (d != null)
				Value = d;

			var s = value as string;
			if (s != null)
				String = s;

			var b = value as bool?;
			if (b != null)
				Boolean = b;
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

		public double? Value
		{
			get
			{
				// If its a variable, we need to get the variable's value instead of the direct value
				if (string.IsNullOrEmpty(VarName))
					return m_value as double?;

				return m_varManager.GetVar(VarName) as double?;
			}
			protected set
			{
				// If its a variable, we need to get the variable's value instead of the direct value
				if (string.IsNullOrEmpty(VarName))
				{
					m_value = value;
					return;
				}

				m_varManager.UpdateOrCreateVar(VarName, value);
			}
		}

		public string String
		{
			get
			{
				if (string.IsNullOrEmpty(VarName))
					return m_value as string;

				return m_varManager.GetVar(VarName) as string;
			}
			protected set
			{
				// If its a variable, we need to get the variable's value instead.
				if (string.IsNullOrEmpty(VarName))
				{
					m_value = value;
					return;
				}

				m_varManager.UpdateOrCreateVar(VarName, value);
			}
		}

		public bool? Boolean
		{
			get
			{
				if (string.IsNullOrEmpty(VarName))
					return m_value as bool?;

				return m_varManager.GetVar(VarName) as bool?;
			}
			protected set
			{
				// If its a variable, we need to get the variable's value instead.
				if (string.IsNullOrEmpty(VarName))
				{
					m_value = value;
					return;
				}

				m_varManager.UpdateOrCreateVar(VarName, value);
			}
		}

		public object Object
		{
			get
			{
				var dbl = Value;
				if (dbl.HasValue)
					return dbl;

				var str = String;
				if (str != null)
					return str;

				var b = Boolean;
				if (b.HasValue)
					return b;

				return null;
			}
		}

		public string VarName { get; protected set; }
	}
}
