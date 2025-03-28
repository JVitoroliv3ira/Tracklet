namespace Tracklet.Domain.Entities;

public class Session
{
    public string Id { get; set; }
    public string VisitorId { get; set; }
    public string DeviceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public Visitor Visitor { get; set; }
    public Device Device { get; set; }
}