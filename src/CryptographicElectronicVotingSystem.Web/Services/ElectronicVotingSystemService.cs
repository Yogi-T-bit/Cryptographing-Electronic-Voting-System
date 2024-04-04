using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using CryptographicElectronicVotingSystem.Dal.Data;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace CryptographicElectronicVotingSystem.Web.Services
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

        partial void OncandidatesRead(ref IQueryable<candidate> items);

        public async Task<IQueryable<candidate>> Getcandidates(Query query = null)
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

        partial void OncandidateGet(candidate item);
        partial void OnGetcandidateByCandidateId(ref IQueryable<candidate> items);


        public async Task<candidate> GetcandidateByCandidateId(int candidateid)
        {
            var items = Context.candidates
                              .AsNoTracking()
                              .Where(i => i.CandidateID == candidateid);

 
            OnGetcandidateByCandidateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OncandidateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OncandidateCreated(candidate item);
        partial void OnAftercandidateCreated(candidate item);

        public async Task<candidate> Createcandidate(candidate candidate)
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

        public async Task<candidate> CancelcandidateChanges(candidate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OncandidateUpdated(candidate item);
        partial void OnAftercandidateUpdated(candidate item);

        public async Task<candidate> Updatecandidate(int candidateid, candidate candidate)
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

        partial void OncandidateDeleted(candidate item);
        partial void OnAftercandidateDeleted(candidate item);

        public async Task<candidate> Deletecandidate(int candidateid)
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

        partial void OntallyingcentersRead(ref IQueryable<tallyingcenter> items);

        public async Task<IQueryable<tallyingcenter>> Gettallyingcenters(Query query = null)
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

        partial void OntallyingcenterGet(tallyingcenter item);
        partial void OnGettallyingcenterByCenterId(ref IQueryable<tallyingcenter> items);


        public async Task<tallyingcenter> GettallyingcenterByCenterId(int centerid)
        {
            var items = Context.tallyingcenters
                              .AsNoTracking()
                              .Where(i => i.CenterID == centerid);

 
            OnGettallyingcenterByCenterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OntallyingcenterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OntallyingcenterCreated(tallyingcenter item);
        partial void OnAftertallyingcenterCreated(tallyingcenter item);

        public async Task<tallyingcenter> Createtallyingcenter(tallyingcenter tallyingcenter)
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

        public async Task<tallyingcenter> CanceltallyingcenterChanges(tallyingcenter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OntallyingcenterUpdated(tallyingcenter item);
        partial void OnAftertallyingcenterUpdated(tallyingcenter item);

        public async Task<tallyingcenter> Updatetallyingcenter(int centerid, tallyingcenter tallyingcenter)
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

        partial void OntallyingcenterDeleted(tallyingcenter item);
        partial void OnAftertallyingcenterDeleted(tallyingcenter item);

        public async Task<tallyingcenter> Deletetallyingcenter(int centerid)
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

        partial void OnvotersRead(ref IQueryable<voter> items);

        public async Task<IQueryable<voter>> Getvoters(Query query = null)
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

        partial void OnvoterGet(voter item);
        partial void OnGetvoterByVoterId(ref IQueryable<voter> items);


        public async Task<voter> GetvoterByVoterId(int voterid)
        {
            var items = Context.voters
                              .AsNoTracking()
                              .Where(i => i.VoterID == voterid);

 
            OnGetvoterByVoterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnvoterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnvoterCreated(voter item);
        partial void OnAftervoterCreated(voter item);

        public async Task<voter> Createvoter(voter voter)
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

        public async Task<voter> CancelvoterChanges(voter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvoterUpdated(voter item);
        partial void OnAftervoterUpdated(voter item);

        public async Task<voter> Updatevoter(int voterid, voter voter)
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

        partial void OnvoterDeleted(voter item);
        partial void OnAftervoterDeleted(voter item);

        public async Task<voter> Deletevoter(long voterid)
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

        partial void OnvotesRead(ref IQueryable<vote> items);

        public async Task<IQueryable<vote>> Getvotes(Query query = null)
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

        partial void OnvoteGet(vote item);
        partial void OnGetvoteByVoteId(ref IQueryable<vote> items);


        public async Task<vote> GetvoteByVoteId(int voteid)
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

        partial void OnvoteCreated(vote item);
        partial void OnAftervoteCreated(vote item);

        public async Task<vote> Createvote(vote vote)
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

        public async Task<vote> CancelvoteChanges(vote item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvoteUpdated(vote item);
        partial void OnAftervoteUpdated(vote item);

        public async Task<vote> Updatevote(int voteid, vote vote)
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

        partial void OnvoteDeleted(vote item);
        partial void OnAftervoteDeleted(vote item);

        public async Task<vote> Deletevote(int voteid)
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

        partial void OnvotetalliesRead(ref IQueryable<votetally> items);

        public async Task<IQueryable<votetally>> Getvotetallies(Query query = null)
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

        partial void OnvotetallyGet(votetally item);
        partial void OnGetvotetallyByTallyId(ref IQueryable<votetally> items);


        public async Task<votetally> GetvotetallyByTallyId(int tallyid)
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

        partial void OnvotetallyCreated(votetally item);
        partial void OnAftervotetallyCreated(votetally item);

        public async Task<votetally> Createvotetally(votetally votetally)
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

        public async Task<votetally> CancelvotetallyChanges(votetally item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnvotetallyUpdated(votetally item);
        partial void OnAftervotetallyUpdated(votetally item);

        public async Task<votetally> Updatevotetally(int tallyid, votetally votetally)
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

        partial void OnvotetallyDeleted(votetally item);
        partial void OnAftervotetallyDeleted(votetally item);

        public async Task<votetally> Deletevotetally(int tallyid)
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