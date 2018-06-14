function DeletePicture(newsid) {
    $.post('/Json/DeletePicture/', { newsId: newsid });
    document.getElementById("image").remove();
    document.getElementById("pushImage").style.display = "block";
}
function deleteNews(id) {
    var idNews = id.replace("delete", "news");
    document.getElementById(idNews).remove();
    $.post('/Json/DeleteNew', { parameters: id });
}

function deleteNewsIndex(id) {
    id = id + " fullNews";
    document.getElementById(id).remove();
    $.post('/Json/DeleteNew', { parameters: id });
}

function NewAuthor(id) {
    $.post('/Json/ChangeSession', { param: id });
}