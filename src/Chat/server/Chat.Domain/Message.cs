namespace Chat.Domain;

public class Message
{
    public Guid Id { get; set; }
    
    public string Body { get; set; }
    
    public DateTime Created { get; set; }
    public string Username { get; set; }
}