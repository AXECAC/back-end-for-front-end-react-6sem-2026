using DataBase;
namespace Services;

// Интерфейс IUserServices
public interface IUserServices
{
    int GetMyId();

    Task<IBaseResponse<UserPubData>> GetMyProfile();

    Task<IBaseResponse> ChangeMyPassword(string newPassword);

    Task<IBaseResponse> UpdateMyProfile(User updatedUser);

    Task<IBaseResponse<IEnumerable<UserPubData>>> GetUsers();

    Task<IBaseResponse<UserPubData>> GetUser(int id);

    Task<IBaseResponse> CreateUser(User userEntity);

    Task<IBaseResponse> DeleteUser(int id);

    Task<IBaseResponse<UserPubData>> GetUserByEmail(string email);

    Task<IBaseResponse> Edit(string oldEmail, User userEntity);
}
