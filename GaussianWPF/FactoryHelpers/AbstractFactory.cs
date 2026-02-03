using Microsoft.Extensions.Logging;

namespace GaussianWPF.FactoryHelpers;

/// <summary>
/// Provides a factory implementation for creating instances of type <typeparamref name="T"/> using dependency injection.
/// </summary>
/// <typeparam name="T">The type of object to create.</typeparam>
/// <param name="factory">The factory function used to create instances.</param>
/// <param name="logger">The logger instance for logging factory operations.</param>
public class AbstractFactory<T>(Func<T> factory, ILogger<AbstractFactory<T>> logger) : IAbstractFactory<T>
{
	private readonly Func<T> _factory = factory;
	private readonly ILogger<AbstractFactory<T>> _logger = logger;

	/// <inheritdoc/>
	public T Create()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class}<{Type}> {Method} called.", nameof(AbstractFactory<>), typeof(T).Name, nameof(Create));
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class}<{Type}> {Method} returning {FactoryType}<{Type}> {Factory}", nameof(AbstractFactory<>), typeof(T).Name, nameof(Create), nameof(Func<>), typeof(T).Name, _factory);
		}

		return _factory();
	}
}
