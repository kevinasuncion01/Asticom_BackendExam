using Asticom_BackendExam.Validations;
using FluentValidation;

namespace Asticom_BackendExam.Models.Request
{
    public class AddUserRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public void Validate()
        {
            var request = new AddUserValidator();
            var validationResults = request.Validate(this);
            if (validationResults.Errors.Count > 0)
                throw new ValidationException(validationResults.Errors);
        }
    }
}
