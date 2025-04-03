function filterMenuItems() {
    var input, filter, cards, cardContainer, title, category, restaurant, i, visibleCount = 0;
    input = document.getElementById("searchInput");
    filter = input.value.toLowerCase();
    cards = document.getElementsByClassName("menu-item-card");
    var noResultsMessage = document.getElementById("noResultsMessage");
    var paginationContainer = document.querySelector(".pagination");

    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".card-title").innerText.toLowerCase();
        category = cards[i].querySelector(".text-muted").innerText.toLowerCase();
        restaurant = cards[i].querySelector(".text-muted strong").innerText.toLowerCase();

        if (title.includes(filter) || category.includes(filter) || restaurant.includes(filter)) {
            cards[i].style.display = "";
            visibleCount++;
        } else {
            cards[i].style.display = "none";
        }
    }

    // Show "No results" message if no menu items are visible
    noResultsMessage.style.display = visibleCount === 0 ? "block" : "none";

    // Hide pagination if no results are found
    if (paginationContainer) {
        paginationContainer.style.display = visibleCount === 0 ? "none" : "flex";
    }
}
