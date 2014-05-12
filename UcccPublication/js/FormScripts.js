var bFormChanged=false;
var keysPressed="";

function treatBackspace() 
{ 
	if (event.keyCode == 8 && (event.srcElement.form == null || event.srcElement.isTextEdit == false))
	{
		event.cancelBubble = true;
		event.returnValue = false;
	}
} 

function formatPhone(myfield)
{ 
  var _OUTPUT=1
  var _return=false;
  /*
   * 7181238748 to 1(718)123-8748
   */ 

  if(myfield.length != 10)
  { 
    /* 
     * if user did not enter 10 digit phone number then simply print whatever user entered 
     */ 
	_return=_OUTPUT?myfield:false;
  } 
  else
  { 
    /* formating phone number here */ 
	_return="(";
	var ini = myfield.substring(0,3);
	_return+=ini+")";
	var st = myfield.substring(3,6);
	_return+=st+"-";
	var end = myfield.substring(6,10);
	_return+=end;
  }
  return _return; 
} 


function checkDate(cntrl)
{
//debugger;
  if ((window.event.keyCode==46)||(window.event.keyCode>=48 && window.event.keyCode<=57))
  {
    var i=0;
    if(cntrl.value.length>0)
    {
      i=cntrl.value.length;
      if ((i==2)||(i==5))
      {
        cntrl.value=cntrl.value+"/";
      }
      else if(i==11)
      {
        event.returnValue=false;
      }
    }
  }
  else
  {
    event.returnValue=false;
  }

}

function CheckDecimal(myfield,e)
{
var keycode;
if (window.event)
{ 
	keycode = window.event.keyCode;
}
else if (e)
{ 
	keycode = e.which;
}
else return true;

if (((keycode>47) && (keycode<58) )  || (keycode==8) ||keycode==46) { return true; }
else return false;
}

function checkNumeric(myfield,e)
{

var keycode;
if (window.event) keycode = window.event.keyCode;
else if (e) keycode = e.which;
else return true;

if (((keycode>47) && (keycode<58) )  || (keycode==8)) { return true; }
else return false;
}

function checkSSN(cntrl)
{
  if ((window.event.keyCode==46)||(window.event.keyCode>=48 && window.event.keyCode<=57))
  {
    var i=0;
    if(cntrl.value.length>0)
    {
      i=cntrl.value.length;
      if ((i==3)||(i==6))
      {
        cntrl.value=cntrl.value+"-";
      }
    }
  }
  else
  {
    event.returnValue=false;
  }

}

function checkTime(cntrl)
{
  if (cntrl.value!="")
  {
	var valid= /^([01]?[0-9]|[2][0-3])(:[0-5][0-9])?$/.test(cntrl.value) 
	if (!valid)
	{
		alert("Invalid time!");
		cntrl.focus();
	}
 }
}


function clearKeys()
{
  keysPressed="";
}

function confirmDeleteSimple()
{
  if (confirm("This record will be permanently deleted. Are you sure you want to continue?"))
  {
		formChanged(false);
		return true;
       //sendDelete();
  }
  else
  {
	//event.returnValue=false;
	return false;
  }
}

function confirmDelete(msg)
{
  if (confirm(msg))
  {
		formChanged(false);
		return true;
  }
  else
  {
	return false;
  }
}



function disallowKeyEntry()
{
  if (window.event.keyCode<127)
  {
    window.event.returnValue=false;
  }
}



function displayStudyMessage() 
{ 
	Page_ClientValidate();
	if(Page_IsValid)
	{
		document.all.message.innerText="Please wait while the system is gathering data for this report. This process may take several minutes.";
	} 
 
 }

function enter()
{
var keycode;
if (window.event)
  keycode = window.event.keyCode;
else if (e)
  keycode = e.which;
else
  keyCode=0;
if (keycode == 13)
{
  formChanged(false);
  document.editForm.action+="?updateAction=Save";
  document.editForm.submit();
}
}

