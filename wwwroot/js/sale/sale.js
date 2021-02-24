//variables para seleccionar y mostrar precios del producto
const productsContainer = query.id("products-list"),
    template = query.id('producto-information'),
    notSelected = query.id('empty'),
    destination = query.id('information'),
    itemPrice = query.id('item-price');
let saleProducts = [];

//variables, para poder pintar los productos a vender en la modal
const productsDestination = query.id('product-list-sales'),
    productTemplate = query.id('product-sale'),
    subTotal = query.id('subtotal'),
    iva = query.id('iva'),
    total = query.id('total');

//buttons and forms
let formSale = query.id('formSale'),
    buttonCancel = query.id('cancel'),
    actionAjax = "actionAjax";

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    categoriesList = query.id('categories-container')
showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    category = "",
    quantity = 8,
    currentPage = 1;

//mostramos la selección vacía de productos
destination.innerHTML = notSelected.innerHTML;
productsDestination.innerHTML = "<div class='text-center pt-2'>Seleccionar productos</div>";

productsList();
function productsList() {
    let data = {
        search: search,
        category: category,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: '/Products/InventoryProductsAjax',
        method: 'get',
        data: data,
        success: function (data) {
            productsContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(productsContainer, "#filter-results");
            paintSelectionAjax();
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

//seleccionando categoría
categoriesList.onclick = function (e) {
    let current = e.target;
    if (current.matches('button[type=button]')) {
        category = current.getAttribute("data-category");
        if (category === "Todo") {
            category = "";
        }
        filter("category");
    }
}

searchData.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    quantity = parseInt(showQuantity.value);
    filter("input");
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    filter("select");
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    filter("pagination");
}

function filter(option) {
    if (option === "input") {
        currentPage = 1;
        category = "";
    } else if (option === "select") {
        currentPage = 1;
        category = "";
        search = searchData.value = "";
    } else if (option === "category") {
        currentPage = 1;
        search = searchData.value = "";
    }
    productsList();
    actionAjax = "actionAjax";
    displayProductoInformation(null);
}

function paintSelectionAjax() {
    if (saleProducts.length > 0) {
        //obtenemos el listado de cards seleccionados
        let card = productsContainer.querySelectorAll('.card');
        card.forEach(c => {
            data = query.getAttributes(c);
            if (data != null) {
                if (!query.validate(data.idproduct, "int")) {
                    //recorremos el listado y pintamos los card correspondiente
                    saleProducts.forEach(pr => {
                        if (parseInt(pr.idproduct) == parseInt(data.idproduct)) {
                            addFooter(c);
                            addClicksQuantity(data.idproduct, "quitarclicks", 0);
                        }
                    });
                }
            }
        });
    }
}

productsContainer.onclick = (e) => {
    let current = e.target;
    let card;
    if (current.matches('img')) {
        card = current.parentNode;
        actionAjax = "";
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
        let product = searchProduct(data.idproduct);
        addClicksQuantity(data.idproduct, "clicks", 0);
        //contar los clicks para quitar el background
        if (product != null) {
            if (product.clicks == 2 || data.action != null) {
                cardFooter.classList.remove('bg-info');
                card.setAttribute('data-selected', false);
                removeProduct(data.idproduct);
                displayProductoInformation(null);
            } else {
                displayProductoInformation(data);
            }
        } else {
            cardFooter.classList.remove('bg-info');
            card.setAttribute('data-selected', false);
            removeProduct(data.idproduct);
            displayProductoInformation(null);
        }
    } else {
        cardFooter.classList.add('bg-info');
        card.setAttribute('data-selected', true);
        idProduct = card.getAttribute('data-idproduct');
        data = query.getAttributes(card);
        addProducto(data);
        displayProductoInformation(data);
    }
}

//función que agrega un producto seleccionado
function addProducto(params) {
    params.idpriceproduct = 0;
    params.quantity = 1;
    params.price = 0.00;
    params.clicks = 0;
    if (searchProduct(params.idproduct) == null) {
        saleProducts.push(params);
        displayList();
    }
}

//remueve un producto
function removeProduct(idproduct) {
    let element = null;
    let position = -1;
    if (saleProducts.length > 0) {
        for (let i = 0; i < saleProducts.length; i++) {
            if (saleProducts[i].idproduct == idproduct) {
                element = saleProducts[i];
                break;
            } else {
                element = null;
            }
        }
        if (element != null) {
            position = saleProducts.indexOf(element);
            if (position > -1) {
                saleProducts.splice(position, 1);
                displayList();
            }
        }
    }
}

//verifica si existe un producto
function searchProduct(id) {
    for (let k = 0; k < saleProducts.length; k++) {
        if (saleProducts[k].idproduct === id) {
            return saleProducts[k];
        }
    }
    return null;
}

//muestra la información del producto con el listado de precios de
//venta que le han sido asignados al producto, para que el usuario
//pueda seleccionar uno
function displayProductoInformation(data) {
    if (data != null && actionAjax === "") {
        let father = template.content;
        let information = father.querySelector(".align-items-center");
        let image = information.children[0].children[0];
        image.src = data.image;
        let name = information.children[1].children[0];
        name.innerHTML = data.name;
        let category = information.children[1].children[1];;
        category.innerHTML = data.category;
        let available = information.children[2].children[0].children[0];
        available.innerHTML = data.stock;
        let tbody = father.querySelector("#table-prices");
        tbody.innerHTML = "";
        query.get({
            url: 'Sales',
            method: 'get',
            data: { handler: "PricesList", Id: parseInt(data.idproduct) },
            success: function (data) {
                let { prices } = JSON.parse(data);
                //seteo de los precios
                let price = 0;
                for (let j = 0; j < prices.length; j++) {
                    prices[j].idcompraNavigation.detallecompra.forEach(dc => {
                        if (dc.idproducto == prices[j].idproducto && dc.idcompra == prices[j].idcompra) {
                            price = dc.precio + (dc.precio * (prices[j].margen / 100));
                        }
                    });
                    let col = itemPrice.content.children[0];
                    col.setAttribute("data-idpriceproduct", prices[j].id);
                    col.setAttribute("data-idproduct", prices[j].idproducto);
                    col.setAttribute("data-priceproduct", price.toFixed(2));
                    col.children[0].innerText = "Precio " + (j + 1);
                    col.children[1].innerText = "$ " + price.toFixed(2);
                    let clon = document.importNode(itemPrice.content, true);
                    tbody.appendChild(clon);
                }
                destination.innerHTML = template.innerHTML;
                selectPrice();
                paintPrice();
            },
            fail: function () {
                query.toast("Error", "Los precios del producto no se pueden visualizar.", "error");
            }
        });
    } else {
        destination.innerHTML = notSelected.innerHTML;
    }
}
//seleccionando precio para la venta
function selectPrice() {
    let tBody = destination.querySelector("#table-prices");
    let colums = tBody.querySelectorAll('tr');
    //se procede a agregar una clase de selección de precio
    colums.forEach(tr => {
        //agregamos hover
        tr.addEventListener("mouseover", function (e) {
            let current = e.target;
            if (current.nodeName == "TD") {
                current = current.parentNode;
            }

            if (!current.classList.contains('alert-success')) {
                current.classList.add('alert-success')
            }
            //pintamos el precio seleccionado, al salirnos del área
            //de selección
            paintPrice();
        }, false);
        //removemos el hover
        tr.addEventListener("mouseout", function (e) {
            let current = e.target;
            if (current.nodeName == "TD") {
                current = current.parentNode;
            }
            if (current.classList.contains('alert-success')) {
                current.classList.remove('alert-success')
            }
            //pintamos el precio seleccionado, al salirnos del área
            //de selección
            paintPrice();
        }, false);
        //se selecciona el precio, con el que se realizara la venta
        tr.addEventListener("click", function (e) {
            let current = e.target;
            let idPriceProduct = 0, idProduct = 0;
            //limpiamos todas las filas
            colums.forEach(tr => {
                if (tr.classList.contains('alert-success')) {
                    tr.classList.remove('alert-success');
                }
            });
            if (current.nodeName == "TD") {
                current = current.parentNode;
            }
            if (current.getAttribute("data-idproduct") != null &&
                current.getAttribute("data-idpriceproduct") != null) {
                idPriceProduct = parseInt(current.getAttribute("data-idpriceproduct"));
                idProduct = parseInt(current.getAttribute("data-idproduct"));
                saleProducts.forEach(p => {
                    if (p.idproduct == idProduct) {
                        p.idpriceproduct = idPriceProduct;
                        p.price = parseFloat(current.getAttribute("data-priceproduct"));
                    }
                });
                //pintamos el precio seleccionado, al salirnos del área
                //de selección
                paintPrice();
                displayList();
            } else {
                query.toast("Error", "Los precios del producto no se pueden visualizar.", "error");
            }
        }, false);
    });
}

//pinta el precio seleccionado, de la lista
function paintPrice() {
    let tBody = destination.querySelector("#table-prices");
    let colums = tBody.querySelectorAll('tr');
    let data = null;
    //agregamos el alert, al precio seleccionado
    colums.forEach(tr => {
        saleProducts.forEach(pr => {
            if (pr.idpriceproduct == parseInt(tr.getAttribute("data-idpriceproduct"))) {
                data = pr;
                if (!tr.classList.contains('alert-success')) {
                    tr.classList.add('alert-success');
                }
            }
        });
    });

    //agregamos el precio seleccionado el footer
    let tFooter = tBody.nextElementSibling;
    let trFooter = tFooter.lastElementChild;
    if (data != null) {
        if (trFooter.classList.contains('alert-warning')) {
            trFooter.classList.remove('alert-warning');
        }
        trFooter.classList.add('alert-info');
        trFooter.children[1].innerHTML = "$ " + data.price;
    } else {
        if (trFooter.classList.contains('alert-info')) {
            trFooter.classList.remove('alert-info');
        }
        trFooter.classList.add('alert-warning');
        trFooter.children[1].innerHTML = "$ 0.00";
    }
}

function displayList() {
    if (saleProducts.length > 0) {
        productsDestination.innerHTML = "";
        let subTotalVenta = 0, totalRow = 0, ivaVenta = 0, totalVenta = 0;
        saleProducts.forEach(pr => {
            let content = productTemplate.content;
            let row = content.querySelector(".row");
            let col = row.children;
            //nombre
            col[0].innerHTML = pr.name;
            //cantidad
            col[1].firstChild.value = pr.quantity;
            col[1].firstChild.setAttribute('data-idproduct', pr.idproduct);
            //precio
            col[2].innerHTML = "$ " + pr.price.toFixed(2);
            //total
            totalRow = pr.price * pr.quantity;
            subTotalVenta += totalRow;
            col[3].innerHTML = "$ " + totalRow.toFixed(2);
            //icon
            col[4].firstChild.firstElementChild.setAttribute('data-idproduct', pr.idproduct);

            let clon = document.importNode(productTemplate.content, true);
            productsDestination.appendChild(clon);
        });
        subTotal.innerHTML = "$ " + subTotalVenta.toFixed(2);
        ivaVenta = subTotalVenta * 0.13;
        iva.innerHTML = "$ " + ivaVenta.toFixed(2);
        totalVenta = subTotalVenta + ivaVenta;
        total.innerHTML = "$ " + totalVenta.toFixed(2);
        //función para modificar la cantidad del producto
        actionsRow();
    } else {
        productsDestination.innerHTML = "<div class='text-center pt-2'>Seleccionar productos</div>";
        subTotal.innerHTML = "$ 0.00";
        iva.innerHTML = "$ 0.00";
        total.innerHTML = "$ 0.00";
    }
}

function actionsRow() {
    let content = productsDestination;
    let inputs = content.querySelectorAll('input[type=number]');
    let icons = content.querySelectorAll("i[class='mdi mdi-delete mdi-24px']");
    inputs.forEach(input => {
        //agregamos los eventos a los inputs para validar las cantidades ingresadas
        input.addEventListener('blur', productQuantity, false);
        input.addEventListener('input', productQuantity, false);
    });
    icons.forEach(icon => {
        icon.addEventListener('click', function (e) {
            let current = e.target;
            let idproduct = 0;
            let data = null;
            let cardSelected = null;
            if (current.nodeName === 'I') {
                idproduct = current.getAttribute('data-idproduct');
                if (!query.validate(idproduct, "int")) {
                    //quitamos background del card
                    let card = productsContainer.querySelectorAll('.card');
                    card.forEach(c => {
                        data = query.getAttributes(c);
                        if (data != null) {
                            if (!query.validate(data.idproduct, "int") && data.idproduct === idproduct) {
                                cardSelected = c;
                            }
                        }
                    });
                    //verificamos que la data del card no sea null con el contenedor
                    if (cardSelected != null && card != null) {
                        //removemos de la lista de los card seleccionados
                        cardSelected.setAttribute('data-action', 'delete');
                        addFooter(cardSelected);
                        cardSelected.removeAttribute('data-action', 'delete');
                        cardSelected = null;
                        data = null;
                    } else {
                        query.toast("Error", "La información del servidor ha sido alterada.", "error");
                    }
                } else {
                    query.toast("Error", "La información del servidor ha sido alterada.", "error");
                }
            } else {
                query.toast("Error", "La información del servidor ha sido alterada.", "error");
            }
        }, false);
    });
}

function addClicksQuantity(id, action, q) {
    switch (action) {
        case 'clicks':
            for (let k = 0; k < saleProducts.length; k++) {
                if (saleProducts[k].idproduct === id) {
                    saleProducts[k].clicks += 1;
                } else {
                    saleProducts[k].clicks = 0;
                }
            }
            break;
        case 'quitarclicks':
            for (let k = 0; k < saleProducts.length; k++) {
                if (saleProducts[k].idproduct === id) {
                    saleProducts[k].clicks = 0;
                } else {
                    saleProducts[k].clicks = 0;
                }
            }
            break;
        case 'quantity':
            for (let k = 0; k < saleProducts.length; k++) {
                if (saleProducts[k].idproduct === id) {
                    saleProducts[k].quantity = q;
                }
            }
            break;
        default:
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
            break;
    }
}

function productQuantity(e) {
    let current = e.target;
    let quantity = current.value;
    let id = current.getAttribute("data-idproduct");
    if (!query.validate(quantity, "int")) {
        if (!query.isUndefinedNull(id)) {
            addClicksQuantity(id, "quantity", parseInt(quantity));
            displayList();
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    } else {
        if (!query.isUndefinedNull(id)) {
            if (e.type === 'blur') {
                e.target.value = 1;
                addClicksQuantity(id, "quantity", 1);
                displayList();
            }
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    }
}

buttonCancel.onclick = function (e) {
    clean();
}

function clean() {
    saleProducts = [];
    query.resetForm(formSale);
    productsDestination.innerHTML = "<div class='text-center pt-2'>Seleccionar productos</div>";
    subTotal.innerHTML = "$ 0.00";
    iva.innerHTML = "$ 0.00";
    total.innerHTML = "$ 0.00";
    let card = productsContainer.querySelectorAll('.card');
    card.forEach(c => {
        data = query.getAttributes(c);
        if (data != null && data.selected === 'true') {
            c.setAttribute('data-action', 'delete');
            addFooter(c);
            c.removeAttribute('data-action', 'delete');
        }
    });
    $("#modal-sale").modal("hide");
}

formSale.onsubmit = function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    let error = [], priceEmpty = [], quantityEmpty = [];
    let message = "La información del servidor ha sido alterada.";
    let data;
    let productSale = null;

    //variables del encabezado venta
    let numDocument = query.id('Sale_Documento'),
        idClient = query.id('Sale_Idcliente'),
        date = query.id('Sale_Fecha'),
        saleType = query.id('Sale_Tipo');

    if (saleProducts.length < 1) {
        query.toast("Seleccionar productos", "Seleccione los productos necesarios, para poder realizar la venta.", "warning");
        return;
    }

    for (let i = 0; i < saleProducts.length; i++) {
        productSale = saleProducts[i];
        if (productSale != null) {
            //validando el id del producto
            data = parseInt(productSale.idproduct);
            if (data <= 0) {
                error.push(message);
            }
            //validando el id del precio del producto
            data = parseInt(productSale.idpriceproduct);
            formData.append('Details[' + i + '].Idproducto', data);
            if (data <= 0) {
                priceEmpty.push(message);
            }
            //validando la cantidad ingresada
            data = parseInt(productSale.quantity);
            formData.append('Details[' + i + '].Cantidad', data);
            if (data <= 0) {
                quantityEmpty.push(message);
            }
            //validando el precio seleccionado
            data = parseFloat(productSale.price);
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
        query.toast("Producto sin cantidad", "Por favor, ingrese las cantidades de los productos seleccionados.", "warning");
        return;
    }

    if (priceEmpty.length > 0) {
        query.toast("Seleccionar precio", "Por favor, seleccione un precio de venta, para los productos seleccionados.", "warning");
        return;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    if (numDocument == null || idClient == null || date == null || saleType == null) {
        query.toast("Error", "La información del formulario ha sido alterada.", "error");
        return;
    }

    //validación del número de documento
    if (query.validate(numDocument.value, "string")) {
        query.toast("Campos vacíos", "Ingrese el número de documento de la venta.", "warning");
        return;
    }

    if (query.validate(idClient.value, "string")) {
        query.toast("Campos vacíos", "Seleccione el cliente, al que se realizara la venta.", "warning");
        return;
    }

    if (query.validate(date.value, "string")) {
        query.toast("Campos vacíos", "Seleccione la fecha en que se realizara la venta.", "warning");
        return;
    }

    if (query.validate(saleType.value, "string")) {
        query.toast("Campos vacíos", "Seleccione el tipo de la venta.", "warning");
        return;
    }
    //envio de la información de la venta
    if (!query.emptyElements(this) &&
        error.length == 0 &&
        quantityEmpty.length == 0 &&
        priceEmpty.length == 0) {
        let token = query.names("__RequestVerificationToken");

        query.ajax({
            url: "Sales",
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, sale, message, icon } = JSON.parse(data);
                if (action == "save" && action != null && action != "") {
                    if (sale != null) {
                        query.toast("Registro de venta", message, icon);
                        clean();
                        productsList();
                    } else {
                        query.toast("Registro de venta", message, icon);
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