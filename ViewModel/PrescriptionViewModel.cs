using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace CMSv2026WebApp.ViewModel
{
    public class PrescriptionViewModel
    {
        public int PatientId { get; set; } // Patient for whom the prescription is given
        public int DoctorId { get; set; }  // Doctor issuing the prescription
        public int AppointmentId { get; set; } // Associated Appointment ID

        [Required]
        public List<MedicinePrescriptionModel> Prescriptions { get; set; } = new List<MedicinePrescriptionModel>();
    }

    public class MedicinePrescriptionModel
    {
        public int MedicineId { get; set; }  // Medicine selected
        public string MedicineName { get; set; } // Display purpose (optional)

        [Required, MaxLength(50)]
        public string Dosage { get; set; } // Example: "500mg"

        [Required, MaxLength(50)]
        public string Frequency { get; set; } // Example: "Twice a day"

        [Required, MaxLength(50)]
        public string Duration { get; set; } // Example: "7 days"
    }
}
