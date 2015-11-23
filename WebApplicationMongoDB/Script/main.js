/*Menu*/

(function ($) {
    $('#header-icon').click(function (e) {
        e.preventDefault();
        $('body').toggleClass('with--sidebar');
    })
    //$('.with--sidebar > #site-cache').click(function (e) {
    //    $('body').removeClass('with--sidebar');
    //})
    $('#menu-cross').click(function (e) {
        $('body').removeClass('with--sidebar');
    })
    if ($(window).width() > 850) {
        $('body').removeClass('with--sidebar');
    }

})(jQuery);


/*Autoscroll*/

function juizScrollTo(element) {
    $(element).click(function () {
        var goscroll = false;
        var the_hash = $(this).attr("href");
        var regex = new RegExp("\#(.*)", "gi");
        var the_element = '';

        if (the_hash.match("\#(.+)")) {
            the_hash = the_hash.replace(regex, "$1");

            if ($("#" + the_hash).length > 0) {
                the_element = "#" + the_hash;
                goscroll = true;
            }
            else if ($("a[name=" + the_hash + "]").length > 0) {
                the_element = "a[name=" + the_hash + "]";
                goscroll = true;
            }

            if (goscroll) {
                $('html, body').animate({
                    scrollTop: $(the_element).offset().top
                }, 'slow');
                return false;
            }
        }
    });
};
juizScrollTo('a[href^="#"]');


/*Pop up*/

(function ($) {
    $('#modal-open-button').click(function (e) {
        e.preventDefault();
        $('body').addClass('poping-up');
    })
    $('#modal-close-button').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up');
    })
    $('#modal-close-btn').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up');
    })
    $('.poping-up > #black').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up');
    })
    //var $popupContent = $("#popup-content");
    //var top = ($(window).height() - $popupContent.outerHeight()) / 3;
    //var left = ($(window).width() - $popupContent.outerWidth()) ;
    //$("#popup-content").css({
    //    'top': top,
    //    'left': left
    //});

})(jQuery);


(function ($) {
    $('#modal-open-button2').click(function (e) {
        e.preventDefault();
        $('body').addClass('poping-up2');
    })
    $('#modal-close-button2').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up2');
    })
    $('#modal-close-btn2').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up2');
    })
    $('.poping-up > #black').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up2');
    })
    //var $popupContent = $("#popup-content2");
    //var top = ($(window).height() - $popupContent.outerHeight()) / 3;
    //var left = ($(window).width() - $popupContent.outerWidth());
    //$("#popup-content2").css({
    //    'top': top,
    //    'left': left
    //});

})(jQuery);


/*Page2 inscription popping up*/

(function ($) {
    

    $('#testBtn').click(function (e) {
        e.preventDefault();
        $('body').addClass('page2-Inscription');
    })
    $('#testBtn').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up');
    })
    $('#modal-close-button3').click(function (e) {
        e.preventDefault();
        $('body').removeClass('page2-Inscription');
    })
    $('#modal-close-btn3').click(function (e) {
        e.preventDefault();
        $('body').removeClass('page2-Inscription');
    })
    $('.poping-up > #black').click(function (e) {
        e.preventDefault();
        $('body').removeClass('poping-up2');
    })
    $('#openPage2Btn').click(function (e) {
        e.preventDefault();
        var email = $('intMailPage1').val();
        $('#intMailPage2').val(email);
    })

    //var $popupContent = $("#pageInscription2");
    //var top = ($(window).height() - $popupContent.outerHeight()) / 3;
    //var left = ($(window).width() - $popupContent.outerWidth());
    //$("#pageInscription2").css({
    //    'top': top,
    //    'left': left
    //});

})(jQuery);