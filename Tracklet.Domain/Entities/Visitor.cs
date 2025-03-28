namespace Tracklet.Domain.Entities;

public class Visitor
{
    public string Id { get; set; }
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
    public DateTime CreatedAt { get; set; }
}