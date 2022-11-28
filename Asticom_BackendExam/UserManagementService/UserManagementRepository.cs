using Asticom_BackendExam.Context;
using Asticom_BackendExam.Models;
using Asticom_BackendExam.Models.DbModel;
using Asticom_BackendExam.Models.Request;
using Asticom_BackendExam.Models.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Asticom_BackendExam.UserManagementService
{
    public class UserManagementRepository: IUserManagementRepository
    {
        private readonly AsticomContext _context;
        private readonly IMapper _mapper;
        public UserManagementRepository(AsticomContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponse> AddUser(AddUserRequest request)
        {
            request.Validate(); //perform validation first

            var userInfo = _mapper.Map<UserInfo>(request);

            //generate password
            userInfo.Password = GenerateToken(10);
            
            //add to database and save
            _context.UserInfo.Add(userInfo);
            await _context.SaveChangesAsync();

            //map response
            return _mapper.Map<UserResponse>(userInfo);

        }

        public async Task<UserResponse?> GetUser(int id)
        {

            var response = await _context.UserInfo.FindAsync(id);

            if (response != null)
            {
                //map response
                return _mapper.Map<UserResponse>(response);
            }
            else { return null; }
        }

        public async Task<string?> DeleteUser(int id)
        {
            var response = await _context.UserInfo.FindAsync(id);

            if (response != null)
            {
                //delete object
                _context.UserInfo.Remove(response);
                _context.SaveChanges();
                return "Deleted Successfully";
            }
            else { return null; }
        }

        public async Task<UserResponse> UpdateUser(AddUserRequest request, int id)
        {
            var response = await _context.UserInfo.FindAsync(id);

            if (response != null)
            {
                request.Validate(); //perform validation
                //bind fields
                response.FirstName = request.FirstName;
                response.LastName = request.LastName;
                response.PhoneNumber = request.PhoneNumber;
                response.Email = request.Email;
                response.PostCode = request.PostCode;
                response.UserName = request.UserName; //has also the power to update username
                response.Address = request.Address;

                _context.SaveChanges();

                //map response
                return _mapper.Map<UserResponse>(response);
            }
            else { throw new KeyNotFoundException("User does not exist"); } //throw an error
        }

        public UserListResponse GetAllUsers(PaginationRequest request)
        {
            var queryable = _context.UserInfo.AsQueryable();

            var totalItemsCount = queryable.Count();

            var response = new UserListResponse();

            if(totalItemsCount > 0)
            {
                response.PageNumber = request.PageNumber;
                response.PageSize = request.PageSize;
                response.TotalItems = totalItemsCount;
            }

            var userList = queryable
                .Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            //map
            var userListModel = _mapper.Map<List<UserModel>>(userList);
            response.Users = new List<UserModel>(userListModel);
            return response;   
        }

        public async Task<List<int>?> DeleteListOfUsers(List<int> id)
        {
            var listOfUsers = await _context.UserInfo
                .Where(x => id.Contains(x.Id))
                .ToListAsync();

            var successfullyDeleteIds = new List<int>();
            if (listOfUsers.Count > 0)
            {
                //delete objects
                _context.UserInfo.RemoveRange(listOfUsers);
                successfullyDeleteIds.AddRange(listOfUsers.Select(x => x.Id).ToList());
                _context.SaveChanges();
                return successfullyDeleteIds;
            }
            else { return null; }
        }
        #region PrivateMethods
        private string GenerateToken(int length)
        {
            using (var cryptRNG = RandomNumberGenerator.Create())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }
        #endregion
    }
}
