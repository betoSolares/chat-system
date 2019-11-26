$(document).ready(function () {
    if ($('#state').val() == 'SUCCESS') {
        $('#alertBox').show()
        $('#alertBox').addClass('SUCCESS')
        $('#message').text("La operación se realizo de manera exitosa");
    } else if ($('#state').val() != "") {
        $('#alertBox').show()
        $('#alertBox').addClass('failed')
        $('#message').text($('#error').val());
    }
})

$('#closebtn').click(function () {
    $('#alertBox').hide()
})