function findOption(cntrl)
{
  var keyString=String.fromCharCode(window.event.keyCode).toUpperCase();
  var matchFound=false;
  keysPressed+=keyString;
  for (var i=0;i<cntrl.options.length;i++)
  {
    optionSection=cntrl.options[i].text.substring(0,keysPressed.length).toUpperCase();
    if (optionSection==keysPressed)
    {
        cntrl.selectedIndex=i;
        matchFound=true;
        break;
    }
  }
  if (matchFound==false)
  {
    keysPressed="";
  }
  window.event.returnValue=false;
}

function formatCurrency(num) {
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
//sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
//((sign)?'':'-')+  --this needs to go before num in the next line to block negatives
return (   num + '.' + cents);
}

function formatDecimal(num,length) {
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
//sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
//((sign)?'':'-')+  --this needs to go before num in the next line to block negatives
var newnum=num+'.'+cents;
if (newnum.toString().length>length)
{
  alert ("This number exceeds the maximum allowed.");
  return '';
}
else
{
  return (   num + '.' + cents);
}
}


function formatTime(cntrl)
{
  if ((window.event.keyCode>=48 && window.event.keyCode<=57))
  {
    var i=0;
    if(cntrl.value.length>0)
    {
      i=cntrl.value.length; 
      if ((i==1))
      {
        cntrl.value=cntrl.value+":";
        event.returnValue=true;
      }
      else if (i==4)
      {
		
		var temp=cntrl.value.substr(0,1);
		temp=temp+cntrl.value.substr(2,1);
		temp=temp+":";
		temp=temp+cntrl.value.substr(3,2);
		cntrl.value=temp;
		event.returnValue=true;
	  }
	  else if (i>5)
      {
		event.returnValue=false;
	  }
	  
	  
    }
    
  }
  else
  {
    event.returnValue=false;
  }

}

function formChanged(bChanged)
{
  bFormChanged=bChanged;
  keysPressed="";
}




function onKeyPress () 
{
	var keycode;
	if (window.event) keycode = window.event.keyCode;
	else if (e) keycode = e.which;
	else return true;
	if (keycode == 13) 
	{
		//alert("Please Click on the Submit button to send this");
		return false;
	}
	return true ;
}
function SendForm () 
{
	var keycode;
	if (window.event) keycode = window.event.keyCode;
	else if (e) keycode = e.which;
	else return true;
	if (keycode == 13) 
	{
		
		if (document.forms[0].ContinueButton!=null)
		{
			document.forms[0].ContinueButton.click();
		}
		return false;
	}
	return true ;
}


function sendDelete()
{
alert(document.forms[0]);
    if (document.forms[0]!=null)
    {
      formChanged(false);
      document.forms[0].submit();
    }
}

function setUpdaterFocus(controlName)
{

	if (document.all['AddButton'].style.display == 'block')
	{
		document.forms[0].Add.focus();
	}
	else
	{
		document.forms[0].elements[controlName].focus();
	}
}


function toggleCRAFTE()
{
  if (document.all['LPersonnelSuperGroupIds_0'].checked)
  {
      document.all['CRADiv'].style.display='block';
      document.all['CRADiv2'].style.display='block';
      
  }
  else
  {
      document.all['CRADiv'].style.display='none';
      document.all['CRADiv2'].style.display='none';
  }
}

function warnOnExit()
{
    if (bFormChanged==true)
    {
    if(document.editForm != null)
    {
      if (document.editForm.loadBoxes!=null)
      {
        if (document.editForm.loadBoxes.value=="false")
        {
          event.returnValue = "Warning: Modified data has not been saved.";
        }
      }
      else
      {
         event.returnValue = "Warning: Modified data has not been saved.";
      }
      }
      else
      {
		event.returnValue = "Warning: Modified data has not been saved.";
      }
    }
    
}




<!-- Original: Richard Gorremans (xbase@volcano.net) ==>
<!-- Updates: www.spiritwolfx.com
<!-- Begin

// Check browser version
var isNav4 = false, isNav5 = false, isIE4 = false
var strSeperator = "/"; 
// If you are using any Java validation on the back side you will want to use the / because 
// Java date validations do not recognize the dash as a valid date separator.

