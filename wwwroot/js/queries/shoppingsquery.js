let container = query.id('custom-accordion-one');

//para la creación de la paginación de la consulta de ventas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idProvider = query.id('idprovider'),
    date = query.id('date'),
    typeShopping = query.id('typeShopping'),
    btnFilter = query.id('btnfilter');

let provider = 0,
    search = "",
    type = "",
    quantity = 6,
    currentPage = 1;

shoppingsList();

function shoppingsList() {
    let data = {
        idprovider: provider,
        date: search,
        typeShopping: type,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ShoppingsQueryAjax',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "El listado de compras no se puede visualizar.", "error");
        }
    });
}

btnFilter.onclick = function (e) {
    currentPage = 1;
    filters();
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? parseInt(child.getAttribute('data-page')) : parseInt(current.getAttribute('data-page'));
    filters();
}

function filters() {
    //idprovider
    if (!query.validate(idProvider.value, "int") && !query.validate(idProvider.value, "numbers")) {
        provider = parseInt(idProvider.value);
    } else {
        provider = 0;
    }

    //date
    if (!query.validate(date.value, "string")) {
        search = date.value;
    } else {
        search = "";
    }

    //type
    if (!query.validate(typeShopping.value, "string")) {
        type = typeShopping.value;
    } else {
        type = "";
    }
    quantity = parseInt(showQuantity.value);
    shoppingsList();
}