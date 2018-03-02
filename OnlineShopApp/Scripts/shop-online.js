// my jquery scripts

$('.image-edit-input').hide();

$(function () {
    $('#IsChangeImage').change(function () {
        $('.image-edit-input').toggle(this.checked);
    }).change();

    $('.quantity').bind('keyup mouseup', function () {
        var addButton = $(this).next();
        var href = addButton.attr('href');
        addButton.attr('href', href.replace(/&?quantity=\d+/, ''));
        addButton.attr('href', addButton.attr('href') + '&quantity=' + $(this).val());

        var quantity = $(this);
        var maxQuantity = $('#max-' + quantity.attr('id'));
        var validation = $('.validation-summary-valid');
        if (parseInt($.trim(maxQuantity.text())) < parseInt(quantity.val())) {
            addButton.css('visibility', 'hidden');
            validation.show();
            
        } else {
            addButton.css('visibility', 'visible');
            validation.hide();
        }
    });
});