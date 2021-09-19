var dataTablePending;

function Accept(url) {
    swal({
        title: "By clicking confirm you will accept this Game Request!",
        text: "Notice, if your team or the adversary team accepted another request, you will be unable to accept this Game Request!",
        icon: "warning",
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
                    console.log(data.endTime);
                    console.log(data.startTime);

                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => dataTablePending.ajax.reload(), 500);
                        setTimeout(() => window.location.href = "/Matches/AcceptForMatch/" + data.gameRoomId + "?startTime=" + data.startTime + "&endTime=" + data.endTime, 1000);
                    }else{
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function Refuse(url) {
    swal({
        title: "Are you sure you want to refuse this Game Request?",
        text: "Once refused, the Match will be cancelled!",
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
                        setTimeout(() => dataTablePending.ajax.reload(), 500);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function loadDataTablePending() {
    dataTablePending = $('#DT_loadPending').DataTable({
        "ajax": {
            "url": "/Matches/Pending",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [{
            "data": "name",
            "name": "Match Name",
            "autowidth": true
        },
        {    "render": function (data, type, row) {
                return `<td class="d-block">
                                    <div class="d-inline">
                                        <a type="button"  class="btn" style="border-radius:50% !important;background-color:#ffc107;border-color:#ffc107" href="/Matches/MatchDetails/${row.id}">
                                            <i class="fas fa-info-circle"></i>
                                        </a>
                                    </div>
                                </td>`;
            },
            "name": "Details",
            "autowidth": true
            },
            {
            "render": function (data, type, row) {
                return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-success" style="border-radius:50% !important" onclick="Accept('/Matches/AcceptForMatchC/${row.id}')">
                                            <i class="fas fa-calendar-check"></i>
                                        </button>
                                    </div>
                                </td>`;
            },
            "name": "Accept",
            "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-danger" style="border-radius:50% !important" onclick="Refuse('/Matches/RefuseForMatch/${row.id}')">
                                            <i class="fas fa-trash-alt"></i>
                                        </button>
                                    </div>
                                </td>`;
                },
                "name": "Refuse",
                "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}
function loadDataTablePM() {
    console.log("hello");
    dataTablePM = $('DT_loadPM').DataTable({
        "ajax": {
            "url": "/Matches/Teams",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [{
                "data": "name",
                "name": "MatchName",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <a type="button"  class="btn btn-success" style="border-radius:50% !important" href="/Matches/AcceptForMatch/${row.id}">
                                            <i class="fas fa-calendar-check"></i>
                                        </a>
                                        
                                    </div>
                                </td>`;
                },
                "name": "AcceptRequest",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-danger" style="border-radius:50% !important" onclick="Refuse('/Matches/RefuseForMatch/${row.id}')">
                                            <i class="fas fa-trash-alt"></i>
                                        </button>
                                       
                                    </div>
                                </td>`;
                },
                "name": "RefuseRequest",
                "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}