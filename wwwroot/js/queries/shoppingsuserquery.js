let container = query.id('custom-accordion-one');

//para la creación de la paginación de la consulta de ventas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idUser = query.id('iduser'),
    month = query.id('month'),
    currentYes = query.id('yes'),
    currentNot = query.id('not'),
    btnFilter = query.id('btnfilter');

let user = 0,
    search = 0,
    year = false,
    quantity = 6,
    currentPage = 1;

shoppingsUser();

function shoppingsUser() {
    let data = {
        handler: "ShoppingsUser",
        iduser: user,
        month: search,
        current: year,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'ShoppingsUser',
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
    //idUser
    if (!query.validate(idUser.value, "int") && !query.validate(idUser.value, "numbers")) {
        user = parseInt(idUser.value);
    } else {
        user = 0;
    }

    //month
    if (!query.validate(month.value, "int") && !query.validate(month.value, "numbers")) {
        search = parseInt(month.value);
    } else {
        search = 0;
    }

    //current year
    if (currentYes.checked) {
        year = true;
    }

    if (currentNot.checked) {
        year = false;
    }

    quantity = parseInt(showQuantity.value);
    shoppingsUser();
}