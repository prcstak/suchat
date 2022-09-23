namespace Shop.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name) : base($"{name} with such credentials not found") { }
}