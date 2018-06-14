
var newsTag = [];
function AddTag() {
    var input = document.getElementById("tag").value;
    input = input.replace(/ /g, "");
    if (input == "")
        console.log("Выражение не может быть пустым");
    else {
        for (var i in newsTag) {
            if (newsTag[i] == input) {
                alert("Уже добавлен");
                return ;
            }
        }
        newsTag.push(input);

        $.post('/Json/PushTag', { param: input });
        var listComment = $("#tagList");
        listComment.append(`
        <a id="${input}" class="btn btn-success">${input} <img  onclick="deleteTag('${input}')" style="width:15px; height:15px" src='http://localhost:52310/Content/img/kreuz.png'/></a>
`);
    }
}

function deleteTag(id) {  
    $.post('/Json/RemoveTag', { param: id });
    var position = newsTag.indexOf(id);
    if (~position) newsTag.splice(position, 1);
    
    var listComment = $("#tagList");
    $("#"+id).remove();
}

var availableTags = []

$(function () {
    $.ajax({
        url: "/Json/getTagForSearch",
        
        success: function (data) {
            var i = 0;
            while (data[i] != null) {
                availableTags.push(data[i]);
                i++;
            }
        },
        dataType: 'json',
    });
    $("#tag").autocomplete({
        source: availableTags
    });
});

let searchTagList = []
function AddSearchTag(id) {
    
    document.getElementById(id).parentNode.removeChild(document.getElementById(id));
    $.post('/Json/PushTagSearch', { param: id });

    searchTagList[searchTagList.length] = id;
    document.getElementById("_search").style.display = "block";

    var listComment = $("#searchTags");
    listComment.append(`
        <a id="${id}" class="btn btn-success" style="margin-bottom:5px">${id} <img onclick="DeleteTagSearch('${id}')"  style="width:15px; height:15px" src='http://localhost:52310/Content/img/kreuz.png'/></a>
`);
}

function DeleteTagSearch(id) {
    document.getElementById(id).parentNode.removeChild(document.getElementById(id));
    $.post('/Json/RemoveTagSearch', { param: id });

    searchTagList.splice(searchTagList.indexOf(id), 1);
    if (searchTagList.length == 0) {
        document.getElementById("_search").style.display = "none";

    }

    var listComment = $("#fullTags");
    listComment.prepend(`
                <input id=${id} onclick="AddSearchTag(this.id)" style="margin-left:15px;margin-bottom:5px" type="submit" name="search" class="btn btn-success" value=${id}>
`);
}

function DeleteNewsTag(id) {
    document.getElementById(id).parentNode.removeChild(document.getElementById(id));
    $.post('/Json/RemoveTagNews', { param: id });
}

function AddTagSearch() {
    var input = document.getElementById("tagSearch").value;
    input = input.replace(/ /g, "");
    $.post('/Json/InsertNewPair', { param: input });
    var listComment = $("#tagListChangeNews");
    listComment.append(`
        <a id=${input} style="margin-left:15px;margin-bottom:8px;" class="btn btn-success">${input}<img onclick="DeleteNewsTag('${input}')" style="width:15px; height:15px" src='http://localhost:52310/Content/img/kreuz.png' /></a>
`);
}