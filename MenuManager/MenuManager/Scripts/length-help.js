$(".length-help").on("focus", function () {
    $(".help-block").hide(); // Hide any help-block already showing
    $(this).next(".help-block").show(); // Show the one below the focused field
    setLengthHelpText($(this));
});

$(".length-help").on("input", function () {
    setLengthHelpText($(this));
});

function setLengthHelpText(field) {
    var usedChars = field.val().length;
    var maxChars = field.attr("data-val-length-max")
    var helpBlock = field.next(".help-block");
    helpBlock.toggleClass("has-error", (usedChars > maxChars));
    return helpBlock.text(usedChars + " of " + maxChars + " characters");
}