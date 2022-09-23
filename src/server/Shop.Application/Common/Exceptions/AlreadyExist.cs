namespace Shop.Application.Common.Exceptions;

public class AlreadyExist : Exception
{
    public AlreadyExist(string name, object key) 
        : base($"{name} with {key} already exists") { }
}