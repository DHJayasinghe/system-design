$(document).ready(function () {
	$("#btnSignIn").click(function (event) { 
		var username = $('#Username').val();
		var password = $('#Password').val();
		if (username == "" && password == "") {
			event.preventDefault();
			$('.incorrect-pw').html("<div class='primary-red'>Email address and Password are required.</div>");
			$('#Username').focus();
		}
		else if (username == ""){
			$('.incorrect-pw').html("<div class='primary-red'>Email address is required.</div>");
			event.preventDefault();
			$('#Username').focus();
		}
		else {
			var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
			if (!regex.test(username)) {
				event.preventDefault();
				$('.incorrect-pw').html("<div class='primary-red'>Incorrect Email Address.</div>");
				$('#Username').focus();
			};
        }
          
		
	});
}); 