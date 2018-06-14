function changeSearchNews() {
    var item = document.getElementById("searchNews").value; 
    $.post('/Json/SetSearchNews', { param: item });
}