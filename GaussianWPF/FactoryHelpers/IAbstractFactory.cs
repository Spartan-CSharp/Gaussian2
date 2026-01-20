namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Defines a contract for an abstract factory that creates instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object that this factory creates.</typeparam>
/// <remarks>
/// This interface implements the Abstract Factory design pattern, providing a way to encapsulate
/// the creation of objects without specifying their concrete types. It is particularly useful
/// when working with dependency injection containers to resolve factory dependencies.
/// </remarks>
public interface IAbstractFactory<T>
{
	/// <summary>
	/// Creates a new instance of type <typeparamref name="T"/> using the configured factory function.
	/// </summary>
	/// <returns>A new instance of type <typeparamref name="T"/>.</returns>
	T Create();
}
