namespace Chat.Api.Utils.Rooms;

public class UserConnection
{
    public string Username { get; init; }
    public RoomStatus Status { get; set; }
    public bool IsAdmin { get; init; }

    public UserConnection(string username, RoomStatus status, bool isAdmin)
    {
        Username = username;
        Status = status;
        IsAdmin = isAdmin;
    }
}