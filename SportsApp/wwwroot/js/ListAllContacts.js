var dataTableContacts;

$(document).ready(function () {
    loadDataTableContacts();
});

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this Contact?",
        text: "Once deleted, you will not be able to recover the Contact?!",
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
                        setTimeout(() => dataTableContacts.ajax.reload(), 300);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function loadDataTableContacts() {
    dataTableContacts = $('#DT_loadContacts').DataTable({
        "ajax": {
            "url": "/ContactUs/Get",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "title", "name": "Title", "autowidth": true },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/contactus/details/${row.id}'">
                                            <span>View Details</span>
                                            <i class="fas fa-info"></i>
                                        </button>
                                    </div>
                                </td>`;
                }, "name": "Actions", "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/contactus/delete/${row.id}')">
                                            <span>Delete</span>
                                            <i class="far fa-trash-alt"></i>
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
