using System.Collections.Generic;

namespace CsScripterLib
{
	public class VarManager : IVarManager
	{
		readonly Dictionary<string, object> m_variables = new Dictionary<string, object>(); 

		public void UpdateOrCreateVar(string name, double value, string str)
		{
			if (double.IsNaN(value))
				m_variables[name] = str;
			else
				m_variables[name] = value;
		}

		public object GetVar(string name)
		{
			if (m_variables.ContainsKey(name))
				return m_variables[name];

			return null;
		}

		public void Clear()
		{
			m_variables.Clear();
		}
	}
}
