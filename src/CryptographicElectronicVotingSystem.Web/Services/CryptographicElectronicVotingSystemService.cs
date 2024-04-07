using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Dal.Data;

namespace CryptographicElectronicVotingSystem.Web.Services
{
    public partial class CryptographicElectronicVotingSystemService
    {
        CryptographicElectronicVotingSystemContext Context
        {
           get
           {
             return this.context;
           }
        }

        protected readonly CryptographicElectronicVotingSystemContext context;
        private readonly NavigationManager navigationManager;

        public CryptographicElectronicVotingSystemService(CryptographicElectronicVotingSystemContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportCandidatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/candidates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/candidates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCandidatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/candidates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/candidates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCandidatesRead(ref IQueryable<Candidate> items);

        public async Task<IQueryable<Candidate>> GetCandidates(Query query = null)
        {
            var items = Context.Candidates.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCandidatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCandidateGet(Candidate item);
        partial void OnGetCandidateByCandidateId(ref IQueryable<Candidate> items);


        public async Task<Candidate> GetCandidateByCandidateId(int candidateid)
        {
            var items = Context.Candidates
                              .AsNoTracking()
                              .Where(i => i.CandidateID == candidateid);

 
            OnGetCandidateByCandidateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCandidateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCandidateCreated(Candidate item);
        partial void OnAfterCandidateCreated(Candidate item);

        public async Task<Candidate> CreateCandidate(Candidate candidate)
        {
            OnCandidateCreated(candidate);

            var existingItem = Context.Candidates
                              .Where(i => i.CandidateID == candidate.CandidateID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Candidates.Add(candidate);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(candidate).State = EntityState.Detached;
                throw;
            }

            OnAfterCandidateCreated(candidate);

            return candidate;
        }

        public async Task<Candidate> CancelCandidateChanges(Candidate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCandidateUpdated(Candidate item);
        partial void OnAfterCandidateUpdated(Candidate item);

        public async Task<Candidate> UpdateCandidate(int candidateid, Candidate candidate)
        {
            OnCandidateUpdated(candidate);

            var itemToUpdate = Context.Candidates
                              .Where(i => i.CandidateID == candidate.CandidateID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(candidate);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCandidateUpdated(candidate);

            return candidate;
        }

        partial void OnCandidateDeleted(Candidate item);
        partial void OnAfterCandidateDeleted(Candidate item);

        public async Task<Candidate> DeleteCandidate(int candidateid)
        {
            var itemToDelete = Context.Candidates
                              .Where(i => i.CandidateID == candidateid)
                              .Include(i => i.Votetallies)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCandidateDeleted(itemToDelete);


            Context.Candidates.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCandidateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTallyingcentersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/tallyingcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/tallyingcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTallyingcentersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/tallyingcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/tallyingcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTallyingcentersRead(ref IQueryable<Tallyingcenter> items);

        public async Task<IQueryable<Tallyingcenter>> GetTallyingcenters(Query query = null)
        {
            var items = Context.Tallyingcenters.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTallyingcentersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTallyingcenterGet(Tallyingcenter item);
        partial void OnGetTallyingcenterByCenterId(ref IQueryable<Tallyingcenter> items);


        public async Task<Tallyingcenter> GetTallyingcenterByCenterId(int centerid)
        {
            var items = Context.Tallyingcenters
                              .AsNoTracking()
                              .Where(i => i.CenterID == centerid);

 
            OnGetTallyingcenterByCenterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTallyingcenterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTallyingcenterCreated(Tallyingcenter item);
        partial void OnAfterTallyingcenterCreated(Tallyingcenter item);

        public async Task<Tallyingcenter> CreateTallyingcenter(Tallyingcenter tallyingcenter)
        {
            OnTallyingcenterCreated(tallyingcenter);

            var existingItem = Context.Tallyingcenters
                              .Where(i => i.CenterID == tallyingcenter.CenterID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Tallyingcenters.Add(tallyingcenter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tallyingcenter).State = EntityState.Detached;
                throw;
            }

            OnAfterTallyingcenterCreated(tallyingcenter);

            return tallyingcenter;
        }

        public async Task<Tallyingcenter> CancelTallyingcenterChanges(Tallyingcenter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTallyingcenterUpdated(Tallyingcenter item);
        partial void OnAfterTallyingcenterUpdated(Tallyingcenter item);

        public async Task<Tallyingcenter> UpdateTallyingcenter(int centerid, Tallyingcenter tallyingcenter)
        {
            OnTallyingcenterUpdated(tallyingcenter);

            var itemToUpdate = Context.Tallyingcenters
                              .Where(i => i.CenterID == tallyingcenter.CenterID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tallyingcenter);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTallyingcenterUpdated(tallyingcenter);

            return tallyingcenter;
        }

        partial void OnTallyingcenterDeleted(Tallyingcenter item);
        partial void OnAfterTallyingcenterDeleted(Tallyingcenter item);

        public async Task<Tallyingcenter> DeleteTallyingcenter(int centerid)
        {
            var itemToDelete = Context.Tallyingcenters
                              .Where(i => i.CenterID == centerid)
                              .Include(i => i.Votetallies)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTallyingcenterDeleted(itemToDelete);


            Context.Tallyingcenters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTallyingcenterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVotersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/voters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/voters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVotersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/voters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/voters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVotersRead(ref IQueryable<Voter> items);

        public async Task<IQueryable<Voter>> GetVoters(Query query = null)
        {
            var items = Context.Voters.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnVotersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVoterGet(Voter item);
        partial void OnGetVoterByVoterId(ref IQueryable<Voter> items);


        public async Task<Voter> GetVoterByVoterId(long voterid)
        {
            var items = Context.Voters
                              .AsNoTracking()
                              .Where(i => i.VoterID == voterid);

 
            OnGetVoterByVoterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVoterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVoterCreated(Voter item);
        partial void OnAfterVoterCreated(Voter item);

        public async Task<Voter> CreateVoter(Voter voter)
        {
            OnVoterCreated(voter);

            var existingItem = Context.Voters
                              .Where(i => i.VoterID == voter.VoterID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Voters.Add(voter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(voter).State = EntityState.Detached;
                throw;
            }

            OnAfterVoterCreated(voter);

            return voter;
        }

        public async Task<Voter> CancelVoterChanges(Voter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVoterUpdated(Voter item);
        partial void OnAfterVoterUpdated(Voter item);

        public async Task<Voter> UpdateVoter(long voterid, Voter voter)
        {
            OnVoterUpdated(voter);

            var itemToUpdate = Context.Voters
                              .Where(i => i.VoterID == voter.VoterID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(voter);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVoterUpdated(voter);

            return voter;
        }

        partial void OnVoterDeleted(Voter item);
        partial void OnAfterVoterDeleted(Voter item);

        public async Task<Voter> DeleteVoter(long voterid)
        {
            var itemToDelete = Context.Voters
                              .Where(i => i.VoterID == voterid)
                              .Include(i => i.Votes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVoterDeleted(itemToDelete);


            Context.Voters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVoterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVotesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/votes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/votes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVotesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/votes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/votes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVotesRead(ref IQueryable<Vote> items);

        public async Task<IQueryable<Vote>> GetVotes(Query query = null)
        {
            var items = Context.Votes.AsQueryable();

            items = items.Include(i => i.Voter);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnVotesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVoteGet(Vote item);
        partial void OnGetVoteByVoteId(ref IQueryable<Vote> items);


        public async Task<Vote> GetVoteByVoteId(int voteid)
        {
            var items = Context.Votes
                              .AsNoTracking()
                              .Where(i => i.VoteID == voteid);

            items = items.Include(i => i.Voter);
 
            OnGetVoteByVoteId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVoteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVoteCreated(Vote item);
        partial void OnAfterVoteCreated(Vote item);

        public async Task<Vote> CreateVote(Vote vote)
        {
            OnVoteCreated(vote);

            var existingItem = Context.Votes
                              .Where(i => i.VoteID == vote.VoteID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Votes.Add(vote);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(vote).State = EntityState.Detached;
                throw;
            }

            OnAfterVoteCreated(vote);

            return vote;
        }

        public async Task<Vote> CancelVoteChanges(Vote item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVoteUpdated(Vote item);
        partial void OnAfterVoteUpdated(Vote item);

        public async Task<Vote> UpdateVote(int voteid, Vote vote)
        {
            OnVoteUpdated(vote);

            var itemToUpdate = Context.Votes
                              .Where(i => i.VoteID == vote.VoteID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(vote);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVoteUpdated(vote);

            return vote;
        }

        partial void OnVoteDeleted(Vote item);
        partial void OnAfterVoteDeleted(Vote item);

        public async Task<Vote> DeleteVote(int voteid)
        {
            var itemToDelete = Context.Votes
                              .Where(i => i.VoteID == voteid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVoteDeleted(itemToDelete);


            Context.Votes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVoteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVotetalliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/votetallies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/votetallies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVotetalliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/CryptographicElectronicVotingSystem/votetallies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/CryptographicElectronicVotingSystem/votetallies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVotetalliesRead(ref IQueryable<Votetally> items);

        public async Task<IQueryable<Votetally>> GetVotetallies(Query query = null)
        {
            var items = Context.Votetallies.AsQueryable();

            items = items.Include(i => i.Candidate);
            items = items.Include(i => i.Tallyingcenter);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnVotetalliesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVotetallyGet(Votetally item);
        partial void OnGetVotetallyByTallyId(ref IQueryable<Votetally> items);


        public async Task<Votetally> GetVotetallyByTallyId(int tallyid)
        {
            var items = Context.Votetallies
                              .AsNoTracking()
                              .Where(i => i.TallyID == tallyid);

            items = items.Include(i => i.Candidate);
            items = items.Include(i => i.Tallyingcenter);
 
            OnGetVotetallyByTallyId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVotetallyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVotetallyCreated(Votetally item);
        partial void OnAfterVotetallyCreated(Votetally item);

        public async Task<Votetally> CreateVotetally(Votetally votetally)
        {
            OnVotetallyCreated(votetally);

            var existingItem = Context.Votetallies
                              .Where(i => i.TallyID == votetally.TallyID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Votetallies.Add(votetally);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(votetally).State = EntityState.Detached;
                throw;
            }

            OnAfterVotetallyCreated(votetally);

            return votetally;
        }

        public async Task<Votetally> CancelVotetallyChanges(Votetally item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVotetallyUpdated(Votetally item);
        partial void OnAfterVotetallyUpdated(Votetally item);

        public async Task<Votetally> UpdateVotetally(int tallyid, Votetally votetally)
        {
            OnVotetallyUpdated(votetally);

            var itemToUpdate = Context.Votetallies
                              .Where(i => i.TallyID == votetally.TallyID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(votetally);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVotetallyUpdated(votetally);

            return votetally;
        }

        partial void OnVotetallyDeleted(Votetally item);
        partial void OnAfterVotetallyDeleted(Votetally item);

        public async Task<Votetally> DeleteVotetally(int tallyid)
        {
            var itemToDelete = Context.Votetallies
                              .Where(i => i.TallyID == tallyid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVotetallyDeleted(itemToDelete);


            Context.Votetallies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVotetallyDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}