function filterOrders() {
    var input = document.getElementById("searchInput").value.toLowerCase();
    var cards = document.getElementsByClassName("order-card");
    var noResultsMessage = document.getElementById("noResultsMessage");
    var paginationContainer = document.querySelector(".pagination");
    var visibleCount = 0;

    Array.from(cards).forEach(card => {
        var customerName = card.querySelector(".card-title").innerText.toLowerCase();
        var customerPhone = card.querySelector("p strong").innerText.toLowerCase();
        var address = card.querySelector(".text-muted").innerText.toLowerCase();
        var status = card.querySelector(".badge").innerText.toLowerCase();

        if (
            customerName.includes(input) ||
            customerPhone.includes(input) ||
            address.includes(input) ||
            status.includes(input)
        ) {
            card.style.display = "";
            visibleCount++;
        } else {
            card.style.display = "none";
        }
    });

    noResultsMessage.style.display = visibleCount === 0 ? "block" : "none";
    if (paginationContainer) {
        paginationContainer.style.display = visibleCount === 0 ? "none" : "flex";
    }
}
