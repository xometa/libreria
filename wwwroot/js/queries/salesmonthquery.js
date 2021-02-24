let container = query.id('custom-accordion-one');

//para la creación de la paginación de la consulta de ventas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idClient = query.id('idclient'),
    month = query.id('month'),
    btnFilter = query.id('btnfilter');

let client = 0,
    search = 0,
    quantity = 6,
    currentPage = 1;

salesMonthList();

function salesMonthList() {
    let data = {
        handler: "MonthYear",
        idclient: client,
        month: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'SalesMonth',
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
    //idclient
    if (!query.validate(idClient.value, "int") && !query.validate(idClient.value, "numbers")) {
        client = parseInt(idClient.value);
    } else {
        client = 0;
    }

    //idclient
    if (!query.validate(month.value, "int") && !query.validate(month.value, "numbers")) {
        search = parseInt(month.value);
    } else {
        search = 0;
    }
    quantity = parseInt(showQuantity.value);
    salesMonthList();
}