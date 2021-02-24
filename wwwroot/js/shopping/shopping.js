let productsContainer = query.id('products-list'),
    btnAddProducts = query.id('btn-addproducts'),
    formShopping = query.id('formShopping'),
    buttonCancel = query.id('btn-cancelshopping');

let shoppingProducts = [];

//variables para pintar los productos a comprar
const productsDestination = query.id('data-body'),
    productTemplate = query.id('rowProduct'),
    empty = query.id('empty'),
    subTotal = query.id('subtotal'),
    iva = query.id('iva'),
    total = query.id('total');

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    categoriesList = query.id('categories-container');
let category = "",
    quantity = 24,
    currentPage = 1;

//de entrada poner datos vacíos
productsDestination.innerHTML = empty.innerHTML;

btnAddProducts.onclick = function (e) {
    productsList();
}

//seleccionando categoría
categoriesList.onclick = function (e) {
    let current = e.target;
    if (current.matches('button[type=button]')) {
        category = current.getAttribute("data-category");
        if (category === "Todo") {
            category = "";
        }
        currentPage = 1;
        productsList();
    }
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    category = "";
    productsList();
}

function productsList() {
    let data = {
        search: category,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: '/Products/ProductsListAjax',
        method: 'get',
        data: data,
        success: function (data) {
            productsContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(productsContainer, "#filter-results");
            paintSelectionAjax();
            $('#modal-products').modal("show");
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

function paintSelectionAjax() {
    if (shoppingProducts.length > 0) {
        //obtenemos el listado de cards seleccionados
        let card = productsContainer.querySelectorAll('.card');
        card.forEach(c => {
            data = query.getAttributes(c);
            if (data != null) {
                if (!query.validate(data.idproduct, "int")) {
                    //recorremos el listado y pintamos los card correspondiente
                    shoppingProducts.forEach(pr => {
                        if (parseInt(pr.idproduct) == parseInt(data.idproduct)) {
                            addFooter(c);
                        }
                    });
                }
            }
        });
    }
}

//metodo para seleccionar la imagen
productsContainer.onclick = (e) => {
    let current = e.target;
    let card;
    if (current.matches('img')) {
        card = current.parentNode;
        addFooter(card);
    }
}
//permitira seleccionar o no el footer del producto
function addFooter(card) {
    let cardFooter;
    let data;
    cardFooter = card.querySelector('.card-footer');
    if (card.getAttribute('data-selected') == 'true') {
        data = query.getAttributes(card);
        cardFooter.classList.remove('bg-info');
        card.setAttribute('data-selected', false);
        removeProduct(data.idproduct);
    } else {
        cardFooter.classList.add('bg-info');
        card.setAttribute('data-selected', true);
        idProduct = card.getAttribute('data-idproduct');
        data = query.getAttributes(card);
        addProducto(data);
    }
}

//agregando los productos a la lista
function addProducto(params) {
    params.quantity = 1;
    params.price = 0.00;
    if (searchProduct(params.idproduct) == null) {
        shoppingProducts.push(params);
        displayList();
    }
}

//verifica si el producto ya esta asignado en la lista
function searchProduct(id) {
    for (let k = 0; k < shoppingProducts.length; k++) {
        if (shoppingProducts[k].idproduct === id) {
            return shoppingProducts[k];
        }
    }
    return null;
}

//quitando un producto de la lista
function removeProduct(idproduct) {
    let element = null;
    let position = -1;
    if (shoppingProducts.length > 0) {
        for (let i = 0; i < shoppingProducts.length; i++) {
            if (shoppingProducts[i].idproduct == idproduct) {
                element = shoppingProducts[i];
                break;
            } else {
                element = null;
            }
        }
        if (element != null) {
            position = shoppingProducts.indexOf(element);
            if (position > -1) {
                shoppingProducts.splice(position, 1);
                displayList();
            }
        }
    }
}

//lista todos los productos seleccionados
function displayList() {
    if (shoppingProducts.length > 0) {
        productsDestination.innerHTML = "";
        let subTotalCompra = 0, totalRow = 0, ivaCompra = 0, totalCompra = 0;
        shoppingProducts.forEach(pr => {
            let content = productTemplate.content;
            let row = content.querySelector("tr");
            let col = row.children;

            //nombre
            col[0].children[0].src = pr.image;
            col[0].children[1].innerHTML = pr.name;
            //cantidad
            col[1].firstElementChild.value = pr.quantity;
            col[1].firstElementChild.setAttribute('data-idproduct', pr.idproduct);
            //precio
            if (isNaN(pr.price)) {

                col[2].firstElementChild.value = "0.00";
            } else {

                col[2].firstElementChild.value = parseFloat(pr.price).toFixed(2);
            }
            col[2].firstElementChild.setAttribute('data-idproduct', pr.idproduct);
            //total fila
            totalRow = pr.price * pr.quantity;
            //subtotal
            subTotalCompra += totalRow;
            if (isNaN(totalRow)) {

                col[3].innerHTML = "0.00";
            } else {
                col[3].innerHTML = "$ " + totalRow.toFixed(2);
            }
            //icon
            col[4].firstElementChild.firstElementChild.setAttribute('data-idproduct', pr.idproduct);
            let clon = document.importNode(productTemplate.content, true);
            productsDestination.appendChild(clon);
        });
        if (isNaN(subTotalCompra)) {
            subTotal.innerHTML = "$ 0.00";
            iva.innerHTML = "$ 0.00";
            total.innerHTML = "$ 0.00";
        } else {
            subTotal.innerHTML = "$ " + subTotalCompra.toFixed(2);
            ivaCompra = subTotalCompra * 0.13;
            iva.innerHTML = "$ " + ivaCompra.toFixed(2);
            totalCompra = subTotalCompra + ivaCompra;
            total.innerHTML = "$ " + totalCompra.toFixed(2);
        }
        //función para modificar la cantidad del producto
        actionsRow();
    } else {
        productsDestination.innerHTML = empty.innerHTML;
        subTotal.innerHTML = "$ 0.00";
        iva.innerHTML = "$ 0.00";
        total.innerHTML = "$ 0.00";
    }
}

//acciones asignadas a los input con el icono de eliminar
function actionsRow() {
    let content = productsDestination;
    let inputQuantity = content.querySelectorAll('input[type=number]');
    let inputPrice = content.querySelectorAll('input[type=text]');
    let icons = content.querySelectorAll("i[class='mdi mdi-delete mdi-24px']");

    for (let i = 0; i < inputQuantity.length; i++) {
        //agregamos los eventos a los inputs para validar las cantidades ingresadas
        inputQuantity[i].addEventListener('blur', productQuantity, false);
        inputQuantity[i].addEventListener('input', productQuantity, false);
    }

    for (let i = 0; i < inputPrice.length; i++) {
        //agregamos los eventos a los inputs para validar las cantidades ingresadas
        inputPrice[i].addEventListener('blur', productPrice, false);
    }
    icons.forEach(icon => {
        icon.addEventListener('click', function (e) {
            let current = e.target;
            let idproduct = 0;
            if (current.nodeName === 'I') {
                idproduct = current.getAttribute('data-idproduct');
                if (!query.validate(idproduct, "int")) {
                    removeProduct(idproduct)
                } else {
                    query.toast("Error", "La información del servidor ha sido alterada.", "error");
                }
            } else {
                query.toast("Error", "La información del servidor ha sido alterada.", "error");
            }
        }, false);
    });
}


function productQuantity(e) {
    let current = e.target;
    let quantity = current.value;
    let id = current.getAttribute("data-idproduct");
    if (!query.validate(quantity, "int")) {
        if (!query.isUndefinedNull(id)) {
            addQuantityPrice(id, "quantity", parseInt(quantity));
            displayList();
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    } else {
        if (!query.isUndefinedNull(id)) {
            if (e.type === 'blur') {
                e.target.value = 1;
                addQuantityPrice(id, "quantity", 1);
                displayList();
            }
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function productPrice(e) {
    let current = e.target;
    let price = current.value;
    let id = current.getAttribute("data-idproduct");
    if (!query.validate(price, "string") && parseFloat(price) > 0) {
        if (!query.isUndefinedNull(id)) {
            addQuantityPrice(id, "price", parseFloat(price));
            displayList();
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    } else {
        if (!query.isUndefinedNull(id)) {
            if (e.type === 'blur') {
                e.target.value = 0.00;
                addQuantityPrice(id, "price", 0.00);
                displayList();
            }
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function addQuantityPrice(id, action, q) {
    switch (action) {
        case 'price':
            for (let k = 0; k < shoppingProducts.length; k++) {
                if (shoppingProducts[k].idproduct === id) {
                    shoppingProducts[k].price = q;
                }
            }
            break;
        case 'quantity':
            for (let k = 0; k < shoppingProducts.length; k++) {
                if (shoppingProducts[k].idproduct === id) {
                    shoppingProducts[k].quantity = q;
                }
            }
            break;
        default:
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
            break;
    }
}

//mandamos la información a registrar
formShopping.onsubmit = function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    let error = [], priceEmpty = [], quantityEmpty = [];
    let message = "La información del servidor ha sido alterada.";
    let data;
    let shoppingProduct = null;

    //variables del encabezado compra
    let numDocument = query.id('Shopping_Documento'),
        idProvider = query.id('Shopping_Idproveedor'),
        date = query.id('Shopping_Fecha'),
        shoppingType = query.id('Shopping_Tipo');

    if (shoppingProducts.length < 1) {
        query.toast("Seleccionar productos", "Seleccione los productos necesarios, para poder realizar la compra.", "warning");
        return;
    }

    for (let i = 0; i < shoppingProducts.length; i++) {
        shoppingProduct = shoppingProducts[i];
        if (shoppingProduct != null) {
            //validando el id del producto
            data = parseInt(shoppingProduct.idproduct);
            formData.append('Details[' + i + '].Idproducto', data);
            if (data <= 0) {
                error.push(message);
            }
            //validando la cantidad ingresada
            data = parseInt(shoppingProduct.quantity);
            formData.append('Details[' + i + '].Cantidad', data);
            if (data <= 0) {
                quantityEmpty.push(message);
            }

            //validando el id del precio del producto
            data = parseInt(shoppingProduct.price);
            formData.append('Details[' + i + '].Precio', data);
            if (data <= 0) {
                priceEmpty.push(message);
            }
        } else {
            error.push(message);
        }
    }

    if (error.length > 0) {
        query.toast("Error", "La información del servidor ha sido alterada.", "error");
        return;
    }

    if (quantityEmpty.length > 0) {
        query.toast("Ingresar cantidades", "Por favor, ingrese las cantidades respectivas de los productos seleccionados.", "warning");
        return;
    }

    if (priceEmpty.length > 0) {
        query.toast("Ingresar precios de compra", "Por favor, ingrese los respectivos precios de compra, para los productos seleccionados.", "warning");
        return;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    if (numDocument == null || idProvider == null || date == null || shoppingType == null) {
        query.toast("Error", "La información del formulario ha sido alterada.", "error");
        return;
    }

    //validación del número de documento
    if (query.validate(numDocument.value, "string")) {
        query.toast("Campos vacíos", "Ingrese el número de documento de la compra.", "warning");
        return;
    }

    if (query.validate(idProvider.value, "string")) {
        query.toast("Campos vacíos", "Seleccione el proveedor, al que se le realizara la compra.", "warning");
        return;
    }

    if (query.validate(date.value, "string")) {
        query.toast("Campos vacíos", "Seleccione la fecha en que se realizara la compra.", "warning");
        return;
    }

    if (query.validate(shoppingType.value, "string")) {
        query.toast("Campos vacíos", "Seleccione el tipo de la compra.", "warning");
        return;
    }

    //envio de la información de la compra
    if (!query.emptyElements(this) &&
        error.length == 0 &&
        quantityEmpty.length == 0 &&
        priceEmpty.length == 0) {
        let token = query.names("__RequestVerificationToken");
        query.ajax({
            url: "Shoppings",
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, shopping, message, icon } = JSON.parse(data);
                if (action == "save" && action != null && action != "") {
                    if (shopping != null) {
                        query.toast("Registro de compra", message, icon);
                        clean();
                    } else {
                        query.toast("Registro de compra", message, icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });

    } else {
        query.toast("Error", "La información del servidor ha sido alterada.", "error");
    }
}

buttonCancel.onclick = function (e) {
    clean();
}

function clean() {
    shoppingProducts = [];
    query.resetForm(formShopping);
    productsDestination.innerHTML = empty.innerHTML;
    subTotal.innerHTML = "$ 0.00";
    iva.innerHTML = "$ 0.00";
    total.innerHTML = "$ 0.00";
}