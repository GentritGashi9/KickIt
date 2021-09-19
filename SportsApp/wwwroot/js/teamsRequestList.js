function FilterTeamRequests() {
    var category = $(".ca-search").next().children(".current").text();
    var city = $(".lo-search").next().children(".current").text();

    $.ajax({
        type: "GET",
        url: "/Team/GetFiltredTR",
        dataType: "json",
        data: { category: category, city: city },
        success: function (result) {
            $("#filteredTeams").empty();
            for (var i = 0; i < result.data.length; i++) {
                $("#filteredTeams").append(`
                    <div class= "col-lg-3 col-md-4 col-sm-6" >
                        <div class="contact-box center-version">
                            <a href="/team/details/${result.data[i].id}">
                                <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>

                                <div class="font-bold">${result.data[i].cityName}</div>
                                <address class="m-t-md">
                                    <strong>${result.data[i].categoryName}</strong><br>
                                    ${result.data[i].teamLeaderName}<br>
                                </address>
                            </a>
                            <div class="contact-box-footer">
                                <div class="m-t-xs btn-group">
                                    <button type="button"  class="btn btn-xs btn-black" style="color:red!important" onclick="UnsendRequest('/Team/UnsendRequest','${result.data[i].id}')">
                                       Cancel Request <i class="far fa-times-circle"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
            }
        }
    });
}

function FilterAllTeamRequests() {
    $.ajax({
        type: "GET",
        url: "/Team/GetAllFiltredTR",
        success: function (result) {
            $("#filteredTeams").empty();
            for (var i = 0; i < result.data.length; i++) {
                $("#filteredTeams").append(`
                    <div class= "col-lg-3 col-md-4 col-sm-6" >
                        <div class="contact-box center-version">
                            <a href="/team/details/${result.data[i].id}">
                                <img alt="image" class="img-circle" src="/img/categories/sports/${result.data[i].categoryName}.png">
                                <h3 class="m-b-xs"><strong>${result.data[i].name}</strong></h3>

                                <div class="font-bold">${result.data[i].cityName}</div>
                                <address class="m-t-md">
                                    <strong>${result.data[i].categoryName}</strong><br>
                                    ${result.data[i].teamLeaderName}<br>
                                </address>
                            </a>
                            <div class="contact-box-footer">
                                <div class="m-t-xs btn-group">
                                    <button type="button"  class="btn btn-xs btn-black" style="color:red!important" onclick="UnsendRequest('/Team/UnsendRequest','${result.data[i].id}')">
                                       Cancel Request <i class="far fa-times-circle"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
            }
        }
    });
}
function UnsendRequest(url,teamId) {
    swal({
        title: "Are you sure you want to cancel this request?",
        text: "Once cancelled, your request to join this team will be automatically removed!",
        icon: "warning",
        buttons: {
            cancel: true,
            confirm: true,
        },
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "GET",
                url: url,
                data: {
                    teamId: teamId
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        toastr.error(data.message);
                        setTimeout(() => window.location.reload(), 1000);

                    }
                }
            });
            
        }
    });
    
}