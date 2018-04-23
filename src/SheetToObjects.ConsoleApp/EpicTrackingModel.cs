namespace SheetToObjects.ConsoleApp
{
    public class EpicTrackingModel
    {
        public int? SprintNumber { get; set; }
        public string SprintName { get; set; }
        public double? StoryPointsCompleted { get; set; }
        public double? TotalCompleted { get; set; }
        public double? ForecastNormal { get; set; }
        public double? ForecastHigh { get; set; }
        public double? ForecastLow { get; set; }
        public double? Scope { get; set; }
    }
}
