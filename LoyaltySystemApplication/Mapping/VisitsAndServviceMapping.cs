using System;
using System.Collections.Generic;
using System.Linq;


namespace DentalSystem.Application.Mapping
{
    public static class VisitsAndServviceMapping
    {
        public static Domain.Dto.VisitsAndServviceViewFilterDto Convert(this Domain.Entities.VisitsAndServvice input)
        {
            if (input == null)
                return null;

            return new Domain.Dto.VisitsAndServviceViewFilterDto()
            {
			 Id = input.Id,
			 Date = input.Date.Date.ToString("dd/MM/yyyy"),
			 Services = input.Services,
			 PatientId = input.Patient_Id,
                PatientName =input.Patient.Name,
                PatientPhone=input.Patient.Phone,
                Paid=input.Patient.Paid??0,//input.Patient.CostHistories.Sum(a => a.Paid),
                Cost=input.Patient.Toatal_Cost??0,//input.Patient.CostHistories.Sum(a => a.Cost)

            };
        }

        public static List<Domain.Dto.VisitsAndServviceViewFilterDto> Convert(this List<Domain.Entities.VisitsAndServvice> input)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.VisitsAndServviceViewFilterDto>();
            result.AddRange(input.Select(z => z.Convert()));
            return result;
        }

        public static Domain.Dto.VisitsAndServviceViewPatientDto ConvertViewPatient(this Domain.Entities.VisitsAndServvice input)
        {
            if (input == null)
                return null;

            return new Domain.Dto.VisitsAndServviceViewPatientDto()
            {
                Id = input.Id,
                Date = input.Date.Date.ToString("dd/MM/yyyy"),
                Services = input.Services,
            };
        }

        public static List<Domain.Dto.VisitsAndServviceViewPatientDto> ConvertViewPatient(this List<Domain.Entities.VisitsAndServvice> input)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.VisitsAndServviceViewPatientDto>();
            result.AddRange(input.Select(z => z.ConvertViewPatient()));
            return result;
        }
        public static Domain.Entities.VisitsAndServvice Convert(this Domain.Dto.VisitsAndServviceAddEditDto input)
        {
            if (input == null)
                return null;

            return new Domain.Entities.VisitsAndServvice
            {
			 Date = input.Date,
			 Services = input.Services,
			 Patient_Id = input.PatientId,	 
			 Note = input.Note,
             

            };
        }
        public static void Convert(this Domain.Dto.VisitsAndServviceAddEditDto input , Domain.Entities.VisitsAndServvice visits)
        {

                visits.Date = input.Date;
            visits.Services = input.Services;
                visits.Patient_Id = input.PatientId;
                visits.Note = input.Note;

   
        }

        public static Domain.Dto.VisitsAndServviceDto ConvertToAddOrEdit(this Domain.Entities.VisitsAndServvice input)
        {
            if (input == null)
                return null;
            var patient = input.Patient;
            return new Domain.Dto.VisitsAndServviceDto()
            { 
			 Id = input.Id,
			 Date = input.Date.Date.ToString("yyyy-MM-dd"),
			 Services = input.Services,
			 PatientId = input.Patient_Id,
			 Note = input.Note,
             PatientPhone=patient.Phone,
             PatientName=patient.Name,

            };
        }
    }
}