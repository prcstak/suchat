namespace Chat.Domain;

public class Message
{
    public Guid Id { get; set; }
    
    public string Body { get; set; }
    
    public DateTime Created { get; set; }
    
    public User User { get; set; }
    public string UserId { get; set; }
}