var basket = new Set();

$(document).ready(function() {
    $('.bestill-btn').on("click", function() {
        let clickedButton = $(this);
        let text = clickedButton.text();
        if (clickedButton.text() === 'LEGG TIL') {
            clickedButton.text('FJERN')
            basket.add(clickedButton.data("cake-id"));
        } else {
            clickedButton.text('LEGG TIL');
            basket.delete(clickedButton.data("cake-id"));
        }
    });
});

$("#meny-form").submit(function() {
    let orderedCakes = JSON.stringify(Array.from(basket));
    alert("Submitted: " + orderedCakes);
    $("#ordered-cakes").val(orderedCakes);
})