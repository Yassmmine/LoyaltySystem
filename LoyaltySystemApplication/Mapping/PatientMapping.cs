using DentalSystem.Domain.Dto;
using DentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DentalSystem.Application.Mapping
{
    public static class PatientMapping
    {

        public static Domain.Dto.GetPatientsFilterDto ConvertToFilter(this Patient input)
        {
            return new Domain.Dto.GetPatientsFilterDto
            {
                //Address = input.Address,
                //Job = input.Job,
                Name = input.Name,
               // BirthDate = input.Birth_Date.HasValue ? input.Birth_Date.Value.Date.ToString("yyyy-MM-dd") : null,
                Phone = input.Phone,
                Id=input.Id
            };
        }
        public static List<Domain.Dto.GetPatientsFilterDto> Convert(this List<Domain.Entities.Patient> input)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.GetPatientsFilterDto>();
            result.AddRange(input.Select(z => z.ConvertToFilter()));
            return result;
        }



        public static Domain.Dto.TeethChartViewDto Convert(this Domain.Entities.TeethChart input)
        {
            if (input == null)
                return null;

            return new Domain.Dto.TeethChartViewDto()
            {
                TeethNum=input.Teeth_Num.Split(',').Select(int.Parse).ToList(),
                Code=input.Code,
                //PatientId=input.Patient_Id,
                Id=input.Id,
        };
        }

        public static List<Domain.Dto.TeethChartViewDto> Convert(this List<Domain.Entities.TeethChart> input)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.TeethChartViewDto>();
            result.AddRange(input.Select(z => z.Convert()));
            return result;
        }

    //    public static Domain.Entities.Patient Convert(this Domain.Dto.PatientViewFilterDto input)
    //    {
    //        if (input == null)
    //            return null;

    //        return new Domain.Entities.Patient
    //        {
			 //Id = input.Id,
			 //Address = input.Address,
			 //Job = input.Job,
			 //Name = input.Name,
			 //Birth_Date = input.BirthDate,
			 //Phone = input.Phone,
			 //Cost = input.ToatalCost??0,
			 //Paid = input.Paid??0,

    //        };
    //    }

        public static Domain.Entities.Patient Convert(this Domain.Dto.PatientAddDto input)
        {
            if (input == null)
                return null;

            return new Domain.Entities.Patient
            {
                Address = input.Address,
                Job = input.Job,
                Name = input.Name,
                Birth_Date = input.BirthDate,
                Phone = input.Phone,

            };
        } 
        public static Domain.Entities.CostHistory Convert(this Domain.Dto.AddCostDto input)
        {
            if (input == null)
                return null;

            return new Domain.Entities.CostHistory
            {
                Patient_Id=input.Id,
                Paid=input.Paid,
                Cost=input.Cost
            };
        }  
        public static Domain.Dto.GetPatientDto Convert(this Patient input)
        {
            return new Domain.Dto.GetPatientDto
            {
                Address = input.Address,
                Job = input.Job,
                Name = input.Name,
                BirthDate = input.Birth_Date.HasValue?input.Birth_Date.Value.Date.ToString("yyyy-MM-dd"):null,
                CreatedDate = input.Create_Date.Date.ToString("yyyy-MM-dd"),
                Phone = input.Phone,
                Cost=input.Toatal_Cost??0,//input.CostHistories.Sum(a=>a.Cost),
                Paid=input.Paid??0,//input.CostHistories.Sum(a => a.Paid),
            };
        }

        public static Domain.Entities.TeethChart Convert(this Domain.Dto.TeethChartAddEditDto input)
        {
            return new Domain.Entities.TeethChart
            {
                Code=input.Code,
                Patient_Id=input.PatientId,
                Teeth_Num=String.Join(",", input.TeethNum),
            };
        }
        public static void Convert(this Domain.Entities.TeethChart teethChart, Domain.Dto.TeethChartAddEditDto input)
        {
            teethChart.Teeth_Num = String.Join(",", input.TeethNum);
        }
    }
}