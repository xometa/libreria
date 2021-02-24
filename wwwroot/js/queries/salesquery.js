let container = query.id('custom-accordion-one');

//para la creación de la paginación de la consulta de ventas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idClient = query.id('idclient'),
    date = query.id('date'),
    typeSale = query.id('typeSale'),
    btnFilter = query.id('btnfilter');

let client = 0,
    search = "",
    type = "",
    quantity = 6,
    currentPage = 1;

salesList();

function salesList() {
    let data = {
        idclient: client,
        date: search,
        typeSale: type,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'SalesQueryAjax',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "El listado de ventas no se puede visualizar.", "error");
        }
    });
}

btnFilter.onclick = function (e) {
    currentPage=1;
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
    //idcliente
    if (!query.validate(idClient.value, "int") && !query.validate(idClient.value, "numbers")) {
        client = parseInt(idClient.value);
    } else {
        client = 0;
    }

    //fecha
    if (!query.validate(date.value, "string")) {
        search = date.value;
    } else {
        search = "";
    }

    //tipo venta
    if (!query.validate(typeSale.value, "string")) {
        type = typeSale.value;
    } else {
        type = "";
    }
    quantity = parseInt(showQuantity.value);
    salesList();
}