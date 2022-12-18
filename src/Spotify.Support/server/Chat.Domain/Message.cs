namespace Chat.Domain;

public class Message
{
    public Guid Id { get; set; }
    
    public string Body { get; set; }
    
    public string Created { get; set; }
    public string Username { get; set; }
    public bool IsFile { get; set; }
    public string Room { get; set; }
}