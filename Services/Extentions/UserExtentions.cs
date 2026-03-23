using DataBase;
using Services;

namespace Extentions;

// Класс UserExtentions
public static class UserExtentions
{
    public static UserPubData ConvertToUserPubData(this User user)
    {
        var uPubData = new UserPubData
        {
            Id = user.Id,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Email = user.Email
        };
        return uPubData;
    }
    // Валидация User
    public static bool IsValid(this User user)
    {
        if (user.Id < 0 || !user.Email.IsValidEmail() || !user.Password.IsValidPassword() ||
                user.FirstName == "" || user.SecondName == "")
        {
            return false;
        }
        return true;
    }

    // Валидация Login User
    public static bool IsValid(this LoginUser form)
    {
        if (!form.Email.IsValidEmail() || !form.Password.IsValidPassword())
        {
            return false;
        }
        return true;
    }

    // Валидация Email
    public static bool IsValidEmail(this string email)
    {
        // Продолжить в будущем
        if (email == "" || !email.Contains('@'))
        {
            return false;
        }
        return true;
    }

    // Валидация Password
    public static bool IsValidPassword(this string password)
    {
        // Продолжить в будущем
        if (password == "")
        {
            return false;
        }
        return true;
    }
}
