using API.Models;

namespace API.Services
{
    public interface ITokenService
    {
        string GetToken(Customer customer);
    }
}
