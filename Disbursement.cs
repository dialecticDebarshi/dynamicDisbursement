using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Xml.Linq;

namespace API_Disbursement.Models
{
    public class Disbursement
    {
        //public IFormFile? FileUpload { set; get; }

        //public string? FilePath { get; set; }

        public int? TenantID { get; set; }
		// public string? LocationName { get; set; }

		public int? CompanyKey { get; set; }
		public int? HEAD_DISBURSEMENT_KEY { get; set; }
        public int? HEAD_KEY { get; set; }



        public string? DISBURSEMENT_HEADING { get; set; }

        public string? DISBURSEMENT_REFNO { get; set; }
        public string? DISBURSEMENT_REF_DATE { get; set; }
        public string? DISBURSEMENT_REMARKS { get; set; }
		
		public int? MAST_DISBURSEMENT_TYPE_KEY { get; set; }

		public int? COMPLETE_FLAG { get; set; }

		public int? APPROVAL_TAG { get; set; }

	}
	public class DisbursementDetails : Disbursement
	{
		public string? ENT_DATE { get; set; }

		//EDIT_DATE

		public int? ENT_USER_KEY { get; set; }
		//EDIT_USER_KEY
		public int? EMPLOYEE_MASTER_KEY { get; set; }
		public int? TAG_DELETE { get; set; }
		public int? DTLS_DISBURSEMENT_KEY { get; set; }
		public int? DTLS_KEY { get; set; }
		public int? PAY_TYPE_KEY { get; set; }
		

		public string? DISBURSEMENT_TYPE { get; set; }
		public decimal? NET_PAYABLE_AMOUNT { get; set; }
		public decimal? DISBURSING_AMOUNT { get; set; }

		public DateTime? FR_DT { get; set; }

		public DateTime? TO_DT { get; set; }
		public string? combinedString { get; set; }

		public string? SALARY_REFNO { get; set; }

		public List<DataDictionary>? disburseList { get; set; }


		public string? selectedValuescostcenter { get; set; }
		public string? selectedValuesdivision { get; set; }
		public string? selectedValueswarehouse { get; set; }

		public string? selectedValuesEmp { get; set; }
		public string? selectedValuesDisburse { get; set; }

	}
    public class DataDictionary
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? NetAmnt { get; set; }
    }
    //   public partial class DisbursementDetails : Disbursement
    //   {
    //       public string? ENT_DATE { get; set; }

    //       //EDIT_DATE

    //       public int? ENT_USER_KEY { get; set; }
    //       //EDIT_USER_KEY
    //       public int? EMPLOYEE_MASTER_KEY { get; set; }
    //       public int? TAG_DELETE { get; set; }
    //       public int? DTLS_DISBURSEMENT_KEY { get; set; }
    //       public int? DTLS_KEY { get; set; }
    //       public int? PAY_TYPE_KEY { get; set; }
    //       public int? MAST_DISBURSEMENT_TYPE_KEY { get; set; }
    //       public decimal? NET_PAYABLE_AMOUNT { get; set; }

    //       public decimal? DISBURSING_AMOUNT { get; set; }

    //       public DateTime? FR_DT { get; set; }

    //       public DateTime? TO_DT { get; set; }

    //       public string? combinedString { get; set; }

    //	public int? HEAD_KEY { get; set; }

    //	public string? SALARY_REFNO { get; set; }

    //       public List<DataDictionary> disburseList { get; set; }


    //	public string? selectedValuescostcenter { get; set; }
    //	public string? selectedValuesdivision { get; set; }
    //	public string? selectedValueswarehouse { get; set; }


    //}
    //public class DataDictionary
    //{
    //	public string? Key { get; set; }
    //	public string? Value { get; set; }
    //}

}
