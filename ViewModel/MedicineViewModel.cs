using CMSv2026WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.ViewModel
{
    public class MedicineViewModel
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int MedicineTypeId { get; set; }
        public string? MedicineTypeName { get; set; } // For display
        public int Unit { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StockInHand { get; set; }
        public List<SelectListItem> MedicineTypes { get; set; } // Drop-down items

        public MedicineType MedicineType { get; set; }
    }
}
