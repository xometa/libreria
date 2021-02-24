let container = query.id('custom-accordion-one');

//para la creación de la paginación de la consulta de ventas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idProvider = query.id('idprovider'),
    month = query.id('month'),
    btnFilter = query.id('btnfilter');

let provider = 0,
    search = 0,
    quantity = 6,
    currentPage = 1;

shoppingsMonthList();

function shoppingsMonthList() {
    let data = {
        handler: "MonthYear",
        idprovider: provider,
        month: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ShoppingsMonth',
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

    //idprovider
    if (!query.validate(month.value, "int") && !query.validate(month.value, "numbers")) {
        search = parseInt(month.value);
    } else {
        search = 0;
    }
    quantity = parseInt(showQuantity.value);
    shoppingsMonthList();
}