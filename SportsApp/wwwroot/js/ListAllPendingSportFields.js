var dataTablePF;

$(document).ready(function () {
    loadDataTablePF();
});

function Refuse(url) {
    swal({
        title: "Are you sure you don't want to accept this Field?",
        text: "Once refused, the sport field will be permanently Deleted and you wont be able to recover it!",
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
                        setTimeout(() => dataTablePF.ajax.reload(), 500);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function Accept(url) {
    swal({
        title: "Are you sure you want to accept this Field?",
        text: "Once accepted, the sport field will be automatically added and visible for everyone, you can decide to edit or delete it in the AllFields Section!",
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
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => dataTablePF.ajax.reload(), 500);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function loadDataTablePF() {
    dataTablePF = $('#DT_loadPendingSportFields').DataTable({
        "ajax": {
            "url": "/Admin/GetAllPendingFields",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [{
                "data": "name",
                "name": "Name",
                "autowidth": true
            },
            {
                "data": "address",
                "name": "Address",
                "autowidth": true
            },
            {
                "data": "contactNr",
                "name": "ContactNr",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <a type="button" class="btn btn-warning" style="border-radius:50%" href="/Admin/FieldDetails/${row.sportFieldId}">
                                            <i class="fas fa-info-circle"></i>
                                        </a>
                                        
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-success" style="border-radius:50% !important" onclick="Accept('/Admin/AcceptField/${row.sportFieldId}')">
                                            <i class="fas fa-check"></i>
                                        </button>
                                        
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button"  class="btn btn-danger" style="border-radius:50% !important" onclick="Refuse('/Admin/DenyField/${row.sportFieldId}')">
                                            <i class="fas fa-trash-alt"></i>
                                        </button>
                                       
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}