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

        private readonly CryptographicElectronicVotingSystemContext context;
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

        partial void OnCandidatesRead(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> items);

        public async Task<IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate>> GetCandidates(Query query = null)
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

        partial void OnCandidateGet(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);
        partial void OnGetCandidateByCandidateId(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> items);


        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> GetCandidateByCandidateId(int candidateid)
        {
            var items = Context.Candidates
                              .AsNoTracking()
                              .Where(i => i.CandidateID == candidateid);

 
            OnGetCandidateByCandidateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCandidateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCandidateCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);
        partial void OnAfterCandidateCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> CreateCandidate(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate candidate)
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

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> CancelCandidateChanges(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCandidateUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);
        partial void OnAfterCandidateUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> UpdateCandidate(int candidateid, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate candidate)
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

        partial void OnCandidateDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);
        partial void OnAfterCandidateDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> DeleteCandidate(int candidateid)
        {
            var itemToDelete = Context.Candidates
                              .Where(i => i.CandidateID == candidateid)
                              .Include(i => i.Votes)
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

        partial void OnTallyingcentersRead(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> items);

        public async Task<IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter>> GetTallyingcenters(Query query = null)
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

        partial void OnTallyingcenterGet(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);
        partial void OnGetTallyingcenterByCenterId(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> items);


        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> GetTallyingcenterByCenterId(int centerid)
        {
            var items = Context.Tallyingcenters
                              .AsNoTracking()
                              .Where(i => i.CenterID == centerid);

 
            OnGetTallyingcenterByCenterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTallyingcenterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTallyingcenterCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);
        partial void OnAfterTallyingcenterCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> CreateTallyingcenter(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter tallyingcenter)
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

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> CancelTallyingcenterChanges(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTallyingcenterUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);
        partial void OnAfterTallyingcenterUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> UpdateTallyingcenter(int centerid, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter tallyingcenter)
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

        partial void OnTallyingcenterDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);
        partial void OnAfterTallyingcenterDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> DeleteTallyingcenter(int centerid)
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

        partial void OnVotersRead(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> items);

        public async Task<IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter>> GetVoters(Query query = null)
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

        partial void OnVoterGet(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);
        partial void OnGetVoterByVoterId(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> items);


        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> GetVoterByVoterId(long voterid)
        {
            var items = Context.Voters
                              .AsNoTracking()
                              .Where(i => i.VoterID == voterid);

 
            OnGetVoterByVoterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVoterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVoterCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);
        partial void OnAfterVoterCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> CreateVoter(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter voter)
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

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> CancelVoterChanges(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVoterUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);
        partial void OnAfterVoterUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> UpdateVoter(long voterid, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter voter)
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

        partial void OnVoterDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);
        partial void OnAfterVoterDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> DeleteVoter(long voterid)
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

        partial void OnVotesRead(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> items);

        public async Task<IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote>> GetVotes(Query query = null)
        {
            var items = Context.Votes.AsQueryable();

            items = items.Include(i => i.Candidate);
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

        partial void OnVoteGet(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);
        partial void OnGetVoteByVoteId(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> items);


        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> GetVoteByVoteId(int voteid)
        {
            var items = Context.Votes
                              .AsNoTracking()
                              .Where(i => i.VoteID == voteid);

            items = items.Include(i => i.Candidate);
            items = items.Include(i => i.Voter);
 
            OnGetVoteByVoteId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVoteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVoteCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);
        partial void OnAfterVoteCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> CreateVote(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote vote)
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

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> CancelVoteChanges(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVoteUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);
        partial void OnAfterVoteUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> UpdateVote(int voteid, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote vote)
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

        partial void OnVoteDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);
        partial void OnAfterVoteDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> DeleteVote(int voteid)
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

        partial void OnVotetalliesRead(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> items);

        public async Task<IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally>> GetVotetallies(Query query = null)
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

        partial void OnVotetallyGet(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);
        partial void OnGetVotetallyByTallyId(ref IQueryable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> items);


        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> GetVotetallyByTallyId(int tallyid)
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

        partial void OnVotetallyCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);
        partial void OnAfterVotetallyCreated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> CreateVotetally(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally votetally)
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

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> CancelVotetallyChanges(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVotetallyUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);
        partial void OnAfterVotetallyUpdated(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> UpdateVotetally(int tallyid, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally votetally)
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

        partial void OnVotetallyDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);
        partial void OnAfterVotetallyDeleted(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally item);

        public async Task<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> DeleteVotetally(int tallyid)
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