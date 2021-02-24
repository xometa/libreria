let container = query.id('custom-accordion-one');

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

SalesList();

function SalesList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ShoppingsAjax',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

searchData.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    SalesList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    SalesList();
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

function refresh(action) {
    quantity = parseInt(showQuantity.value);
    if (searchData.value !== "") {
        search = searchData.value;
    } else {
        search = "";
    }
    currentPage = currentPage;
    SalesList();

}

date.onchange = function (e) {
    let current = e.target;
    if (current.value !== "") {
        searchPayment = current.value;
    } else {
        searchPayment = "";
    }
    quantityPayment = parseInt(showPaymentQuantity.value);
    currentPaymentPage = 1;
    paymentsList(idPaymentSale);
}

showPaymentQuantity.onchange = function (e) {
    let current = e.target;
    quantityPayment = parseInt(current.value);
    searchPayment = date.value = "";
    currentPaymentPage = 1;
    paymentsList(idPaymentSale);
}
