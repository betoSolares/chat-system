﻿// Hide or show the menu in mobile display
$(document).ready(function () {
    $(".navbar-burger").click(function () {
        $(".navbar-burger").toggleClass("is-active")
        $(".navbar-menu").toggleClass("is-active")
    })
})