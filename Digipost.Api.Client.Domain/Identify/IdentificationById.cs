using System;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identify
{
    public class IdentificationById : IIdentification
    {
        public IdentificationById(IdentificationType identificationType, string value)
        {
            IdentificationType = identificationType;
            Value = value;
        }

        public IdentificationType IdentificationType { get; private set; }

        public object Data
        {
            get { return Value; }
        }

        [Obsolete("Use IdentificationType instead. Will be removed in future versions" )]
        public IdentificationChoiceType IdentificationChoiceType {
            get
            {
                return ParseIdentificationChoiceToIdentificationChoiceType();
            } 
        }

        internal IdentificationChoiceType ParseIdentificationChoiceToIdentificationChoiceType()
        {
            IdentificationChoiceType identificationChoiceType;

            if (IdentificationType == IdentificationType.OrganizationNumber)
            {
                identificationChoiceType = OrganizationEnumWithSpellingDifference();
            }
            else
            {
                identificationChoiceType = (IdentificationChoiceType)
                Enum.Parse(typeof(IdentificationChoiceType), IdentificationType.ToString(), ignoreCase: true);    
            }
            
            return identificationChoiceType;
        }

        internal IdentificationChoiceType OrganizationEnumWithSpellingDifference()
        {
            return IdentificationChoiceType.OrganisationNumber;
        }

        public string Value { get; set; }
    }
}
