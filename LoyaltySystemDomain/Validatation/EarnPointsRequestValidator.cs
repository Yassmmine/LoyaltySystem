
using FluentValidation;
using LoyaltySystemDomain.Models;

namespace LoyaltySystemDomain.Validatation
{
    public class EarnPointsRequestValidator : AbstractValidator<EarnPointsRequest>
    {
        public EarnPointsRequestValidator()
        {
            RuleFor(x => x.Points).GreaterThan(0);
        }
    }
}
