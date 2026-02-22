using Microsoft.AspNetCore.Http;
using Context;
using DataBase;
using System.Security.Claims;
namespace Services;

// Класс UserServices
public class UserServices : IUserServices
{
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly IUserRepository _UserRepository;
    private readonly IHashingServices _HashingServices;


    public UserServices(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
            IHashingServices hashingServices)
    {
        _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _UserRepository = userRepository;
        _HashingServices = hashingServices;
    }
    public int GetMyId()
    {
        return Convert.ToInt32(_HttpContextAccessor.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
    }

    public async Task<IBaseResponse<IEnumerable<User>>> GetUsers()
    {
        BaseResponse<IEnumerable<User>> baseResponse;
        var users = await _UserRepository.Select();

        // Ok (204) but 0 elements
        if (!users.Any())
        {
            baseResponse = BaseResponse<IEnumerable<User>>.NoContent("Find 0 elements");
            return baseResponse;
        }

        baseResponse = BaseResponse<IEnumerable<User>>.Ok(users);
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> GetUser(int id)
    {
        BaseResponse<User> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }
        baseResponse = BaseResponse<User>.Ok(user, "User found");
        baseResponse.Data = user;
        return baseResponse;
    }

    public async Task<IBaseResponse> CreateUser(User userEntity)
    {
        userEntity = _HashingServices.Hashing(userEntity);
        userEntity.Id = 0;
        await _UserRepository.Create(userEntity);
        var baseResponse = BaseResponse.Created("User created");
        return baseResponse;
    }

    public async Task<IBaseResponse> DeleteUser(int id)
    {
        BaseResponse baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            baseResponse = BaseResponse.NotFound("User not found");
            return baseResponse;
        }

        await _UserRepository.Delete(user);
        baseResponse = BaseResponse.NoContent();
        return baseResponse;
    }

    public async Task<IBaseResponse<User>> GetUserByEmail(string email)
    {
        BaseResponse<User> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            baseResponse = BaseResponse<User>.NotFound("User not found");
            return baseResponse;
        }

        baseResponse = BaseResponse<User>.Ok(user);
        return baseResponse;
    }

    public async Task<IBaseResponse> Edit(string oldEmail, User userEntity)
    {
        // Хэширование Password
        userEntity = _HashingServices.Hashing(userEntity);

        BaseResponse baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == oldEmail);

        if (user == null)
        {
            baseResponse = BaseResponse.NotFound("User not found");
            return baseResponse;
        }

        user.Email = userEntity.Email;
        user.Password = userEntity.Password;
        user.FirstName = userEntity.FirstName;
        user.SecondName = userEntity.SecondName;

        await _UserRepository.Update(user);
        baseResponse = BaseResponse.Created();
        return baseResponse;
    }
}
