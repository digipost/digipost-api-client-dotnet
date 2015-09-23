using System;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Extensions
{
    public static class IdentificationTypeExtensions
    {
        internal static IdentificationChoiceType ToIdentificationChoiceType(this IdentificationType identificationType)
        {
            var identificationChoiceType = 
                identificationType == IdentificationType.OrganizationNumber 
                ? OrganizationEnumWithSpellingDifference() 
                : ParseEnumIgnoreCasing(identificationType);

            return identificationChoiceType;
        }

        private static IdentificationChoiceType ParseEnumIgnoreCasing(IdentificationType identificationType)
        {
            return (IdentificationChoiceType) Enum.Parse(typeof(IdentificationChoiceType), identificationType.ToString(), ignoreCase: true);
        }

        internal static IdentificationChoiceType OrganizationEnumWithSpellingDifference()
        {
            return IdentificationChoiceType.OrganisationNumber;
        }

    }
}
