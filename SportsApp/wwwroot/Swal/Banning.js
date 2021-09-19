// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function UnBan(url, un) {
    swal({
        title: "Are you sure you want to Unban this User: " + un + "?",
        text: "Once Unbanned, the user: ' " + un + " ' will be able to Log-in again!",
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function Ban(url,un) {
    
    swal({
        title: "Are you sure you want to Ban this User: "+un+" ?",
        text: "Once Banned, the user: ' "+un+" ' won't be able to Log-in!",
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        },
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}