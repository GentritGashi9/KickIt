var dataTableSport;
var dataTableSportNA;

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this Sport Field?",
        text: "Once deleted, you will not be able to recover the Sport Field?!",
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
                        setTimeout(() => dataTableSport.ajax.reload(), 300);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function loadDataTableFieldAdmin() {
    dataTableSport = $('#DT_loadSport').DataTable({
        "ajax": {
            "url": "/SportField/Get",
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
                "data": "categoryName",
                "name": "categoryName",
                "autowidth": true
            },
            {
                "data": "fieldOwner",
                "name": "fieldOwner",
                "autowidth": true
            },

            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/sportfield/details/${row.sportFieldId}'">
                                            <span>View Details</span>
                                            <i class="fas fa-info"></i>
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
                                        <button type="button" class="btn btn-success" onclick="window.location.href='/sportfield/edit/${row.sportFieldId}'">
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
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/Sportfield/Delete/${row.sportFieldId}')">
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

function loadDataTableField() {
    dataTableSportNA = $('#DT_loadSportNoAction').DataTable({
        "ajax": {
            "url": "/SportField/Get",
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
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/sportfield/details/${row.sportFieldId}'">
                                            <span>View Details</span>
                                            <i class="fas fa-user"></i>
                                        </button>
                                        
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            },
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function loadDataTableFieldClientWithout(id) {
    dataTableSportNA = $('#DT_loadSportNoAction').DataTable({
        "ajax": {
            "url": "/SportField/GetSpecificFieldsWithout/" + id,
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
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/sportfield/details/${row.sportFieldId}'">
                                            <span>View Details</span>
                                            <i class="fas fa-user"></i>
                                        </button>
                                        
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            },
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function loadDataTableFieldClient(id) {
    dataTableSport = $('#DT_loadSport').DataTable({
        "ajax": {
            "url": "/SportField/GetSpecificFields/" + id,
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
                "data": "categoryName",
                "name": "categoryName",
                "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="d-inline">
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/sportfield/details/${row.sportFieldId}'">
                                            <span>View Details</span>
                                            <i class="fas fa-user"></i>
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
                                        <button type="button" class="btn btn-success" onclick="window.location.href='/sportfield/edit/${row.sportFieldId}'">
                                            <span>Edit</span>

                                            <i class="fas fa-user-edit"></i>
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
                                        <button type="button" rel="tooltip" class="btn btn-danger " onclick="Delete('/Sportfield/Delete/${row.sportFieldId}')">
                                            <span>Delete</span>
                                            <i class="fas fa-user-times"></i>
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

function loadDataTableFieldClientPending(id) {
    dataTableSport = $('#DT_loadSportPending').DataTable({
        "ajax": {
            "url": "/SportField/GetSpecificFieldsPending/" + id,
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
                                        <button type="button" class="btn btn-primary" onclick="window.location.href='/sportfield/details/${row.sportFieldId}'">
                                            <span>View Details</span>
                                            <i class="fas fa-user"></i>
                                        </button>
                                        
                                    </div>
                                </td>`;
                },
                "name": "Actions",
                "autowidth": true
            },
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}