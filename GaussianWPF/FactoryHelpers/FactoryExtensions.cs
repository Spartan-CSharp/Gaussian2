using Microsoft.Extensions.DependencyInjection;

namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Provides extension methods for configuring dependency injection services with factory patterns.
/// </summary>
public static class FactoryExtensions
{
	/// <summary>
	/// Registers a form or window type with the dependency injection container along with factory services
	/// to enable dynamic creation of instances.
	/// </summary>
	/// <typeparam name="TForm">The type of form or window to register. Must be a class.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
	/// <remarks>
	/// This method registers three services:
	/// <list type="bullet">
	/// <item><description>The form type itself as a transient service.</description></item>
	/// <item><description>A <see cref="Func{TForm}"/> factory delegate as a singleton that creates new instances.</description></item>
	/// <item><description>An <see cref="IAbstractFactory{TForm}"/> implementation as a singleton for abstracted instance creation.</description></item>
	/// </list>
	/// The factory will throw an <see cref="InvalidOperationException"/> if the form cannot be instantiated.
	/// </remarks>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the factory is unable to create an instance of <typeparamref name="TForm"/>.
	/// </exception>
	public static IServiceCollection AddFormFactory<TForm>(this IServiceCollection services) where TForm : class
	{
		_ = services.AddTransient<TForm>();
		_ = services.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>() ?? throw new InvalidOperationException($"Could not create instance of {typeof(TForm).Name}"));
		_ = services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();

		return services;
	}
}
