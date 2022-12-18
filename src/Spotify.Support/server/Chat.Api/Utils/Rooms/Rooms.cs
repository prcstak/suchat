using System.Collections.Concurrent;

namespace Chat.Api.Utils.Rooms;

public class Rooms
{
    private readonly ConcurrentDictionary<string, UserConnection> _rooms;

    public Rooms()
    {
        _rooms = new ConcurrentDictionary<string, UserConnection>();
    }

    public void Join(string username, string connectionId, bool isAdmin)
    {
        _rooms.TryAdd(connectionId, new UserConnection(username, RoomStatus.Waiting, isAdmin));
    }

    public KeyValuePair<string, UserConnection> Leave(string connectionId)
    {
        var toRemove = _rooms.First(room => room.Key == connectionId);
        _rooms.TryRemove(toRemove);

        return toRemove;
    }

    public void Close(string connectionId)
    {
        var room = _rooms.First(room => room.Key == connectionId);
        room.Value.Status = RoomStatus.Closed;
    }

    public List<string> GetAllWaiting() => 
        _rooms
            .Where(room => room.Value.Status == RoomStatus.Waiting)
            .Select(room => room.Value.Username)
            .ToList();
}