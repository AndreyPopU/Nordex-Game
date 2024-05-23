using Unity.Services.Analytics;

public class PuzzleCompletedEvent : Event
{
    public PuzzleCompletedEvent() : base("PuzzleCompleted")
    {
    }

    public string PuzzleName { set { SetParameter("puzzleName", value); } }
    public float Duration { set { SetParameter("duration", value); } }
}