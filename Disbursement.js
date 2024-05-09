
	var rowCount;
	$(document).ready(function () {
				//GetCostCentre(1);
				var key0 = '';
				var key1 = '';
				var key2 = '';

				$('#CloseDisburse').addClass('disabled').removeAttr('data-bs-dismiss').removeAttr('data-bs-target').css('pointer-events', 'none');

				var buttonAccessKey = 0;

				debugger;
				GetDisbursementType(0);
				GetSession();
				$("#AC_HEAD_DISBURSEMENT_KEY").val("0");

				$('#datepickerFr').datepicker({
					format: 'mm/dd/yyyy', // Set the desired date format
					autoclose: true
				});

				// Apply Date Masking using Inputmask plugin for From Date
				$('#datepickerFr').inputmask('mm/dd/yyyy', {
					placeholder: 'MM/DD/YYYY',
					showMaskOnFocus: false,
					showMaskOnHover: false
				});

				// Initialize Datepicker for To Date
				$('#datepickerTo').datepicker({
					format: 'mm/dd/yyyy', // Set the desired date format
					autoclose: true
				});

				// Apply Date Masking using Inputmask plugin for To Date
				$('#datepickerTo').inputmask('mm/dd/yyyy', {
					placeholder: 'MM/DD/YYYY',
					showMaskOnFocus: false,
					showMaskOnHover: false
				});
			});

	$('#costcenter').click(function () {
				let arr = []; // Initialize an empty array
				$('.checkboxCC').each(function () {
					//debugger;
					var id = $(this).find('input[type="checkbox"]').attr('id');
					//alert(id);

					if ($(this).find('input[type="checkbox"]').prop('checked')) {
						//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
						arr.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
						//alert($(this).find('input[type="checkbox"]').val() + ' is checked');
					}
				});

				// console.log(arr);
				// COMMENT ON 281120232
		//	$.ajax({
		//	url: '/Disbursement/GetDivision',
		//	type: 'POST',
		//	data: { dataArray: arr },
		//	dataType: 'json',
		//	success: function (data) {
		//		//console.log(data.message); // Handle the response from the server
		//		$('#division').html(data);
		//	},
		//	error: function (error) {
		//		console.error('Error:', error);
		//	}
		//});
	});

	$('#division').click(function () {
		let arr = []; // Initialize an empty array
	$('.checkboxDV').each(function () {
					//debugger;
					var id = $(this).find('input[type="checkbox"]').attr('id');
	//alert(id);

	if ($(this).find('input[type="checkbox"]').prop('checked')) {
		//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
			arr.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array  CLOSE IT 28112023
		// alert($(this).find('input[type="checkbox"]').val() + ' is checked');
	}
				});

				// console.log(arr);
		// COMMENT ON 281120232
		//	$.ajax({
		//	url: '/Disbursement/GetWarehouse',
		//	type: 'POST',
		//	data: { dataArray: arr },
		//	dataType: 'json',
		//	success: function (data) {
		//		//console.log(data.message); // Handle the response from the server
		//		$('#warehouse').html(data);
		//	},
		//	error: function (error) {
		//		console.error('Error:', error);
		//	}
		//});
	});

	let arrWH = [];
	$('#warehouse').click(function () {
		// Initialize an empty array
		$('.checkboxWH').each(function () {
			//debugger;
			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {
				//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
				arrWH.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
				// alert($(this).find('input[type="checkbox"]').val() + ' is checked');
			}
		});
			});

	$('#btnSearch').click(function () {
			//	debugger;

	let arrWH = [];

	$('.checkboxWH').each(function () {
				//debugger;
				var id = $(this).find('input[type="checkbox"]').attr('id');
	//alert(id);

	if ($(this).find('input[type="checkbox"]').prop('checked')) {
		//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
		arrWH.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
					// alert($(this).find('input[type="checkbox"]').val() + ' is checked');
				}
	});


		let arrDV = [];

		$('.checkboxDV').each(function () {
			
			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {
			
				arrDV.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
			
			}
		});


	var InputData1 = {
		MAST_DISBURSEMENT_TYPE_KEY: $('#ddlDisbursementType').val(),
		FR_DT: $('#hf_frDt').val(),
		TO_DT: $('#hf_toDt').val(),
		selectedValuesDivision: arrDV,
		selectedValuesWarehouse: arrWH,
	
	HEAD_KEY:$('#ddlDisbursementHead').val(),
				};
	var InputData2 = {
		dataArray: arrWH
				};
	//console.log(InputData1, InputData2);
	// Rest of your code for the #btnSearch click event
	$.ajax({
		url: '/Disbursement/GetDisbursementDetails',
	type: 'POST',
	data: {
		model: InputData1, // Ensure that the parameter name matches the action method
	dataArray: InputData2.dataArray // Make sure you're sending the 'dataArray' property of InputData2
				},
			  //data: {dataModel: InputData1, dataArray: InputData2 },
	dataType: 'json',
	success: function (data) {
				//console.log(data.message); // Handle the response from the server
		rowCount = data.rowCount;
		//if (data.Completeflag == 1) {
		//	$('#dv_ChangedValue').hide();
		//	$('#btnSaveDisburseDetails').hide();
			
		//}
				$('#Div_DisbursementHead').html(data.html);
				},
	error: function (error) {
				console.error('Error:', error);
				}
			});
				// ...
	});
	// Save dtls Disbursement
	$('#btnSaveDisburseDetails').click(function () {
		DtlsDisbursement();
			});
	function DtlsDisbursement() {
		debugger;

		
	var selectedValuescostcenter = [];

	$('.checkboxCC').each(function () {
			var id = $(this).find('input[type="checkbox"]').attr('id');

	if ($(this).find('input[type="checkbox"]').prop('checked')) {
		selectedValuescostcenter.push($(this).find('input[type="checkbox"]').attr('id'));
			}
		});

	//var checkboxesdivision = document.querySelectorAll('input[id^="division"]');
	var selectedValuesdivision = [];
	$('.checkboxDV').each(function () {
			var id = $(this).find('input[type="checkbox"]').attr('id');

	if ($(this).find('input[type="checkbox"]').prop('checked')) {
		selectedValuesdivision.push($(this).find('input[type="checkbox"]').attr('id'));
			}
		});
		//checkboxesdivision.forEach(function (checkbox) {
		//	if (checkbox.checked) {
		//		selectedValuesdivision.push(checkbox.value);
		//	}
		//});

		//var checkboxeswarehouse = document.querySelectorAll('input[id^="warehouse"]');
		var selectedValueswarehouse = [];
	$('.checkboxWH').each(function () {
			var id = $(this).find('input[type="checkbox"]').attr('id');

	if ($(this).find('input[type="checkbox"]').prop('checked')) {
		selectedValueswarehouse.push($(this).find('input[type="checkbox"]').attr('id'));
			}
		});
		//checkboxeswarehouse.forEach(function (checkbox) {
		//	if (checkbox.checked) {
		//		selectedValueswarehouse.push(checkbox.value);
		//	}
		//});

		if (selectedValuescostcenter.length == 0) {
		OpenErrPopup("Please Select Cost Center");
	return false;
		}

	if (selectedValuesdivision.length == 0) {
		OpenErrPopup("Please Select Division");
	return false;
		}

	if (selectedValueswarehouse.length == 0) {
		OpenErrPopup("Please Select Warehouse");
	return false;
		}

	var myList = [];
	var EmpList = [];
	var DisburseAmntList = [];
		var errorCode = 0;
	var NetAmntList = [];

	var EMPLOYEE_MASTER_KEY = 0;
	var NET_PAYABLE_AMOUNT = 0;
	var selectedCostCenterString = selectedValuescostcenter.join(', ');
	var selectedDivisionString = selectedValuesdivision.join(', ');
	var selectedWHString = selectedValueswarehouse.join(', ');
	for (var i = 0; i < rowCount; i++) {
		var disbursementField = $('input[name="DisbursementAmnt[]"]').eq(i);
		var disbursementVal = $('input[name="DisbursementAmnt[]"]').eq(i).val();
		 //= $('input[name="EMPLOYEE_MASTER_KEY[]"]').eq(i);
		var disbursementNetField = $('input[name="DisbursNetAmnt[]"]').eq(i).val();
		//DisbursNetAmnt = DisbursNetAmnt1.val();
		
	if (disbursementField) {
		if (disbursementField.val() > disbursementNetField) {
			errorCode = 1;
		}
		else if (disbursementField.val() == 0 || disbursementField.val() == '' || disbursementField.val() == undefined ) {
			errorCode = 2;
		}

		NET_PAYABLE_AMOUNT = $('input[name="DisbursNetAmnt[]"]').eq(i).val();
		EMPLOYEE_MASTER_KEY = $('input[name="EMPLOYEE_MASTER_KEY[]"]').eq(i).val();
		EmpList.push(EMPLOYEE_MASTER_KEY);
		DisburseAmntList.push(disbursementField.val());

		//DisburseAmntList.push(disbursementField.val());
		NetAmntList.push(disbursementNetField);
		//console.log(NetAmntList);
		//myList.push({ Key: EMPLOYEE_MASTER_KEY, Value: disbursementField.val() });
		myList.push({ Key: EMPLOYEE_MASTER_KEY, Value: disbursementField.val(), NetAmnt: disbursementNetField });
			}
		}

		console.log(myList);
	var selectedEmpString = EmpList.join(', ');
	var selectedDisbursedString = DisburseAmntList.join(', ');
	var postdata = {
		disburseList: myList,
		HEAD_DISBURSEMENT_KEY: $('#AC_HEAD_DISBURSEMENT_KEY').val(),
		MAST_DISBURSEMENT_TYPE_KEY: $('#ddlDisbursementType').val(),
		HEAD_KEY: $("#ddlDisbursementHead").val() ,//$("#AC.HEAD_KEY").val(),
		NET_PAYABLE_AMOUNT: NET_PAYABLE_AMOUNT,

		FR_DT: $('#FromDt').val(),
		TO_DT: $('#ToDt').val(),

	selectedValuescostcenter: selectedCostCenterString,
	selectedValuesdivision: selectedDivisionString,
	selectedValueswarehouse: selectedWHString
			//selectedValuesEmp: selectedEmpString,
			//selectedValuesDisburse: selectedDisbursedString
		};
	
//	console.log(postdata);
		if (errorCode != 1 || errorCode != 2) {
			$.post("/Disbursement/SaveDtlsDisbursement", postdata, function (response) {
				if (response.id > 0) {
					document.getElementById('newHedDisbursementKey').value = response.id;
					Swal.fire({
						title: "Data Saved Successfully",
						icon: "success",
						showConfirmButton: false,
						timer: 2500
					});
					buttonAccessKey = 1;
					if (buttonAccessKey == 1) {
						$('#btnSaveDisburseDetails').prop('disabled', true);
						$('#btnSendDtlsApproval').prop('disabled', false);

						buttonAccessKey = 2;
					}
				} else {
					Swal.fire('Error!', 'An error occurred.', 'error');
				}
			})
				.fail(function (error) {
					console.log("Request failed:", error);
				});
		}
		else {
			Swal.fire({
				title: "Disburse amount can not be more than Net payble amount or Disburse amount can not be null.",
				icon: "error",
				showConfirmButton: false,
				timer: 2500
			});
		}
		return Swal.fire({
			title: "Disburse amount can not be more than Net payable amount or Disburse amount can not be null.",
			icon: "error",
			showConfirmButton: false,
			timer: 2500
		}).then(function () {
			// Additional actions here
		});

	/*return false;*/
	}

	function SendApproval(HeadDisburseKey) {
		 debugger;
		$.get("/Disbursement/SendApproval",
			{
				id: HeadDisburseKey // Change 'id' to 'DisburseKey'
			},
			function (data) {
				//console.log(data);
				debugger;
				if (parseInt(data.id) > 0) {
					//alert('send return',data.id);
					//console.log(data.id);
					Swal.fire({
						title: "Data Send to the Approval path.",
						icon: "success",
						showConfirmButton: false,
						timer: 2500
					});
					//swal.fire("Done", "Data Send to Approval path !!", "success");
				//	ClearValues();
					if (buttonAccessKey==2) {
						$('#btnSaveDisburseDetails').prop('disabled', true);
						$('#btnSendDtlsApproval').prop('disabled', true);
						$('#CloseDisburse').removeClass('disabled').attr('aria-disabled', 'false');
					}
					location.reload(); 
				}
				else {
					swal.fire("Alert!", "Please Contact Admin", "error");
				}


			}
		);
	}
	function AddDisbursementDetails(id,DisbursementTypeKey,headKey,completeFlag) {
		// Get the value from the header textbox
		debugger;
		
		$("#AC.HEAD_KEY").val(headKey);
		//GetDisbursementType(DisbursementTypeKey);
		$("#ddlDisbursementType").val(DisbursementTypeKey);
		$('#DisburseModal').modal('toggle');
		var recType = '';
		//$('#DisburseModal').show();
		//console.log('HEAD_DISBURSEMENT_KEY' + id);
		// 01_03_2023

		if (DisbursementTypeKey == 1) {
			recType = 'HeadSal';
		}
		if (DisbursementTypeKey == 2) {
			recType = 'HeadClaim';
		}
		if (DisbursementTypeKey == 3) {
			recType = 'HeadAdvance';
		}
		if (DisbursementTypeKey == 4) {
			recType = 'HeadBonus';
		}

		ShowDisburseHead(recType, headKey);
		$('#ddlDisbursementHead').val(headKey);

		// 20_02_2024 ====@AC.HEAD_KEY
		recType = '';
		
		if (DisbursementTypeKey == 1) {
			recType = 'SubDivision_Salary_';
		}
		if (DisbursementTypeKey == 2) {
			recType = 'SubDivision_Claim_';
		}
		if (DisbursementTypeKey == 3) {
			recType = 'SubDivision_Advance_';
		}
		if (DisbursementTypeKey == 4) {
			recType = 'SubDivision_Bonus_';
		}
		// BASED ON DISBURSEMENT TYPE
		if (DisbursementTypeKey == 2 || DisbursementTypeKey == 3) {
		
			ShowSubDivisionAll(DisbursementTypeKey, id, recType, headKey);
			$('#DisburseModal').modal('show');
			$('#AC_HEAD_DISBURSEMENT_KEY').val(id);
		}
		else { 
		ShowSubDivisionAll(DisbursementTypeKey, id, recType, headKey);
		//GetDtlsDisbursement(headDisburseKey, HeadDisburseTypeKey, headKey);
		//GetDtlsDisbursement(id);
		// Show the modal after processing the response
		$('#DisburseModal').modal('show');
		
		// ========
			$('#AC_HEAD_DISBURSEMENT_KEY').val(id);

		}

		$("#validateDisbursement").show();
		// =====
		var closeDisburseLink = document.getElementById('CloseDisburse');
		if (completeFlag === 1) {
			$('#btnSaveDisburseDetails').prop('disabled', true);
			//$('#CloseDisburse').removeClass('disabled').attr('aria-disabled', 'false'); 
			$("#dv_ChangedValue").hide();
			// === enabling the close button in modal
			//closeDisburseLink.removeClass('disabled').attr('data-bs-dismiss', 'modal').attr('data-bs-target', '#DisburseModal').css('pointer-events', 'auto');
			closeDisburseLink.classList.remove('disabled');
			closeDisburseLink.removeAttribute('aria-disabled');
			closeDisburseLink.setAttribute('data-bs-dismiss', 'modal');
			closeDisburseLink.setAttribute('data-bs-target', '#DisburseModal');
			closeDisburseLink.style.pointerEvents = 'auto'; // Restore pointer events
			//===
			/*$('#btnSaveDisburseDetails').hide();*/
		}


		//	var headerDisburseAmnt = document.getElementById("HeaderDisburseAmnt").value;

		//	var ArrayHead_key = document.getElementsByName("head_key[]");


		//	var ArrayHeading = document.getElementsByName("heading[]");
		//	var ArrayRefNo = document.getElementsByName("refNo[]");

		//	var ArrayRefDate = document.getElementsByName("refDt[]");
		//	var ArrayRemarks = document.getElementsByName("Remarks[]");

		//		console.log(rowno);

		//	var jsonDataDisbursementDetails = {

		//	DISBURSEMENT_HEADING: ArrayHeading[rowno].value,
		//	DISBURSEMENT_REFNO: ArrayRefNo[rowno].value,
		//	DISBURSEMENT_REF_DATE: ArrayRefDate[rowno].value,
		//	DISBURSEMENT_REMARKS: ArrayRemarks[rowno].value,
		//	HEAD_KEY: ArrayHead_key[rowno].textContent,
		//	HEAD_DISBURSEMENT_KEY: id
		//};

		//// Set the value to the first "Disbursement Amount" textbox in the table
		//var disbursementAmountField = document.querySelector('#TBody_TableBonusDtls input[type="text"]');
		//if (disbursementAmountField) {
		//	disbursementAmountField.value = headerDisburseAmnt;
		//}
		
	}

	//	function DtlsDisbursement() {
		//	debugger;
		//	var selectedValuescostcenter = [];// document.querySelectorAll('input[id^="costcenter"]');


		//	$('.checkbox').each(function () {
		//		//debugger;
		//		var id = $(this).find('input[type="checkbox"]').attr('id');
		//		//alert(id);

		//		if ($(this).find('input[type="checkbox"]').prop('checked')) {
		//			//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
		//			selectedValuescostcenter.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
		//			//alert($(this).find('input[type="checkbox"]').val() + ' is checked');
		//		}
		//	}
		//			//var selectedValuescostcenter = [];

		//			//checkboxescostcenter.forEach(function (checkbox) {
		//			//	if (checkbox.checked) {
		//			//		selectedValuescostcenter.push(checkbox.value);
		//			//	}
		//			//});
		//			console.log(selectedValuescostcenter);
		//	var checkboxesdivision = document.querySelectorAll('input[id^="division"]');

		//	var selectedValuesdivision = [];

		//	checkboxesdivision.forEach(function (checkbox) {
		//		if (checkbox.checked) {
		//			selectedValuesdivision.push(checkbox.value);
		//		}
		//	});
		//	console.log(selectedValuesdivision);
		//	var checkboxeswarehouse = document.querySelectorAll('input[id^="warehouse"]');

		//	var selectedValueswarehouse = [];

		//	checkboxeswarehouse.forEach(function (checkbox) {
		//		if (checkbox.checked) {
		//			selectedValueswarehouse.push(checkbox.value);
		//		}
		//	});
		//	console.log(selectedValueswarehouse);
		//	if (selectedValuescostcenter.length == 0) {
		//		OpenErrPopup("Please Select Cost Center");
		//		return false;
		//	}
		//	if (selectedValuesdivision.length == 0) {
		//		OpenErrPopup("Please Select Division");
		//		return false;
		//	}
		//	if (selectedValueswarehouse.length == 0) {
		//		OpenErrPopup("Please Select Warehouse");
		//		return false;
		//	}
		//	var myList = [];

		//	// Add items dynamically
		//	var DisbursNetAmnt = 0;
		//	var EMPLOYEE_MASTER_KEY = 0;

		//	for (var i = 0; i < rowCount; i++) {

		//		var disbursementField = $('input[name="DisbursementAmnt[]"]').eq(i);
		//		if (disbursementField) {
		//			//		DisbursNetAmnt = $('input[name="DisbursNetAmnt[]"]').eq(i).val();
		//			EMPLOYEE_MASTER_KEY = $('input[name="EMPLOYEE_MASTER_KEY[]"]').eq(i).val();
		//			//DisbursNetAmnt = DisbursNetAmnt1.val();
		//			//alert(DisbursNetAmnt)
		//			//console.log(parseFloat(DisbursNetAmnt).toFixed(2));
		//			//console.log(parseFloat(headerDisburseAmntValue).toFixed(2));
		//			//console.log(headerDisburseAmntValue);
		//			//= parseFloat(DisbursNetAmnt).toFixed(2)
		//			//DisbursNetAmnt = (parseFloat(DisbursNetAmnt).toFixed(2) * parseFloat(headerDisburseAmntValue).toFixed(2)) / 100;
		//			//disbursementField.val(DisbursNetAmnt);

		//			myList.push({ Key: EMPLOYEE_MASTER_KEY, Value: disbursementField.val() });

		//		}
		//	}
		//	// Make an AJAX request to the server-side method

		//	var postdata = {
		//		myList: myList,
		//		HEAD_DISBURSEMENT_KEY: $('#HEAD_DISBURSEMENT_KEY').val(),
		//		DISBURSEMENT_TYPE_KEY: $('#ddlDisbursementType').val(),
		//		DISBURSEMENT_HEAD_KEY: $('#ddlDisbursementHead').val(),
		//		selectedValuescostcenter: selectedValuescostcenter,
		//		selectedValuesdivision: selectedValuesdivision,
		//		selectedValueswarehouse: selectedValueswarehouse

		//	}
		//	console.log(postdata);

		//	$.post("/Disbursement/SaveDtlsDisbursement", postdata, function (response) {
		//		// Success callback function


		//		if (response.id > 0) {

		//			Swal.fire({
		//				title: "Data Saved Successfully",
		//				icon: "success",
		//				showConfirmButton: false,
		//				timer: 2500  // Adjust the duration as needed
		//			});


		//		}
		//		else {
		//			Swal.fire('Error!', 'An error occurred.', 'error');

		//		}
		//	}).
		//		fail(function (error) {
		//			// Error callback function
		//			//alert("Invalid Login");
		//			console.log("Request failed:", error);
		//		});

		//	return false;
		//}


		function SaveHeadDisbursement(id, rowno) {
			//debugger;


			var ArrayHead_key = document.getElementsByName("head_key[]");


			var ArrayHeading = document.getElementsByName("heading[]");
			var ArrayRefNo = document.getElementsByName("refNo[]");

			var ArrayRefDate = document.getElementsByName("refDt[]");
			var ArrayRemarks = document.getElementsByName("Remarks[]");

		
				var heading_ = $('#heading\\[' + rowno + '\\]').val();
			//alert(inputArray[rowno].value);
			//alert(rowno);
		//	console.log(rowno);

			var jsonData = {

				DISBURSEMENT_HEADING: ArrayHeading[rowno].value,
				DISBURSEMENT_REFNO: ArrayRefNo[rowno].value,
				DISBURSEMENT_REF_DATE: ArrayRefDate[rowno].value,
				DISBURSEMENT_REMARKS: ArrayRemarks[rowno].value,
				HEAD_KEY: ArrayHead_key[rowno].textContent,
				HEAD_DISBURSEMENT_KEY: id
			};

		//	console.log(jsonData);

			// Use the 'jsonData' object as needed (e.g., send it via AJAX)

			$.post("/Disbursement/SaveHeadDisbursement", jsonData, function (response) {
				// Success callback function


				if (response.id > 0) {

					Swal.fire({
						title: "Data Saved Successfully",
						icon: "success",
						showConfirmButton: false,
						timer: 2500  // Adjust the duration as needed
					});

					NewGrid(id, Type);
				}
				else {
					Swal.fire('Error!', 'An error occurred.', 'error');

				}
			}).
				fail(function (error) {
					// Error callback function
					//alert("Invalid Login");
					console.log("Request failed:", error);
				});
			return false;
		}
		// GET DISBURSEMENT TYPE
		function GetDisbursementType(id) {

		//  alert('GetGroupHeadType')

			$.get("/Disbursement/GetDisbursementType", id, function (response) {

			var idList = response;


			var selectElement = $("#ddlDisbursementType");
			//selectElement.empty(); // Clear existing options


			// $('#costselectAll').show();

			$("#ddlDisbursementType").empty();
			$('#ddlDisbursementType').append($('<option>', {
				value: 0,
				text: '--Select--'
			}));

			$.each(idList, function (key, value) {
			//	console.log(key);
			//	console.log(value);
				$('#ddlDisbursementType')
					.append($("<option></option>")
						.attr("value", value.masT_DISBURSEMENT_TYPE_KEY)
						.text(value.disbursemenT_TYPE));

			});
		});

		}

	// GET Cost-Center
	function GetCostCentre(id) {
				//var company = $("#CompanyIDforBnkGst");

				var jsonData = {CompanyKey: id }
	$.post("/Disbursement/FillNewCostCenters/", jsonData, function (response) {

		// Loop through the API response and populate checkboxes
		$('#costcenter').html(response);
					//   $("#costcenter").val(@ViewBag.costcenter);
				});

			}
	$('#ddlCostCenterId').change(function (e) {

				var id = $('#ddlCostCenterId').val();
	GetDivision(id);
			}

	);
	// GET DISBURSEMENT TYPE
	function GetDisbursementType() {
		debugger;
		//  alert('GetGroupHeadType')

		$.get("/Disbursement/GetDisbursementType", function (response) {

			var idList = response;


			var selectElement = $("#ddlDisbursementType");
			//selectElement.empty(); // Clear existing options


			$("#ddlDisbursementType").empty();
			$('#ddlDisbursementType').append($('<option>', {
				value: 0,
				text: '--Select--'
			}));

			$.each(idList, function (key, value) {
			  //console.log(key);
			//	console.log(value);
				$('#ddlDisbursementType')
					.append($("<option></option>")
						.attr("value", value.masT_DISBURSEMENT_TYPE_KEY)
						.text(value.disbursemenT_TYPE));

			});
		});

}
	// GETING SESSION DATA
	function GetSession() {
		$.ajax({
			type: "GET",
			url: "/Disbursement/GetSessionValue",
			success: function (response) {
				var sessionValue = response.sessionValue;
				// Login Session 
			},
			error: function () {
				console.log("Error retrieving session value");
			}
		});

}
	// ADD NEW ROW TO THE HEAD TABLE
	function addNewRow() {


			// Get all "Add Row" buttons
			var addRowButtons = document.querySelectorAll('.add-row-button');

	// Disable all "Add Row" buttons
	addRowButtons.forEach(function (button) {
		button.disabled = true;
			});
	// Get the table body
	var tableBody = document.querySelector('#example tbody');

	// Get the last row in the table
	var lastRow = tableBody.lastElementChild;

	// Clone the last row
	var newRow = lastRow.cloneNode(true);
	// Initialize rowNo based on the last row
	var rowNo = lastRow ? parseInt(lastRow.querySelector('label[name="head_key[]"]').textContent) + 1 : 1;

	// Increment the row number
	//rowNo++;
	//console.log('Last Row Number:', rowNo);
	// Update the label in the cloned row with the new row number
	var label = newRow.querySelector('label[name="head_key[]"]');
	label.textContent = rowNo;

	// Clear input values in the cloned row (optional)
	var inputs = newRow.querySelectorAll('input');
	inputs.forEach(function (input) {
		input.value = '';
			});

	// Append the cloned row to the table
	tableBody.appendChild(newRow);

	// Get DataTable instance and redraw
	//var dataTable = $('#example').DataTable();
	//dataTable.draw();
	// Get the rowNo value from the last row
	var lastRowNo = parseInt(lastRow.querySelector('label[name="head_key[]"]').textContent);

	// Now you can use lastRowNo as needed
	//console.log('Last Row Number:', lastRowNo);
		}

	$("#ddlDisbursementType").on("change", function () {

		var DisbursementTypeKey = $("#ddlDisbursementType").val();
		var recType = '';

	//console.log(DisburseKey);
		if (DisbursementTypeKey == 2 || DisbursementTypeKey == 3) {
			if (DisbursementTypeKey == 2) {
				recType = 'Claim';
				ShowDisburseHead(recType, DisbursementTypeKey);
			}
			if (DisbursementTypeKey == 3) {
				recType = 'Advance';
				ShowDisburseHead(recType, DisbursementTypeKey);
			}
			
		}
		else {
			recType = 'HeadType';
			ShowDisburseHead(recType, DisbursementTypeKey);
		}
			});

