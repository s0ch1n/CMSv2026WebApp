using CMSv2026WebApp.Models;
using System.Collections.Generic;

namespace CMSv2026WebApp.Services
{
    public interface ILabTestPrescriptionService
    {
        IEnumerable<LabTestPrescription> GetAllPrescriptions();
        LabTestPrescription GetPrescriptionById(int prescriptionId);
    }
}
