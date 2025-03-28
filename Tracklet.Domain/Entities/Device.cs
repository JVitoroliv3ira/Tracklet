namespace Tracklet.Domain.Entities;

public class Device
{
    public string Id { get; set; }
    public string UserAgent { get; set; }
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public string OperatingSystem {get; set;}
    public DateTime CreatedAt { get; set; }
}