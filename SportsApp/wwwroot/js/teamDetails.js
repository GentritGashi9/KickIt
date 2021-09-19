function DeleteFromDetails(url) {
    swal({
        title: "Are you sure you want to delete this Team?",
        text: "Once deleted, you will not be able to recover the Team?!",
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
                        setTimeout(() => dataTableTeam.ajax.reload(), 100);
                        window.location.href = '//localhost:44342/Home/Index'
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function InvitePlayerFromDetails(username) {
    var username = document.getElementsByName('username')[0].value;
    if (username != null) {
        $.ajax({
            type: "Get",
            url: "/team/inviteForJoin?username=" + username,
            success: function (data) {
                if (data.success) {
                    console.log(data.message);
                    toastr.success(data.message);
                } else {
                    console.log(data.message);
                    toastr.error(data.message);
                }
            }
        });
    }
}

function changeAccess(url, string) {
    if (string == 'public') {
        swal({
            title: "Are you sure you want to make your team "+string+"?",
            text: "When your team is public other players can join without your permission!",
            icon: "warning",
            buttons: {
                cancel: true,
                confirm: true,
            }
        }).then((willChange) => {
            if (willChange) {
                $.ajax({
                    type: "POST",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            setTimeout(() => window.location.reload(), 1000);
                        } else {
                            toastr.error(data.message);
                        }
                    }
                });
            }
        });
    } else if (string == 'private') {
        swal({
            title: "Are you sure you want to make your team private?",
            text: "When your team is private other players have to ask for your permission in order to join!",
            icon: "warning",
            buttons: {
                cancel: true,
                confirm: true,
            }
        }).then((willChange) => {
            if (willChange) {
                $.ajax({
                    type: "POST",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            setTimeout(() => window.location.reload(), 900);
                        } else {
                            toastr.error(data.message);
                        }
                    }
                });
            }
        });
    }

}

function Remove(url, playerName, playerImg, playerId, teamId) {
    swal({
        title: "Are you sure you want to remove: " + playerName + " from your team?",
        text: "Once removed, the player won't be part of your team anymore!",
        html:
            '<div class="d-inline text-center">Player Username: <b>' + playerName + '</b> ' +
            '<img class="img-cirlce" src="/ProfileImg/' + playerImg + '" /></div> ',
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                dataType: "json",
                data: { playerId: playerId, teamId: teamId },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                        setTimeout(() => window.location.reload(), 1000);

                    }
                }
            });
        }
    });
}


function ApproveR(url, playerId) {
    swal({
        title: "Are you sure you want to accept this player to your team?",
        text: "Once accepted, this player will be automatically become part of your team!",
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        },
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    playerId: playerId
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function RefuseR(url, playerId) {
    swal({
        title: "Are you sure you want to refuse this player to your team?",
        text: "Once refused, this player will be automatically removed from this list!",
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
        dangerMode: true
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "DELETE",
                url: url,
                data: {
                    playerId: playerId
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                        setTimeout(() => window.location.reload(), 1500);
                    }
                }
            });
        }
    });
}
function Leave(url) {
    swal({
        title: "Are you sure you want to leave this team?",
        html:
            '<div class="d-inline text-center">Player Username: <b></b> ' +
            '<img class="img-cirlce" src="/ProfileImg/" /></div> ',
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        }
    }).then((willLeave) => {
        if (willLeave) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function AskToJoin(url) {
    swal({
        title: "Are you sure you want to ask to join this team?",
        text: "Notice your request will be send to the team leader for approval!",
        html:
            '<div class="d-inline text-center">Player Username: <b></b> ' +
            '<img class="img-cirlce" src="/ProfileImg/" /></div> ',
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        }
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function Join(url) {
    swal({
        title: "Are you sure you want to join this team?",
        text: "Notice your requests to join other teams will be deleted!",
        html:
            '<div class="d-inline text-center">Player Username: <b></b> ' +
            '<img class="img-cirlce" src="/ProfileImg/" /></div> ',
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        }
    }).then((willJoin) => {
        if (willJoin) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
