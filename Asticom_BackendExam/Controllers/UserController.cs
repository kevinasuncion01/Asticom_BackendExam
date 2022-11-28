using Asticom_BackendExam.Models;
using Asticom_BackendExam.Models.Request;
using Asticom_BackendExam.UserManagementService;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asticom_BackendExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementRepository _userRepo;
        public UserController(IUserManagementRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUserRequest request)
        {
            try
            {
                var userInfo = await _userRepo.AddUser(request);
                return Created($"/api/user/{userInfo.Id}", userInfo);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(x => x.ErrorMessage).ToList());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int id) 
        {
            var userInfo = await _userRepo.GetUser(id);
            if (userInfo == null) return NotFound("No user found");
            return Ok(userInfo);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var resMsg = await _userRepo.DeleteUser(id);
                if (resMsg == null) return BadRequest("No user found");
                return Ok(resMsg);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AddUserRequest request, [FromQuery] int id)
        {
            try
            {
                var userInfo = await _userRepo.UpdateUser(request, id);
                return Ok(userInfo);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(x => x.ErrorMessage).ToList());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    
        [HttpGet("list")]
        public IActionResult List([FromQuery]PaginationRequest request)
        {
            try
            {
                return Ok(_userRepo.GetAllUsers(request));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("list")]
        public async Task<IActionResult> List([FromBody] List<int> listOfId)
        {
            var userInfo = await _userRepo.DeleteListOfUsers(listOfId);
            if (userInfo == null) return NotFound("No valid user on the list to be deleted");
            var msg = $"Successfully deleted the list of id's: {string.Join(", ", userInfo)}";
            return Ok(msg);
        }
    }
}
