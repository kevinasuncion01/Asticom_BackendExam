using Microsoft.AspNetCore.Identity;

namespace Asticom_BackendExam.Models.DbModel
{
    public class AdminInfo: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
