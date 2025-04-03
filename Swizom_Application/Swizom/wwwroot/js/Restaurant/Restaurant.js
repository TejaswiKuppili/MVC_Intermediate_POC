function filterRestaurants() {
    var input, filter, cards, cardContainer, title, address, i, visibleCount = 0;
    input = document.getElementById("searchInput");
    filter = input.value.toLowerCase();
    cardContainer = document.getElementById("restaurantList");
    cards = cardContainer.getElementsByClassName("restaurant-card");
    var noResultsMessage = document.getElementById("noResultsMessage");
    var paginationContainer = document.getElementById("paginationContainer");

    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".restaurant-name").innerText.toLowerCase();
        address = cards[i].querySelector(".restaurant-address").innerText.toLowerCase();

        if (title.includes(filter) || address.includes(filter)) {
            cards[i].style.display = "";
            visibleCount++;
        } else {
            cards[i].style.display = "none";
        }
    }

    // Show "No results" message if no restaurants are visible
    noResultsMessage.style.display = visibleCount === 0 ? "block" : "none";

    // Hide pagination if no results are found
    paginationContainer.style.display = visibleCount === 0 ? "none" : "block";
}
