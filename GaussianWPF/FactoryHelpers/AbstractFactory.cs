using Microsoft.Extensions.Logging;

namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Provides a generic implementation of the Abstract Factory pattern with logging support.
/// This class wraps a factory function to create instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to be created by the factory.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="AbstractFactory{T}"/> class.
/// </remarks>
/// <param name="factory">The factory function that creates instances of type <typeparamref name="T"/>.</param>
/// <param name="logger">The logger instance for logging factory operations.</param>
public class AbstractFactory<T>(Func<T> factory, ILogger<AbstractFactory<T>> logger) : IAbstractFactory<T>
{
	/// <summary>
	/// The factory function used to create instances of type <typeparamref name="T"/>.
	/// </summary>
	private readonly Func<T> _factory = factory;
	
	/// <summary>
	/// The logger instance used to log factory creation operations.
	/// </summary>
	private readonly ILogger<AbstractFactory<T>> _logger = logger;

	/// <summary>
	/// Creates a new instance of type <typeparamref name="T"/> using the configured factory function.
	/// </summary>
	/// <returns>A new instance of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// This method logs a trace-level message when called, if trace logging is enabled.
	/// </remarks>
	public T Create()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} of {Factory}<{Type}> Called", nameof(Create), nameof(AbstractFactory<>), typeof(T).Name);
		}

		return _factory();
	}
}