function ShowDisburseHead(recType,DisburseKey) {
		 debugger;
		$.get("/Disbursement/GetDisbursementHead",
			{
				recType: recType,
				id: DisburseKey // Change 'id' to 'DisburseKey'
			},
			function (data) {
				// console.log(data);
				$('#ddlDisbursementHead').empty();
				$('#ddlDisbursementHead')
					.append($("<option></option>")
						.attr("value", "0")
						.text("Select"));

				$.each(data, function (key, value) {
					console.log(key);
					console.log(value);
					$('#ddlDisbursementHead')
						.append($("<option></option>")
							.attr("value", value.heaD_KEY)
							.text(value.heaD_REFNO));
					
				});
			}
		);


}
$("#CloseDisburse").on("click", function () {
	window.location.reload();
});
$("#ddlDisbursementHead").on("change", function () {


	$("#dvSubdivision").show();
		//var ddlDisbursementHead = $("#ddlDisbursementHead").val();
	var ddlDisbursementHeadType = $("#ddlDisbursementType").val();
	var headKey = $("#ddlDisbursementHead").val();
	$("#AC.HEAD_KEY").val(headKey);
	
	//console.log(ddlDisbursementHead);

	var headDisburseKey = 0;
	if ($("#AC_HEAD_DISBURSEMENT_KEY").val() != '') {
		headDisburseKey = $("#AC_HEAD_DISBURSEMENT_KEY").val();
	}
	var recType = '';
	if (ddlDisbursementHeadType == 1) {
		recType = 'SubDivision_Salary_';
		}
	if (ddlDisbursementHeadType == 2) {
		recType = 'SubDivision_Claim_';
		}
	if (ddlDisbursementHeadType == 3) {
		recType = 'SubDivision_Advance_';
		}
	if (ddlDisbursementHeadType == 4) {
		recType = 'SubDivision_Bonus_';
		}

	ShowSubDivisionAll(ddlDisbursementHeadType, headDisburseKey, recType, headKey);
	//GetDtlsDisbursement(headDisburseKey, HeadDisburseTypeKey, headKey);
	$("#validateDisbursement").show();
	//validateDisbursement
	});
	// 
