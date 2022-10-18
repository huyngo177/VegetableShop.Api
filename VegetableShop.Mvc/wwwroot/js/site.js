$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

jQueryPost = (elementId, form) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $(elementId).html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $.notify('Submit successfully', { globalPosition: 'top center', className: 'success', autoHideDelay: 2000 });
                }
                else {
                    $('#form-modal .modal-body').html(res.html);
                    $.notify('Submit failure', { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryDelete = (elementId, form, str, typeAction) => {
    if (confirm(str)) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $(elementId).html(res.html);
                        $.notify(typeAction + ' successfully', { globalPosition: 'top center', className: 'success', autoHideDelay: 2000 });
                    } else {
                        $(elementId).html(res.html);
                        $.notify(typeAction + ' failure', { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    return false;
}