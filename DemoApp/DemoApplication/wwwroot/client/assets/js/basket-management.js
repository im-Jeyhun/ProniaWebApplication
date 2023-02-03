//let baskets = []

//import { config } from "process"

//import ca from "../../../admin/assets/plugins/moment/locale/ca"

//const BASKET_PRODUCTS_KEY = "products";
//let cardBlock = $(".cart-block")


//// #region Initial setups

//setupAddTriggers();

//setupRemoveTriggers();

//function setupAddTriggers() {

//    let addButtons = document.querySelectorAll(".add-product-to-basket-btn")
//    addButtons.forEach(b => b.addEventListener("click", this.addProduct));
//}

//function setupRemoveTriggers() {
//    let removeButtons = document.querySelectorAll(".remove-product-to-basket-btn")
//    removeButtons.forEach(b => b.addEventListener("click", this.removeProduct));
//}

// #endregion



//// #region Add and update products and show

//function addProduct(event) {
//    event.preventDefault();

//    let endpoint = $(event.target).attr("href");
//    console.log(endpoint)

//    $.ajax({
//        url: endpoint,
//        success: function (response) {
//            console.log(response)
//            cardBlock.html(response);
//        }
//    });
//}




//// #endregion


//// #region Remove product and show


//var deleteBtns = document.querySelectorAll(".remove-product-to-basket-btn");

//$(document).on('click', ".remove-product-to-basket-btn", function (e) {
//    e.preventDefault();
//    var url = e.target.href;

//    console.log(url)
//    $.ajax({
//        url: url,
//        success: function (response) {
//            console.log(response)
//            cardBlock.html(response);
//        }
//    });
//})


//// #endregion


let btns = document.querySelectorAll(".add-product-to-basket-btn")

btns.forEach(x => x.addEventListener("click", function (e) {
    e.preventDefault()

    var url = e.target.parentElement.href
    console.log(e.target.parentElement)
    $.ajax({
        type: "POST",
        url: url,
        success: function (response) {
            console.log(response)
            $('.cart-block').html(response);

        },
        error: function (err) {
            $(".product-details-modal").html(err.responseText);
        }
    });
    //fetch(url)
    //    .then(response => response.text())
    //    .then(data => {
    //        $('.cart-block').html(data);
    //    })
}))

$(document).on("click", '.add-product-to-basket-data', function (e) {
    e.preventDefault();
    let aHref = e.target.href;
    console.log(e.target)
    let color = e.target.parentElement.parentElement.previousElementSibling.previousElementSibling.previousElementSibling.children[1]
    let ColorId = color.value;

    let size = e.target.parentElement.parentElement.previousElementSibling.previousElementSibling.children[1]
    let SizeId = size.value;

    let quantity = e.target.parentElement.previousElementSibling.children[0].children[0]

    let Quantity = quantity.value
    console.log(ColorId)
    console.log(SizeId)
    console.log(Quantity)

    console.log(Quantity)
    $.ajax(
        {
            type: "POST",
            url: aHref,

            data: {
                ColorId: ColorId,
                SizeId: SizeId,
                Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.cart-block').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})


$(document).on("click", ".remove-product-to-basket-btn", function (e) {
    e.preventDefault();
    console.log(e.target.parentElement.href)
    fetch(e.target.parentElement.href)
        .then(response => response.text())
        .then(data => {
            $('.cart-block').html(data);
        })

})

//home page javasictript end

$(document).on("click", ".product-counter", function (e) {
    e.preventDefault()
    console.log(e.target.nextElementSibling)
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.cart-page').html(data);

            fetch(e.target.previousElementSibling.href)
                .then(response => response.text())
                .then(data => {
                    console.log(data)
                    $('.cart-block').html(data);
                })

        })

})

$(document).on("click", ".product-decounter", function (e) {
    e.preventDefault();
    console.log(e.target.parentElement.href)
    console.log(e.target.nextElementSibling.nextElementSibling.href)
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.cart-page').html(data);

            fetch(e.target.nextElementSibling.nextElementSibling.href)
                .then(response => response.text())
                .then(data => {
                    console.log(data)
                    $('.cart-block').html(data);
                })
        })

})
// invidual delete add ended

$(document).on("click", ".product-remove-bulke", function (e) {
    e.preventDefault();
    console.log(e.target.parentElement.href)
    fetch(e.target.parentElement.href)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.cart-page').html(data);

            fetch(e.target.parentElement.nextElementSibling.href)
                .then(response => response.text())
                .then(data => {
                    console.log(data)
                    $('.cart-block').html(data);
                })
        })

})