var vDateType = 3; // Global value for type of date format
//                1 = mm/dd/yyyy
//                2 = yyyy/dd/mm  (Unable to do date check at this time)
//                3 = dd/mm/yyyy

var vYearType = 4; //Set to 2 or 4 for number of digits in the year for Netscape
var vYearLength = 2; // Set to 4 if you want to force the user to enter 4 digits for the year before validating.

var err = 0; // Set the error code to a default of zero


if(navigator.appName == "Netscape") 
{
   if (navigator.appVersion < "5")  
   {
      isNav4 = true;
      isNav5 = false;
	}
   else
   if (navigator.appVersion > "4") 
   {
      isNav4 = false;
      isNav5 = true;
	}
}
else  
{
   isIE4 = true;
}


function DateFormat(vDateName, vDateValue, e, dateCheck, dateType)  {

vDateType = dateType;
mDateValue = vDateValue;

        
// vDateName = object name
// vDateValue = value in the field being checked
// e = event
// dateCheck 
//       True  = Verify that the vDateValue is a valid date
//       False = Format values being entered into vDateValue only
// vDateType
//       1 = mm/dd/yyyy
//       2 = yyyy/mm/dd
//       3 = dd/mm/yyyy

   
   //Enter a tilde sign for the first number and you can check the variable information.
   if (vDateValue == "~")
   {
      alert("AppVersion = "+navigator.appVersion+" \nNav. 4 Version = "+isNav4+" \nNav. 5 Version = "+isNav5+" \nIE Version = "+isIE4+" \nYear Type = "+vYearType+" \nDate Type = "+vDateType+" \nSeparator = "+strSeperator);
      vDateName.value = "";
      vDateName.focus();
      return true;
   }
      
   var whichCode = (window.Event) ? e.which : e.keyCode;
 
   // Check to see if a seperator is already present.
   // bypass the date if a seperator is present and the length greater than 8
   if (vDateValue.length > 8 && isNav4)
   {
      if ((vDateValue.indexOf("-") >= 1) || (vDateValue.indexOf("/") >= 1))
         return true;
   }
   
   //Eliminate all the ASCII codes that are not valid
   var alphaCheck = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/-";
   if (alphaCheck.indexOf(vDateValue) >= 1)  
   {
      if (isNav4)
      {
         vDateName.value = "";
         vDateName.focus();
         vDateName.select();
         return false;
      }
      else
      {
         vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
         return false;
      } 
   }
   if (whichCode == 8) //Ignore the Netscape value for backspace. IE has no value
      return false;
   else 
   {
      //Create numeric string values for 0123456789/
      //The codes provided include both keyboard and keypad values
      
      var strCheck = '47,48,49,50,51,52,53,54,55,56,57,58,59,95,96,97,98,99,100,101,102,103,104,105';
      if (strCheck.indexOf(whichCode) != -1)  
      {
         if (isNav4)  
         {
            if (((vDateValue.length < 6 && dateCheck) || (vDateValue.length == 7 && dateCheck)) && (vDateValue.length >=1))
            {
               alert("Invalid Date\nPlease Re-Enter");
               vDateName.value = "";
               vDateName.focus();
               vDateName.select();
               return false;
            }
            if (vDateValue.length == 6 && dateCheck)  
            {
               var mDay = vDateName.value.substr(2,2);
               var mMonth = vDateName.value.substr(0,2);
               var mYear = vDateName.value.substr(4,4)
               
               //Turn a two digit year into a 4 digit year
               if (mYear.length == 2 && vYearType == 4) 
               {
                  var mToday = new Date();
                  
                  //If the year is greater than 30 years from now use 19, otherwise use 20
                  var checkYear = mToday.getFullYear() + 30; 
                  var mCheckYear = '20' + mYear;
                  if (mCheckYear >= checkYear)
                     mYear = '19' + mYear;
                  else
                     mYear = '20' + mYear;
               }
               var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
               
               if (!dateValid(vDateValueCheck))  
               {
                  alert("Invalid Date\nPlease Re-Enter");
                  vDateName.value = "";
                  vDateName.focus();
                  vDateName.select();
                  return false;
		         }
               vDateName.value = vDateValueCheck;
               return true;
            
            }
            else
            {
               // Reformat the date for validation and set date type to a 1
               
               
               if (vDateValue.length >= 8  && dateCheck)  
               {
                  if (vDateType == 1) // mmddyyyy
                  {
                     var mDay = vDateName.value.substr(2,2);
                     var mMonth = vDateName.value.substr(0,2);
                     var mYear = vDateName.value.substr(4,4)
                     vDateName.value = mMonth+strSeperator+mDay+strSeperator+mYear;
                  }
                  if (vDateType == 2) // yyyymmdd
                  {
                     var mYear = vDateName.value.substr(0,4)
                     var mMonth = vDateName.value.substr(4,2);
                     var mDay = vDateName.value.substr(6,2);
                     vDateName.value = mYear+strSeperator+mMonth+strSeperator+mDay;
                  }
                  if (vDateType == 3) // ddmmyyyy
                  {
                     var mMonth = vDateName.value.substr(2,2);
                     var mDay = vDateName.value.substr(0,2);
                     var mYear = vDateName.value.substr(4,4)
                     vDateName.value = mDay+strSeperator+mMonth+strSeperator+mYear;
                  }
                  
                  //Create a temporary variable for storing the DateType and change
                  //the DateType to a 1 for validation.
                  
                  var vDateTypeTemp = vDateType;
                  vDateType = 1;
                  var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
                  
                  if (!dateValid(vDateValueCheck))  
                  {
                     alert("Invalid Date\nPlease Re-Enter");
                     vDateType = vDateTypeTemp;
                     vDateName.value = "";
                     vDateName.focus();
                     vDateName.select();
                     return false;
		            }
                     vDateType = vDateTypeTemp;
                     return true;
	            }
               else
               {
                  if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >=1))
                  {
                     alert("Invalid Date\nPlease Re-Enter");
                     vDateName.value = "";
                     vDateName.focus();
                     vDateName.select();
                     return false;
                  }
               }
            }
         }
         else  
         {
         // Non isNav Check
            if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >=1))
            {
               alert("Invalid Date\nPlease Re-Enter");
               vDateName.value = "";
               vDateName.focus();
               return true;
            }
            
            // Reformat date to format that can be validated. mm/dd/yyyy
            
            
            if (vDateValue.length >= 8 && dateCheck)  
            {
            
               // Additional date formats can be entered here and parsed out to
               // a valid date format that the validation routine will recognize.
               
               if (vDateType == 1) // mm/dd/yyyy
               {
                  var mMonth = vDateName.value.substr(0,2);
                  var mDay = vDateName.value.substr(3,2);
                  var mYear = vDateName.value.substr(6,4)
               }
               if (vDateType == 2) // yyyy/mm/dd
               {
                  var mYear = vDateName.value.substr(0,4)
                  var mMonth = vDateName.value.substr(5,2);
                  var mDay = vDateName.value.substr(8,2);
               }
               if (vDateType == 3) // dd/mm/yyyy
               {
                  var mDay = vDateName.value.substr(0,2);
                  var mMonth = vDateName.value.substr(3,2);
                  var mYear = vDateName.value.substr(6,4)
               }
               if (vYearLength == 4)
               {
                  if (mYear.length < 4)
                  {
                     alert("Invalid Date\nPlease Re-Enter");
                     vDateName.value = "";
                     vDateName.focus();
                     return true;
                  }
               }
               
               // Create temp. variable for storing the current vDateType
               var vDateTypeTemp = vDateType;
               
               // Change vDateType to a 1 for standard date format for validation
               // Type will be changed back when validation is completed.
               vDateType = 1;
               
               // Store reformatted date to new variable for validation.
               var vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
               
               if (mYear.length == 2 && vYearType == 4 && dateCheck)  
               {
                  
                  //Turn a two digit year into a 4 digit year
                  var mToday = new Date();
                  
                  //If the year is greater than 30 years from now use 19, otherwise use 20
                  var checkYear = mToday.getFullYear() + 30; 
                  var mCheckYear = '20' + mYear;
                  if (mCheckYear >= checkYear)
                     mYear = '19' + mYear;
                  else
                     mYear = '20' + mYear;
                  vDateValueCheck = mMonth+strSeperator+mDay+strSeperator+mYear;
                  
                  // Store the new value back to the field.  This function will
                  // not work with date type of 2 since the year is entered first.
                  
                  if (vDateTypeTemp == 1) // mm/dd/yyyy
                     vDateName.value = mMonth+strSeperator+mDay+strSeperator+mYear;
                  if (vDateTypeTemp == 3) // dd/mm/yyyy
                     vDateName.value = mDay+strSeperator+mMonth+strSeperator+mYear;

               } 
               
               
               if (!dateValid(vDateValueCheck))  
               {
                  alert("Invalid Date\nPlease Re-Enter");
                  vDateType = vDateTypeTemp;
                  vDateName.value = "";
                  vDateName.focus();
                  return true;
		         }
               vDateType = vDateTypeTemp;
               return true;
            
            }
            else
            {
               
               if (vDateType == 1)
               {
                  if (vDateValue.length == 2)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
                  if (vDateValue.length == 5)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
               }
               if (vDateType == 2)
               {
                  if (vDateValue.length == 4)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
                  if (vDateValue.length == 7)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
               } 
               if (vDateType == 3)
               {
                  if (vDateValue.length == 2)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
                  if (vDateValue.length == 5)  
                  {
                     vDateName.value = vDateValue+strSeperator;
                  }
               }
               return true;
            }
         }
         if (vDateValue.length == 10   && dateCheck)  
         {
            if (!dateValid(vDateName))  
            {
// Un-comment the next line of code for debugging the dateValid() function error messages
//               alert(err);  
               alert("Invalid Date\nPlease Re-Enter");
               vDateName.focus();
               vDateName.select();
	         }
         }
         return false;
      }
      else  
      {
         // If the value is not in the string return the string minus the last
         // key entered.
         if (isNav4)
         {
            vDateName.value = "";
            vDateName.focus();
            vDateName.select();
            return false;
         }
         else
         {
            vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
            return false;
         }
		}
	}
}
	function NoLaterThanToday(theDate) {
	    var earlyDate = new Date("1/1/2008");
	    var enteredDate = new Date(theDate.value)
	    var today = new Date();

	    if (enteredDate > today || enteredDate < earlyDate) {
	        alert(theDate.value + " is invalid date.");
	        theDate.value = "";
	        return false;
	    }
	}

   function dateValid(objName) {
      var strDate;
      var strDateArray;
      var strDay;
      var strMonth;
      var strYear;
      var intday;
      var intMonth;
      var intYear;
      var booFound = false;
      var datefield = objName;
      var strSeparatorArray = new Array("-"," ","/",".");
      var intElementNr;
      // var err = 0;
      var strMonthArray = new Array(12);
      strMonthArray[0] = "Jan";
      strMonthArray[1] = "Feb";
      strMonthArray[2] = "Mar";
      strMonthArray[3] = "Apr";
      strMonthArray[4] = "May";
      strMonthArray[5] = "Jun";
      strMonthArray[6] = "Jul";
      strMonthArray[7] = "Aug";
      strMonthArray[8] = "Sep";
      strMonthArray[9] = "Oct";
      strMonthArray[10] = "Nov";
      strMonthArray[11] = "Dec";
      
      //strDate = datefield.value;
      strDate = objName;
      
      if (strDate.length < 1) {
         return true;
      }
      for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
         if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) 
         {
            strDateArray = strDate.split(strSeparatorArray[intElementNr]);
            if (strDateArray.length != 3) 
            {
               err = 1;
               return false;
            }
            else 
            {
               strDay = strDateArray[0];
               strMonth = strDateArray[1];
               strYear = strDateArray[2];
            }
            booFound = true;
         }
      }
      if (booFound == false) {
         if (strDate.length>5) {
            strDay = strDate.substr(0, 2);
            strMonth = strDate.substr(2, 2);
            strYear = strDate.substr(4);
         }
      }
      //Adjustment for short years entered
      if (strYear.length == 2) {
         strYear = '20' + strYear;
      }
      strTemp = strDay;
      strDay = strMonth;
      strMonth = strTemp;
      intday = parseInt(strDay, 10);
      if (isNaN(intday)) {
         err = 2;
         return false;
      }
      
      intMonth = parseInt(strMonth, 10);
      if (isNaN(intMonth)) {
         for (i = 0;i<12;i++) {
            if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
               intMonth = i+1;
               strMonth = strMonthArray[i];
               i = 12;
            }
         }
         if (isNaN(intMonth)) {
            err = 3;
            return false;
         }
      }
      intYear = parseInt(strYear, 10);
      if (isNaN(intYear)) {
         err = 4;
         return false;
      }
      if (intMonth>12 || intMonth<1) {
         err = 5;
         return false;
      }
      if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
         err = 6;
         return false;
      }
      if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
         err = 7;
         return false;
      }
      if (intMonth == 2) {
         if (intday < 1) {
            err = 8;
            return false;
         }
         if (LeapYear(intYear) == true) {
            if (intday > 29) {
               err = 9;
               return false;
            }
         }
         else {
            if (intday > 28) {
               err = 10;
               return false;
            }
         }
      }
         return true;
      }

   function LeapYear(intYear) {
      if (intYear % 100 == 0) {
         if (intYear % 400 == 0) { return true; }
      }
      else {
         if ((intYear % 4) == 0) { return true; }
      }
         return false;
      }

 function echeck(str) {
  var at="@"
  var dot="."
  var lat=str.indexOf(at)
  var lstr=str.length
  var ldot=str.indexOf(dot)
  if (str.indexOf(at)==-1){
     alert("Invalid E-mail ID")
     return false
  }
  if (str.indexOf(at)==-1 || str.indexOf(at)==0 || str.indexOf(at)==lstr){
     alert("Invalid E-mail ID")
     return false
  }
  if (str.indexOf(dot)==-1 || str.indexOf(dot)==0 || str.indexOf(dot)==lstr){
      alert("Invalid E-mail ID")
      return false
  }
   if (str.indexOf(at,(lat+1))!=-1){
      alert("Invalid E-mail ID")
      return false
   }
   if (str.substring(lat-1,lat)==dot || str.substring(lat+1,lat+2)==dot){
      alert("Invalid E-mail ID")
      return false
   }
   if (str.indexOf(dot,(lat+2))==-1){
      alert("Invalid E-mail ID")
      return false
   }
   if (str.indexOf(" ")!=-1){
      alert("Invalid E-mail ID")
      return false
   }
   return true          
}

