using Asticom_BackendExam.Models;
using Asticom_BackendExam.Models.DbModel;
using Asticom_BackendExam.Models.Request;
using Asticom_BackendExam.Models.Response;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Asticom_BackendExam.UserManagementService
{
    public interface IUserManagementRepository
    {
        /// <summary>
        /// Creates a new user on the database
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>The message if successfully added</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ValidationException"></exception>
        Task<UserResponse> AddUser(AddUserRequest request);

        /// <summary>
        /// Deletes list of users
        /// </summary>
        /// <param name="id">The id of the user to be deleted</param>
        /// <returns>The id that was successfully deleted</returns>
        Task<List<int>?> DeleteListOfUsers(List<int> id);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The message if the operation failed or succeed</returns>
        Task<string?> DeleteUser(int id);

        /// <summary>
        /// Gets all the user
        /// </summary>
        /// <param name="request">The Pagination Request</param>
        /// <returns>The paginated list of users</returns>
        UserListResponse GetAllUsers(PaginationRequest request);

        /// <summary>
        /// Get the user details
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The user object or null if no entry found</returns>
        Task<UserResponse?> GetUser(int id);

        /// <summary>
        /// Updates the user info, not including the 'Password'
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns>The updated user object</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<UserResponse> UpdateUser(AddUserRequest request, int id);
    }
}
