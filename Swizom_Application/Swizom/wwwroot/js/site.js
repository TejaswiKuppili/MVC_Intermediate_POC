document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var links = document.querySelectorAll(".sidebar a, .menu-item-link");

    links.forEach(link => {
        link.addEventListener("click", function () {
            document.getElementById("loader").style.display = "flex";
        });
    });

    window.addEventListener("load", function () {
        document.getElementById("loader").style.display = "none";
    });
});
