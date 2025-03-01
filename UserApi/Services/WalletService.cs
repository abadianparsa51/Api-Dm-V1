using UserApi.Core.Interfaces;
using UserApi.Core.Models;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Wallet> GetWalletByUserIdAsync(string userId)
    {
        return await _walletRepository.GetWalletByUserIdAsync(userId);
    }
}
