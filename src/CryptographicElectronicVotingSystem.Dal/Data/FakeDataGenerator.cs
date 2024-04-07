using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.Collections.Generic;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;


namespace CryptographicElectronicVotingSystem.Dal.Data
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
            var random = new Random();
            bool overallSuccess = true;

            var voterFaker = new Faker<Voter>()
                .RuleFor(v => v.FirstName, f => f.Name.FirstName())
                .RuleFor(v => v.LastName, f => f.Name.LastName())
                // Email is first name + last name + domain
                .RuleFor(v => v.Email, (f, v) => $"{
                    v.FirstName.ToLower()}.{v.LastName.ToLower()}@example.com")
                .RuleFor(v => v.IsAuthorized, f => f.Random.Bool())
                .RuleFor(v => v.HasVoted, false)
                .RuleFor(v => v.VoterPrivateKey, "");

            foreach (var voter in voterFaker.Generate(numberOfVoters))
            {
                using var transaction = await _cryptographicElectronicVotingSystemContext.Database.BeginTransactionAsync();

                try
                {
                    await _cryptographicElectronicVotingSystemContext.Voters.AddAsync(voter);
                    await _cryptographicElectronicVotingSystemContext.SaveChangesAsync();
                    Console.WriteLine($"Voter {voter.FirstName} {voter.LastName} created successfully.");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    if (transaction != null && transaction.GetDbTransaction().Connection != null)
                    {
                        await transaction.RollbackAsync();
                    }
                    Console.WriteLine($"Error during voter generation: {ex.Message}");
                    overallSuccess = false;
                }
            }

            return overallSuccess;
        }
    }
}