namespace Asticom_BackendExam.Models.Response
{
    public class UserListResponse:PaginationResponse
    {
        public List<UserModel> Users { get; set; }

    }

    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
