let container = query.id("data-body");
//para la creación de la paginación de empleados
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idProduct = query.id('idproduct');
let product = 0,
    quantity = 6,
    currentPage = 1;

//llamado de funciones
pricesWithoutSales();
//funciones
function pricesWithoutSales() {
    let data = {
        handler: "NotSales",
        idproduct: product,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'NotSales',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "La información de los empleados no se ha podido cargar.", "error");
        }
    });
}


//metodo de filtros
idProduct.onchange = function (e) {
    let current = e.target;
    if (!query.validate(current.value, "int") && !query.validate(current.value, "numbers")) {
        product = parseInt(current.value);
    } else {
        product = 0;
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    pricesWithoutSales();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    idProduct.selectedIndex = 0;
    product = 0;
    currentPage = 1;
    pricesWithoutSales();
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh();
}

function refresh() {
    quantity = parseInt(showQuantity.value);
    //idProduct
    if (!query.validate(idProduct.value, "int") && !query.validate(idProduct.value, "numbers")) {
        product = parseInt(idProduct.value);
    } else {
        product = 0;
    }
    currentPage = currentPage;
    pricesWithoutSales();
}