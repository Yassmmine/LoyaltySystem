using DentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DentalSystem.Application.Mapping
{
    public static class PatientsMedicalMapping
    {
        public static Domain.Dto.PatientsMedicalViewFilterDto Convert(this Domain.Entities.PatientsMedical input)
        {
            if (input == null)
                return null;

            return new Domain.Dto.PatientsMedicalViewFilterDto()
            {
			 MedicalHistory = input.MedicalHistory,
			 Complaint = input.Complaint,
			 TreatmentPlan = input.TreatmentPlan,
			 MedicationSideEffects = input.Medication_Side_Effects,
			 ComplicationsWithDentalAnesthetic = input.Complications_With_Dental_Anesthetic,
			 Chronicdiseases = input.Chronic_diseases,
            };
        }


        public static Domain.Entities.PatientsMedical Convert(this Domain.Dto.PatientsMedicalAddEditDto input)
        {
            if (input == null)
                return null;

            return new Domain.Entities.PatientsMedical
            {
			 MedicalHistory = input.MedicalHistory,
			 Complaint = input.Complaint,
			 TreatmentPlan = input.TreatmentPlan,
			 Medication_Side_Effects = input.MedicationSideEffects,
			 Complications_With_Dental_Anesthetic = input.ComplicationsWithDentalAnesthetic,
			 Chronic_diseases = input.Chronicdiseases,
			 Id = input.Id,

            };
        }
        public static void Convert(this Domain.Dto.PatientsMedicalAddEditDto input, PatientsMedical patientsMedical)
        {

            patientsMedical.MedicalHistory = input.MedicalHistory;

            patientsMedical.Complaint = input.Complaint;
                patientsMedical.TreatmentPlan = input.TreatmentPlan;
                patientsMedical.Medication_Side_Effects = input.MedicationSideEffects;
                patientsMedical.Complications_With_Dental_Anesthetic = input.ComplicationsWithDentalAnesthetic;
                patientsMedical.Chronic_diseases = input.Chronicdiseases;

            
        }

    }
}