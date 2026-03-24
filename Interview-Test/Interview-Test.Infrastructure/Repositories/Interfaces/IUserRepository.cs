using Interview_Test.Models;

namespace Interview_Test.Repositories.Interfaces;

public interface IUserRepository
{
    dynamic GetUserById(string id);
    dynamic GetUsers();
    int CreateUser();
}