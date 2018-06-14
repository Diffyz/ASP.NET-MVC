
function CreateCommentFunc(userId, newsId) {

    var info = document.getElementById("comment").value;
    var textComment = String(userId + " " + newsId + " " + info);
    //var textComment = infoUserIdNewsId + info;
 //<text>${name}</text> 
    $.post('/Json/CreateComment', { parameters: textComment },
        function (data) {
            var commendId = data.split(' ')[0];
            var name = data.split(' ')[1];
            var countLike = data.split(' ')[2];
            data = data.substr(commendId.length + name.length+countLike.length + 3);
            document.getElementById("comment").value = "";
            var listComment = $("#CommentContaner");
            listComment.prepend(` 
<div class ="thumbnail" style="margin:0 auto 20px auto !important; max-width:500px; height:auto"> 
    <div> 
        <div>
            <span>
                <a href='/Account/UserRoom',name="login",value='${name}'>${name}</a>
                <img style="width:20px; margin-right: 10px; height:20px; " src='http://localhost:52310/Content/img/likeFull.png' />
                <span id=${name}> ${countLike}
                </span>
           </span>
        </div>
    <p class="someClass" style="float: left; word-wrap: break-word; width:400px;margin-left:15px">${data}<p> 
    </div> 
    <div class ="caption"> 
         <div style="text-align:right; margin-right: 10px;">
            <span ><a style="margin-right:350px" onclick="Answer('${name}')" href="#createNewComment">Ответить</a></span>
            <img id="${commendId}" onclick="Like(id,'${name}')" style="width:20px; margin-right: 10px; height:20px; cursor:pointer"
            src="../Content/img/like_empty.png" /><span id="${commendId} 1"> ${0}</span>
        </div>
    </div> 
</div> 
`)});
}

function Like(id, _login) {
   
    var elem = document.getElementsByTagName('*');


    if (document.getElementById(id).src == 'http://localhost:52310/Content/img/likeFull.png') {

        document.getElementById(id).src = '/Content/img/like_empty.png';
        var digit = parseInt(document.getElementById(id + " 1").innerHTML);
        for (var i in elem) {
            if (elem[i].id == _login) {
                elem[i].innerHTML = parseInt(elem[i].innerHTML)-1;
            }
        }
        document.getElementById(id + " 1").innerHTML = digit-1;
        $.post('/Json/ChangeLikeLess', { parameters: id });
    }
    else if (document.getElementById(id).src == 'http://localhost:52310/Content/img/like_empty.png') {
        document.getElementById(id).src = '/Content/img/likeFull.png';
        var digit = parseInt(document.getElementById(id + " 1").innerHTML);
        for (var i in elem) {
            if (elem[i].id == _login) {
                elem[i].innerHTML = parseInt(elem[i].innerHTML)+ 1;
            }
        }
        document.getElementById(id + " 1").innerHTML = digit + 1;
        $.post('/Json/ChangeLikeMore', { parameters: id });
    }
}

function timer() {
   
    var info = document.getElementById("GetnewsId").value;
    var userId = document.getElementById("GetuserId").value;
    $.post('/Json/GetNewNews', { parameters: info },
        function (data) {
            for (var i in data) {
                
                if (userId != data[i].userId) {
                    var listComment = $("#CommentContaner");
                    listComment.prepend(` 
<div class ="thumbnail" style="margin:0 auto 20px auto !important; max-width:500px; height:auto"> 
    <div> 
        <div>
           <span>
                <a href='/Account/UserRoom',name="login",value='${ data[i].userLogin}'>${data[i].userLogin }</a>
                <img style="width:20px; margin-right: 10px; height:20px; " src='http://localhost:52310/Content/img/likeFull.png' />
                <span id=${ data[i].userLogin }> 0
                </span>
           </span>
        </div>
    <p class="someClass" style="float: left; word-wrap: break-word; width:400px">${data[i].text}</p> 
    </div> 
    <div class ="caption"> 
         <div style="text-align:right; margin-right: 10px;">
            <span ><a style="margin-right:350px" onclick="Answer('${data[i].userLogin}')" href="#createNewComment">Ответить</a></span>
             <img id="${data[i].commentId}" onclick="Like(id,'${data[i].userLogin}')" style="width:20px; margin-right: 10px; height:20px; cursor:pointer"
             src="../Content/img/like_empty.png" /><span id="${data[i].commentId} 1"> ${0}</span> 
        </div>
    </div> 
</div> 
`)}}});
}


setTimeout(function run() {
    timer();
    setTimeout(run, 4000);
}, 4000);

function Answer(nameAuthor) {
    document.getElementById("comment").value =nameAuthor + ", ";
}


