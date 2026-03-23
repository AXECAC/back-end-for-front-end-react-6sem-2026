using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Context;
using DataBase;
using System.Security.Claims;
using Extentions;
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

    public async Task<IBaseResponse<UserPubData>> GetMyProfile()
    {
        BaseResponse<UserPubData> response;

        int myId = GetMyId();

        var user = await _UserRepository.FirstOrDefaultAsync(us => us.Id == myId);

        if (user == null)
        {
            response = BaseResponse<UserPubData>.NotFound("User not found");
            return response;
        }

        response = BaseResponse<UserPubData>.Ok(user.ConvertToUserPubData());
        return response;
    }

    public async Task<IBaseResponse> ChangeMyPassword(string newPassword)
    {
        BaseResponse response;

        int myId = GetMyId();

        var user = await _UserRepository.FirstOrDefaultAsync(us => us.Id == myId);

        if (user == null)
        {
            response = BaseResponse.NotFound("User not found");
            return response;
        }

        user.Password = newPassword;
        _HashingServices.Hashing(user);

        await _UserRepository.Update(user);

        response = BaseResponse.NoContent();
        return response;
    }

    public async Task<IBaseResponse> UpdateMyProfile(User updatedUser)
    {
        BaseResponse response;

        int myId = GetMyId();

        var user = await _UserRepository.FirstOrDefaultAsync(us => us.Id == myId);

        if (user == null)
        {
            response = BaseResponse.NotFound("User not found");
            return response;
        }

        user.FirstName = updatedUser.FirstName;
        user.SecondName = updatedUser.SecondName;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;

        await _UserRepository.Update(user);

        response = BaseResponse.NoContent();
        return response;
    }

    public async Task<IBaseResponse<IEnumerable<UserPubData>>> GetUsers()
    {
        BaseResponse<IEnumerable<UserPubData>> baseResponse;
        var users = await _UserRepository.Select();

        // Ok (204) but 0 elements
        if (!users.Any())
        {
            baseResponse = BaseResponse<IEnumerable<UserPubData>>.NoContent("Find 0 elements");
            return baseResponse;
        }

        baseResponse = BaseResponse<IEnumerable<UserPubData>>.Ok(users.Select(UserExtentions.ConvertToUserPubData));
        return baseResponse;
    }

    public async Task<IBaseResponse<UserPubData>> GetUser(int id)
    {
        BaseResponse<UserPubData> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            baseResponse = BaseResponse<UserPubData>.NotFound("User not found");
            return baseResponse;
        }
        baseResponse = BaseResponse<UserPubData>.Ok(user.ConvertToUserPubData(), "User found");
        baseResponse.Data = user.ConvertToUserPubData();
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

    public async Task<IBaseResponse<UserPubData>> GetUserByEmail(string email)
    {
        BaseResponse<UserPubData> baseResponse;
        var user = await _UserRepository.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            baseResponse = BaseResponse<UserPubData>.NotFound("User not found");
            return baseResponse;
        }

        baseResponse = BaseResponse<UserPubData>.Ok(user.ConvertToUserPubData());
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
