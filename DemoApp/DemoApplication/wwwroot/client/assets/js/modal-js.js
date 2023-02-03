//let btns = document.querySelectorAll("#fillmodal")

//btns.forEach(x => x.addEventListener("click", function (e) {
//    e.preventDefault()
//    var url = e.target.href

//    console.log("salam")


//    var area = document.querySelector(".plant-details-moda")
//    //fetch(url)
//    //    .then(response => response.text())
//    //    .then(data => {
//    //        console.log(area)
//    //        console.log(data)
//    //    //    area.innerHTML = data
//    //    })
//}))

$(document).on("click", ".fillmodal", function (e) {
    e.preventDefault();

    console.log(e.target.parentElement)
    var url = e.target.parentElement.href;

    console.log(url)

    fetch(url)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.plant-details-modal').html(data);
        })

    $("#quickModal").modal("show");
})