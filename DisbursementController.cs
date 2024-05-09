using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Text;
using TenantCompany.Helpers;
using TenantCompany.Models;
using System;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


namespace TenantCompany.Controllers
{
    public class DisbursementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;
        private readonly AesHmacEncryption enc;
        private readonly HttpClient _httpClient;

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public DisbursementController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            enc = new AesHmacEncryption(_configuration);

            BaseUrl = configuration["BaseUrlDisbursement"];
        }


        // GET: DisbursementController
        public async Task<IActionResult> Index(string param1)
        {
            string ActionIds = Request.Query["param1"];
            string[] actionIdArray = ActionIds.Split(',').Select(s => s.Trim()).ToArray();
            ViewBag.MyArray = actionIdArray;
            string urlParameters = "GetDisbursement";
            using (var httpClient = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(new { });
                StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");

                //return View();

                HttpResponseMessage response = await httpClient.PostAsync(BaseUrl + urlParameters, Content);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the API response to your desired data model or use dynamic if the structure is unknown.
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<DisbursementModel> DisbursementView = JsonConvert.DeserializeObject<List<DisbursementModel>>(responseContent);


                 
                    return View(DisbursementView);
                }
                else
                {
                    return View();

                   // return View("Error");
                }
            }
         
        }


		[HttpGet]
		public IActionResult GetSessionValue()
		{
			var sessionValue = HttpContext.Session.GetString("UserData");
			return Json(new { sessionValue });
		}



		// GET: DisbursementController/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DisbursementController/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SaveHeadDisbursement(DisbursementModel model)
        {
            try
            {
                string urlparameters = "SaveHeadDisbursement";
                model.TenantID = 2;

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl + urlparameters, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Disbursement Profile Updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.ToString());

                // Return a 500 Internal Server Error response with an error message
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
		[HttpPost]
		public async Task<ActionResult> SaveDtlsDisbursement(DisbursementDetails model)
		{
			try
			{
				string urlparameters = "SaveDtlsDisbursement";
				model.TenantID = 2;
                model.DTLS_DISBURSEMENT_KEY = 0;
                if (model.HEAD_DISBURSEMENT_KEY == null)
                {
                    model.HEAD_DISBURSEMENT_KEY = 0;

				}
				var Sessiondata1 = HttpContext.Session.GetString("UserData");
				VM_UserLoginResponse sessionData = JsonConvert.DeserializeObject<VM_UserLoginResponse>(Sessiondata1);

				model.CompanyKey = Convert.ToInt32(sessionData.Company);
                model.ENT_USER_KEY= Convert.ToInt32(sessionData.Employee_Master_Key);

				string data = JsonConvert.SerializeObject(model);
				StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                model.DISBURSING_AMOUNT = 0;
				HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl + urlparameters, content);
				if (response.IsSuccessStatusCode)
				{
					var responseData = await response.Content.ReadAsStringAsync();

					var result = new { id = responseData };
					return Json(result);
				}
				else
				{

					var errorData = await response.Content.ReadAsStringAsync();
					var result = new { id = errorData };
					return Json(result);

				}
			}
			catch (Exception ex)
			{
				// Log the exception for debugging purposes
				Console.WriteLine(ex.ToString());

				// Return a 500 Internal Server Error response with an error message
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}

		[HttpPost]
        public async Task<JsonResult> FillNewCostCenters(int CompanyKey)
        {
            //string urlParametrs = "GetNewCostCentersName";
            string urlParameters = $"GetNewCostCentersName/{CompanyKey}";
            using (var httpClient = new HttpClient())
            {
                // var req = new { CompanyId = CompanyKey };

                // var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                //var response = await httpClient.PostAsync(BaseUrl + urlParametrs, content);
                var response = await httpClient.GetAsync(BaseUrl + urlParameters);

                if (response.IsSuccessStatusCode)
                {

                    var data = await response.Content.ReadAsStringAsync();

                    return Json(data);

                    //var responseData = await response.Content.ReadAsStringAsync();
                    //var pDataTypeList = JsonConvert.DeserializeObject<List<CustomSelectListItem2>>(responseData);
                    //var pDataTypeList = JsonConvert.DeserializeObject<List<object>>(responseData);

                    //ViewBag.PDataTypeList = pDataTypeList;

                    // Process the list of DataRow objects as needed

                    //var result = new { id = pDataTypeList };
                    //return Json(result);
                }
                else
                {
                    // Handle the error response
                    var responseData = await response.Content.ReadAsStringAsync();
                    var errorData = await response.Content.ReadAsStringAsync();
                    var result = new { id = errorData };
                    return Json(result);
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetDivision(string[] dataArray)
        {
            string combinedString = string.Join(",", dataArray);

            var httpclient = new HttpClient();
            string urlparameters = "GET_Division/";
            //var CompanyId = 1;
            var url = BaseUrl + urlparameters + combinedString ;
            var response = await httpclient.GetAsync(url);
            //HttpResponseMessage response = _httpClient.GetAsync(BaseUrl + id).Result;
            if (response.IsSuccessStatusCode)
            {

                var respondata = await response.Content.ReadAsStringAsync();
                return Json(respondata);
                //    var list = JsonConvert.DeserializeObject<List<ApprovalShowModel>>(respondata);

                //    ViewBag.EnquiryList = list;
                //    return View();
            }
            else
            {
                return View("error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetWarehouse(string[] dataArray)
        {
            string combinedString = string.Join(",", dataArray);

            var httpclient = new HttpClient();
            string urlparameters = "GET_WAREHOUSE/";
            var CompanyId = 1;
            var url = BaseUrl + urlparameters + combinedString; // + "/" + CompanyId.ToString();
            var response = await httpclient.GetAsync(url);
            //HttpResponseMessage response = _httpClient.GetAsync(BaseUrl + id).Result;
            if (response.IsSuccessStatusCode)
            {

                var respondata = await response.Content.ReadAsStringAsync();
                return Json(respondata);
                //    var list = JsonConvert.DeserializeObject<List<ApprovalShowModel>>(respondata);

                //    ViewBag.EnquiryList = list;
                //    return View();
            }
            else
            {
                return View("error");
            }
        }


		[HttpGet]
		public async Task<IActionResult> GetSubDivision(int id1,int id2,string recType)
		{
			
			var httpclient = new HttpClient();
			string urlparameters = "GetSubDivision";
            
			//var CompanyId = 1;
			var url = BaseUrl + urlparameters +"/"+ id1+ "/" + id2+ "/" + recType;
			var response = await httpclient.GetAsync(url);
			//HttpResponseMessage response = _httpClient.GetAsync(BaseUrl + id).Result;
			if (response.IsSuccessStatusCode)
			{

				var respondata = await response.Content.ReadAsStringAsync();
				return Json(respondata);
				//    var list = JsonConvert.DeserializeObject<List<ApprovalShowModel>>(respondata);

				//    ViewBag.EnquiryList = list;
				//    return View();
			}
			else
			{
				return View("error");
			}
		}

		[HttpGet]
        public async Task<JsonResult> GetDisbursementType(int id)

        {
            string urlParametrs = "GetDisbursementType" + "/" + id;
            var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("http://192.168.2.112:5042/api/TenantCompanyProfiles");
            //httpClient.BaseAddress = new Uri("http://192.168.2.112:5042/api/TenantCompanyProfiles");
            var response = await httpClient.GetAsync(BaseUrl + urlParametrs);


            //var response = await httpClient.GetAsync("GetState");
            if (response.IsSuccessStatusCode)
            {

                var responseData = await response.Content.ReadAsStringAsync();
                var pDataTypeList = JsonConvert.DeserializeObject<List<DisbursementDetails>>(responseData);


                return Json(pDataTypeList);
            }
            else
            {
                // Handle the error response
                var responseData = await response.Content.ReadAsStringAsync();
                var errorData = await response.Content.ReadAsStringAsync();
                var result = new { id = errorData };
                return Json(result);
            }


        }
        [HttpPost]
        public async Task<JsonResult> GetDisbursementDetails(DisbursementDetails model, string[] dataArray)
        {
			string pattern = @"count=\d+";
			string patternFlg = @" flag=\d+";
			model.combinedString = string.Join(",", dataArray); //model.combinedString

            int count = 0;
            int flag = 0;
            //model.TO_DT = DateTime.ParseExact("09-30-2023", "MM-dd-yyyy", CultureInfo.InvariantCulture).Date;
            var data = HttpContext.Session.GetString("UserData");
			VM_UserLoginResponse sessionData = JsonConvert.DeserializeObject<VM_UserLoginResponse>(data);

            model.CompanyKey = Convert.ToInt32(sessionData.Company);

			//Convert.ToDateTime("09-30-2023").ToString("MM-dd-yyyy");
			var httpclient = new HttpClient();
            string urlparameters = "GET_DISBURSEMENT_DETAILS";
            var CompanyId = 1;
            //var url = BaseUrl + urlparameters + model +"/"+ combinedString; // + "/" + CompanyId.ToString();
            //var dataToSerialize = new { Approval_level_One_key = model.Approval_level_One_key };

            
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(BaseUrl + urlparameters, content);
            //var response = await httpclient.GetAsync(url);
            //HttpResponseMessage response = _httpClient.GetAsync(BaseUrl + id).Result;
            if (response.IsSuccessStatusCode)
            {

                var respondata = await response.Content.ReadAsStringAsync();


                // Use Regex.Match to find the first match in the input
                //Match match1 = Regex.Match(respondata, pattern);
                //Match match2 = Regex.Match(respondata, patternFlg);
                pattern = @"count=(\d+)";
                patternFlg = @"Completeflag=(\d+)";
                Match countMatch = Regex.Match(respondata, @"count=(\d+)");
                Match flagMatch = Regex.Match(respondata, @"Completeflag=(\d+)");

                // Extract count and flag values
                if (countMatch.Success && flagMatch.Success)
                {
                     count = int.Parse(countMatch.Groups[1].Value);
                     flag = int.Parse(flagMatch.Groups[1].Value);
                }

                    // If a match is found, return the matched value; otherwise, return an empty string
                    //string count = match1.Value;
				//string flag = match2.Value;
                //count = respondata.Substring((respondata.Length + 6) - count.Length );
                //count = respondata.Substring((respondata.Length + 6+6) - (count.Length+flag.Length));
                ViewBag.test = Convert.ToInt32(count);

				ViewBag.COMPLETE_FLAG = Convert.ToInt32(flag);

				string replacedData = Regex.Replace(respondata, pattern, "");
                replacedData = Regex.Replace(replacedData, patternFlg, "");

                return Json(new { rowCount = count, flag = flag, html = replacedData });
              //  return Json(new { rowCount = count, flag=flag, html = Regex.Replace(respondata, pattern , "") });
				//// If a match is found, return the matched value; otherwise, return an empty string
				//string test = match1.Value;
				//test= respondata.Substring((respondata.Length+6)-test.Length);
				//            ViewBag.test = Convert.ToInt32(test);
				//return Json( new { rowCount = test, html = Regex.Replace(respondata, pattern, "") });

			}
            else
            {
                return Json("error");
            }

          //  string urlparametrs = "GET_DISBURSEMENT_DETAILS";
          //  var httpclient = new HttpClient();


          ////  var dataToSerialize = new { Approval_level_One_key = model.Approval_level_One_key };

          //  var content = new StringContent(JsonConvert.SerializeObject(dataToSerialize), Encoding.UTF8, "application/json");

          //  var response = await httpclient.PostAsync(BaseUrl + urlparametrs, content);
          //  if (response.IsSuccessStatusCode)
          //  {
          //      var responseData = await response.Content.ReadAsStringAsync();

          //      //var result = new { responseData };

          //      return Json(responseData);
          //  }
          //  else
          //  {
          //      return Json("Error");
          //  }

        }

		[HttpGet]
		public async Task<JsonResult> GetDisbursementHead(string recType,int id)

		{
			string urlParameters = "GetDisbursementHead/";
			var httpClient = new HttpClient();
			//httpClient.BaseAddress = new Uri("http://192.168.2.112:5042/api/TenantCompanyProfiles");

			var response = await httpClient.GetAsync(BaseUrl + urlParameters + recType +"/"+ id);

			if (response.IsSuccessStatusCode)
			{

				var responseData = await response.Content.ReadAsStringAsync();
				var pDataTypeList = JsonConvert.DeserializeObject<List<DisbursementDetails>>(responseData);


				return Json(pDataTypeList);
			}
			else
			{
				// Handle the error response
				var responseData = await response.Content.ReadAsStringAsync();
				var errorData = await response.Content.ReadAsStringAsync();
				var result = new { id = errorData };
				return Json(result);
			}


		}

		[HttpGet]
		public async Task<JsonResult> SendApproval(int id)

		{
			string urlParameters = "SendApproval/";
			var httpClient = new HttpClient();
			//httpClient.BaseAddress = new Uri("http://192.168.2.112:5042/api/TenantCompanyProfiles");

			var response = await httpClient.GetAsync(BaseUrl + urlParameters + id);

			if (response.IsSuccessStatusCode)
			{
				//var responseData = await response.Content.ReadAsStringAsync();
				//var pDataTypeList = JsonConvert.DeserializeObject<List<DisbursementDetails>>(responseData);

				var responseData = await response.Content.ReadAsStringAsync();

				var result = new { id = responseData };
				return Json(result);
				//return Json(pDataTypeList);
			}
			else
			{
				// Handle the error response
				var responseData = await response.Content.ReadAsStringAsync();
				var errorData = await response.Content.ReadAsStringAsync();
				var result = new { id = errorData };
				return Json(result);
			}


		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DisbursementController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DisbursementController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DisbursementController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DisbursementController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
