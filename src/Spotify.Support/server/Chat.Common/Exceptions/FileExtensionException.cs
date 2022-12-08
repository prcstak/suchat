namespace Chat.Common.Exceptions;

public class FileExtensionException : Exception
{
    public FileExtensionException(string extension) : base($"Extensions {extension} is not supported.") { }
}