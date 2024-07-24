using Microsoft.AspNetCore.Mvc;
using CustomerFileProcessing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CustomerFileProcessing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private static List<FileUpload> fileUploads = new List<FileUpload>();
        private static Queue<FileUpload> processingQueue = new Queue<FileUpload>();

        // ... Previous actions ...

        [HttpPost("upload")]
        public IActionResult UploadFiles([FromForm] FileUploadRequest request)
        {
            fileUploads = new List<FileUpload>();
            foreach (var file in request.Files)
            {
                var fileUpload = new FileUpload
                {
                    CustomerId = request.CustomerId,
                    FileName = file.FileName,
                    UploadTime = DateTime.Now,
                    ProcessingStatus = "Pending",
                    SLA = request.SLA
                };

                fileUploads.Add(fileUpload);
                processingQueue.Enqueue(fileUpload);
            }

            ProcessFiles();

            return Ok(fileUploads);
        }

        [HttpGet("list")]
        public IActionResult GetFileList()
        {
            return Ok(fileUploads);
        }


        private void ProcessFiles()
        {
            while (processingQueue.Count > 0)
            {
                var fileUpload = processingQueue.Dequeue();
                fileUpload.ProcessingStatus = "Processing";

                // Note: Simulate processing time based on SLA
                // Currently set to 2,4 and 6 seconds to make it testable
                // But need to SET 30,60 and 1440 minutes in production
                var processingTime = fileUpload.SLA == (int)SLA.SLA1 ? 2 : (fileUpload.SLA == (int)SLA.SLA1 ? 4 : 6); // 6 seconds for SLA 3
                Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();

                fileUpload.ProcessingStatus = "Processed";
            }
        }
    }

    public class FileUploadRequest
    {
        public int CustomerId { get; set; }
        public int SLA { get; set; }
        public List<IFormFile>  Files { get; set; }
    }

    public enum SLA 
    {
        SLA1 = 1,
        SLA2 = 2,
        SLA3 = 3
    }


    
}
