using CsScripterLib.Functions;
using CsScripterLib.SimpleOperations;
using Microsoft.Practices.Unity;

namespace CsScripterLib
{
	public static class BootStrapper
	{
		static IUnityContainer m_unityContainer;

		private static void Initialize()
		{
			m_unityContainer = new UnityContainer();

			m_unityContainer.RegisterType<IInterpreter, Interpreter>(new ContainerControlledLifetimeManager());
			m_unityContainer.RegisterType<IParserFunction, ParserFunction>(new ContainerControlledLifetimeManager());
			m_unityContainer.RegisterType<IVarManager, VarManager>(new ContainerControlledLifetimeManager());

			m_unityContainer.RegisterType<ISimpleOperation, AddOperation>(ExtensionMethods.GetOperationSymbol(typeof(AddOperation)).ToString());
			m_unityContainer.RegisterType<ISimpleOperation, SubtractOperation>(ExtensionMethods.GetOperationSymbol(typeof(SubtractOperation)).ToString());
			m_unityContainer.RegisterType<ISimpleOperation, MultiplyOperation>(ExtensionMethods.GetOperationSymbol(typeof(MultiplyOperation)).ToString());
			m_unityContainer.RegisterType<ISimpleOperation, DivideOperator>(ExtensionMethods.GetOperationSymbol(typeof(DivideOperator)).ToString());
			m_unityContainer.RegisterType<ISimpleOperation, ModulusOperation>(ExtensionMethods.GetOperationSymbol(typeof(ModulusOperation)).ToString());
			m_unityContainer.RegisterType<ISimpleOperation, EqualsOperator>(ExtensionMethods.GetOperationSymbol(typeof(EqualsOperator)).ToString());

			m_unityContainer.RegisterType<ISimpleOperation, EmptyOperation>(new InjectionConstructor(typeof(IVarManager)));

			m_unityContainer.RegisterType<IFunction, PrintFunction>(ExtensionMethods.GetFunctionSymbol(typeof(PrintFunction)));
		}

		public static IUnityContainer UnityContainer
		{
			get
			{
				if (m_unityContainer == null)
					Initialize();

				return m_unityContainer;
			}
		}
	}
}
