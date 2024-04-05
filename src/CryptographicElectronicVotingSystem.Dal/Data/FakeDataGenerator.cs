using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;


namespace CryptographicElectronicVotingSystem.Dal.Data
{
    public class FakeDataGenerator
    {
        public List<Candidate> GenerateCandidates(int numberOfCandidates)
        {
            var candidateId = 10;
            var candidateFaker = new Faker<Candidate>()
                .RuleFor(c => c.CandidateID, _ => candidateId++)
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Party, f => f.PickRandom(new string[] { "Democratic", "Republican", "Libertarian" }));
            
            return candidateFaker.Generate(numberOfCandidates);
        }
        
        public List<Tallyingcenter> GenerateTallyingCenters(int numberOfCenters)
        {
            var centerFaker = new Faker<Tallyingcenter>()
                .RuleFor(t => t.Name, f => f.Company.CompanyName())
                .RuleFor(t => t.Location, f => f.Address.FullAddress())
                .RuleFor(t => t.CenterPublicKey, f => f.Random.AlphaNumeric(20));
    
            return centerFaker.Generate(numberOfCenters);
        }
        
        public List<Voter> GenerateVoters(int numberOfVoters, List<ApplicationUser> users)
        {
            // Check if we have enough users to match the number of voters
            if (users.Count < numberOfVoters)
            {
                throw new InvalidOperationException("Not enough users to create voters.");
            }

            // Shuffle the list of users to randomly pick users for voters
            var shuffledUsers = users.OrderBy(_ => Guid.NewGuid()).ToList();

            var voterFaker = new Faker<Voter>()
                //.RuleFor(v => v.VoterId, f => f.Random.Long(1000000000, 9999999999)) // Assuming VoterId is auto-generated
                .RuleFor(v => v.FullName, (f, v) => f.Name.FullName())
                .RuleFor(v => v.Email, (f, v) => f.Internet.Email())
                .RuleFor(v => v.IsAuthorized, f => f.Random.Bool())
                .RuleFor(v => v.HasVoted, f => false) // Assuming new voters haven't voted yet
                .RuleFor(v => v.VoterPublicKey, f => f.Random.AlphaNumeric(20))
                // Assigning ApplicationUserId from the shuffled list of users
                .FinishWith((f, v) =>
                {
                    var user = shuffledUsers.ElementAt(f.IndexFaker);
                    v.ApplicationUserId = user.Id;
                });

            var voters = voterFaker.Generate(numberOfVoters);

            return voters;
        }
        
        public List<Votetally> GenerateVoteTallies(int numberOfTallies, List<Candidate> candidates, List<Tallyingcenter> centers)
        {
            var tallyFaker = new Faker<Votetally>()
                .RuleFor(t => t.CandidateID, f => f.PickRandom(candidates).CandidateID)
                .RuleFor(t => t.CenterID, f => f.PickRandom(centers).CenterID)
                .RuleFor(t => t.VoteCount, f => f.Random.Int(0, 1000));
    
            return tallyFaker.Generate(numberOfTallies);
        }
        
        public List<Vote> GenerateVotes(int numberOfVotes, List<Candidate> candidates, List<Voter> voters)
        {
            // Ensure that we have candidates and voters to assign votes to.
            if (!candidates.Any() || !voters.Any())
            {
                throw new InvalidOperationException("Candidates and voters lists must not be empty.");
            }

            List<Vote> votes = new List<Vote>();
            var availableVoters = voters.ToList(); // Create a copy of the voters list.

            for (int i = 0; i < numberOfVotes; i++)
            {
                if (!availableVoters.Any()) // If all voters have voted, break the loop.
                {
                    break;
                }

                var voter = availableVoters.First(); // Pick the first available voter.
                availableVoters.Remove(voter); // Remove the picked voter from available voters.

                var voteFaker = new Faker<Vote>()
                    //.RuleFor(v => v.VoteId, f => 0) // Assuming VoteId is auto-generated
                    .RuleFor(v => v.CandidateID, f => f.PickRandom(candidates).CandidateID)
                    .RuleFor(v => v.VoterID, _ => voter.VoterID) // Use the picked voter's ID
                    .RuleFor(v => v.Timestamp, f => f.Date.Recent())
                    .RuleFor(v => v.VoteProof, f => f.Random.Hash());

                votes.Add(voteFaker.Generate()); // Generate and add the vote to the list.
            }

            return votes;
        }

    }
}