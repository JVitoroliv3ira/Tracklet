namespace Tracklet.Domain.Entities;

public class Visitors
{
    public string Id { get; set; }
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
    public DateTime CreatedAt { get; set; }
}