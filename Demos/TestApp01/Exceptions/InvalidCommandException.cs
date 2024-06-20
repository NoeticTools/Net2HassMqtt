namespace NoeticTools.TestApp01.Exceptions;

internal class InvalidCommandException : Exception
{
    public InvalidCommandException(string message) : base(message)
    {
    }
}