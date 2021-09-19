function AcceptJoinInvitation(url, teamId, notificationId) {
    swal({
        title: "Do you want to join this Team?",
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        }
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "GET",
                url: url,
                dataType: "json",
                data: {
                    notificationId: notificationId
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.href = '/team/details/'+teamId, 700);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function RefuseJoinInvitation(url) {
    swal({
        title: "Do you want to refuse this Invitation?",
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => FilterNotifications(), 700);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function RemoveUslessNotification(url) {
    swal({
        title: "Do you want to remove this notification?",
        text: "Notice if this notification is from a previous match or a refused match its probably usless and you should remove it!",

        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => FilterNotifications(), 700);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}