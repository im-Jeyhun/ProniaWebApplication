let btns = document.querySelectorAll(".get-subcategories")

btns.forEach(x => x.addEventListener("click", function (e) {
    e.preventDefault()
    var id = document.querySelector("#categoriess").value
    var url = `https://localhost:7026/admin/plant/get-current-subcategories/${id}`

    var area = document.querySelector("#subcategory-container")
    fetch(url)
        .then(response => response.text())
        .then(data => {
            console.log(area)
            console.log(data)
            area.innerHTML = data
        })
}))
