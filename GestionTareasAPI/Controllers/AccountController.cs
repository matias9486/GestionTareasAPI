using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Models;
using GestionTareasAPI.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {            
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //var user = _userManager.Users.SingleOrDefault(u => u.UserName == loginDto.Email);
            
            if (user == null) return Unauthorized("Invalid email or password");

            //Check if password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!result.Succeeded) return Unauthorized("Invalid email or password");
            
            return Ok(new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Token = await _tokenService.GenerateToken(user) //Generate token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {            
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Check if user exists
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null) 
                return BadRequest("User already exists");
            
            var user = new Usuario
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Name = registerDto.Name
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            //Check if user was created
            if (!result.Succeeded) return BadRequest(result.Errors);

            //Add role
            var roleResult = await _userManager.AddToRoleAsync(user, "USER");
            //Check if role was added
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            //Return user
            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user) //Generate token
            });
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var users =  _userManager.Users;
            
            return Ok(users);
        }
    }
}
