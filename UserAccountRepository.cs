using API_Disbursement.Models;
//using Humanizer;
using Microsoft.Extensions.Configuration;
using System.CodeDom.Compiler;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection;
using System.Text;
using API_Disbursement.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Reflection.Metadata;


namespace API_Disbursement.Repositories
{
    public class UserAccountRepository
    {
        IConfiguration _configuration;
        DbAccess _DbAccess;
        public UserAccountRepository(IConfiguration configuration)
        {
            _configuration= configuration; 
            _DbAccess = new DbAccess(_configuration);
        }

        public List<object> GetDisbursement()
        {
            try
            {
                string[] pname = { "@REC_TYPE", "@ID" };
                string[] pvalue = { "Head_Disburse_All", "0" };
                DataSet DS = _DbAccess.Ds_Process("SP_GET_HEAD_DISBURSEMENT", pname, pvalue);
                List<object> dataList = new List<object>();

                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    dynamic dataObject = new System.Dynamic.ExpandoObject();
                    foreach (DataColumn column in DS.Tables[0].Columns)
                    {
                        var columnName = column.ColumnName;
                        var columnValue = row[column];
                        ((IDictionary<string, object>)dataObject)[columnName] = columnValue;
                    }
                    dataList.Add(dataObject);
                }

                return dataList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SaveHeadDisbursement(Disbursement DSMNT)
        {
            try
            {
              
                string[] pname = { "@HEAD_DISBURSEMENT_KEY","@DISBURSEMENT_HEADING", "@DISBURSEMENT_REFNO", "@DISBURSEMENT_REMARKS"};
                string[] pvalue = { DSMNT.HEAD_DISBURSEMENT_KEY.ToString(), DSMNT.DISBURSEMENT_HEADING.ToString(), DSMNT.DISBURSEMENT_REFNO.ToString(),  DSMNT.DISBURSEMENT_REMARKS.ToString()};
                int result = _DbAccess.ExecuteNonQuery("SP_SAVE_HEAD_DISBURSEMENT", pname, pvalue);
                if (result > 0)
                {
                    //string emailBody = "Your registration is successful! Attached is your System generated User Password"+Password; // You can customize the email body
                    //_DbAccess.SendEmail(model.ClientEmailId, "Registration Successful", emailBody);
                    return result;
                }
                else
                {
                    return 0;
                }



            }
            catch (Exception ex)
            {

                throw;
            }



        }

		private static DataTable ConvertListToDataTable(List<DataDictionary> list)
		{
			DataTable dataTable = new DataTable();

			// Add columns to match the structure of your table type
			dataTable.Columns.Add("Key", typeof(int));
			dataTable.Columns.Add("Value", typeof(decimal));
            dataTable.Columns.Add("NetAmnt", typeof(decimal));
            // Add more columns as needed

            // Add rows from the list
            foreach (var item in list)
			{
				DataRow row = dataTable.NewRow();
				row["Key"] = item.Key;
				row["Value"] = item.Value;
                row["NetAmnt"] = item.NetAmnt;
                // Set values for other columns as needed

                dataTable.Rows.Add(row);
			}

			return dataTable;
		}


		public int SaveDtlsDisbursement(DisbursementDetails DSMNTdTLS)
		{
			if(DSMNTdTLS.DISBURSING_AMOUNT==null)
            {
                DSMNTdTLS.DISBURSING_AMOUNT = 0;

			}
			
			try
			{
                DataTable dtEmpDisburse = ConvertListToDataTable(DSMNTdTLS.disburseList);
				string[] parametername = { "@TenantID","@COMPANY_KEY","@HEAD_DISBURSEMENT_KEY", "@HEAD_KEY", "@MAST_DISBURSEMENT_TYPE_KEY","@DTLS_DISBURSEMENT_KEY",
                          		"@NET_PAYABLE_AMOUNT", "@DISBURSING_AMOUNT","@COSTCENTER_KEY","@DIVISION_KEY","@WAREHOUSE_KEY","@FROM_DATE","@TO_DATE",
                    "@USER_KEY","@EMP_AMOUNT" };
				object[] parametervalue = {DSMNTdTLS.TenantID,DSMNTdTLS.CompanyKey, DSMNTdTLS.HEAD_DISBURSEMENT_KEY, DSMNTdTLS.HEAD_KEY,
					DSMNTdTLS.MAST_DISBURSEMENT_TYPE_KEY,DSMNTdTLS.DTLS_DISBURSEMENT_KEY,
					DSMNTdTLS.NET_PAYABLE_AMOUNT,DSMNTdTLS.DISBURSING_AMOUNT,DSMNTdTLS.selectedValuescostcenter,DSMNTdTLS.selectedValuesdivision,
                    DSMNTdTLS.selectedValueswarehouse,DSMNTdTLS.FR_DT.ToString(),DSMNTdTLS.TO_DT.ToString(),DSMNTdTLS.ENT_USER_KEY,dtEmpDisburse };
				SqlCommand cmd = new SqlCommand("SP_SAVE_DETAILS_DISBURSEMENT");
				cmd.CommandTimeout = 0;
				cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parametername.Length; i++)
                {

                    if (parametername[i] == "@EMP_AMOUNT")
                    {
						//cmd.Parameters.Add(parametername[i], SqlDbType.Structured);
						//cmd.Parameters.Add(dtEmpDisburse);
						SqlParameter parameter = new SqlParameter(parametername[i], SqlDbType.Structured);
						parameter.Value = dtEmpDisburse;
						cmd.Parameters.Add(parameter);

					}
                    else
                    {
                        // if (parametername[i] == "@DTLS_DISBURSEMENT_KEY")
                        //  {
						//	SqlParameter parameter1 = new SqlParameter(parametername[i], SqlDbType.Int);
						//	parameter1.Value = DSMNTdTLS.DTLS_DISBURSEMENT_KEY;
						//	cmd.Parameters.Add(parameter1);
						//	parameter1.Direction = ParameterDirection.ReturnValue;

						//}
                        cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                    }

                }
				int result = _DbAccess.ExecuteNonQuery(cmd);
				//int result = parameter1.Value;
				if (result > 0)
				{
					//string emailBody = "Your registration is successful! Attached is your System generated User Password"+Password; // You can customize the email body
					//_DbAccess.SendEmail(model.ClientEmailId, "Registration Successful", emailBody);
					return result;
				}
				else
				{
					return 0;
				}



			}
			catch(Exception ex)
			{

				throw;
			}



		}
        // To Get subdivision and from date to date
		public List<string> GetSubDivision(int id1, int id2,string recType) //List<object>
		{
			try
			{
				string ii1="", ii2="" ,ii3="",ii4="", ii5 = "";
				string[] pname = { "@ID", "@REC_TYPE" };

				string[] pvalue = { id1.ToString(), recType + id2.ToString() };
				DataSet DS = _DbAccess.Ds_Process("SP_GET_HEAD_DISBURSEMENT", pname, pvalue);
				List<object> types = new List<object>();
				
				DataTable dataTable = DS.Tables[0];
				//Dictionary<int, string> subDivisions = new Dictionary<int, string>();
				List<string> subDivisions = new List<string>();
				var filteredRows1 = from DataRow row in dataTable.Rows
								   where row.Field<int>("LabelKey") == 0
								   select row;

				var filteredRows2 = from DataRow row in dataTable.Rows
									where row.Field<int>("LabelKey") == 1
									select row;
				var filteredRows3 = from DataRow row in dataTable.Rows
									where row.Field<int>("LabelKey") == 2
									select row;
                
                // FROM-DATE TO-DATE
                var filteredFrDt = from DataRow row in dataTable.Rows
                                    where row.Field<string>("FR_DT") != ""
                                    select row;

                var filteredToDt = from DataRow row in dataTable.Rows
                                   where row.Field<string>("TO_DT") != ""
                                   select row;

                var fr_Dt="";
                var to_Dt = "";

                // Display the filtered rows
                if (filteredRows1.Count() > 0)
                {
					ii1 += "<ul class='list-group' id='ItemList'>";
					foreach (DataRow item in filteredRows1)
                    {

                        var SubDivisionId = Convert.ToInt32(item["SubDivisionId"]).ToString();

                        //int Info = EmpWiseCompany(CompanyId, EmpID);

                        ii1 += "<li class='list-group-item'>";
                        ii1 += "<div class='checkboxCC'>";
                        ii1 += "<input type='checkbox' id=" + SubDivisionId + " checked  disabled  />&nbsp;&nbsp;&nbsp;";


                        ii1 += "<label for=" + SubDivisionId + " >" + item["SubDivisionName"].ToString() + "</ label > ";
                        ii1 += "</div>";
                        ii1 += "</li>";
                    }

                    ii1 += "</ul>";
					//subDivisions.Add(0,ii1);
					subDivisions.Add(ii1);
				}
				
				if (filteredRows2.Count() > 0)
				{
					ii2 += "<ul class='list-group' id='ItemList'>";
					foreach (DataRow item in filteredRows2)
					{

						var SubDivisionId = Convert.ToInt32(item["SubDivisionId"]).ToString();

						//int Info = EmpWiseCompany(CompanyId, EmpID);

						ii2 += "<li class='list-group-item'>";
						ii2 += "<div class='checkboxDV'>";
						ii2 += "<input type='checkbox' id=" + SubDivisionId + " checked  disabled  />&nbsp;&nbsp;&nbsp;";


						ii2 += "<label for=" + SubDivisionId + " >" + item["SubDivisionName"].ToString() + "</ label > ";
						ii2 += "</div>";
						ii2 += "</li>";
					}

					ii2 += "</ul>";
					//subDivisions.Add(1, ii2);
					subDivisions.Add(ii2);
				}
				if (filteredRows3.Count() > 0)
				{
					ii3 += "<ul class='list-group' id='ItemList'>";
					foreach (DataRow item in filteredRows3)
					{

						var SubDivisionId = Convert.ToInt32(item["SubDivisionId"]).ToString();

						//int Info = EmpWiseCompany(CompanyId, EmpID);

						ii3 += "<li class='list-group-item'>";
						ii3 += "<div class='checkboxWH'>";
						ii3 += "<input type='checkbox' id=" + SubDivisionId + " checked disabled  />&nbsp;&nbsp;&nbsp;";


						ii3 += "<label for=" + SubDivisionId + " >" + item["SubDivisionName"].ToString() + "</ label > ";
						ii3 += "</div>";
						ii3 += "</li>";

                         fr_Dt = item["FR_DT"].ToString();
                         to_Dt = item["TO_DT"].ToString();
                    }
                    
                    ii3 += "</ul>";

                    ii4 += fr_Dt;
                    ii5 += to_Dt;
                    //subDivisions.Add(2, ii3);
                    subDivisions.Add(ii3);

                    subDivisions.Add(ii4);
                    subDivisions.Add(ii5);
                }

                
                return subDivisions;



			}
			catch (Exception ex)
			{

				throw;
			}


		}
		

