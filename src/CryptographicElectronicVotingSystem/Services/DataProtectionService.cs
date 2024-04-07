using Microsoft.AspNetCore.DataProtection;

namespace CryptographicElectronicVotingSystem.Services;

public class DataProtectionService
{
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public DataProtectionService(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }

    // Get a protector based on the role and, for voters, their unique ID
    public IDataProtector GetProtector(string role, string userId = null)
    {
        // No change for Administrators and Election Officials since they access all data
        if (role == "Administrators" || role == "ElectionOfficials")
        {
            // A single protector for all votes, allowing these roles to decrypt any data within the "Votes" scope
            return _dataProtectionProvider.CreateProtector("Votes");
        }
        else if (role == "Voters" && userId != null)
        {
            // Unique protector for each voter, ensuring data can only be accessed by the specific voter or roles with broader access
            return _dataProtectionProvider.CreateProtector($"Votes.Voter.{userId}");
        }
        else
        {
            throw new ArgumentException("Invalid role or missing userId for voter.");
        }
    }
}