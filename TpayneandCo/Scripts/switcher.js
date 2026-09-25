$(document).ready(function () {

    $.validator.addMethod("greaterThan",
    function (value, element, param) {
        var $min = $('#minprice'); //$(param);
        //        //if (this.settings.onfocusout) {
        //            //$min.off(".validate-greaterThan").on("changed.bs.select", function () {
        //$(element).valid();
        //            //});
        return parseInt(value) > parseInt($min.val());
    });

    $.validator.addMethod("lessThan",
    function (value, element, param) {
        var $max = $('#maxprice').val();
        return parseInt(value) < parseInt($max);
    });

    $('form#sales').validate({
        submitHandler: function (form) {
            form.submit();
        },
        errorClass: 'error-msg',
        errorPlacement: function (error, element) {
            error.insertAfter(element);
        },
        highlight: function (element, errorClass) {
            $(element).parent('.inp-wrap').addClass("error");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parent('.inp-wrap').removeClass("error");
        },
        rules: {
            MaxPrice: { greaterThan: 'MinPrice' },
            MinPrice: { lessThan: 'MaxPrice' }
        },
        messages: {
            MaxPrice: { greaterThan: 'Max Price must be greater than Min Price' },
            MinPrice: { lessThan: 'Min Price must be less than Max Price' }
        }
    });

    $('#list').click(function (event) {
        event.preventDefault();
        $('#products .item').addClass('list-group-item');
    });
    $('#grid').click(function (event) {
        event.preventDefault();
        $('#products .item').removeClass('list-group-item');
        $('#products .item').addClass('grid-group-item');
    });

    $('#orderby').on('changed.bs.select', function (event) {

        var currentUrl = window.location.href;

        if (currentUrl.indexOf('orderby=') > -1) {

            //url = url.slice(0, url.indexOf('&'));
            var parsedUrl = $.url(currentUrl);
            var currentOrder = url('?orderby', parsedUrl);
            //if (url.match("your string").length > 0) {
            var newOrder = $(this).val();

            var newUrl = location.href.replace("orderby=" + currentOrder, "orderby=" + newOrder);
            window.location.href = newUrl;
        }
        else {
            var params = { 'orderby': $(this).val() };
            window.location.href = currentUrl + '?' + jQuery.param(params);
        }

    });

    var currentUrl = window.location.href;
    var parsedUrl = $.url(currentUrl);

    var order = url('?orderby', parsedUrl);//$.url.attr('orderby');//$.url().param('?orderby');
    var region = url('?Location', parsedUrl);
    var minBeds = url('?MinBedrooms', parsedUrl);
    var minPrice = url('?MinPrice', parsedUrl);
    var maxPrice = url('?MaxPrice', parsedUrl);

    $('.selectpicker').val(order);

    $('[name="Location"]').val(region);

    $('[name="MinBedrooms"]').val(minBeds);

    $('[name="MinPrice"]').val(minPrice);

    $('[name="MaxPrice"]').val(maxPrice);
    //
    $('.selectpicker').selectpicker('render');

    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
        $('.selectpicker').selectpicker('mobile');
    }
});