using internship.Context;
using internship.ModelView.User;
using internship.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using internship.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using WebApplication1.DTO.User;

namespace internship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public userController(ApplicationDbContext context,     
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _context = context;        
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("get/all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAccount([FromQuery] PaginationDTO pagination)
        {
            // Perform the search query
            var usersQuery = _context.Users.AsQueryable();

            // Get the total count before applying pagination
            var totalCount = await usersQuery.CountAsync();

            // Apply pagination
            if (pagination.Page.HasValue && pagination.Page.Value > 0 && pagination.RowOfPage.HasValue && pagination.RowOfPage.Value > 0)
            {
                var skip = (pagination.Page.Value - 1) * pagination.RowOfPage.Value;
                usersQuery = usersQuery.Skip(skip).Take(pagination.RowOfPage.Value);
            }

            // Fetch the results
            var users = await usersQuery.ToListAsync();

            // Check if any users were found
            if (users == null || users.Count < 1)
            {
                return Ok(new ApiResult<object>(200, "Result not found.", new { Pagination = pagination, TotalRow = 0, Results = new object[] { } }));
            }

            // Map the results to DTOs
            var userDTOs = _mapper.Map<List<UserDTO>>(users);

            // Return success response with search results and total count
            return Ok(new ApiResult<object>(200, "Search completed successfully.", new { Pagination = pagination, TotalRow = totalCount, Results = userDTOs }));
        }


        [HttpGet("get/one/{id}")]
        public async Task<IActionResult> GetInfo(string id)
        {
            Guid.TryParse(id, out Guid convertId);
            // Find the user by id
            var user = await _context.Users.FindAsync(convertId);
            if (user == null)
            {
                return NotFound(new ApiResult<object>(404, "User not found.", null));
            }

            // Return success response
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(new ApiResult<object>(200, "Get user infomation successfully.", userDTO));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchUserDTO searchParams)
        {
            if (searchParams == null ||
                (string.IsNullOrEmpty(searchParams.Name) &&
                 string.IsNullOrEmpty(searchParams.Email) &&
                 string.IsNullOrEmpty(searchParams.PhoneNumber) &&
                 string.IsNullOrEmpty(searchParams.Address)))
            {
                return BadRequest(new ApiResult<object>(404, "Search term at least one value",
                    new { SearchTerm = searchParams, Results = new object[] { } }));
            }
            // Perform the search query
            var usersQuery = _context.Users.AsQueryable();

            // Apply search criteria
            if (!string.IsNullOrEmpty(searchParams.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(searchParams.Name));

            if (!string.IsNullOrEmpty(searchParams.Email))
                usersQuery = usersQuery.Where(u => u.Email.Contains(searchParams.Email));

            if (!string.IsNullOrEmpty(searchParams.PhoneNumber))
                usersQuery = usersQuery.Where(u => u.PhoneNumber.Contains(searchParams.PhoneNumber));

            if (!string.IsNullOrEmpty(searchParams.Address))
                usersQuery = usersQuery.Where(u => u.Address.Contains(searchParams.Address));

            // Get the total count before applying fetch and limit
            var totalCount = await usersQuery.CountAsync();

            // Fetch a specific number of results if fetch is specified
            if (searchParams.Page.HasValue && searchParams.Page.Value > 0)
            {
                int rowOfPage = searchParams.RowOfPage ?? 10;
                var skip = (searchParams.Page.Value - 1) * rowOfPage;
                usersQuery = usersQuery.Skip(skip);
            }

            // Limit the number of results if limit is specified
            if (searchParams.RowOfPage.HasValue && searchParams.RowOfPage.Value > 0)
                usersQuery = usersQuery.Take(searchParams.RowOfPage.Value);

            var users = await usersQuery.ToListAsync();

            if (users == null || users.Count < 1)
                return Ok(new ApiResult<object>(204, "Result not find.",
                    new { SearchParams = searchParams, TotalRow = 0, Results = new object[] { } }));


            // Map the results to DTOs
            var userDTOs = _mapper.Map<List<UserDTO>>(users);

            // Return success response with search results
            return Ok(new ApiResult<object>(200, "Search completed successfully.", new { SearchParams = searchParams, TotalRow = totalCount, Results = userDTOs }));
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (string.Compare(request.Password, request.ConfirmPassword) != 0)
            {
                return BadRequest(new ApiResult<object>(400, "Password and confirm password must be same.", null));
            }
            // Check Exists
            var userExists = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (userExists != null)
            {
                return BadRequest(new ApiResult<object>(400, "Email have been used!", null));
            }

            User user = new()
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Password = request.Password,
                Role = "User", 
                Status = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var usersResponse = await _context.Users.Where(u => u.Email == request.Email).Select(u => _mapper.Map<UserDTO>(u)).ToListAsync();
                return Ok(new ApiResult<object>(200, "Create User Successfully!", usersResponse));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDTO request)
        {
            // Find the user by id
            Guid.TryParse(id, out Guid convertId);

            var user = await _context.Users.FindAsync(convertId);
            if (user == null)
            {
                return NotFound(new ApiResult<object>(404, "User not found.", null));
            }

            // Update user properties
            if (!string.IsNullOrEmpty(request.Name))
            {
                user.Name = request.Name;
            }         

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                user.Address = request.Address;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = request.Password;
            }

            // Update user in database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Return success response
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(new ApiResult<object>(200, "User updated successfully.", userDTO));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid.TryParse(id, out Guid convertId);
            // Find the user by id
            var user = await _context.Users.FindAsync(convertId);
            if (user == null)
            {
                return NotFound(new ApiResult<object>(404, "User not found.", null));
            }

            // Remove user from database
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Return success response
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(new ApiResult<object>(200, "User deleted successfully.", userDTO));
        }
    }
}
