$('#CreateNewConversation').on('click', function (e) {
    $('#newMessageBox').show()
})

$('#closeInfo').click(function () {
    $('#newMessageBox').hide()
})

$('#ModifyMessage').on('click', function (e) {
    $('#modifyMessage').show()
})