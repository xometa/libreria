let tbody = query.id('data-body');

//para la creación de la paginación de la consulta de productos
//con stock minímo
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idProduct = query.id('idproduct'),
    tag = query.id('tag'),
    category = query.id('category'),
    btnfilter = query.id('btnfilter');

let product = 0,
    tagFilter = "",
    categoryFilter = "",
    quantity = 6,
    currentPage = 1;


notPricessList();

function notPricessList() {
    let data = {
        handler: "NotPrices",
        idproduct: product,
        tag: tagFilter,
        category: categoryFilter,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'NotPrices',
        method: 'get',
        data: data,
        success: function (data) {
            tbody.innerHTML = data;
            pagination.innerHTML = query.createPagination(tbody, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

btnfilter.onclick = function (e) {
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
    //product
    if (!query.validate(idProduct.value, "int") && !query.validate(idProduct.value, "numbers")) {
        product = parseInt(idProduct.value);
    } else {
        product = 0;
    }

    //tag
    if (!query.validate(tag.value, "string")) {
        tagFilter = tag.value;
    } else {
        tagFilter = "";
    }

    //category
    if (!query.validate(category.value, "string")) {
        categoryFilter = category.value;
    } else {
        categoryFilter = "";
    }
    quantity = parseInt(showQuantity.value);
    notPricessList();
}