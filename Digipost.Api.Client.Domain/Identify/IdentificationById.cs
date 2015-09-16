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
            if (IdentificationType == IdentificationType.OrganizationNumber)
            {
                return IdentificationChoiceType.OrganisationNumber;
            }

            return (IdentificationChoiceType)
                Enum.Parse(typeof (IdentificationChoiceType), IdentificationType.ToString(), ignoreCase: true);
        }

        public string Value { get; set; }
    }
}
