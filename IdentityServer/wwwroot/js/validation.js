$(document).ready(function () ***REMOVED***
	$("#btnSignIn").click(function (event) ***REMOVED*** 
		var username = $('#Username').val();
		var password = $('#Password').val();
		if (username == "" && password == "") ***REMOVED***
			event.preventDefault();
			$('.incorrect-pw').html("<div class='primary-red'>Email address and Password are required.</div>");
			$('#Username').focus();
		***REMOVED***
		else if (username == "")***REMOVED***
			$('.incorrect-pw').html("<div class='primary-red'>Email address is required.</div>");
			event.preventDefault();
			$('#Username').focus();
		***REMOVED***
		else ***REMOVED***
			var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]***REMOVED***2,4***REMOVED***)+$/;
			if (!regex.test(username)) ***REMOVED***
				event.preventDefault();
				$('.incorrect-pw').html("<div class='primary-red'>Incorrect Email Address.</div>");
				$('#Username').focus();
			***REMOVED***;
        ***REMOVED***
          
		
	***REMOVED***);
***REMOVED***); 