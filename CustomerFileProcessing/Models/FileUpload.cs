namespace CustomerFileProcessing.Models
{
    public class FileUpload
    {
        public int CustomerId { get; set; }
        public string FileName { get; set; }
        public DateTime UploadTime { get; set; }
        public string ProcessingStatus { get; set; }
        public int SLA { get; set; }
    }
}
