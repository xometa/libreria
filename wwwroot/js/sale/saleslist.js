let container = query.id('custom-accordion-one'),
    destination = query.id('data-body'),
    status = query.id('sale-status'),
    restante = query.id('restante'),
    idPaymentSale = 0;

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//para la creación de la paginación de labonos de la venta
let paymentPagination = query.id('payment-pagination'),
    showPaymentQuantity = query.id('show-payment-quantity'),
    date = query.id('payment-searchdate');
let searchPayment = "",
    quantityPayment = 3,
    currentPaymentPage = 1;

SalesList();

function SalesList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ListAjax',
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
    refresh("SalesList");
}

paymentPagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPaymentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Payments");
}

function refresh(action) {
    if (action === "SalesList") {
        quantity = parseInt(showQuantity.value);
        if (searchData.value !== "") {
            search = searchData.value;
        } else {
            search = "";
        }
        currentPage = currentPage;
        SalesList();
    } else {
        quantityPayment = parseInt(showPaymentQuantity.value);
        if (date.value !== "") {
            searchPayment = date.value;
        } else {
            searchPayment = "";
        }
        currentPaymentPage = currentPaymentPage;
        paymentsList(idPaymentSale);
    }
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

//////////////////// FINALIZACIÓN DE CREACIÓN DE PAGINACIONES ///////////////////////////////

container.onclick = function (e) {
    let current = e.target;
    let idSale = 0;
    if (current.matches('button')) {
        idSale = current.getAttribute('data-idsale');
        if (!query.validate(idSale, "int")) {
            reboot();
            paymentsList(parseInt(idSale));
        } else {
            query.toast("Error", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function paymentsList(Id) {
    idPaymentSale = Id;
    let data = {
        IdSale: Id,
        date: searchPayment,
        quantity: quantityPayment,
        currentPage: currentPaymentPage,
        handler: "Payments"
    }
    query.get({
        url: 'SalesList',
        method: 'get',
        data: data,
        success: function (data) {
            destination.innerHTML = data;
            let r = parseFloat(query.id('price-filters').getAttribute('data-restante')).toFixed(2);
            restante.innerHTML = "$ " + r;
            let statusSale = parseInt(query.id('price-filters').getAttribute('data-Status'));
            let bg = status.parentElement;
            if (statusSale === 1) {
                status.innerHTML = "No cancelada";
                if (bg.classList.contains('bg-success')) {
                    bg.classList.remove('bg-success');
                }
                bg.classList.add('bg-danger');
            } else {
                status.innerHTML = "Cancelada";
                if (bg.classList.contains('bg-danger')) {
                    bg.classList.remove('bg-danger');
                }
                bg.classList.add('bg-success');
            }
            paymentPagination.innerHTML = query.createPagination(destination, "#price-filters");
            $('#modal-payments').modal("show");
        },
        fail: function () {
            query.toast("Error", "El listado de abonos de la venta no se pueden visualizar.", "error");
        }
    });
}

function reboot() {
    showPaymentQuantity.selectedIndex = 0;
    date.value = null;
    searchPayment = "";
    quantityPayment = 3;
    currentPaymentPage = 1;
}
/*
function displayList(paymentsList, total, statusSale) {
    destination.innerHTML = "";
    let bg = status.parentElement;
    let father = template.content;
    var options = { year: 'numeric', month: 'long', day: 'numeric' };
    if (paymentsList.length > 0) {
        paymentsList.forEach(p => {
            let tr = father.querySelector("tr");
            let payment = p.idabonoNavigation.monto;
            let date = new Date(p.idabonoNavigation.fechaabono);
            tr.children[0].innerHTML = "$ " + payment.toFixed(2);
            tr.children[1].innerHTML = date.toLocaleDateString("es-ES", options);
            let clon = document.importNode(template.content, true);
            destination.appendChild(clon);
        });
    } else {
        destination.innerHTML = empty.innerHTML;
    }
    restante.innerHTML = "$ " + total;
    if (statusSale === 1) {
        status.innerHTML = "No cancelada";
        if (bg.classList.contains('bg-success')) {
            bg.classList.remove('bg-success');
        }
        bg.classList.add('bg-danger');
    } else {
        status.innerHTML = "Cancelada";
        if (bg.classList.contains('bg-danger')) {
            bg.classList.remove('bg-danger');
        }
        bg.classList.add('bg-success');
    }
}*/