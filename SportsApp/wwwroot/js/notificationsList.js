$(document).ready(function () {
    FilterNotifications();
});

function FilterNotifications() {
    $.ajax({
        type: "GET",
        url: "/Notifications/GetNotifications",
        dataType: "json",
        success: function (result) {
            $("#notifSection").empty();
            if (result.notificationNR.length == 0) {
                $("#notificationsNumber").hide().text("0");
            }
            if (result.notificationNR.length == 0 && result.notificationR.length == 0) {
                $("#notifSection").append(`
                    <p style="margin:0 !important" class="dropdown-item waves-effect waves-light">You have no notifications</p>
                `);
            } else {
                $("#notificationsNumber").show().text(result.notificationNR.length)
                for (var i = 0; i < result.notificationNR.length; i++) {
                    var notid = "notifNotRead" + i.toString();
                    $("#notifSection").append(`
                        <div id="${notid}" style="background-color: #ffc107 !important" class="dropdown-item waves-effect waves-light text-center divNotifications">
                            <a style="color: black !important" class="aNotification" href="/Team/JoinNotificationIsRead/?id=${result.notificationNR[i].teamId}&notificationId=${result.notificationNR[i].id}">${result.notificationNR[i].name}</a>
                        </div>
                    `);
                    if (result.notificationNR[i].type === "TeamInvite") {
                        var nr = "#notifNotRead" + i.toString();
                        $(nr).append(`
                            <button class="notificationButtonAccept" onclick="AcceptJoinInvitation('/team/JoinInvitation/${result.notificationNR[i].teamId}', '${result.notificationNR[i].teamId}', '${result.notificationNR[i].id}')">Join <i class="fas fa-paper-plane"></i></button>
                            <button class="notificationButtonRefuse" onclick="RefuseJoinInvitation('/notifications/RefuseInvitation/${result.notificationNR[i].id}')">Refuse  <i class="fas fa-times-circle"></i></button>
                        `);
                    }
                    if (result.notificationNR[i].type === "MatchInvite") {
                        var nr = "#notifNotRead" + i.toString();
                        $(nr).append(`
                            <button class="notificationButtonUsless" onclick="RemoveUslessNotification('/notifications/RemoveUsless/${result.notificationNR[i].id}')"><i class="fas fa-times-circle"></i></button>
                        `);
                    }

                }
                for (var i = 0; i < result.notificationR.length; i++) {
                    var rid = "notifRead" + i.toString();
                    $("#notifSection").append(`
                           <div id="${rid}" class="dropdown-item waves-effect waves-light text-center divNotifications">
                                <a class="aNotification" href="/Team/JoinNotificationAlreadyRead/?id=${result.notificationR[i].teamId}&notificationId=${result.notificationR[i].id}">${result.notificationR[i].name}</a>
                                
                           </div>
                    `);
                    if (result.notificationR[i].type === "TeamInvite") {
                        var nr = "#notifRead" + i.toString();
                        $(nr).append(`
                             <button class="notificationButtonAccept" onclick="AcceptJoinInvitation('/team/JoinInvitation/${result.notificationR[i].teamId}', '${result.notificationR[i].teamId}', '${result.notificationR[i].id}')">Join <i class="fas fa-paper-plane"></i></button>
                             <button class="notificationButtonRefuse" onclick="RefuseJoinInvitation('/notifications/RefuseInvitation/${result.notificationR[i].id}')">Refuse  <i class="fas fa-times-circle"></i></button>
                        `)
                    }
                    if (result.notificationR[i].type === "MatchInvite") {
                        var nr = "#notifRead" + i.toString();
                        $(nr).append(`
                            <button class="notificationButtonUsless" onclick="RemoveUslessNotification('/notifications/RemoveUsless/${result.notificationR[i].id}')"><i class="fas fa-times-circle"></i></button>
                        `);
                    }
                }
            }
        }
    });
}