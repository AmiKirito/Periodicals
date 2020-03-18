var firstAuthor = true;
var firstTopic = true;

$(document).ready(function (e) {
    $("#ExistingAuthors").change(function () {
        var alltext = $('input[name=Authors]').val();
        var textval = $(":selected", this).val();

        if (textval != '') {
            if (alltext == '') {
                firstAuthor = true;
            }
            if (firstAuthor && alltext == '') {
                firstAuthor = false;
                alltext += textval;
            }
            else {
                alltext += ';' + textval;
            }
        }

        $('input[name=Authors]').val(alltext);
        $("#ExistingAuthors option:selected").remove();
    });
    $("#ExistingTopics").change(function () {
        var alltext = $('input[name=Topics]').val();
        var textval = $(":selected", this).val();

        if (textval != '') {
            if (alltext == '') {
                firstTopic = true;
            }
            if (firstTopic && alltext == '') {
                firstTopic = false;
                alltext += textval;
            }
            else {
                alltext += ';' + textval;
            }
        }

        $('input[name=Topics]').val(alltext);
        $("#ExistingTopics option:selected").remove();
    })
});