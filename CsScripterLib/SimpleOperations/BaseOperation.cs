namespace CsScripterLib.SimpleOperations
{
	/// <summary>
	/// Base class for re-using code.
	/// </summary>
	public abstract class BaseOperation : ISimpleOperation
	{
		int m_iPriority;

		public void StoreValue(double value)
		{
			Value = value;
		}

		public void StoreString(string str)
		{
			String = str;
		}

		public virtual ISimpleOperation Evaluate(ISimpleOperation next)
		{
			Priority = next.Priority;

			return next;
		}

		public int Priority { get { return m_iPriority; } protected set { m_iPriority = value; } }
		public double Value { get; protected set; }
		public string String { get; protected set; }
	}
}
