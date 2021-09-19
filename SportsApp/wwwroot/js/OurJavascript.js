function UnhideP() {
    var pwinput = document.getElementById("pwinput");
    var ilabel = document.getElementById("UnhP");
    var iHidlabel = document.getElementById("UnhP1");

    if (pwinput.type === "password") {
        pwinput.type = "text";
        ilabel.style.display = "none";
        iHidlabel.style.display = "block"
    } else {
        pwinput.type = "password";
        ilabel.style.display = "block";
        iHidlabel.style.display = "none"
    }
}
function UnhideCP() {
    var cpwinput = document.getElementById("cpwinput");
    var iclabel = document.getElementById("UnhCP");
    var icHidlabel = document.getElementById("UnhCP1");

    if (cpwinput.type === "password") {
        cpwinput.type = "text";
        iclabel.style.display = "none";
        icHidlabel.style.display = "block"
    } else {
        cpwinput.type = "password";
        iclabel.style.display = "block";
        icHidlabel.style.display = "none"
    }
}

