var newsId = -1;
var ratingId = -1;
function GetRating() {
    $.ajax({
        type: "POST",
        url: "/Json/GetRating",
        data: param = "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc1,
        error: errorFunc
    });
}
function errorFunc(){
    alert("Error ajax request rating");
}
function successFunc1(data, status) {
    var id = newsId + " result";
    document.getElementById(id).innerHTML = data;
    document.getElementById(id).style.visibility = 'visible';

    document.getElementById(newsId + " 1").onclick = false;
    document.getElementById(newsId + " 2").onclick = false;
    document.getElementById(newsId + " 3").onclick = false;
    document.getElementById(newsId + " 4").onclick = false;
    document.getElementById(newsId + " 5").onclick = false;

}

function SetRating(id) {
    var str = id
    var splits = str.split(' ');

    newsId = splits[0];
    ratingId = splits[1];

    $.post('/Json/SetRating', { parameters: str });
    document.getElementById(id).disabled = true;
    setTimeout(GetRating, 1000);
}

function changeRatingAt(value,value2) {
    var rating = parseFloat(value);
    if (isNaN(rating) || rating > 5) {
        alert("Введите число меньше 5");
        document.getElementById("ratingAt").value = value2;
    }
    else {
        $.post('/Json/ChangeRating', { param: rating })
    }
}
