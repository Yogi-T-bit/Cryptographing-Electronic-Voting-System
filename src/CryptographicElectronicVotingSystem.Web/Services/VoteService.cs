using CryptographicElectronicVotingSystem.Dal.Data;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace CryptographicElectronicVotingSystem.Web.Services;

public class VoteService : CryptographicElectronicVotingSystemService
{
    public VoteService(CryptographicElectronicVotingSystemContext context, NavigationManager navigationManager) : base(context, navigationManager)
    {
    }
    
    public async Task<bool> Voting(long voterId, int candidateId)
    {
        // Assuming we retrieve the voter and candidate from the database
        var voter = context.Voters.Find(voterId);
        var candidate = context.Candidates.Find(candidateId);

        if (voter == null || candidate == null || (bool)voter.HasVoted)
        {
            return false;
        }
    
        // Generate a new public/private key pair for the vote
        var (publicKey, privateKey) = RsaCryptoService.GenerateKeys();
    
        // Simulate generating a vote proof with candidate name and timestamp
        var timestamp = DateTime.UtcNow;
        string voteProof = $"Vote for candidate {candidate.Name} by voter {voter.VoterID} at {timestamp}";

        // Encrypting the vote proof with the new public key
        string encryptedVote = RsaCryptoService.Encrypt(publicKey, voteProof);

        var vote = new Vote
        {
            // Assuming VoterID is still included for identifying the voter without revealing their choice
            VoterID = voterId,
            EncryptedVote = encryptedVote,
            Timestamp = timestamp,
            VotePublicKey = publicKey // Store the public key with the vote for future verification
        };
    
        // Use CreateVote method to add the vote to the database
        await CreateVote(vote);

        voter.VoterPrivateKey = privateKey;
        voter.HasVoted = true;
        await context.SaveChangesAsync();

        // Securely transmit the private key to the voter for their records
        // This step is crucial and must be implemented securely
        // [Your code to securely send the private key to the voter]

        return true;
    }



}