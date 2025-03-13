using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.ViewModel
{
    public class StaffViewModel
    {
        public Staff Staff { get; set; } = new Staff();

        public int? SpecializationId { get; set; }  // Nullable for non-doctors
        public decimal? ConsultationFee { get; set; } // Nullable for non-doctors

        public IEnumerable<Specialization> Specializations { get; set; } = new List<Specialization>();

        public bool IsDoctor { get; set; } // Checkbox to determine if staff is a doctor
    }
}
