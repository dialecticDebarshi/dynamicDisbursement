
using API_Disbursement.Models;
using Microsoft.AspNetCore.Mvc;
using API_Disbursement.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace API_Disbursement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisbursementController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly UserAccountRepository _AccountRepository;

        public DisbursementController(IConfiguration configuration, UserAccountRepository accountRepository)
        {
            _configuration = configuration;
            _AccountRepository = accountRepository;
           
        }

       

        [HttpPost]
        [Route("GetDisbursement")]
        public List<object> GetDisbursement()
        {
            List<object> dt = _AccountRepository.GetDisbursement();
            return dt;

        }

        [HttpPost]
        [Route("SaveHeadDisbursement")]
        public int SaveHeadDisbursement(Disbursement disburse)
        {
            int r = _AccountRepository.SaveHeadDisbursement(disburse);
            return r;
        }

		[HttpPost]
		[Route("SaveDtlsDisbursement")]
		public int SaveDtlsDisbursement(DisbursementDetails disburse)
		{
			int r = _AccountRepository.SaveDtlsDisbursement(disburse);
			return r;
		}
		
		[HttpGet]
        [Route("GetNewCostCentersName/{id}")]
        public string GetNewCostCentersName(int id)
        {
            string dt = _AccountRepository.GetNewCostCentersName(id);
            return dt;

        }
		[HttpGet]
		[Route("SendApproval/{id}")]
		public int SendApproval(int id)
		{
			int dt = _AccountRepository.SendApproval(id);
			return dt;

		}

		[HttpGet("GET_Division/{combinedString}")]
        public string GET_Division(string combinedString)
        {
            return _AccountRepository.GET_Division(combinedString);

        }

      

        [HttpGet("GET_WAREHOUSE/{combinedString}")]
        public string GET_WAREHOUSE(string combinedString)
        {
            return _AccountRepository.GET_WAREHOUSE(combinedString);

        }


		[HttpGet("GetSubDivision/{id1}/{id2}/{recType}")]
		public List<string> GetSubDivision(int id1, int id2, string recType)
		{
			return _AccountRepository.GetSubDivision(id1,id2, recType);

		}



        [HttpGet("GetDisbursementType/{id}")]
       // [Route("GetDisbursementType")]
        public List<object> GetDisbursementType(int id)
        {
            List<object> dt = _AccountRepository.GetDisbursementType(id);
            return dt;

        }

		[HttpGet]

		[Route("GetDisbursementHead/{recType}/{id}")]
		public List<object> GetDisbursementHead(string recType, int id)
		{
			List<object> dt = _AccountRepository.GetDisbursementHead(recType, id);
			return dt;

		}

		[HttpPost]
        [Route("GET_DISBURSEMENT_DETAILS")]
        public string GET_DISBURSEMENT_DETAILS(DisbursementDetails model)
        {
            return _AccountRepository.GET_DISBURSEMENT_DETAILS(model);


        }

        //[HttpGet]
        //[Route("GetTenantGroupHeadMaster")]
        //public List<SelectListItem> GetTenantGroupHeadMaster()
        //{
        //    List<SelectListItem> dt = _AccountRepository.GetTenantGroupHeadMaster();
        //    return dt;

        //}

        //[HttpGet]
        //[Route("GetGroupHeadType")]
        //public List<SelectListItem> GetGroupHeadType()
        //{
        //    List<SelectListItem> dt = _AccountRepository.GetGroupHeadType();
        //    return dt;

        //}
        //[HttpGet]
        //[Route("VerifyUserName/{Name}")]
        //public List<object> VerifyUserName(string Name)
        //{
        //    List<object> dt = _AccountRepository.VerifyUserName(Name);
        //    return dt;

        //}
     
    }
}
