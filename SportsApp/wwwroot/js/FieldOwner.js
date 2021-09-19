var dataTableOwner;

$(document).ready(function () {
    loadDataTableField();
});

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this Field Owner?",
        text: "Once deleted, you will not be able to recover the Field Owner?!",
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
                        //dataTable.ajax.reload();
                        setTimeout(() => dataTableOwner.ajax.reload(), 300);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function loadDataTableField() {
    dataTableOwner = $('#DT_loadOwner').DataTable({
        "ajax": {
            "url": "/FieldOwner/Get",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "buissnessName", "name": "BissnessName", "autowidth": true },
            { "data": "address", "name": "Address", "autowidth": true },
            { "data": "contactNr", "name": "ContactNr", "autowidth": true },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/fieldowner/details/${row.id}'">
                                            <span>View Details</span>
                                            <i class="fas fa-user"></i>
                                        </button>
                                        
                                    </div>
                                </td>`;
                }, "name": "Actions", "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-edit" onclick="window.location.href='/fieldowner/edit/${row.id}'">
                                            <span>Edit</span>

                                            <i class="fas fa-user-edit"></i>
                                        </button>
                                       
                                    </div>
                                </td>`;
                }, "name": "Actions", "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/fieldowner/delete/${row.id}')">
                                            <span>Delete</span>
                                            <i class="fas fa-user-times"></i>
                                        </button>
                                    </div>
                                </td>`;
                }, "name": "Actions", "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "101%"
    });
}
