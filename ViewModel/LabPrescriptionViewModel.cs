using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.ViewModel
{
    public class LabPrescriptionViewModel
    {
        public int PatientId { get; set; } // Patient for whom the lab tests are prescribed
        public int DoctorId { get; set; }  // Doctor issuing the lab tests
        public int AppointmentId { get; set; } // Associated Appointment ID

        [Required]
        public List<LabTestPrescriptionModel> LabTests { get; set; } = new List<LabTestPrescriptionModel>();
    }
    public class LabTestPrescriptionModel
    {
        public int LabTestId { get; set; }  // Lab test selected
        public string LabTestName { get; set; } // Display purpose (optional)

        [Required, MaxLength(100)]
        public string LabTestValue { get; set; } // Example: "Blood Sugar"

        public int AppointmentId { get; set; }
        public string? Remarks { get; set; } // Optional remarks for the lab test
    }
}
