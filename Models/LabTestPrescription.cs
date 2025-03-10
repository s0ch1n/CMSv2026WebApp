using System;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class LabTestPrescription
    {
        public int LabTestPrescriptionId { get; set; }
        public int LabTestId { get; set; }

        [Required]
        public string LabTestValue { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
        public int AppointmentId { get; set; }
        public virtual LabTest LabTest { get; set; }
    }
}
