using CMSv2026WebApp.Models;
namespace CMSv2026WebApp.ViewModel
{
    public class DispatchMedicineViewModel
    {

        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int AvailableStock { get; set; }
        public int DispatchQuantity { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }

        //public virtual Medicine Medicine { get; set; }
        //public virtual MedicinePrescription MedicinePrescription { get; set; }
        public virtual Appointment Appointment { get; set; }
        //public virtual Doctor Doctor { get; set; }
        //public virtual Staff Staff { get; set; }
        public virtual Patient Patient { get; set; }

    }
}
