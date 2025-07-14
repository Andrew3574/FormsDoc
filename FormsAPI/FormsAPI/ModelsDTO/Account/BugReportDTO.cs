namespace FormsAPI.ModelsDTO.Account
{
    public class BugReportDTO
    {
        public string ReportedBy { get; set; } = null!;
        public string Issue { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Link { get; set; } = null!;
        public IEnumerable<string> AdminEmails { get; set; } = new List<string>();
    }
}
