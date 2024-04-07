using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;


namespace CryptographicElectronicVotingSystem.Data
{
    public class FakeDataGenerator
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationIdentityDbContext _applicationIdentityDbContext;
        private readonly CryptographicElectronicVotingSystemContext _cryptographicElectronicVotingSystemContext;

        public FakeDataGenerator(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationIdentityDbContext applicationIdentityDbContext, CryptographicElectronicVotingSystemContext cryptographicElectronicVotingSystemContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationIdentityDbContext = applicationIdentityDbContext;
            _cryptographicElectronicVotingSystemContext = cryptographicElectronicVotingSystemContext;
        }

        public async Task<bool> GenerateUsersAndRolesAsync(int numberOfUsers)
        {
            var random = new Random();
            bool overallSuccess = true;

            // Fetch existing roles from the database
            var existingRoles = await _roleManager.Roles.ToListAsync();
            if (!existingRoles.Any())
            {
                Console.WriteLine("No roles found in the database.");
                return false; // Early exit if no roles found
            }

            var userFaker = new Faker<ApplicationUser>()
                // Use actions to temporarily store first and last names
                .CustomInstantiator(f => {
                    var firstName = f.Name.FirstName();
                    var lastName = f.Name.LastName();
                    var picture = f.Internet.Avatar();
                    return new ApplicationUser
                    {
                        UserName = $"{firstName}.{lastName}@example.com",
                        Email = $"{firstName}.{lastName}@example.com",
                        EmailConfirmed = f.Random.Bool(),
                        // Assuming you have these properties in your ApplicationUser class
                        FirstName = firstName,
                        LastName = lastName,
                        Picture = picture
                    };
                });


            foreach (var user in userFaker.Generate(numberOfUsers))
            {
                using var transaction = await _applicationIdentityDbContext.Database.BeginTransactionAsync();

                try
                {
                    // Create the user with a temporary password
                    var createUserResult = await _userManager.CreateAsync(user, "Q!w2e3r4t5");
                    if (!createUserResult.Succeeded) throw new Exception($"Failed to create user {user.UserName}.");
                    Console.WriteLine($"User {user.UserName} created successfully.");

                    // Commit the transaction to ensure the user is created in the database
                    await transaction.CommitAsync();
                    
                    // Assign a random role to the user
                    var selectedRole = existingRoles[random.Next(existingRoles.Count)].Name;
                    var addRoleResult = await _userManager.AddToRoleAsync(user, selectedRole);
                    if (!addRoleResult.Succeeded) throw new Exception($"Failed to assign role '{selectedRole}' to user '{user.UserName}'.");
                    Console.WriteLine($"Role '{selectedRole}' assigned to user '{user.UserName}' successfully.");
                    
                    //await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Only roll back if the transaction hasn't been committed. This check is necessary
                    // if the transaction variable was reassigned or if we're using multiple transactions.
                    if (transaction != null && transaction.GetDbTransaction().Connection != null)
                    {
                        await transaction.RollbackAsync();
                    }
                    Console.WriteLine($"Error during user and voter generation: {ex.Message}");
                    overallSuccess = false;
                }

            }

            return overallSuccess;
        }
        
        // generate fake candidates
        public async Task<bool> GenerateCandidatesAsync(int numberOfCandidates)
        {
            var random = new Random();
            bool overallSuccess = true;
            
            var parties = new List<string> { "Democratic", "Republican", "Libertarian" };

            var candidateFaker = new Faker<Candidate>()
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Party, f => f.PickRandom(parties));

            foreach (var candidate in candidateFaker.Generate(numberOfCandidates))
            {
                using var transaction = await _cryptographicElectronicVotingSystemContext.Database.BeginTransactionAsync();

                try
                {
                    await _cryptographicElectronicVotingSystemContext.Candidates.AddAsync(candidate);
                    await _cryptographicElectronicVotingSystemContext.SaveChangesAsync();
                    Console.WriteLine($"Candidate {candidate.Name} created successfully.");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    if (transaction != null && transaction.GetDbTransaction().Connection != null)
                    {
                        await transaction.RollbackAsync();
                    }
                    Console.WriteLine($"Error during candidate generation: {ex.Message}");
                    overallSuccess = false;
                }
            }

            return overallSuccess;
        }
        
        // Generate fake voters
        public async Task<bool> GenerateVotersAsync(int numberOfVoters)
        {
            // Retrieve all users
            var users = await _applicationIdentityDbContext.Users.ToListAsync();
            int count;
            // Check if the number of voters to create matches the number of users
            if (numberOfVoters != users.Count)
            {
                Console.WriteLine($"Number of voters to generate ({numberOfVoters}) does not match the number of users ({users.Count}).");
                count = users.Count;
            }

            bool overallSuccess = true;

            foreach (var user in users)
            {
                using var transaction = await _cryptographicElectronicVotingSystemContext.Database.BeginTransactionAsync();
                try
                {
                    var voter = new Voter
                    {
                        FirstName = user.FirstName, // Assuming the User entity has a FirstName property
                        LastName = user.LastName, // Assuming the User entity has a LastName property
                        Email = user.Email, // Assuming the User entity has an Email property
                        IsAuthorized = true, // Set based on your logic
                        HasVoted = false,
                        VoterPrivateKey = "", // Set based on your logic or leave empty as in your original code
                        UserId = user.Id // Assuming you need to link the Voter to the User via an Id or similar property
                    };

                    await _cryptographicElectronicVotingSystemContext.Voters.AddAsync(voter);
                    await _cryptographicElectronicVotingSystemContext.SaveChangesAsync();
                    Console.WriteLine($"Voter {voter.FirstName} {voter.LastName} created successfully.");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error during voter generation for user {user.Email}: {ex.Message}");
                    overallSuccess = false;
                }
            }

            return overallSuccess;
        }
        
        
        // generate fake Tallying Centers based on US states
        public async Task<bool> GenerateTallyingCentersAsync()
        {
            var states = new List<string> { "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming" };

            bool overallSuccess = true;

            foreach (var state in states)
            {
                using var transaction = await _cryptographicElectronicVotingSystemContext.Database.BeginTransactionAsync();
                try
                {
                    var tallyingCenter = new Tallyingcenter
                    {
                        Name = $"{state} Tallying Center",
                        Location = state,
                        CenterPrivateKey = "" // Set based on your logic or leave empty as in your original code
                    };

                    await _cryptographicElectronicVotingSystemContext.Tallyingcenters.AddAsync(tallyingCenter);
                    await _cryptographicElectronicVotingSystemContext.SaveChangesAsync();
                    Console.WriteLine($"Tallying Center for {state} created successfully.");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error during Tallying Center generation for state {state}: {ex.Message}");
                    overallSuccess = false;
                }
            }
            return overallSuccess;
        }

    }
}