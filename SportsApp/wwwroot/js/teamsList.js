var dataTableTeam;

$(document).ready(function () {
    loadDataTableTeam();
});

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


function Delete(url) {
  swal({
    title: "Are you sure you want to delete this Team?",
    text: "Once deleted, you will not be able to recover the Team?!",
    icon: "warning",
    buttons: {
      cancel: true,
      confirm: true,
    },
    dangerMode: true,
  }).then((willDelete) => {
    if (willDelete) {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function (data) {
          if (data.success) {
            toastr.success(data.message);
            setTimeout(() => dataTableTeam.ajax.reload(), 300);
          } else {
            toastr.error(data.message);
          }
        },
      });
    }
  });
}

function loadDataTableTeam() {
  dataTableTeam = $("#DT_loadTeam").DataTable({
    ajax: {
      url: "/Team/Get",
      type: "GET",
      datatype: "json",
    },
    columns: [
      { data: "name", name: "name", autowidth: true },
      { data: "teamLeaderName", name: "teamLeaderName", autowidth: true },
      { data: "cityName", name: "cityName", autowidth: true },
      { data: "categoryName", name: "categoryName", autowidth: true },
      {
        render: function (data, type, row) {
          return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/Team/Details/${row.id}'">
                                            <span>View Details</span>
                                            <i class="fas fa-info"></i>
                                        </button>                                        
                                    </div>
                                </td>`;
        },
        teamName: "Actions",
        autowidth: true,
      },
      {
        render: function (data, type, row) {
          return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-edit" onclick="window.location.href='/team/edit/${row.id}'">
                                            <span>Edit</span>
                                            <i class="far fa-edit"></i>
                                        </button>
                                    </div>
                                </td>`;
        },
        teamName: "Actions",
        autowidth: true,
      },
      {
        render: function (data, type, row) {
          return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/team/delete/${row.id}')">
                                            <span>Delete</span>
                                            <i class="far fa-trash-alt"></i>
                                        </button>
                                    </div>
                                </td>`;
        },
        teamName: "Actions",
        autowidth: true,
      },
    ],
    language: {
      emptyTable: "no data found",
    },
    width: "100%",
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