function ShowSubDivisionAll(HeadDisburseTypeKey, headDisburseKey, recType, headKey) {
	debugger;
	if (HeadDisburseTypeKey == 1 || HeadDisburseTypeKey == 2 || HeadDisburseTypeKey == 3 || HeadDisburseTypeKey == 4) {


		let arrData = [];
		$.get("/Disbursement/GetSubDivision",
			{
				id1: headKey,//HeadDisburseTypeKey,
				id2: headDisburseKey,
				recType: recType
			},
			function (data) {

				arrData = JSON.parse(data);
				console.log(data);

				console.log(arrData[0]);
				console.log(arrData[1]);
				console.log(arrData[2]);

				key0 = arrData[0];
				key1 = arrData[1];
				key2 = arrData[2];
				//debugger;
				var fr_dt = arrData[3];
				var to_dt = arrData[4];

				// assign to the hidden field
				$('#hf_frDt').val(fr_dt);
				$('#hf_toDt').val(to_dt);

				$('#costcenter').html(function () {
					return key0;
				});
				$('#division').html(function () {
					return key1;
				});
				$('#warehouse').html(function () {
					return key2;
				});

				console.log(key0);
				console.log(key1);
				console.log(key2);

				//$('#costcenter').html(key0);
				//$('#division').html(key1);
				//$('#warehouse').html(key2);

				$('#FromDt').val(fr_dt);
				$('#ToDt').val(to_dt);

				//let htmlString = "<ul class='list-group' id='ItemList'><li class='list-group-item'><div class='checkboxCC'><input type='checkbox' id=80  />&nbsp;&nbsp;&nbsp;<label for=80 >Corporate</ label > </div></li><li class='list-group-item'><div class='checkboxDV'><input type='checkbox' id=6  />&nbsp;&nbsp;&nbsp;<label for=6 >KOLKATA1.3</ label > </div></li><li class='list-group-item'><div class='checkboxWH'><input type='checkbox' id=22  />&nbsp;&nbsp;&nbsp;<label for=22 >IT2</ label > </div></li ></ul > ";

				//let $html = $(htmlString);

				//$html.find('input[type="checkbox"]').each(function () {
				//	let checkboxId = $(this).attr('id');
				//	//if (checkboxId !== null && checkboxId !== undefined) {
				//	$(this).prop('checked', true);
				//	//}
				//});

				//var selectAllCheckbox = document.getElementById("costcenter");
				//var checkboxes = document.querySelectorAll('input[id="ItemList"]');

				//for (var i = 0; i < checkboxes.length; i++) {
				//	checkboxes[i].checked = selectAllCheckbox.checked;
				//	alert(i);
				//}

				GetDtlsDisbursement(headDisburseKey, HeadDisburseTypeKey, headKey);


			}
		);

	}
	else {
		GetDtlsDisbursement(headDisburseKey, HeadDisburseTypeKey, headKey);
	}
		//
	}

