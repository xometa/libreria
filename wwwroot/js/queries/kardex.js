let container = query.id('data-list'),
    pagination = query.id('create-pagination'),
    product = query.id('idproduct'),
    date = query.id('dateO'),
    btnSearch = query.id('btnfilter'),
    stock = query.id('stock'),
    showQuantity = query.id('showQuantity');
let search = "",
    id = 0,
    quantity = 6,
    currentPage = 1;

kardex();

function kardex() {
    let data = {
        Id: parseInt(id),
        search: search,
        quantity: quantity,
        currentPage: currentPage,
        handler: "KardexPartialView"
    }
    query.get({
        url: 'Kardex',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            stock.innerHTML = container.querySelector('#filter-results').getAttribute('data-saldo');
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los movimientos del producto seleccionado, no se pueden visualizar.", "error");
        }
    });
}

btnSearch.onclick = function (e) {
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
    if (!query.validate(date.value, "string")) {
        search = date.value;
    } else {
        search = "";
    }

    if (!query.validate(product.value, "int") && !query.validate(product.value, "numbers")) {
        id = parseInt(product.value);
    } else {
        id = 0;
    }
    quantity = parseInt(showQuantity.value);
    kardex();
}