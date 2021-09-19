function CreateTeam() {
    var categorytext = $("#categoryList").next().children().children().text();
    var categoryId = $(`option:contains("${categorytext}")`).val()
    console.log(categoryId);
    var city = $("#cityList").next().children().children().text();
    var teamName = $("#teamName").val()

    $.ajax({
        type: "POST",
        url: "/Team/Create",
        dataType: "json",
        data: { teamName: teamName, city: city, categoryId: categoryId },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                setTimeout(() => window.location.href = '/Team/Details/'+data.teamId, 500);
            } else {
                toastr.error(data.message);
            }
        }
    });    
}