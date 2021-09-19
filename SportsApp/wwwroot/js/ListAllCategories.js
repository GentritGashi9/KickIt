var dataTableCategories;

$(document).ready(function () {
    loadDataTableCategories();
});
function Delete(url) {
    swal({
        title: "Are you sure you want to delete this Sport Category?",
        text: "Once deleted, you will not be able to recover the Sport Category?!",
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
                        setTimeout(() => dataTableCategories.ajax.reload(), 300);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function loadDataTableCategories() {
    dataTableCategories = $('#DT_loadCategories').DataTable({
        "ajax": {
            "url": "/categories/Get",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [{
                "data": "name",
                "name": "Name",
                "autowidth": true
            },
            {
                "data": "minCapacity",
                "name": "Min Capacity",
                "autowidth": true
            },
            {
                "data": "maxCapacity",
                "name": "Max Capacity",
                "autowidth": true
            },

            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-edit" onclick="window.location.href='/categories/edit/${row.id}'">
                                            <span>Edit</span>
                                            <i class="far fa-edit"></i>
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
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/categories/Delete/${row.id}')">
                                            <span>Delete</span>
                                            <i class="far fa-trash-alt"></i>
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