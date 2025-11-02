namespace DispatcherApp.Common.Exceptions;

public class InvalidCredentialsException : UnauthorizedAccessException, IAlreadyLoggedException
{
    public InvalidCredentialsException() : base("Invalid credentials provided.") { }

    public InvalidCredentialsException(string message) : base(message) { }

    public InvalidCredentialsException(string message, Exception innerException) : base(message, innerException) { }
}
