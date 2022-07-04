namespace Security.Core.Models.UserManagement.Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message, string emailAddress) : base(message)
    {
        EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }
}
