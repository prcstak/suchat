namespace Chat.Common.Exceptions;

public class FileNotFoundException : Exception
{
    public FileNotFoundException(string name) : base ($"File with '{name} not found"){ }
}