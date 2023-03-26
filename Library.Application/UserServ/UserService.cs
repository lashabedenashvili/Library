using Library.Data.Domein.Data;
using Library.DataBase.GeneralRepository;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.UserDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UserServ
{
    public class UserService:IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IGeneralRepository<User> _userRepo;
        private readonly IGeneralRepository<UserPasswordHistory> _userPasswordRepo;

        public UserService(
            IConfiguration configuration,
            IGeneralRepository<User> userRepo,
            IGeneralRepository<UserPasswordHistory> userPasswordRepo
            )
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _userPasswordRepo = userPasswordRepo;
        }
        public async Task<ApiResponse<string>> Registration(UserRegistrationDto request)
        {
            //Cheking if user exist in DB
            var userDb = await _userRepo.GetAsync(x => x.Email == request.Email);
            if (userDb == null)
            {   //Hashing Password
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var userId = new User
                {
                    Email = request.Email,
                    BirthDate = request.BirthDate,
                    Gender = request.Gender,
                    Name = request.Name,
                    SurName = request.SurName,
                    PhoneNumber = request.PhoneNumber,
                    IsActive = true
                };
                var userPassHistory = new UserPasswordHistory
                {
                    User = userId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreateTime = DateTime.Now,
                    IsActive = true,
                };
                await _userPasswordRepo.AddAsync(userPassHistory);
                await _userPasswordRepo.SaveChangesAsync();

                return new SuccessApiResponse<string>("Registration is successful");
            }
            return new BadApiResponse<string>("Email is Already Exist");
        }
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var userDb = await _userRepo.GetAsync(x => x.Id == id);
            if (userDb != null)
            {
                await _userRepo.Delete(userDb);
                await _userRepo.SaveChangesAsync();
                return new SuccessApiResponse<string>("User has been deleted");
            }
            return new BadApiResponse<string>("User Does not exist");
        }
        public async Task<ApiResponse<UserUpdateDto>> Update(int id, UserUpdateDto request)
        {
            var userDb = await _userRepo.GetAsync(x => x.Id == id && x.IsActive == true);
            if (userDb != null)
            {
                userDb.Name = request.Name == null ? userDb.Name : request.Name;
                userDb.SurName = request.SurName == null ? userDb.SurName : request.SurName;
                userDb.BirthDate = request.BirthDate.HasValue ? request.BirthDate : userDb.BirthDate;
                userDb.PhoneNumber = request.PhoneNumber == null ? userDb.PhoneNumber : request.PhoneNumber;
                await _userRepo.Update(userDb);
                await _userRepo.SaveChangesAsync();
                //return Updated User
                return new SuccessApiResponse<UserUpdateDto>(new UserUpdateDto
                {
                    Name = userDb.Name,
                    SurName = userDb.SurName,
                    BirthDate = userDb.BirthDate,
                    PhoneNumber = userDb.PhoneNumber,
                }
                    );
            }
            return new BadApiResponse<UserUpdateDto>("User Does not exist");
        }
        public async Task<ApiResponse<string>> UpdatePassword(ChangePasswordDto request, int id)
        {
            var userDb = await _userRepo.Where(x => x.Id == id && x.IsActive == true)
                                               .Include(e => e.UserPasswordHistory).FirstOrDefaultAsync();
            var checkOldPassword = userDb.UserPasswordHistory.Where(x => x.IsActive == true).FirstOrDefault();
            var verifyPassword = VerifyPasswordHash(request.OldPassword, checkOldPassword.PasswordHash, checkOldPassword.PasswordSalt);
            if (verifyPassword)
            {
                if (userDb.UserPasswordHistory.Any(e => VerifyPasswordHash(request.NewPassword, e.PasswordHash, e.PasswordSalt)))
                    return new BadApiResponse<string>("This Password is matched the old one");

                var MakeInActive = userDb.UserPasswordHistory.Where(e => e.IsActive).First();
                MakeInActive.IsActive = false;
                await _userPasswordRepo.Update(MakeInActive);
                CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                if (userDb.UserPasswordHistory.Count() >= 3)
                {
                    var updateOldRow = userDb.UserPasswordHistory.OrderBy(x => x.UpdateTime == null).FirstOrDefault() == null ? userDb.UserPasswordHistory.OrderBy(x => x.CreateTime).First() : userDb.UserPasswordHistory.OrderBy(x => x.UpdateTime).First();
                    updateOldRow.PasswordHash = passwordHash;
                    updateOldRow.PasswordSalt = passwordSalt;
                    updateOldRow.IsActive = true;
                    updateOldRow.UpdateTime = DateTime.Now;
                    await _userPasswordRepo.Update(updateOldRow);
                }
                else
                {
                    var createNewRow = new UserPasswordHistory
                    {
                        UserId = userDb.Id,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        IsActive = true,
                        CreateTime = DateTime.Now
                    };
                    await _userPasswordRepo.AddAsync(createNewRow);
                }
                await _userRepo.SaveChangesAsync();
                return new SuccessApiResponse<string>("", "Updated successfully!");
            }
            return new BadApiResponse<string>("Incorect Password");
        }
        public async Task<ApiResponse<string>> LogIn(UserLogInDto request)
        {
            var userDb = await _userRepo.Where(x => x.Email == request.Email)
                                               .Include(e => e.UserPasswordHistory
                                               .Where(e => e.IsActive == true))
                                               .FirstOrDefaultAsync();
            var _pasword = userDb?.UserPasswordHistory.FirstOrDefault();

            if (userDb == null || !userDb.IsActive) return new BadApiResponse<string>("User is not active");
            //checking user's password
            var checkpassword = VerifyPasswordHash(request.Password, _pasword.PasswordHash, _pasword.PasswordSalt);
            //return token
            if (checkpassword)
            {
                return new SuccessApiResponse<string>(CreateToken(userDb));
            }
            else return new BadApiResponse<string>("incorrect credentials!");


        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email.ToString())
            };            

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            JwtSecurityTokenHandler tokenHendler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHendler.CreateToken(tokenDescriptor);

            return tokenHendler.WriteToken(token);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //check Hash and salt with Null
            if (passwordHash == null && passwordSalt == null)
                return false;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