function ValidateForm(){
  var emailID=document.AddForm.workEmail
  
  if ((emailID.value==null)||(emailID.value=="")){
    alert("Please Enter your Email ID")
    emailID.focus()
    return false
  }
  if (echeck(emailID.value)==false){
    emailID.value=""
    emailID.focus()
    return false
  }
  return true
 }
 
function emailCheck (emailStr) {

	/* The following variable tells the rest of the function whether or not
	to verify that the address ends in a two-letter country or well-known
	TLD.  1 means check it, 0 means don't. */

	var checkTLD=1;

	/* The following is the list of known TLDs that an e-mail address must end with. */

	var knownDomsPat=/^(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum)$/;

	/* The following pattern is used to check if the entered e-mail address
	fits the user@domain format.  It also is used to separate the username
	from the domain. */

	var emailPat=/^(.+)@(.+)$/;

	/* The following string represents the pattern for matching all special
	characters.  We don't want to allow special characters in the address. 
	These characters include ( ) < > @ , ; : \ " . [ ] */

	var specialChars="\\(\\)><@,;:\\\\\\\"\\.\\[\\]";

	/* The following string represents the range of characters allowed in a 
	username or domainname.  It really states which chars aren't allowed.*/

	var validChars="\[^\\s" + specialChars + "\]";

	/* The following pattern applies if the "user" is a quoted string (in
	which case, there are no rules about which characters are allowed
	and which aren't; anything goes).  E.g. "jiminy cricket"@disney.com
	is a legal e-mail address. */

	var quotedUser="(\"[^\"]*\")";

	/* The following pattern applies for domains that are IP addresses,
	rather than symbolic names.  E.g. joe@[123.124.233.4] is a legal
	e-mail address. NOTE: The square brackets are required. */

	var ipDomainPat=/^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/;

	/* The following string represents an atom (basically a series of non-special characters.) */

	var atom=validChars + '+';

	/* The following string represents one word in the typical username.
	For example, in john.doe@somewhere.com, john and doe are words.
	Basically, a word is either an atom or quoted string. */

	var word="(" + atom + "|" + quotedUser + ")";

	// The following pattern describes the structure of the user

	var userPat=new RegExp("^" + word + "(\\." + word + ")*$");

	/* The following pattern describes the structure of a normal symbolic
	domain, as opposed to ipDomainPat, shown above. */

	var domainPat=new RegExp("^" + atom + "(\\." + atom +")*$");

	/* Finally, let's start trying to figure out if the supplied address is valid. */

	/* Begin with the coarse pattern to simply break up user@domain into
	different pieces that are easy to analyze. */

	var matchArray=emailStr.match(emailPat);

	if (matchArray==null) {
		/* Too many/few @'s or something; basically, this address doesn't
		even fit the general mould of a valid e-mail address. */

		alert("Email address seems incorrect (check @ and .'s)");
		return false;
	}
	var user=matchArray[1];
	var domain=matchArray[2];

	// Start by checking that only basic ASCII characters are in the strings (0-127).

	for (i=0; i<user.length; i++) {
		if (user.charCodeAt(i)>127) {
			alert("Ths username contains invalid characters.");
			return false;
		}
	}
	for (i=0; i<domain.length; i++) {
		if (domain.charCodeAt(i)>127) {
			alert("Ths domain name contains invalid characters.");
			return false;
		}
	}

	// See if "user" is valid 

	if (user.match(userPat)==null) {
		// user is not valid
		alert("The username doesn't seem to be valid.");
		return false;
	}

	/* if the e-mail address is at an IP address (as opposed to a symbolic
	host name) make sure the IP address is valid. */

	var IPArray=domain.match(ipDomainPat);
	if (IPArray!=null) {
		// this is an IP address
		for (var i=1;i<=4;i++) {
			if (IPArray[i]>255) {
				alert("Destination IP address is invalid!");
				return false;
			}
		}
		return true;
	}

	// Domain is symbolic name.  Check if it's valid.
	 
	var atomPat=new RegExp("^" + atom + "$");
	var domArr=domain.split(".");
	var len=domArr.length;
	for (i=0;i<len;i++) {
		if (domArr[i].search(atomPat)==-1) {
			alert("The domain name does not seem to be valid.");
			return false;
		}
	}

	/* domain name seems valid, but now make sure that it ends in a
	known top-level domain (like com, edu, gov) or a two-letter word,
	representing country (uk, nl), and that there's a hostname preceding 
	the domain or country. */

	if (checkTLD && domArr[domArr.length-1].length!=2 && 
			domArr[domArr.length-1].search(knownDomsPat)==-1) 
	{
		alert("The address must end in a well-known domain or two letter " + "country.");
		return false;
	}

	// Make sure there's a host name preceding the domain.

	if (len<2) {
		alert("This address is missing a hostname!");
		return false;
	}

	// If we've gotten this far, everything's valid!
	return true;
}

function validateZIP(field) {
	var valid = "0123456789-";
	var hyphencount = 0;

	if (field.length!=5 && field.length!=10) {
		alert("Please enter your 5 digit or 5 digit+4 zip code.");
		return false;
	}
	for (var i=0; i < field.length; i++) {
		temp = "" + field.substring(i, i+1);
		if (temp == "-") 
			hyphencount++;
		if (valid.indexOf(temp) == "-1") {
			alert("Invalid characters in your zip code.  Please try again.");
			return false;
		}
		if ((hyphencount > 1) || ((field.length==10) && ""+field.charAt(5)!="-")) {
			alert("The hyphen character should be used with a properly formatted 5 digit+four zip code, like '12345-6789'.   Please try again.");
			return false;
		}
	}
	return true;
}