// created  19_02_2024
function GetDtlsDisbursement(Head_Disbursement_Key, Head_Disbursement_Type_Key, Head_Key) {
	

	//var Head_Disbursement_Key = 0;

	//var completeFlag = 0;

	if (typeof Head_Disbursement_Key !== "undefined" && Head_Disbursement_Key !== 0) // if (Head_Disbursement_Key != 0) 
	{
		//GetDisbursementType(Head_Disbursement_Type_Key);
		//$('#costcenter').html(key0);
		//$('#division').html(key1);
		//$('#warehouse').html(key2);
		$('#ddlDisbursementHead').val(Head_Key);
		debugger;
		let arrWH = [];

		$('.checkboxWH').each(function () {
			//debugger;
			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {
				//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
				arrWH.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
				// alert($(this).find('input[type="checkbox"]').val() + ' is checked');
			}
		});


		let arrDV = [];

		$('.checkboxDV').each(function () {

			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {

				arrDV.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array

			}
		});


		var InputData1 = {
			MAST_DISBURSEMENT_TYPE_KEY: Head_Disbursement_Type_Key,//$('#ddlDisbursementType').val(),
			FR_DT: $('#hf_frDt').val(),
			TO_DT: $('#hf_toDt').val(),
			selectedValuesDivision: arrDV,
			selectedValuesWarehouse: arrWH,
			HEAD_DISBURSEMENT_KEY: Head_Disbursement_Key,
			HEAD_KEY: Head_Key //$('#ddlDisbursementHead').val(),
		};
		var InputData2 = {
			dataArray: arrWH
		};
		//	console.log(InputData1, InputData2);
		// Rest of your code for the #btnSearch click event
		$.ajax({
			url: '/Disbursement/GetDisbursementDetails',
			type: 'POST',
			data: {
				model: InputData1, // Ensure that the parameter name matches the action method
				dataArray: InputData2.dataArray // Make sure you're sending the 'dataArray' property of InputData2
			},
			//data: {dataModel: InputData1, dataArray: InputData2 },
			dataType: 'json',
			success: function (data) {
				//		console.log(data.message); // Handle the response from the server
				rowCount = data.rowCount;
				$('#Div_DisbursementHead').html(data.html);

				//console.log(key0);
				//console.log(key1);
				//console.log(key2);
				//$('#costcenter').html(key0);
				//$('#division').html(key1);
				//$('#warehouse').html(key2);
				//completeFlag = data.flag;
			},
			error: function (error) {
				console.error('Error:', error);
			}
		});


	}
	else {

		if ($('#ddlDisbursementHead').val() === '') {
			// write validation
		}
		let arrWH = [];

		$('.checkboxWH').each(function () {
			//debugger;
			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {
				//alert($(this).find('input[type="checkbox"]').attr('id') + ' is checked');
				arrWH.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array
				// alert($(this).find('input[type="checkbox"]').val() + ' is checked');
			}
		});


		let arrDV = [];

		$('.checkboxDV').each(function () {

			var id = $(this).find('input[type="checkbox"]').attr('id');
			//alert(id);

			if ($(this).find('input[type="checkbox"]').prop('checked')) {

				arrDV.push($(this).find('input[type="checkbox"]').attr('id')); // Add the id to the array

			}
		});


		var InputData1 = {
			MAST_DISBURSEMENT_TYPE_KEY: $('#ddlDisbursementType').val(),
			FR_DT: $('#FromDt').val(),
			TO_DT: $('#ToDt').val(),
			selectedValuesDivision: arrDV,
			selectedValuesWarehouse: arrWH,

			HEAD_KEY: $('#ddlDisbursementHead').val(),
		};
		var InputData2 = {
			dataArray: arrWH
		};
		//	console.log(InputData1, InputData2);
		// Rest of your code for the #btnSearch click event
		$.ajax({
			url: '/Disbursement/GetDisbursementDetails',
			type: 'POST',
			data: {
				model: InputData1, // Ensure that the parameter name matches the action method
				dataArray: InputData2.dataArray // Make sure you're sending the 'dataArray' property of InputData2
			},
			//data: {dataModel: InputData1, dataArray: InputData2 },
			dataType: 'json',
			success: function (data) {
				//	console.log(data.message); // Handle the response from the server
				rowCount = data.rowCount;
				$('#Div_DisbursementHead').html(data.html);
			},
			error: function (error) {
				console.error('Error:', error);
			}
		});


	}


}

