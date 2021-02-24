//variables
let productsContainer = query.id('products-list'),
    buttonModalProduct = query.id("btn-modalproduct"),
    tittleModal = query.id("tittle-modal"),
    modalBody = query.id("modalBody"),
    formProduct = query.id("formProduct"),
    formPrice = query.id("formPrice"),
    buttonAction = query.id('btn-action'),
    priceBody = query.id("price-body"),
    token,
    tBody = query.id("prices-list"),
    btnCleanPrice = query.id("btn-cleanprice"),
    modalProductFooter = query.id("modalproduct-footer"),
    mButtonDelete = query.id("mbtn-delete"),
    mTitleDelete = query.id("mtitle-delete"),
    mTitle = query.id("mtitle"),
    idProductPrice = 0;

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//para la creación de la paginación de los precios por productos
let pricePagination = query.id('price-pagination'),
    showPriceQuantity = query.id('show-price-quantity'),
    date = query.id('price-searchdate');
let searchPrice = "",
    quantityPrice = 3,
    currentPricePage = 1;
    
//llamada de funciones
productsList(); 

function productsList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ProductsAjax',
        method: 'get',
        data: data,
        success: function (data) {
            productsContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(productsContainer, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

//metodo de filtros
searchData.onkeyup = function (e) {
    let current = e.target;
    //obtenemos el valor de la caja de texto
    //la búsqueda se puede realizar mediante
    //el nombre del producto, marca ó categoría
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    //obtenemos el total de datos a mostrar
    // y reiniciamos la pagina actual, ya que
    //se filtraran datos
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    productsList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    //tomamos el valor de la cantidad por página a ver
    quantity = parseInt(current.value);
    //limpiamos la caja de texto, de igual forma
    // ponemos en cero la pagina actual
    search = searchData.value = "";
    currentPage = 1;
    productsList();
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Products");
}

pricePagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPricePage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Prices");
}

function refresh(action) {
    if (action === "Products") {
        quantity = parseInt(showQuantity.value);
        if (searchData.value !== "") {
            search = searchData.value;
        } else {
            search = "";
        }
        currentPage = currentPage;
        productsList();
    } else {
        quantityPrice = parseInt(showPriceQuantity.value);
        if (date.value !== "") {
            searchPrice = date.value;
        } else {
            searchPrice = "";
        }
        currentPricePage = currentPricePage;
        partialView(idProductPrice, "PricesListPartialView", "", tBody, "tBody");
    }
}


date.onchange = function (e) {
    let current = e.target;
    if (current.value !== "") {
        searchPrice = current.value;
    } else {
        searchPrice = "";
    }
    quantityPrice = parseInt(showPriceQuantity.value);
    currentPricePage = 1;
    //llamando el listado de precios
    partialView(idProductPrice, "PricesListPartialView", "", tBody, "tBody");
}

showPriceQuantity.onchange = function (e) {
    let current = e.target;
    quantityPrice = parseInt(current.value);
    searchPrice = date.value = "";
    currentPricePage = 1;
    partialView(idProductPrice, "PricesListPartialView", "", tBody, "tBody");
}

//////////////////// FINALIZACIÓN DE CREACIÓN DE PAGINACIONES ///////////////////////////////

//funciones para operaciones de la vista
buttonModalProduct.onclick = (e) => {
    tittleModal.innerHTML = "Nuevo producto";
    buttonIcon(tittleModal);
    partialView(0, "PartialView", "#modal-product", modalBody, "");
}

function buttonIcon(tittle) {
    if (tittle.textContent == "Nuevo producto") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar producto") {
        if (buttonAction.childNodes[0].classList.contains("mdi-content-save")) {
            buttonAction.childNodes[0].classList.remove("mdi-content-save");
        }
        buttonAction.childNodes[0].classList.add("mdi-border-color");
        buttonAction.childNodes[1].textContent = " Modificar";
    }
}

function partialView(Id, Handler, ModalName, ModalBody, Action) {
    token = query.names("__RequestVerificationToken");
    let data = {}
    if (Handler === "PricesListPartialView") {
        data = {
            Id: Id,
            date: searchPrice,
            quantity: quantityPrice,
            currentPage: currentPricePage
        }
    } else {
        data = { Id: Id }
    }

    if (Handler === "PricesListPartialView" || Handler === "PricePartialView") {
        idProductPrice = Id;
    } else {
        idProductPrice = 0;
    }
    query.ajax({
        url: "Products?handler=" + Handler,
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: data,
        success: function (data) {
            ModalBody.innerHTML = data;
            //creamos la paginación para los precios
            if (Handler === "PricesListPartialView") {
                pricePagination.innerHTML = query.createPagination(ModalBody, "#price-filters");
            }
            if (Action === "") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-sm");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-lg");
                modalProductFooter.style.display = "";
                $(ModalName).modal("show");
            } else if (Action === "Information") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-lg");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-sm");
                modalProductFooter.style.display = "none";
                $(ModalName).modal("show");
            }
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

