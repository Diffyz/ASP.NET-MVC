function DeleteUser(id) {
    document.getElementById(id + " user").style.visibility = 'hidden';
    document.getElementById(id + " admit").style.visibility = 'hidden';

    $.post('/Json/DeleteUser', { parameters: id })
}

function BlockUser(id) {
    var _id = id.split(' ')[0];
    if (document.getElementById(id).value == "Заблокировать") {
        $.post('/Json/BlockUser', { parameters: _id });
        document.getElementById(_id + " admit").style.display = 'block';
        if (document.getElementById(_id + " select") != null) {
            document.getElementById(_id + " select").style.display = 'block';
        }

        document.getElementById(id).value = "Разблокировать";
    } else {
        $.post('/Json/UnBlockUser', { parameters: _id })
        document.getElementById(id).value = "Заблокировать";
        if (document.getElementById(_id + " admit").style.visibility == 'hidden') {
            document.getElementById(_id + " admit").style.visibility = 'visible';
        }
        else if (document.getElementById(_id + " select")!=null) {
            document.getElementById(_id + " select").style.visibility = 'visible';
        }
    }


}

function changeAdmit(id) {
    var admit = document.getElementById(id).value;
    str = id + " " + admit;
    $.post('/Json/ChangeAdmit', { parameters: str})
}

function NewGender(sex) {
    $.post('/Json/ChangeSex', { param: sex });
}