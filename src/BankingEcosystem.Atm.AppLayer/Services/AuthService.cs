using System.Threading.Tasks;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface IAuthService
{
    Task<bool> VerifyPinAsync(string cardNumber, string pin);
}

public class MockAuthService : IAuthService
{
    private readonly AtmSessionService _sessionService;

    public MockAuthService(AtmSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<bool> VerifyPinAsync(string cardNumber, string pin)
    {
        // Mock logic: PIN "123456" is valid for any card
        // In real implementation, this would call the Backend API
        await Task.Delay(500); // Simulate network latency

        if (pin == "123456")
        {
            // Simulate successful login
            _sessionService.Authenticate("mock_jwt_token", "John Doe", 101);
            return true;
        }

        return false;
    }
}
