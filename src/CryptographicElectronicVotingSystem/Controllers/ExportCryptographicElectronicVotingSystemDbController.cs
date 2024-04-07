using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CryptographicElectronicVotingSystem.Data;
using CryptographicElectronicVotingSystem.Services;

namespace CryptographicElectronicVotingSystem.Controllers
{
    public partial class Exportcryptographic_electronic_voting_system_dbController : ExportController
    {
        private readonly CryptographicElectronicVotingSystemContext context;
        private readonly CryptographicElectronicVotingSystemService service;

        public Exportcryptographic_electronic_voting_system_dbController(CryptographicElectronicVotingSystemContext context, CryptographicElectronicVotingSystemService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/candidates/csv")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/candidates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCandidatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/candidates/excel")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/candidates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCandidatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/tallyingcenters/csv")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/tallyingcenters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTallyingcentersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/tallyingcenters/excel")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/tallyingcenters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTallyingcentersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/voters/csv")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/voters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/voters/excel")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/voters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/votes/csv")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/votes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/votes/excel")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/votes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/votetallies/csv")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/votetallies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotetalliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVotetallies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/cryptographic_electronic_voting_system_db/votetallies/excel")]
        [HttpGet("/export/cryptographic_electronic_voting_system_db/votetallies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotetalliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVotetallies(), Request.Query, false), fileName);
        }
    }
}
