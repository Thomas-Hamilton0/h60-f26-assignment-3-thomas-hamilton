document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".product-card").forEach(function (card) {
        card.addEventListener("mouseenter", function () {
            card.classList.add("is-hovered");
        });

        card.addEventListener("mouseleave", function () {
            card.classList.remove("is-hovered");
        });
    });
});
