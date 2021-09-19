function FilterTeam() {
    var category = $(".ca-search").next().children(".current").text();
    var city = $(".lo-search").next().children(".current").text();

    $.ajax({
        type: "GET",
        url: "/Home/GetFiltredTeams",
        dataType: "json",
        data: {
            category: category,
            city: city
        },
        success: function (result) {
            $("#filteredTeams").empty();
            for (var i = 0; i < result.data.length; i++) {

                if (!$("#yourTeamId").val()) {
                    console.log($("#yourTeamId").val());
                    $("#filteredTeams").append(`
                        <div class= "col-lg-3 col-md-4 col-sm-6" >
                            <div class="contact-box center-version">
                                <a href="/Team/Details/${result.data[i].id}">
                                    <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                    <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>

                                    <div class="font-bold">${result.data[i].cityName}</div>
                                    <address class="m-t-md">
                                        <strong>${result.data[i].categoryName}</strong><br>
                                        ${result.data[i].teamLeaderName}<br>
                                        Number of Players: ${result.data[i].numberOfPlayers}/${result.data[i].maxPlayers}<br>
                                        ${result.data[i].isPrivate ? "Private" : "Public"}
                                    </address>
                                </a>
                                <div class="contact-box-footer">
                                    <div class = "m-t-xs btn-group" >
                                        <button type = "button"
                                        class = "btn btn-xs btn-black test"
                                        ${result.data[i].isPrivate ? "onclick = \"AskToJoin('/Team/RequestToJoin/${result.data[i].id}')\"" : "onclick = \"AskToJoinR('/Team/Join/${result.data[i].id}')\""}
                                        data-key-id = "${result.data[i].id}"> Join Team <i class = "fas fa-paper-plane" > </i> </button >
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
                } else {
                    $("#filteredTeams").append(`
                        <div class= "col-lg-3 col-md-4 col-sm-6" >
                            <div class="contact-box center-version">
                                <a href="/Team/Details/${result.data[i].id}">
                                    <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                    <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>

                                    <div class="font-bold">${result.data[i].cityName}</div>
                                    <address class="m-t-md">
                                        <strong>${result.data[i].categoryName}</strong><br>
                                        ${result.data[i].teamLeaderName}<br>
                                        Number of Players: ${result.data[i].numberOfPlayers}/${result.data[i].maxPlayers}<br>
                                        ${result.data[i].isPrivate ? "Private" : "Public"}
                                    </address>
                                </a>
                                <div class="contact-box-footer">
                                    <div class = "m-t-xs btn-group" >
                                        <button type = "button"
                                            class = "btn btn-xs btn-black test"
                                            onClick= "readData();loadMap();loadSportFields();"
                                            data-toggle= "modal"
                                            data-target= "#ModalMap"
                                            data-key-id= "${result.data[i].id}" > 
                                            Ask for Match 
                                            <i class="fas fa-paper-plane"></i> 
                                        </button >
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
                }
            }
        }
    });
    window.location.href = "#divTeams";
}

function AskToJoin(url) {
    swal({
        title: "Are you sure you want to ask to join this team?",
        text: "Notice your request will be send to the team leader for approval if the team is private!",
        html: '<div class="d-inline text-center">Player Username: <b></b> ' +
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
function AskToJoinR(url) {
  swal({
    title: "Are you sure you want to join this team?",
    text: "Notice your request will be send to the team leader for approval!",
    html:
      '<div class="d-inline text-center">Player Username: <b></b> ' +
      '<img class="img-cirlce" src="/ProfileImg/" /></div> ',
    icon: "success",
    buttons: {
      cancel: true,
      confirm: true,
    },
  }).then((willDelete) => {
    if (willDelete) {
      $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
          if (data.success) {
            toastr.success(data.message);
            setTimeout(() => window.location.reload(), 700);
          } else {
            toastr.error(data.message);
          }
        },
      });
    }
  });
}

function FilterAllTeams() {
    $.ajax({
        type: "GET",
        url: "/Home/GetAllFiltredTeams",
        success: function (result) {
            $("#filteredTeams").empty();
            for (var i = 0; i < result.data.length; i++) {
                if (!$("#yourTeamId").val()) {
                    $("#filteredTeams").append(`
                    <div class= "col-lg-3 col-md-4 col-sm-6" >
                        <div class="contact-box center-version">
                            <a href="/Team/Details/${result.data[i].id}">
                                <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>
                                <address class="m-t-md">
                                    <strong>${result.data[i].categoryName}</strong><br>
                                    Team Leader: ${result.data[i].teamLeaderName}<br>
                                    Number of Players: ${result.data[i].numberOfPlayers}/${result.data[i].maxPlayers}<br>
                                    ${result.data[i].isPrivate ? "Private" : "Public"}
                                </address>
                                <div class="font-bold">${result.data[i].cityName}</div>
                            </a>
                            <div class="contact-box-footer">
                                <div class="m-t-xs btn-group">
                                    <button type = "button"
                                    class = "btn btn-xs btn-black test"
                                    onclick ="${result.data[i].isPrivate ? `AskToJoin('/Team/RequestToJoin/${result.data[i].id}')` : `AskToJoinR('/Team/Join/${result.data[i].id}')`}"
                                    data-key-id = "${result.data[i].id}">
                                    Join Team <i class="fas fa-paper-plane"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
                } else {
                    $("#filteredTeams").append(`
                    <div class= "col-lg-3 col-md-4 col-sm-6" >
                        <div class="contact-box center-version">
                            <a href="/Team/Details/${result.data[i].id}">
                                <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>
                                <address class="m-t-md">
                                    <strong>${result.data[i].categoryName}</strong><br>
                                    Team Leader: ${result.data[i].teamLeaderName}<br>
                                    Number of Players: ${result.data[i].numberOfPlayers}/${result.data[i].maxPlayers}<br>
                                    ${result.data[i].isPrivate ? "Private" : "Public"}
                                </address>
                                <div class="font-bold">${result.data[i].cityName}</div>
                            </a>
                            <div class="contact-box-footer">
                                <div class="m-t-xs btn-group">
                                    <button type = "button"
                                    class = "btn btn-xs btn-black test"
                                    onClick = "readData();loadMap();loadSportFields();"
                                    data-toggle = "modal"
                                    data-target = "#ModalMap"
                                    data-key-id = "${result.data[i].id}">
                                       Ask for Match <i class="fas fa-paper-plane"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
                }
            }
        }
    });
}
var teamMap

function loadMap() {
    document.getElementById('mapTeams').innerHTML = "<div id='map' style='width: 100%; height: 100%;'></div>";
    teamMap = L.map("map").setView([42.528796, 20.828122], 8);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        gestureHandling: true,
        zoomOffset: -1,
        accessToken: 'sk.eyJ1IjoiZGV3YXI0MDExNSIsImEiOiJja3JnM3c3OGEwZHoyMzFxbmdwbTBncTg5In0.JlLGTEanR3VxCPkOCnnbKA'
    }).addTo(teamMap);
    setTimeout(() => {
        teamMap.invalidateSize();
    }, 170);
}

function requestMatch(url) {
    var team1id = $("#yourTeamId").val();
    var team2id = $("#team2").val();
    var sportfieldId = $("#sportfieldId").val();
    var startTime = $("#startTime").val();
    var endTime = $("#endTime").val();

    if (!team1id || !team2id || !startTime || !endTime || !sportfieldId) {
        document.getElementById('error').innerHTML = 'Please Select the sport field and the schedule!';
    } else {
        swal({
            title: "Are you sure you want to request to play to this team?",
            text: "Once accepted, the request will be automatically send to the chosen adversary team, you need to wait for adversary team response!",
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
                        team1Id: team1id,
                        team2Id: team2id,
                        startTime: startTime,
                        endTime: endTime,
                        sportFieldId: sportfieldId
                    },
                    success: function (data) {

                        if (data.success) {
                            toastr.success(data.message);
                            setTimeout(() => location.reload(), 1000);
                        } else {
                            toastr.error(data.message);
                        }

                    }
                });
            }
        });
    }
}

function readData() {
    $('body').one('click', '.test', function () {
        var id = $(this).attr('data-key-id');
        $('#team2').val(id);
    });
}