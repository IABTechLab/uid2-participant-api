using NuGet.Packaging;
using System.ComponentModel.DataAnnotations;
using UID.Participant.Api.Models;

namespace UID.Participant.Api.Validation
{
    public class ValidateClientTypesAttribute : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var participantApiContext = validationContext.GetService<ParticipantApiContext>();
            if (participantApiContext != null)
            {
                if (value is ICollection<ClientType> clientTypes)
                {
                    var dbClientTypes = participantApiContext.ClientTypes.Where(ct => clientTypes.Contains(ct)).ToList();

                    var invalidClientTypes = clientTypes.Except(dbClientTypes);
                    if (invalidClientTypes.Any())
                    {
                        var message = string.Join(", ", invalidClientTypes.Select(ct => ct.ToString()).ToList());
                        return new ValidationResult($"Invalid ClientTypes: {message}");
                    }

                    clientTypes.Clear();
                    clientTypes.AddRange(dbClientTypes);
                }
            }

            return ValidationResult.Success;
        }
    }
}
