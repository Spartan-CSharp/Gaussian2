using Microsoft.Extensions.DependencyInjection;

namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Provides extension methods for registering abstract factories in the dependency injection container.
/// </summary>
public static class FactoryExtensions
{
	/// <summary>
	/// Registers a transient form instance and a factory for creating instances of <typeparamref name="TForm"/>.
	/// </summary>
	/// <typeparam name="TForm">The type of form to register. Must be a class.</typeparam>
	/// <param name="services">The service collection to register the factory with.</param>
	/// <returns>The service collection for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown when an instance of <typeparamref name="TForm"/> cannot be created from the service provider.</exception>
	/// <remarks>
	/// This method registers three components:
	/// <list type="number">
	/// <item>A transient registration for <typeparamref name="TForm"/></item>
	/// <item>A singleton factory function for creating instances</item>
	/// <item>A singleton <see cref="IAbstractFactory{T}"/> implementation</item>
	/// </list>
	/// </remarks>
	public static IServiceCollection AddFormFactory<TForm>(this IServiceCollection services) where TForm : class
	{
		_ = services.AddTransient<TForm>();
		_ = services.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>() ?? throw new InvalidOperationException($"Could not create instance of {typeof(TForm).Name}"));
		_ = services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();

		return services;
	}
}
