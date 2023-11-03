namespace FlightDocsSystem.Helpers
{
    public class CheckResult
    {
        public bool IsMatched { get; set; }
        public string? Message { get; set; }

        public CheckResult(bool isMatched, string? message)
        {
            IsMatched = isMatched;
            Message = message;
        }
    }
}
