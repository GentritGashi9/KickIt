// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/Admin/GetAllUsers",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userName", "name": "UserName", "autowidth": true },
            { "data": "email", "name": "Email", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "surname", "name": "Surname", "autowidth": true },
            {
                "data": "dateOfBirth", "name": "DateOfBirth",
                "render": function (data) {
                    var Birthdate = data.split("T")[0];
                    var SplittedBd = Birthdate.split("-");
                    var month = SplittedBd[1];
                    var day = SplittedBd[2];
                    var year = SplittedBd[0];
                    return `<td>
                                    ${day}/${month}/${year}
                                </td>`;
                },
                "autowidth": true
            },
            {
                "data": "role", "name": "Role",
                "render": function (data, type, row) {
                    return `<td">
                                    ${data}
                                </td>`;
                }, "autowidth": true
            },
            {
                "data": "profileImg", "name": "ProfileImg",
                "render": function (data, type, row) {
                    return `<td>
                                <img src="/ProfileImg/${data}" class="miniProfileImgs" />
                            </td>`;
                }, "autowidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<td class="d-block">
                                    <div class="nav-item dropdown" style="margin-bottom:10px">
                                        <a class="nav-link dropdown-toggle" style="background-color:#fd7603;border-radius:10px"
                                           href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true"
                                           aria-expanded="false">
                                            Manage Roles
                                        </a>
                                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between">                                                
                                                    <div style="margin:auto 0">Add Role :</div>
                                                    <button type="button" class="btn btn-info adminButtons" onClick="AddR('/Admin/AddR/${row.id}', '${row.userName}','${row.role}')">
                                                    <i class="fas fa-plus-circle"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between">                                                
                                                    <div style="margin:auto 0">Edit Role :</div> 
                                                    <button type="button" class="btn btn-success adminButtons" onClick="EditR('/Admin/EditR/${row.id}', '${row.userName}','${row.role}')">
                                                    <i class="fas fa-edit"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between">                                                
                                                    <div style="margin:auto 0">Remove Role:</div>
                                                    <button style="margin-left:10px" type="button" class="btn btn-danger adminButtons" onClick="DeleteR('/Admin/DeleteR/${row.id}', '${row.userName}','${row.role}')">
                                                    <i class="fas fa-times-circle"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="nav-item dropdown">
                                        <a class="nav-link dropdown-toggle" style="background-color:#fd7603;border-radius:10px"
                                           href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true"
                                           aria-expanded="false">
                                            Menage User
                                        </a>
                                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between ">
                                                    <div style="margin:auto 0">Edit User :</div>
                                                    <a type="button" class="btn btn-success adminButtonsR" href="/Admin/EditUser/${row.id}">
                                                        <i class="fas fa-user-edit"></i>
                                                    </a>
                                                </div>
                                            </div>
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between ">                                                
                                                    <div style="margin:auto 0">Delete User:</div> 
                                                    <button style="margin-left:10px" type="button" rel="tooltip" class="btn btn-danger adminButtonsR" onclick="Delete('/Admin/Delete/${row.id}')">
                                                        <i class="fas fa-user-times"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="dropdown-item">
                                                <div class="d-flex justify-content-between ">                                                
                                                    <div style="margin:auto 0">Ban User :</div>
                                                    <a type="button" rel="tooltip" class="btn btn-danger adminButtonsR" href="/Admin/BanSection/${row.id}">
                                                        <i class="fas fa-user-slash"></i>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>`;
                }, "name": "Actions", "autowidth": true
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this User?",
        text: "Once deleted, you will not be able to recover the user!",
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

                        setTimeout(() => dataTable.ajax.reload(), 1000);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function DeleteR(url,UserName,UserRole) {
        swal({
            title: "Are you sure you want to remove ' " + UserName + " ' from the role: ' " + UserRole + " ' ?",
            text: "Once removed, " + UserName + "'s role will be set up automatically to ' Player ' !",
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
                    data: {
                        role: UserRole
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);

                            setTimeout(() => dataTable.ajax.reload(),500);
                        }
                        else {
                            toastr.error(data.message);
                        }
                    }
                });
            }
        });
    
}

function AddR(url, username,userRole) {
    console.log(username);
    swal({
        title: "Here you can edit the role of " + username + "!",
        text: "Notice " + username + "'s current role is: ' " + userRole + " ' you can change it to be eighter ' Admin ' or ' Client ' ",
        content: "input",
        inputAttributes: {
            autocapitalize: "off"
        },
        icon: "success",
        dangerMode: true,
        buttons: true
    }).then((dataUpdate) => {
        $.ajax({
            type: "POST",
            url: url,
            data: {
                ecom: dataUpdate
            },
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    setTimeout(() => dataTable.ajax.reload(),500);
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}

function EditR(url,un,UserRole) {
        swal({
            title: "Here you can edit " + un + "role!",
            text: "Notice " + un + " is currently a/an ' " + UserRole + " ' you can change it to eighter ' Admin ' or ' Client '",
            content: "input",
            inputAttributes: {
                autocapitalize: "off"
            },
            icon: "success",
            dangerMode: true,
            buttons: true,
        }).then((dataUpdate) => {
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    ecom: dataUpdate,
                    role: UserRole
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() =>dataTable.ajax.reload(),500);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        });
}
function UnBan(url, un, id) {
    $("body").one("click", ".adminButtonsR", function (e) {
        var ur = $(this);
        console.log(ur)
        var ur1 = $(this).first().removeClass("fa-user-check").addClass("fa-user-slash");
        console.log(ur1);
        var ur2 = $(this).setAttribute('onclick', 'Ban(' + url + ',' + un + ',' + id + '})');
        console.log(ur2);
    });
    swal({
        title: "Are you sure you want to Unban this User: " + un + "?",
        text: "Once unbanned, the user: " + un + " will be able to Log-in again!",
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
function Ban(url, un) {
    $("body").one("click", ".adminButtonsR", function (e) {
        var ur = $(this);
        console.log(ur)
        var ur1 = $(this).first().removeClass("fa-user-slash").addClass("fa-user-check");
        console.log(ur1);
        var ur2 = $(this).setAttribute('onclick', 'UnBan(' + url + ',' + un + ',' + id + '})');
        console.log(ur2);
    });
    swal({
        title: "Are you sure you want to Ban this User: "+un+"?",
        text: "Once banned, the user: "+un+" won't be able to Log-in!",
        icon: "success",
        buttons: {
            cancel: true,
            confirm: true,
        },
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function Edit(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            
        }
    });
}