//metodo para cargar la imagen seleccionada del explorador de archivos,
// también para cambiar el estado del producto

modalBody.onclick = function (e) {
    let element = e.target;
    let Id = 0,
        Status = 0;
    if (element.matches('button[data-name=Status]')) {
        if (element.classList.contains("btn-success")) {
            element.classList.remove("btn-success");
            element.classList.add("btn-danger");
            element.textContent = "Deshabilitado";
            Status = 0;
        } else {
            element.classList.remove("btn-danger");
            element.classList.add("btn-success");
            element.textContent = "Habilitado";
            Status = 1;
        }
        Id = element.getAttribute("data-idproduct");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers") &&
            Status >= 0 && Status <= 1 && !isNaN(Status)) {
            options({ Id: parseInt(Id), Status, Option: "Product" }, "EditStatus", "Estado del producto", "");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (element.matches("input[type=file]")) {
        let imagenNuevaProducto = query.q("#Producto_Archivo");
        imagenNuevaProducto.onchange = function () {
            readURL(this);

            function readURL(input) {
                if (input.files && input.files[0]) {
                    let reader = new FileReader();
                    reader.onload = function (e) {
                        let imagen = query.q("#imageview");
                        imagen.src = e.target.result;
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
        }
    }
}

productsContainer.addEventListener("click", function productactions(e) {
    let actual = e.target;
    let Id = 0;
    if (actual.matches('img[id=imageproduct]')) {
        Id = actual.getAttribute("data-idproduct");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            actions(parseInt(Id), "view");
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    } else if (actual.matches('span[id=price]') || actual.classList.contains('btn-info')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idproduct");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            reboot("prices")
            actions(parseInt(Id), "price");
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    } else if (actual.matches('span[id=edit]') || actual.classList.contains('btn-warning')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idproduct");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            actions(parseInt(Id), "edit");
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    } else if (actual.matches('span[id=delete]') || actual.classList.contains('btn-danger')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idproduct");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            mTitle.innerHTML = "Eliminar producto";
            $("#modalDeleteProduct").modal("show");
            mButtonDelete.onclick = (e) => {
                actions(parseInt(Id), "delete");
            }
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    }
});

function actions(Id, action) {
    if (!query.validate(action, "string")) {
        if (action == "view") {
            tittleModal.innerHTML = "Información del producto";
            partialView(Id, "ProductInformation", "#modal-product", modalBody, "Information");
        } else if (action == "edit") {
            tittleModal.innerHTML = "Modificar producto";
            buttonIcon(tittleModal);
            partialView(Id, "PartialView", "#modal-product", modalBody, "");
        } else if (action == "delete") {
            options({ Id }, "Delete", "Eliminar producto", "Product");
        } else if (action == "price") {
            partialView(Id, "PricesListPartialView", "", tBody, "tBody");
            partialView(Id, "PricePartialView", "#modal-prices", priceBody, "");
        }
    } else {
        query.toast("Alteración de información", "La informacion del servidor ha sido alterada.", "error");
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Products?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let obj = JSON.parse(data);
            query.toast(Tittle, obj.message, obj.icon);
            if (Option == "Product") {
                reboot("Products");
                productsList();
            } else if (Option == "Price") {
                reboot("Prices");
                partialView(obj.price.idproducto, "PricesListPartialView", "", tBody, "tBody");
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

//procedimiento para el almacenamiento de la información
formProduct.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    token = query.names("__RequestVerificationToken");
    let Producto_Id;
    let url = "Products";
    if (query.isUndefinedNull(query.id("Producto_Id"))) {
        Producto_Id = 0;
    } else {
        Producto_Id = query.id("Producto_Id").value;
    }
    if (!query.validate(Producto_Id, "int") && !query.validate(Producto_Id, "numbers")) {
        formData = new FormData(this);
        url += "?handler=Edit";
    } else {
        formData = new FormData(this);
        url = url;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //validación del código del producto
    if (query.validate(query.id("Producto_Codigo").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el código, para el producto.", "warning");
        return;
    }

    //validación del nombre del producto
    if (query.validate(query.id("Producto_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del producto.", "warning");
        return;
    }

    //validación del nombre del producto
    if (query.validate(query.id("Producto_Nombre").value, "letters")) {
        query.toast("Nombre inválido", "El nombre ingresado del producto es inválido.", "warning");
        return;
    }

    //validación del stock del producto
    if (query.validate(query.id("Producto_Stockminimo").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el stock minímo para el producto.", "warning");
        return;
    }

    if (query.validate(query.id("Producto_Stockminimo").value, "numbers")) {
        query.toast("Advertencia", "EL stock ingresado es invalido.", "warning");
        return;
    }

    if (query.validate(query.id("Producto_Stockminimo").value, "int")) {
        query.toast("Stock invalido", "El stock debe ser mayor a cero.", "warning");
        return;
    }

    //validación de la marca del producto
    if (!query.validate(query.id("Producto_Idmarca").value, "string")) {
        if (query.validate(query.id("Producto_Idmarca").value, "numbers")) {
            query.toast("Advertencia", "Los datos de la marca han sido alterados.", "warning");
            return;
        }

        if (query.validate(query.id("Producto_Idmarca").value, "int")) {
            query.toast("Campos vacíos", "Seleccione una marca para el producto.", "warning");
            return;
        }
    }

    //validación de la categoría del producto
    if (!query.validate(query.id("Producto_Idcategoria").value, "string")) {
        if (query.validate(query.id("Producto_Idcategoria").value, "numbers")) {
            query.toast("Advertencia", "La información de la categoría ha sido alterada.", "warning");
            return;
        }
    }

    if (query.validate(query.id("Producto_Idcategoria").value, "int")) {
        query.toast("Campos vacíos", "Seleccione una categoría para el producto.", "warning");
        return;
    }

    if (!query.validate(url, "string") && !query.isUndefinedNull(Producto_Id)) {
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let obj = JSON.parse(data);
                if (obj.action == "save" && obj.action != null && obj.action != "") {
                    if (obj.product != null) {
                        query.toast("Registro de producto", obj.message, obj.icon);
                        reboot("Products");
                        productsList();
                        $("#modal-product").modal("hide");
                    } else {
                        query.toast("Registro de producto", obj.message, obj.icon);
                    }
                } else if (obj.action == "edit" && obj.action != null && obj.action != "") {
                    if (obj.product != null) {
                        query.toast("Modificación de producto", obj.message, obj.icon);
                        reboot("Products");
                        productsList();
                        $("#modal-product").modal("hide");
                    } else {
                        query.toast("Modificación de producto", obj.message, obj.icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de producto", "Los datos del servidor han sido alterados.", "error");
    }
}

formPrice.onsubmit = function (e) {
    e.preventDefault();
    let data = query.getDataForm(this);
    token = query.names("__RequestVerificationToken");
    query.ajax({
        url: "Products?handler=ProductSavePrice",
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: data,
        success: function (data) {
            let obj = JSON.parse(data);
            if (obj.action == "save" && obj.action != null && obj.action != "") {
                if (obj.price != null) {
                    query.toast("Registro de precio", obj.message, obj.icon);
                    cleanModalPrices();
                    reboot("Prices");
                    partialView(obj.price.idproducto, "PricesListPartialView", "", tBody, "tBody");
                } else {
                    query.toast("Registro de precio", obj.message, obj.icon);
                }
            } else {
                query.toast("Error", "Ocurrio algo en el servidor.", "error");
            }
        },
        fail: function () {
            query.toast("Error", "La petición solicitada ha fallado.", "error");
        }
    });
}

btnCleanPrice.onclick = function (e) {
    cleanModalPrices();
}

//generar nuevo precio
priceBody.onclick = function (e) {
    let select = e.target;
    if (select.getAttribute("type") == "number" && select.getAttribute("id") == "Preciosproducto_Margen") {
        select.addEventListener("change", newPrice, true);
        select.addEventListener("input", newPrice, true);
    }
}

function newPrice(e) {
    let input = e.target;
    let margen = input.value;
    let viewPrice = priceBody.querySelector("#shoppingNewPrice"),
        lastPrice = priceBody.childNodes[5].children[0].children[0].children[1].children[1].value;
    let price = parseFloat(lastPrice) + (parseFloat(lastPrice) * parseFloat(margen / 100));
    viewPrice.innerHTML = "$ " + parseFloat(price).toFixed(2);
}

//metodos para eliminar un precio del producto
//y para cambiar el estado de un producto
tBody.addEventListener("click", function (e) {
    let element = e.target;
    let Id = 0,
        Status = 0;
    if (element.matches("input[type=checkbox]")) {
        Id = element.getAttribute("data-idprice");
        if (element.checked) {
            Status = 1;
        } else {
            Status = 0;
        }
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers") &&
            (!isNaN(Status) && Status >= 0 && Status != undefined)) {
            options({ Id: parseInt(Id), Status, Option: "Price" }, "EditStatus", "Estado del precio", "");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (element.matches("i[id=delete-price]")) {
        Id = element.parentNode.getAttribute("data-idprice");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            mTitle.innerHTML = "Eliminar precio del producto";
            $("#modalDeleteProduct").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ Id: parseInt(Id) }, "DeletePrice", "Eliminar precio", "Price");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}, true);


function cleanModalPrices() {
    priceBody.querySelector("#Preciosproducto_Margen").value = "0";
    priceBody.querySelector("#shoppingNewPrice").innerHTML = "$ 0.00";
}

function reboot(action) {
    if (action === "Products") {
        showQuantity.selectedIndex = 0;
        searchData.value = "";
        search = "";
        quantity = 6;
        currentPage = 1;
    } else {
        showPriceQuantity.selectedIndex = 0;
        date.value = null;
        searchPrice = "";
        quantityPrice = 3;
        currentPricePage = 1;
    }
}