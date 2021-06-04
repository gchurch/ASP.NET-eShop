// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddProductToBasket(productId) {
    console.log("Sending request to add product to the server.");
    fetch('/Basket/Add/' + productId)
        .then(
            function (response) {
                if (response.status !== 200) {
                    console.log('Looks like there was a problem. Status Code: ' +
                        response.status);
                    return;
                }

                // Examine the text in the response
                response.json().then(function (data) {
                    console.log(data);
                });
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}


function RemoveProductFromBasket(productId) {
    console.log("Sending request to remove product to the server.");
    fetch('/Basket/Remove/' + productId)
        .then(
            function (response) {
                if (response.status !== 200) {
                    console.log('Looks like there was a problem. Status Code: ' +
                        response.status);
                    return;
                }

                // Examine the text in the response
                response.json().then(function (data) {
                    console.log(data);
                    updateProductQuantities(data);
                    if (isProductIdInProductQuantitiesList(productId, data) == false) {
                        console.log("Product " + productId + " no longer in basket");
                        var productElement = document.getElementById("basketProduct" + productId);
                        productElement.remove();
                    }
                    updateTotalCost(data);
                    updateTotalNumberOfProductsInBasket(data);
                });
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

function updateProductQuantities(productQuantitiesList) {
    console.log("Updating product quantities in basket.");
    for (var i = 0; i < productQuantitiesList.length; i++) {
        var quantityElement = document.getElementById("basketProduct" + productQuantitiesList[i].productId + "Quantity");
        quantityElement.innerHTML = productQuantitiesList[i].quantity;
    }
}

function isProductIdInProductQuantitiesList(productId, productQuantitiesList) {
    console.log(productQuantitiesList);
    for (var i = 0; i < productQuantitiesList.length; i++) {
        if (productQuantitiesList[i].productId == productId) {
            return true;
        }
    }
    return false;
}

function updateTotalCost(productQuantitiesList) {
    var totalCost = 0;
    for (var i = 0; i < productQuantitiesList.length; i++) {
        totalCost += productQuantitiesList[i].quantity * productQuantitiesList[i].product.Price;
    }
    console.log("new total cost: " + totalCost);
    var totalCostElement = document.getElementById("basketTotalPrice");
    totalCostElement.innerHTML = "£" + totalCost;
}

function updateTotalNumberOfProductsInBasket(productQuantitiesList) {
    var total = 0;
    for (var i = 0; i < productQuantitiesList.length; i++) {
        total += productQuantitiesList[i].quantity;
    }
    console.log("new total number of products in basket: " + total);
    var totalNumberOfProductsElement = document.getElementById("basketTotalNumberOfProducts");
    totalNumberOfProductsElement.innerHTML = total;
}