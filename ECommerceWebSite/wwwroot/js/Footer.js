$(document).ready(function () {

    $('footer .middleFooter .col1 .panel-heading a').on('click', function () {
        if ($(this).attr('aria-expanded') === 'true') {
            //change span class fa-angle-down
            $('footer .middleFooter .col1 .panel-heading a').find('span.fa').removeClass('fa-angle-up').addClass('fa-angle-down');
            $(this).find('span.fa').removeClass('fa-angle-up').addClass('fa-angle-down')
        } else {
            //change span class fa-angle-up
            $('footer .middleFooter .col1 .panel-heading a').find('span.fa').removeClass('fa-angle-up').addClass('fa-angle-down');
            $(this).find('span.fa').removeClass('fa-angle-down').addClass('fa-angle-up');

        }
    });

});