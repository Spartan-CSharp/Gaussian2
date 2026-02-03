namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Defines the contract for a factory that creates instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to create.</typeparam>
public interface IAbstractFactory<T>
{
	/// <summary>
	/// Creates a new instance of type <typeparamref name="T"/>.
	/// </summary>
	/// <returns>A new instance of <typeparamref name="T"/>.</returns>
	T Create();
}