function updateDisbursementAmount() {
	debugger;
	var disbursementValues = rowCount; // Example values, replace with your data

	// Get the value entered in the HeaderDisburseAmnt input field
	var headerDisburseAmntValue = $('#HeaderDisburseAmnt').val();
	//console.log(headerDisburseAmntValue);
	var DisbursNetAmnt = 0;
	// Loop through each row and set the value in the DisbursementAmnt input field
	for (var i = 0; i < rowCount; i++) {

		var disbursementField = $('input[name="DisbursementAmnt[]"]').eq(i);
		if (disbursementField) {
			DisbursNetAmnt = $('input[name="DisbursNetAmnt[]"]').eq(i).val();
			//DisbursNetAmnt = DisbursNetAmnt1.val();
			//alert(DisbursNetAmnt)
			//console.log(parseFloat(DisbursNetAmnt).toFixed(2));
			//console.log(parseFloat(headerDisburseAmntValue).toFixed(2));
			//console.log(headerDisburseAmntValue);
			//= parseFloat(DisbursNetAmnt).toFixed(2)
			DisbursNetAmnt = (parseFloat(DisbursNetAmnt).toFixed(2) * parseFloat(headerDisburseAmntValue).toFixed(2)) / 100;
			disbursementField.val(DisbursNetAmnt);
		}
	}
}


//	function updateDisbursementAmount() {
//		// Assuming ii is the string containing your dynamically created table HTML
//		// Append the table to a container with id "tableContainer"
//		//document.getElementById("Div_DisbursementHead").innerHTML = data;
//		debugger;
//		// Set values for the "DisbursementAmnt[]" fields dynamically
//		var disbursementValues = rowCount; // Example values, replace with your data
//		//consle.log(disbursementValues);
//		for (var i = 0; i < rowCount; i++) {
//			//var ArrayHeading = document.getElementsByName("heading[]");
//			//var disbursementField = document.getElementsByName("DisbursementAmnt[]")[i];
//			var disbursementField = $('input[name="DisbursementAmnt[]"]').eq(i);
//			//var disbursementField = document.getElementsByName("DisbursementAmnt[]");
//			if (disbursementField) {
//				disbursementField.value[i] = $('#HeaderDisburseAmnt').value();
//			}
//		}
//}
