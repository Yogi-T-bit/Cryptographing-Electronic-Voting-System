using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.Collections.Generic;
using CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;


namespace CryptographingElectronicVotingSystem.Dal.Data
{
    public class FakeDataGenerator
    {
        public List<candidate> GenerateCandidates(int numberOfCandidates)
        {
            var candidateId = 10;
            var candidateFaker = new Faker<candidate>()
                .RuleFor(c => c.CandidateID, _ => candidateId++)
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Party, f => f.PickRandom(new string[] { "Independent", "Party A", "Party B" }));
            
            return candidateFaker.Generate(numberOfCandidates);
        }
        
        public List<tallyingcenter> GenerateTallyingCenters(int numberOfCenters)
        {
            var centerFaker = new Faker<tallyingcenter>()
                .RuleFor(t => t.Name, f => f.Company.CompanyName())
                .RuleFor(t => t.Location, f => f.Address.City())
                .RuleFor(t => t.CenterPublicKey, f => f.Random.AlphaNumeric(20));
    
            return centerFaker.Generate(numberOfCenters);
        }
        
        public List<voter> GenerateVoters(int numberOfVoters)
        {
            var voterFaker = new Faker<voter>()
                .RuleFor(v => v.VoterID, f => f.Random.Long(1000000000, 9999999999))
                .RuleFor(v => v.FullName, f => f.Name.FullName())
                .RuleFor(v => v.Email, f => f.Internet.Email())
                .RuleFor(v => v.IsAuthorized, f => f.Random.Bool())
                .RuleFor(v => v.HasVoted, f => f.Random.Bool())
                .RuleFor(v => v.VoterPublicKey, f => f.Random.AlphaNumeric(20));
    
            return voterFaker.Generate(numberOfVoters);
        }
        
        public List<votetally> GenerateVoteTallies(int numberOfTallies, List<candidate> candidates, List<tallyingcenter> centers)
        {
            var tallyFaker = new Faker<votetally>()
                .RuleFor(t => t.CandidateID, f => f.PickRandom(candidates).CandidateID)
                .RuleFor(t => t.CenterID, f => f.PickRandom(centers).CenterID)
                .RuleFor(t => t.VoteCount, f => f.Random.Int(0, 1000));
    
            return tallyFaker.Generate(numberOfTallies);
        }
        
        public List<vote> GenerateVotes(int numberOfVotes, List<candidate> candidates, List<voter> voters)
        {
            // Ensure that we have candidates and voters to assign votes to.
            if (!candidates.Any() || !voters.Any())
            {
                throw new InvalidOperationException("Candidates and voters lists must not be empty.");
            }

            var voteFaker = new Faker<vote>()
                //.RuleFor(v => v.VoteID, f => 0) // Assuming VoteID is auto-generated
                .RuleFor(v => v.CandidateID, f => f.PickRandom(candidates).CandidateID)
                .RuleFor(v => v.VoterID, f => f.PickRandom(voters).VoterID)
                .RuleFor(v => v.Timestamp, f => f.Date.Recent())
                .RuleFor(v => v.VoteProof, f => f.Random.Hash());

            return voteFaker.Generate(numberOfVotes);
        }

    }
}