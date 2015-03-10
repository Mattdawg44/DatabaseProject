$(document).ready(function () {
    showIngredientsForMenu(true);
});
$("#Menu_ID").change(showIngredientsForMenu);

function showIngredientsForMenu(preventDeselect) {
    var didOptionGroupChange = false;
    var menuText = $("#Menu_ID option:selected").text();
    $("#Ingredients, #hidden-optgroups").children().each(function () {
        if (menuText.indexOf($(this).attr("label")) === 0) {
            if ($(this).showOptionGroup())
                didOptionGroupChange = true;
        }
        else {
            if ($(this).hideOptionGroup())
                didOptionGroupChange = true;
        }
    });
    $("#Ingredients").multiselect("rebuild");
    if (didOptionGroupChange && !preventDeselect)
        $("#Ingredients").multiselect("deselectAll", false).multiselect("refresh");
}

$.fn.hideOptionGroup = function () {
    if ($(this).css("display") === "none")
        return false;
    //alert("Hiding " + $(this).attr("label"));
    $(this).hide();
    $(this).children().attr("disabled", "disabled").removeAttr("selected");
    $(this).appendTo($("#hidden-optgroups"));
    return true;
}

$.fn.showOptionGroup = function () {
    //alert($(this).css("display"));
    if ($(this).css("display") !== "none")
        return false;
    //alert("Showing " + $(this).attr("label"));
    $(this).show();
    $(this).children().removeAttr("disabled");
    $(this).prependTo($("#Ingredients"));
    return true;
}