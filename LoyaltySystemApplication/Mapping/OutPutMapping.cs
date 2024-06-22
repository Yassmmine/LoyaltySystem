using DentalSystem.Application.Common;
using DentalSystem.Application.Resources;

namespace DentalSystem.Application.Mapping
{
    public static class OutPutMapping
    {
        public static Domain.Dto.OutPutViewFilterDto Convert(this Domain.Entities.OutPut input)
        {
            if (input == null)
                return null;

            return new Domain.Dto.OutPutViewFilterDto()
            {
                Id = input.Id,
                Date = input.Date.Date.ToString("dd/MM/yyyy"),
                Paid = input.Paid,
                Cost = input.Toatal_Cost,
                SuppliarName = input.SuppliarName,
                Type = input.Type

            };
        }

        public static List<Domain.Dto.OutPutViewFilterDto> Convert(this List<Domain.Entities.OutPut> input)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.OutPutViewFilterDto>();
            result.AddRange(input.Select(z => z.Convert()));
            return result;
        }



        public static Domain.Entities.OutPut Convert(this Domain.Dto.OutPutAddEditDto input)
        {
            if (input == null)
                return null;

            return new Domain.Entities.OutPut
            {
                Date = input.Date,
                Toatal_Cost = input.Toatal_Cost,
                Note = input.Note,
                SuppliarName = input.SuppliarName,
                Paid = input.Paid,
                Type = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, input.Type) ,

            };
        }
        public static void Convert(this Domain.Dto.OutPutAddEditDto input, Domain.Entities.OutPut outPut)
        {
            outPut.Date = input.Date;
            outPut.Note = input.Note;
            outPut.Paid = input.Paid;
            outPut.Toatal_Cost = input.Toatal_Cost;
            outPut.SuppliarName = input.SuppliarName;
        }

        public static Domain.Dto.OutPutDto ConvertToAddOrEdit(this Domain.Entities.OutPut input)
        {
            if (input == null)
                return null;
            return new Domain.Dto.OutPutDto()
            {
                Id = input.Id,
                Date = input.Date.Date.ToString("yyyy-MM-dd"),
                Note = input.Note,
                SuppliarName = input.SuppliarName,
                Paid = input.Paid,
                Toatal_Cost = input.Toatal_Cost,
                Type = input.Type

            };
        }
    }
}