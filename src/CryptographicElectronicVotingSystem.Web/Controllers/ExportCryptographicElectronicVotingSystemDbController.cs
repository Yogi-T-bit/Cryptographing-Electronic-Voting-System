using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Web.Controllers;
using CryptographicElectronicVotingSystem.Dal.Data;
using CryptographicElectronicVotingSystem.Web.Services;

namespace CryptographicElectronicVotingSystem.Web.Controllers
{
    public partial class ExportCryptographicElectronicVotingSystemController : ExportController
    {
        private readonly CryptographicElectronicVotingSystemContext context;
        private readonly CryptographicElectronicVotingSystemService service;

        public ExportCryptographicElectronicVotingSystemController(CryptographicElectronicVotingSystemContext context, CryptographicElectronicVotingSystemService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/candidates/csv")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/candidates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCandidatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/candidates/excel")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/candidates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCandidatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/tallyingcenters/csv")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/tallyingcenters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTallyingcentersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/tallyingcenters/excel")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/tallyingcenters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTallyingcentersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/voters/csv")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/voters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/voters/excel")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/voters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/votes/csv")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/votes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/votes/excel")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/votes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/votetallies/csv")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/votetallies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotetalliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVotetallies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CryptographicElectronicVotingSystem/votetallies/excel")]
        [HttpGet("/export/CryptographicElectronicVotingSystem/votetallies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVotetalliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVotetallies(), Request.Query, false), fileName);
        }
    }
}
