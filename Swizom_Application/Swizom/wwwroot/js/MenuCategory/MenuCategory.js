function filterMenuCategories() {
    var input = document.getElementById("searchInput").value.toLowerCase();
    var cards = document.getElementsByClassName("menu-category-card");
    var noResultsMessage = document.getElementById("noResultsMessage");
    var paginationContainer = document.querySelector(".pagination");
    var visibleCount = 0;

    Array.from(cards).forEach(card => {
        var title = card.querySelector(".card-title").innerText.toLowerCase();
        var restaurant = card.querySelector(".text-muted strong").innerText.toLowerCase();

        if (title.includes(input) || restaurant.includes(input)) {
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
