using System;

namespace IQF.Framework.DynamicProxy
{
	/// <summary>
	/// 代理工厂
	/// </summary>
	internal interface IDynamicProxyFactory
	{
		/// <summary>
		/// 创建代理
		/// </summary>
		/// <param name="classType">需要代理的类</param>
		/// <param name="constructorArguments">需要代理的类的构造函数参数</param>
		/// <returns></returns>
		object Create(Type classType, params object[] constructorArguments);

		/// <summary>
		/// 创建代理
		/// </summary>
		/// <typeparam name="TClass">需要代理的类</typeparam>
		/// <param name="constructorArguments">需要代理的类的构造函数参数</param>
		/// <returns></returns>
		TClass Create<TClass>(params object[] constructorArguments) where TClass : class;

		/// <summary>
		/// 创建代理，使用IOC容器进行初始化
		/// </summary>
		/// <param name="type">需要代理的类</param>
		/// <param name="serviceProvider">容器</param>
		/// <returns></returns>
		object Create(Type type, IServiceProvider serviceProvider);
	}
}
