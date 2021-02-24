let container = query.id('data-body'),
    containerPayments = query.id('data-payments'),
    pagination = query.id('create-pagination'),
    paginationPayment = query.id('payment-pagination'),
    idSale = 0;

//filters
let showQuantity = query.id('showQuantity'),
    homeDate = query.id('home-date'),
    endDate = query.id('end-date'),
    idClient = query.id('idclient'),
    btnFilter = query.id('btnfilter');

//parameters
let client = 0,
    searchOne = "",
    searchTwo = "",
    quantity = 6,
    currentPage = 1;

//para la creación de la paginación de labonos de la venta
let paymentPagination = query.id('payment-pagination'),
    showPaymentQuantity = query.id('show-payment-quantity'),
    date = query.id('payment-searchdate');
let searchPayment = "",
    quantityPayment = 3,
    currentPaymentPage = 1;

let dayFooter = query.id('day'),
    lastDate = query.id('fechapayment'),
    restantePayment = query.id('restante');

//variables del formulario
let btnCancel = query.id('btn-cancelPayment'),
    btnAddPayment = query.id('btn-addPayment'),
    formPayment = query.id('formPayment');

//llamado de las funciones
salesList();

function salesList() {
    let data = {
        id: client,
        home: searchOne,
        end: searchTwo,
        quantity: quantity,
        currentPage: currentPage,
        handler: "SalesCredit"
    }
    query.get({
        url: 'Payments',
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

btnFilter.onclick = function (e) {
    currentPage = 1;
    filters("Sale");
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? parseInt(child.getAttribute('data-page')) : parseInt(current.getAttribute('data-page'));
    filters("Sale");
}

function filters(option) {

    if (option === "Sale") {
        //idcliente
        if (!query.validate(idClient.value, "int") && !query.validate(idClient.value, "numbers")) {
            client = parseInt(idClient.value);
        } else {
            client = 0;
        }

        //fecha de inicio
        if (!query.validate(homeDate.value, "string")) {
            searchOne = homeDate.value;
        } else {
            searchOne = "";
        }

        //fecha de fin
        if (!query.validate(endDate.value, "string")) {
            searchTwo = endDate.value;
        } else {
            searchTwo = "";
        }
        quantity = parseInt(showQuantity.value);
        salesList();
    } else {
        quantityPayment = parseInt(showPaymentQuantity.value);
        if (date.value !== "") {
            searchPayment = date.value;
        } else {
            searchPayment = "";
        }
        currentPaymentPage = currentPaymentPage;
        paymentsList(idSale);
    }
}

//procedimientos para poder registrar los abonos de 
//la venta al crédito

container.onclick = function (e) {
    let current = e.target;
    let Id = 0;
    if (current.matches('button[type=button]')) {
        Id = current.getAttribute("data-idsale");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            reboot();
            paymentsList(parseInt(Id))
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    }
}

//para los pagos de la venta

date.onchange = function (e) {
    let current = e.target;
    if (current.value !== "") {
        searchPayment = current.value;
    } else {
        searchPayment = "";
    }
    quantityPayment = parseInt(showPaymentQuantity.value);
    currentPaymentPage = 1;
    paymentsList(idSale);
}

showPaymentQuantity.onchange = function (e) {
    let current = e.target;
    quantityPayment = parseInt(current.value);
    searchPayment = date.value = "";
    currentPaymentPage = 1;
    paymentsList(idSale);
}

paymentPagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPaymentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    filters("Payments");
}

btnCancel.onclick = function (e) {
    cleandData();
}

function paymentsList(Id) {
    idSale = Id;
    let data = {
        IdSale: Id,
        date: searchPayment,
        quantityPayment: quantityPayment,
        currentPagePayment: currentPaymentPage,
        handler: "Payments"
    }
    query.get({
        url: 'Payments',
        method: 'get',
        data: data,
        success: function (data) {
            containerPayments.innerHTML = data;
            paginationPayment.innerHTML = query.createPagination(containerPayments, "#payments-filters");
            //agregamos los respectivos dato al footer de información
            let restante = parseFloat(query.id('payments-filters').getAttribute('data-restante'));
            if (restante > 0) {
                let totalR = parseInt(query.id('payments-filters').getAttribute('data-notfilters'));
                dayFooter.innerHTML = query.id('payments-filters').getAttribute('data-day');
                if (totalR > 0) {
                    lastDate.innerHTML = query.id('payments-filters').getAttribute('data-date');
                } else {
                    lastDate.innerHTML = "No hay pagos";
                }
                restantePayment.innerHTML = "$ " + restante.toFixed(2);
                $('#modal-payments').modal('show');
            } else {
                $('#modal-payments').modal('hide');
                filters("Sale");
            }
        },
        fail: function () {
            query.toast("Error", "El listado de abonos de la venta no se pueden visualizar.", "error");
        }
    });
}

containerPayments.onclick = function (e) {
    let current = e.target;
    let Idsale = 0, Idpayment = 0;
    if (current.matches("i[class='mdi mdi-delete mdi-24px']")) {
        console.log(current);
        Idsale = current.parentNode.getAttribute("data-idsale");
        Idpayment = current.parentNode.getAttribute("data-idpayment");
        if (!query.validate(Idsale, "int") && !query.validate(Idsale, "numbers") &&
            !query.validate(Idpayment, "int") && !query.validate(Idpayment, "numbers")) {
            token = query.names("__RequestVerificationToken");
            query.post({
                url: "Payments?handler=DeletePayment",
                token: token[0].value,
                content: "application/x-www-form-urlencoded",
                data: { IdSale: Idsale, IdPayment: Idpayment },
                success: function (data) {
                    let { action, sale, message, icon } = JSON.parse(data);
                    if (action == "delete" && action != null && action != "") {
                        query.toast("Eliminar abono", message, icon);
                        if (sale != null) {
                            paymentsList(parseInt(sale.id));
                        }
                    }
                },
                fail: function () {
                    query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
                }
            });
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function reboot() {
    showPaymentQuantity.selectedIndex = 0;
    date.value = null;
    searchPayment = "";
    quantityPayment = 3;
    currentPaymentPage = 1;
}

formPayment.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    //variables del pago
    let money = query.id('Payment_Monto'),
        datePayment = query.id('Payment_Fechaabono');

    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    if (money == null || datePayment == null) {
        query.toast("Error", "La información del formulario ha sido alterada.", "error");
        return;
    }

    if (query.validate(money.value, "string")) {
        query.toast("Campos vacíos", "Ingrese el monto del abono a realizar.", "warning");
        return;
    }

    if (isNaN(money.value)) {
        query.toast("Monto incorrecto", "El valor del abono ingresado es incorrecto.", "warning");
        return;
    }

    if (parseFloat(money.value) <= 0) {
        query.toast("Monto incorrecto", "El valor ingresado del abono debe ser mayor a cero.", "warning");
        return;
    }

    let restante = parseFloat(query.id('payments-filters').getAttribute('data-restante'));
    if (restante < parseFloat(money.value)) {
        query.toast("Monto superior a la deuda", "El valor ingresado del abono supera a la deuda total del cliente.", "warning");
        return;
    }

    if (query.validate(datePayment.value, "string")) {
        query.toast("Campos vacíos", "Ingrese la fecha en que se realizara el abono.", "warning");
        return;
    }
    if (!query.emptyElements(this)) {
        let token = query.names("__RequestVerificationToken");
        formData = new FormData(this);
        formData.append("IdSale", idSale);
        query.ajax({
            url: "Payments",
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, sale, message, icon } = JSON.parse(data);
                if (action == "save" && action != null && action != "") {
                    query.toast("Registro de abono", message, icon);
                    if (sale != null) {
                        cleandData();
                        paymentsList(parseInt(sale.id));
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    }
    else {
        query.toast("Error", "La información del servidor ha sido alterada.", "error");
    }

}

function cleandData() {
    query.resetForm(formPayment);
}