        public List<object> GetDisbursementType(int id)
        {

            try
            {
                string[] pname = { "@REC_TYPE", "@ID" };
                string[] pvalue = { "Type", id.ToString() };
                DataSet DS = _DbAccess.Ds_Process("SP_GET_HEAD_DISBURSEMENT", pname, pvalue);
                List<object> types = new List<object>();
                foreach (DataRow item in DS.Tables[0].Rows)
                {
                    types.Add(
                        new { MAST_DISBURSEMENT_TYPE_KEY = Convert.ToInt32(item["MAST_DISBURSEMENT_TYPE_KEY"]), DISBURSEMENT_TYPE = item["DISBURSEMENT_TYPE"].ToString() }
                        );


                }

                return types;



            }
            catch (Exception ex)
            {

                throw;
            }


        }
		
        public int SendApproval(int id)
		{
			int result = 0;

			try
			{
				string[] pname = { "@REC_TYPE", "@ID" };
				string[] pvalue = { "SEND_APPROVAL", id.ToString() };
				 result = _DbAccess.ExecuteNonQuery("SP_GET_HEAD_DISBURSEMENT", pname, pvalue);
				if (result > 0)
				{
					return result;
				}
				else
				{
					return result;
				}



			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public List<object> GetDisbursementHead(string REC_TYPE,int id)
		{
			try
			{
				string[] pname = { "@REC_TYPE", "@ID" };
				string[] pvalue = { REC_TYPE, id.ToString() };
				DataSet DS = _DbAccess.Ds_Process("SP_GET_HEAD_DISBURSEMENT", pname, pvalue);
				List<object> types = new List<object>();
				foreach (DataRow item in DS.Tables[0].Rows)
				{
					types.Add(
						new { HEAD_KEY = Convert.ToInt32(item["HEAD_KEY"]), HEAD_REFNO = item["HEAD_REFNO"].ToString() }
						);


				}

				return types;



			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public string GET_DISBURSEMENT_DETAILS(DisbursementDetails model)
        {
            string ii = "";
            try
            {
				
				var sl = 1;
				var flg = 0;
				int rowCount = 0;
                string[] pname = { "@MAST_DISBURSEMENT_TYPE_KEY", "@GET_REC_TYPE", "@WAREIDS", "@FROM_DATE", "@TO_DATE", "@HEAD_KEY", "@HEAD_DISBURSEMENT_KEY", "@DIVISION_KEYS", "@COMPANY_KEY" };
                string[] pvalue = { model.MAST_DISBURSEMENT_TYPE_KEY.ToString(), "DETAILS", model.combinedString.ToString(), model.FR_DT.ToString(),model.TO_DT.ToString(),
                    model.HEAD_KEY.ToString(),model.HEAD_DISBURSEMENT_KEY.ToString(),model.selectedValuesdivision.ToString(),model.CompanyKey.ToString()};
                DataSet ds = _DbAccess.Ds_Process("SP_GET_DISBURSEMENT_DETAILS1", pname, pvalue);
                List<object> types = new List<object>();
                ii += "<table id =" + "example" + " class=" + "table table-striped table-bordered" + " style=" + "width:100% ;text-align:center" + ">";
                ii += "<thead class=" + "table-secondary" + ">";
				ii += "<tr><th> Sl. </th><th> Name </th><th> Disbursing Amount </th><th> Disbursement Amount </th></tr></thead>"; /*< th > Action </ th >*/


				ii += "<tbody>";
				foreach (DataRow item in ds.Tables[0].Rows)
                {


                    ii += "<tr>";
                    //ii += "<td style='display:none'>" + Convert.ToInt32(item["APPROVAL_LVL1_KEY"]) + "</td>";
                    //ii += "<td style='display:none'>" + Convert.ToInt32(item["APPROVAL_LVL2_KEY"]) + "</td>";
                    //ii += "<td style='display:none'>" + Convert.ToInt32(item["APPROVAL_User_Type_Key"]) + "</td>";
                    ii += "<td>"+ rowCount+1;
					ii += "<input class=\"form-control\" type=\"text\" name=\"EMPLOYEE_MASTER_KEY[]\" readonly style=\"display:none;\" value=\"" + Convert.ToInt32(item["EMPLOYEE_MASTER_KEY"]) + "\" /></td>";

					ii += "<td>" + item["Employee_Name"].ToString() + "</td>";
                    //ii += "<td>" + Convert.ToInt32(item["STEP_NO"]) + "</td>";
                    //ii += "<td>" + item["NET SALARY"].ToString() + "</td>";
                    
					ii += "<td><input class=\"form-control me-lg-n5\" type=\"text\" name=\"DisbursNetAmnt[]\" readonly \"  value=\"" + Convert.ToInt32(item["NET AMOUNT"]) + "\" /></td>";
                    if (Convert.ToInt32(item["COMPLETE_FLAG"]) == 1)
                    {
                        flg = 1;
						ii += "<td><input class=\"form-control me-lg-n3\" type=\"text\" name=\"DisbursementAmnt[]\" readonly \"  value=\"" + Convert.ToInt32(item["DISBURSING_AMOUNT"]) + "\" /></td>";
					}
                    else {
						ii += "<td><input class=\"form-control me-lg-n3\" type=\"text\" name=\"DisbursementAmnt[]\"  value=\"\" /></td>";
					}
					
					//ii += "<td style=\"display: none;\">";
     //               ii += "<a href = 'javascript:;'class='action-icon'>";
     //               ii += "<i class='mdi mdi-square-edit-outline' onclick='AddEmployee(" + Convert.ToInt32(item["EMPLOYEE_MASTER_KEY"]) +  ")'data-toggle='tooltip' title='Add' style='color:forestgreen'></i></a>";

     //               ii += "</td>";
                    ii += "</tr>";

                    rowCount = rowCount+1;

				}
                ii += "</tbody>";
                ii += "</table>";
                return ii + "count=" + rowCount.ToString() +" Completeflag="+flg.ToString();


            }
            catch (Exception ex)
            {

                throw;
            }



        }




		#region EXTRA 
		public string GetNewCostCentersName(int ExistingCompanyId) //List<object>
		{
			try
			{
				string ii = "";
				string[] pname = { "@COMPANY_KEY", "@LABEL_KEY" };

				string[] pvalue = { ExistingCompanyId.ToString(), "0" };
				DataSet DS = _DbAccess.Ds_Process("SP_GET_NEW_COSTCENTERS_NAME", pname, pvalue);
				List<object> types = new List<object>();
				ii += "<ul class='list-group' id='ItemList'>";
				foreach (DataRow item in DS.Tables[0].Rows)
				{
					var SubDivisionId = Convert.ToInt32(item["SubDivisionId"]).ToString();

					//int Info = EmpWiseCompany(CompanyId, EmpID);

					ii += "<li class='list-group-item'>";
					ii += "<div class='checkboxCC'>";
					ii += "<input type='checkbox' id=" + SubDivisionId + "  />&nbsp;&nbsp;&nbsp;";


					ii += "<label for=" + SubDivisionId + " >" + item["SubDivisionName"].ToString() + "</ label > ";
					ii += "</div>";
					ii += "</li>";
				}
				ii += "</ul>";
				//ii += "<ul class='list-group' id='ItemList'>";
				//foreach (DataRow item in DS.Tables[0].Rows)
				//{

				//    //var DtlsStatutoryHeaderKey = Convert.ToInt32(item["DtlsStatutoryHeaderKey"]);

				//    //int Info = TemplateWiseDtlsStatutoryHeader(CompanyKey, DtlsStatutoryHeaderKey, Mast_Salary_Template_key);

				//    ii += "<li class='list-group-item'>";
				//    ii += "<div class='checkbox'>";
				//    //if (Info == 1)
				//    //{
				//        ii += "<input type='checkbox' value='" + Convert.ToInt32(item["SubDivisionId"]) +  "' name='SelectedSalaryHeads' class='checkbox-item' />&nbsp;&nbsp;&nbsp;";
				//    //}
				//    //else
				//    //{
				//    //    ii += "<input type='checkbox' value='" + Convert.ToInt32(item["DtlsStatutoryHeaderKey"]) + "' name='SelectedSalaryHeads' class='checkbox-item' />&nbsp;&nbsp;&nbsp;";
				//    //}



				//    ii += "<label for=" + Convert.ToInt32(item["SubDivisionId"]) + " >" + item["SubDivisionName"].ToString() + "</ label > ";
				//    ii += "</div>";
				//    ii += "</li>";
				//}
				//ii += "</ul>";
				return ii;



			}
			catch (Exception ex)
			{

				throw;
			}


		}

		public string GET_Division(string combinedString)
		{
			string ii = "";
			try
			{
				string[] parameterNames = { "@GET_REC_TYPE", "@IDS", "@COMPANY_KEY" };
				string[] parameterValues = { "DIVISION", combinedString.ToString(), "0" };

				DataSet dataSet = _DbAccess.Ds_Process("SP_GET_MAST_DIVISION_FOR_COSTCENTERS", parameterNames, parameterValues);
				ii += "<ul class='list-group' id='ItemList'>";
				foreach (DataRow item in dataSet.Tables[0].Rows)
				{
					var Division_Key = Convert.ToInt32(item["DIVISION_KEY"]).ToString();

					//int Info = EmpWiseCompany(CompanyId, EmpID);

					ii += "<li class='list-group-item'>";
					ii += "<div class='checkboxDV'>";
					ii += "<input type='checkbox' id=" + Division_Key + "  />&nbsp;&nbsp;&nbsp;";


					ii += "<label for=" + Division_Key + " >" + item["DIVISION_NAME"].ToString() + "</ label > ";
					ii += "</div>";
					ii += "</li>";
				}
				ii += "</ul>";
				return ii;


			}
			catch (Exception ex)
			{

				throw;
			}
		}


		public string GET_WAREHOUSE(string combinedString)
		{
			string ii = "";
			try
			{
				string[] parameterNames = { "@GET_REC_TYPE", "@IDS", "@COMPANY_KEY" };
				string[] parameterValues = { "WAREHOUSE", combinedString.ToString(), "1" };

				DataSet dataSet = _DbAccess.Ds_Process("SP_GET_MAST_DIVISIONS_WAREHOUSES", parameterNames, parameterValues);
				ii += "<ul class='list-group' id='ItemList'>";
				foreach (DataRow item in dataSet.Tables[0].Rows)
				{
					var Warehouse_key = Convert.ToInt32(item["WAREHOUSE_KEY"]).ToString();

					//int Info = EmpWiseCompany(CompanyId, EmpID);

					ii += "<li class='list-group-item'>";
					ii += "<div class='checkboxWH'>";
					ii += "<input type='checkbox' id=" + Warehouse_key + "  />&nbsp;&nbsp;&nbsp;";


					ii += "<label for=" + Warehouse_key + " >" + item["WAREHOUSE_NAME"].ToString() + "</ label > ";
					ii += "</div>";
					ii += "</li>";
				}
				ii += "</ul>";
				return ii;


			}
			catch (Exception ex)
			{

				throw;
			}
		}


		#endregion
		//public List<SelectListItem> GetTenantGroupHeadMaster()
		//{


		//    try
		//    {
		//        string[] pname = {};
		//        string[] pvalue = {};
		//        DataSet DS = _DbAccess.Ds_Process("SP_GET_TenantGroupHeadMaster", pname, pvalue);
		//        List<SelectListItem> types = new List<SelectListItem>();
		//        types.Add(new SelectListItem { Text = "--Select--", Value = "" });
		//        foreach (DataRow dr in DS.Tables[0].Rows)
		//        {
		//            types.Add(new SelectListItem { Text = dr["GroupHeadName"].ToString(), Value = dr["GroupHeadId"].ToString() });
		//        }

		//        return types;

		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }

		//}

		//public List<SelectListItem> GetGroupHeadType()
		//{
		//    try
		//    {
		//        string[] pname = { };
		//        string[] pvalue = { };
		//        DataSet DS = _DbAccess.Ds_Process("SP_GET_GroupHeadType", pname, pvalue);
		//        List<SelectListItem> types = new List<SelectListItem>();

		//        types.Add(new SelectListItem { Text = "Select All", Value = "0" });
		//        foreach (DataRow dr in DS.Tables[0].Rows) 
		//        {
		//            types.Add(new SelectListItem { Text = dr["Department_Desc"].ToString(), Value = dr["DepartmentId"].ToString() });
		//        }
		//        return types;

		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }

		//}

		//public List<object> GetDivisionsName(int ExistingCompanyId)
		//{


		//    try
		//    {
		//        string[] pname = { "@COMPANY_KEY" };
		//        string[] pvalue = { ExistingCompanyId.ToString() };
		//        DataSet DS = _DbAccess.Ds_Process("SP_GET_EXISTING_DIVISIONS_NAME", pname, pvalue);
		//        List<object> types = new List<object>();
		//        foreach (DataRow item in DS.Tables[0].Rows)
		//        {
		//            types.Add(
		//                new { value = Convert.ToInt32(item["DIVISION_KEY"]), text = item["DIVISION_NAME"].ToString() }
		//                );


		//        }

		//        return types;



		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }
		//}

		//public List<object> VerifyUserName(string Name)
		//    {


		//        try
		//        {
		//            string[] pname = { "@UserName" };
		//            string[] pvalue = { Name };
		//            DataSet DS = _DbAccess.Ds_Process("SP_GetUniqueUserName", pname, pvalue);
		//            List<object> types = new List<object>();
		//            foreach (DataRow item in DS.Tables[0].Rows)
		//            {
		//                types.Add(
		//                    new { Name = item["Name"].ToString(), UserName = item["UserName"].ToString() }
		//                    );


		//            }

		//            return types;



		//        }
		//        catch (Exception ex)
		//        {

		//            throw;
		//        }



		//    }



		//public List<object> GetClientUniqueCodeTypes()
		//{


		//    try
		//    {
		//        string[] pname = {};
		//        string[] pvalue = {};
		//        //return Int_Process("SP_BUSINESS_USER_REGISTRATION", pname, pvalue);
		//        DataSet DS= _DbAccess.Ds_Process("SP_GET_CLIENT_UNIQUECODE_TYPES", pname, pvalue);
		//        List<object> types = new List<object>();
		//        foreach (DataRow item in DS.Tables[0].Rows)
		//        {
		//            types.Add(
		//                new  { value = Convert.ToInt32( item["ClientUniqueCodeTypeId"] ), text = item["ClientUniqueCodeTypeDesc"].ToString(), Description = item["ClientCodeValidationRules"] }
		//                );


		//        }

		//        return types;

		//        //if (ds != null)
		//        //{

		//        //    var result = new 
		//        //    {
		//        //        Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientUniqueCodeTypeId"]),
		//        //        Value = ds.Tables[0].Rows[0]["ClientUniqueCodeTypeDesc"].ToString(),
		//        //        ValidationRule = ds.Tables[0].Rows[0]["ClientCodeValidationRules"].ToString()
		//        //    };
		//        //    return result;
		//        //}



		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }



