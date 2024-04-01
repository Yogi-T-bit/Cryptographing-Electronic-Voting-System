using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CryptographingElectronicVotingSystem.Dal.Data;
using CryptographingElectronicVotingSystem.Web.Services;

namespace CryptographingElectronicVotingSystem.Web.Controllers
{
    public partial class Exportelectronic_voting_systemController : ExportController
    {
        private readonly ElectronicVotingSystemContext context;
        private readonly ElectronicVotingSystemService service;

        public Exportelectronic_voting_systemController(ElectronicVotingSystemContext context, ElectronicVotingSystemService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/electronic_voting_system/candidates/csv")]
        [HttpGet("/export/electronic_voting_system/candidates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportcandidatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getcandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/candidates/excel")]
        [HttpGet("/export/electronic_voting_system/candidates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportcandidatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getcandidates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/tallyingcenters/csv")]
        [HttpGet("/export/electronic_voting_system/tallyingcenters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExporttallyingcentersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Gettallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/tallyingcenters/excel")]
        [HttpGet("/export/electronic_voting_system/tallyingcenters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExporttallyingcentersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Gettallyingcenters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/voters/csv")]
        [HttpGet("/export/electronic_voting_system/voters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getvoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/voters/excel")]
        [HttpGet("/export/electronic_voting_system/voters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getvoters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/votes/csv")]
        [HttpGet("/export/electronic_voting_system/votes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getvotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/votes/excel")]
        [HttpGet("/export/electronic_voting_system/votes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getvotes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/votetallies/csv")]
        [HttpGet("/export/electronic_voting_system/votetallies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotetalliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getvotetallies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/electronic_voting_system/votetallies/excel")]
        [HttpGet("/export/electronic_voting_system/votetallies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportvotetalliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getvotetallies(), Request.Query, false), fileName);
        }
    }
}
