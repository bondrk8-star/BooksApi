using BooksApi.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using BooksApi.Server.DTOs.UserDTOs.Request;
using BooksApi.Server.DTOs.UserDTOs.Response;
using Microsoft.EntityFrameworkCore;
using BooksApi.Server.Data;
using BooksApi.Server.Interfaces;


namespace BooksApi.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class UserController: ControllerBase
    {
        private readonly PostgresContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;

        public UserController(PostgresContext dbContext, ITokenService tokenService, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
            this.passwordHasher = passwordHasher;
        }

        private readonly ITokenService tokenService;

        [HttpPost("Register")]
        public async Task<ActionResult<UserCreateResponse>> Create([FromBody] UserCreateRequest request)
        {
            if (await dbContext.Users.AnyAsync(u => u.Username == request.Username)) return BadRequest("Такой пользователь уже есть");

            var user = new User() { Username = request.Username};
            string hash = passwordHasher.HashPassword(user, request.Password);
            user.Password = hash;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return new UserCreateResponse
            {
                UserId = user.Id,
                Username = request.Username,
                Token = tokenService.CreateToken(user)
                
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserLoggerResponse>> Login([FromBody] UserLoggerRequest request)
        {
            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null) return BadRequest("Неверный логин или пароль.");

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result == PasswordVerificationResult.Failed) return BadRequest("Неверный логин или пароль.");


            return new UserLoggerResponse
            {
                UserId = user.Id,
                Token = tokenService.CreateToken(user)
            };
        }

    }
}
