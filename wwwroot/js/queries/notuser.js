let employedContainer = query.id("data-body");
//para la creación de la paginación de empleados
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//llamado de funciones
notUserList();
//funciones
function notUserList() {
    let data = {
        handler: "NotUser",
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'WithoutUser',
        method: 'get',
        data: data,
        success: function (data) {
            employedContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(employedContainer, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "La información de los empleados no se ha podido cargar.", "error");
        }
    });
}


//metodo de filtros
searchData.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    notUserList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    notUserList();
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

function refresh() {
    quantity = parseInt(showQuantity.value);
    if (searchData.value !== "") {
        search = searchData.value;
    } else {
        search = "";
    }
    currentPage = currentPage;
    notUserList();
}