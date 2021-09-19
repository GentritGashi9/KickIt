var dataTableTeam;
var dataTablePM;




function requestMatch(url) {
    var team1id = $("#yourTeamId").val();
    var team2id = $("#team2").val();
    var sportfieldId = $("#sportfieldId").val();
    var startTime = $("#startTime").val();
    var endTime = $("#endTime").val();

    console.log(team1id);
    console.log(team2id);
    console.log(sportfieldId);
    console.log(startTime);
    console.log(endTime);
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

function loadDataTableTeam() {
    dataTableTeam = $('#DT_loadTeams').DataTable({
        "ajax": {
            "url": "/Matches/Teams",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [{
                "data": "name",
                "name": "TeamName",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-success test" style="border-radius:50% !important" onClick="readData();loadMap()" data-toggle="modal" data-target="#ModalMap" test>
                                            <i class="fas fa-paper-plane"></i>
                                        </button>
                                    </div>
                                </td>`;
                },
                "name": "Request to play",
                "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function readData() {
    $('body').one('click', '.test', function () {
        var id = $(this).attr('data-key-id');
        $('#team2').val(id);
    });
}