//color change are 
$(document).on("change", '.change-product-color', function (e) {
    e.preventDefault();
    console.log(e.target)
    let fUrl = e.target.previousElementSibling;

    console.log(fUrl)
    console.log(fUrl)
    let color = e.target
    let ColorId = color.value;
   

    let Quantity = parseInt("0")

    console.log(ColorId)
    console.log(Quantity)

   
    $.ajax(
        {
            type: "GET",
            url: fUrl,

            data: {
                ColorId: ColorId,
                //SizeId: SizeId,
                Quantity: Quantity
            },

            success: function (response) {
                $('.cart-page').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

$(document).on("change", '.change-product-size', function (e) {
    e.preventDefault();
    console.log(e.target)
    let fUrl = e.target.previousElementSibling;

    console.log(fUrl)
    let size = e.target
    let SizeId = size.value;


    let Quantity = parseInt("0")

    //let Quantity = quantity.innerHTML

    console.log(SizeId)


    $.ajax(
        {
            type: "GET",
            url: fUrl,

            data: {
                //ColorId: ColorId
                SizeId: SizeId,
                Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.cart-page').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

//category area

$(document).on("click", '.get-categories', function (e) {
    e.preventDefault();
    let aHref = e.target.parentElement.href;
    let category = e.target.parentElement.previousElementSibling
    let CategoryId = category.value;
    console.log(CategoryId)


  
    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                CategoryId: CategoryId
            },

            success: function (response) {
                console.log(response)
                $('.filtered-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

//color are
$(document).on("click", '.get-colors', function (e) {
    e.preventDefault();
    let aHref = e.target.parentElement.href;
   
    let color = e.target.parentElement.previousElementSibling
    let ColorId = color.value


    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                //CategoryId: CategoryId,
                ColorId: ColorId
                //Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.filtered-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

//tag area
$(document).on("click", '.get-tags', function (e) {
    e.preventDefault();

    let aHref = e.target.href;

    console.log(e.target)
    let tag = e.target.previousElementSibling
    let TagId = tag.value
    console.log(aHref)
    console.log(tag)


    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                //CategoryId: CategoryId,
                TagId: TagId
                //Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.filtered-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

//search are 
$(document).on("change", '.search-by', function (e) {
    e.preventDefault();

    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                //CategoryId: CategoryId,
                TagId: TagId
                //Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.filtered-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

//price

$(document).on("change", '.get-price', function (e) {
    e.preventDefault();
    let minPrice = e.target.previousElementSibling.children[0].children[3].innerText.slice(1);
    let MinPrice = parseInt(minPrice);
    let maxPrice = e.target.previousElementSibling.children[0].children[4].innerText.slice(1);
    let MaxPrice = parseInt(maxPrice);
    let aHref = document.querySelector(".productBySearchQuery").href;
    console.log(MinPrice)
    console.log(MaxPrice)
    console.log(aHref)
    $.ajax(
        {
            url: aHref,

            data: {
                MinPrice: MinPrice,
                MaxPrice: MaxPrice
            },

            success: function (response) {
                $('.filtered-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);
            }

        });

})


//blog are

//tag area
$(document).on("click", '.get-blog-tags', function (e) {
    e.preventDefault();

    let aHref = e.target.href;

    console.log(e.target)
    let tag = e.target.previousElementSibling
    let TagId = tag.value
    console.log(aHref)
    console.log(tag)


    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                //CategoryId: CategoryId,
                TagId: TagId
                //Quantity: Quantity
            },

            success: function (response) {
                console.log(response)
                $('.blog-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})

$(document).on("click", '.get-blog-categories', function (e) {
    e.preventDefault();
    let aHref = e.target.parentElement.href;
    let category = e.target.parentElement.previousElementSibling
    let CategoryId = category.value;
    console.log(CategoryId)



    $.ajax(
        {
            type: "GET",
            url: aHref,

            data: {
                CategoryId: CategoryId
            },

            success: function (response) {
                console.log(response)
                $('.blog-area').html(response);

            },
            error: function (err) {
                $(".product-details-modal").html(err.responseText);

            }

        });

})
