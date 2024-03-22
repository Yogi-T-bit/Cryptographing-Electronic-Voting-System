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

using CryptographingElectronicVotingSystem.Dal.Data;

namespace CryptographingElectronicVotingSystem.Service.Services
{
    public partial class ElectronicVotingSystemService
    {
        ElectronicVotingSystemContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ElectronicVotingSystemContext context;
        private readonly NavigationManager navigationManager;

        public ElectronicVotingSystemService(ElectronicVotingSystemContext context, NavigationManager navigationManager)
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


        public async Task ExportcandidatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/candidates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/candidates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportcandidatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/candidates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/candidates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OncandidatesRead(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> items);

        public async Task<IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate>> Getcandidates(Query query = null)
        {
            var items = Context.candidates.AsQueryable();


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

            OncandidatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OncandidateGet(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);
        partial void OnGetcandidateByCandidateId(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> items);


        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> GetcandidateByCandidateId(int candidateid)
        {
            var items = Context.candidates
                              .AsNoTracking()
                              .Where(i => i.CandidateID == candidateid);

 
            OnGetcandidateByCandidateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OncandidateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OncandidateCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);
        partial void OnAftercandidateCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> Createcandidate(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate candidate)
        {
            OncandidateCreated(candidate);

            var existingItem = Context.candidates
                              .Where(i => i.CandidateID == candidate.CandidateID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.candidates.Add(candidate);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(candidate).State = EntityState.Detached;
                throw;
            }

            OnAftercandidateCreated(candidate);

            return candidate;
        }

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> CancelcandidateChanges(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OncandidateUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);
        partial void OnAftercandidateUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> Updatecandidate(int candidateid, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate candidate)
        {
            OncandidateUpdated(candidate);

            var itemToUpdate = Context.candidates
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

            OnAftercandidateUpdated(candidate);

            return candidate;
        }

        partial void OncandidateDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);
        partial void OnAftercandidateDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> Deletecandidate(int candidateid)
        {
            var itemToDelete = Context.candidates
                              .Where(i => i.CandidateID == candidateid)
                              .Include(i => i.votes)
                              .Include(i => i.votetallies)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OncandidateDeleted(itemToDelete);


            Context.candidates.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftercandidateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExporttallyingcentersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/tallyingcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/tallyingcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExporttallyingcentersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/tallyingcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/tallyingcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OntallyingcentersRead(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> items);

        public async Task<IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter>> Gettallyingcenters(Query query = null)
        {
            var items = Context.tallyingcenters.AsQueryable();


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

            OntallyingcentersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OntallyingcenterGet(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);
        partial void OnGettallyingcenterByCenterId(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> items);


        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> GettallyingcenterByCenterId(int centerid)
        {
            var items = Context.tallyingcenters
                              .AsNoTracking()
                              .Where(i => i.CenterID == centerid);

 
            OnGettallyingcenterByCenterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OntallyingcenterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OntallyingcenterCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);
        partial void OnAftertallyingcenterCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> Createtallyingcenter(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter tallyingcenter)
        {
            OntallyingcenterCreated(tallyingcenter);

            var existingItem = Context.tallyingcenters
                              .Where(i => i.CenterID == tallyingcenter.CenterID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.tallyingcenters.Add(tallyingcenter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tallyingcenter).State = EntityState.Detached;
                throw;
            }

            OnAftertallyingcenterCreated(tallyingcenter);

            return tallyingcenter;
        }

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> CanceltallyingcenterChanges(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OntallyingcenterUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);
        partial void OnAftertallyingcenterUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> Updatetallyingcenter(int centerid, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter tallyingcenter)
        {
            OntallyingcenterUpdated(tallyingcenter);

            var itemToUpdate = Context.tallyingcenters
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

            OnAftertallyingcenterUpdated(tallyingcenter);

            return tallyingcenter;
        }

        partial void OntallyingcenterDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);
        partial void OnAftertallyingcenterDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> Deletetallyingcenter(int centerid)
        {
            var itemToDelete = Context.tallyingcenters
                              .Where(i => i.CenterID == centerid)
                              .Include(i => i.votetallies)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OntallyingcenterDeleted(itemToDelete);


            Context.tallyingcenters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftertallyingcenterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportvotersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/voters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/voters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportvotersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/voters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/voters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnvotersRead(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> items);

        public async Task<IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter>> Getvoters(Query query = null)
        {
            var items = Context.voters.AsQueryable();


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

            OnvotersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnvoterGet(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);
        partial void OnGetvoterByVoterId(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> items);


        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> GetvoterByVoterId(int voterid)
        {
            var items = Context.voters
                              .AsNoTracking()
                              .Where(i => i.VoterID == voterid);

 
            OnGetvoterByVoterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnvoterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnvoterCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);
        partial void OnAftervoterCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> Createvoter(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter voter)
        {
            OnvoterCreated(voter);

            var existingItem = Context.voters
                              .Where(i => i.VoterID == voter.VoterID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.voters.Add(voter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(voter).State = EntityState.Detached;
                throw;
            }

            OnAftervoterCreated(voter);

            return voter;
        }

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> CancelvoterChanges(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvoterUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);
        partial void OnAftervoterUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> Updatevoter(int voterid, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter voter)
        {
            OnvoterUpdated(voter);

            var itemToUpdate = Context.voters
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

            OnAftervoterUpdated(voter);

            return voter;
        }

        partial void OnvoterDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);
        partial void OnAftervoterDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> Deletevoter(int voterid)
        {
            var itemToDelete = Context.voters
                              .Where(i => i.VoterID == voterid)
                              .Include(i => i.votes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnvoterDeleted(itemToDelete);


            Context.voters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftervoterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportvotesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/votes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/votes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportvotesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/votes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/votes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnvotesRead(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> items);

        public async Task<IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote>> Getvotes(Query query = null)
        {
            var items = Context.votes.AsQueryable();

            items = items.Include(i => i.candidate);
            items = items.Include(i => i.voter);

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

            OnvotesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnvoteGet(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);
        partial void OnGetvoteByVoteId(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> items);


        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> GetvoteByVoteId(int voteid)
        {
            var items = Context.votes
                              .AsNoTracking()
                              .Where(i => i.VoteID == voteid);

            items = items.Include(i => i.candidate);
            items = items.Include(i => i.voter);
 
            OnGetvoteByVoteId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnvoteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnvoteCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);
        partial void OnAftervoteCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> Createvote(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote vote)
        {
            OnvoteCreated(vote);

            var existingItem = Context.votes
                              .Where(i => i.VoteID == vote.VoteID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.votes.Add(vote);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(vote).State = EntityState.Detached;
                throw;
            }

            OnAftervoteCreated(vote);

            return vote;
        }

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> CancelvoteChanges(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvoteUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);
        partial void OnAftervoteUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> Updatevote(int voteid, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote vote)
        {
            OnvoteUpdated(vote);

            var itemToUpdate = Context.votes
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

            OnAftervoteUpdated(vote);

            return vote;
        }

        partial void OnvoteDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);
        partial void OnAftervoteDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote> Deletevote(int voteid)
        {
            var itemToDelete = Context.votes
                              .Where(i => i.VoteID == voteid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnvoteDeleted(itemToDelete);


            Context.votes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftervoteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportvotetalliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/votetallies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/votetallies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportvotetalliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ElectronicVotingSystem/votetallies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ElectronicVotingSystem/votetallies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnvotetalliesRead(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> items);

        public async Task<IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally>> Getvotetallies(Query query = null)
        {
            var items = Context.votetallies.AsQueryable();

            items = items.Include(i => i.candidate);
            items = items.Include(i => i.tallyingcenter);

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

            OnvotetalliesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnvotetallyGet(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);
        partial void OnGetvotetallyByTallyId(ref IQueryable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> items);


        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> GetvotetallyByTallyId(int tallyid)
        {
            var items = Context.votetallies
                              .AsNoTracking()
                              .Where(i => i.TallyID == tallyid);

            items = items.Include(i => i.candidate);
            items = items.Include(i => i.tallyingcenter);
 
            OnGetvotetallyByTallyId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnvotetallyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnvotetallyCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);
        partial void OnAftervotetallyCreated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> Createvotetally(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally votetally)
        {
            OnvotetallyCreated(votetally);

            var existingItem = Context.votetallies
                              .Where(i => i.TallyID == votetally.TallyID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.votetallies.Add(votetally);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(votetally).State = EntityState.Detached;
                throw;
            }

            OnAftervotetallyCreated(votetally);

            return votetally;
        }

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> CancelvotetallyChanges(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvotetallyUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);
        partial void OnAftervotetallyUpdated(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> Updatevotetally(int tallyid, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally votetally)
        {
            OnvotetallyUpdated(votetally);

            var itemToUpdate = Context.votetallies
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

            OnAftervotetallyUpdated(votetally);

            return votetally;
        }

        partial void OnvotetallyDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);
        partial void OnAftervotetallyDeleted(CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally item);

        public async Task<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> Deletevotetally(int tallyid)
        {
            var itemToDelete = Context.votetallies
                              .Where(i => i.TallyID == tallyid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnvotetallyDeleted(itemToDelete);


            Context.votetallies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftervotetallyDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}