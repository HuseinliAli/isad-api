namespace Core.Domain.Exceptions;

public class PermissionDeniedException : Exception
{
    public PermissionDeniedException(string message) : base(message) { }
}