		//}

		//public int UserNameValidation(string UserName)
		//{

		//    try
		//    {
		//        string[] pname = { "@ID" };
		//        string[] pvalue = { UserName };
		//        return _DbAccess.ExecuteNonQuery("CHECK_USERNAME_VALIDATION", pname, pvalue);
		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }
		//}

		//public List<object> GetGroupHeadPersonnel_Guest()
		//{
		//    try
		//    {
		//        string[] pname = { };
		//        string[] pvalue = { };
		//        DataSet DS = _DbAccess.Ds_Process("[SP_GET_GroupHeadPersonnel_Guest]", pname, pvalue);
		//        List<object> dataList = new List<object>();

		//        foreach (DataRow row in DS.Tables[0].Rows)
		//        {
		//            dynamic dataObject = new System.Dynamic.ExpandoObject();
		//            foreach (DataColumn column in DS.Tables[0].Columns)
		//            {
		//                var columnName = column.ColumnName;
		//                var columnValue = row[column];
		//                ((IDictionary<string, object>)dataObject)[columnName] = columnValue;
		//            }
		//            dataList.Add(dataObject);
		//        }

		//        return dataList;
		//    }
		//    catch (Exception ex)
		//    {
		//        throw;
		//    }
		//}
		//public List<SelectListItem> GetTenantGroupHeadMaster_Guest()
		//{


		//    try
		//    {
		//        string[] pname = { };
		//        string[] pvalue = { };
		//        DataSet DS = _DbAccess.Ds_Process("[SP_GET_TenantGroupHeadMaster_Guest]", pname, pvalue);
		//        List<SelectListItem> types = new List<SelectListItem>();
		//        types.Add(new SelectListItem { Text = "--Select--", Value = "" });
		//        foreach (DataRow dr in DS.Tables[0].Rows)
		//        {
		//            types.Add(new SelectListItem { Text = dr["GroupHeadName"].ToString(), Value = dr["GroupHeadId"].ToString() });
		//        }

		//        return types;

		//    }
		//    catch (Exception ex)
		//    {

		//        throw;
		//    }

		//}

	}
}
