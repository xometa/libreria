let employedContainer = query.id("data-body");
//para la creación de la paginación de empleados
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    idUser = query.id('iduser');
let user = 0,
    quantity = 6,
    currentPage = 1;

//para mostrar los permisos

let destination = query.id('permissions-list'),
    message = query.id('message'),
    permissionList = query.id('permission');

let clon = document.importNode(message.content, true);
destination.appendChild(clon);

//llamado de funciones
withPermissions();
//funciones
function withPermissions() {
    let data = {
        handler: "WithPermission",
        id: user,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'WithPermission',
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
idUser.onchange = function (e) {
    let current = e.target;
    if (!query.validate(current.value, "int") && !query.validate(current.value, "numbers")) {
        user = parseInt(current.value);
    } else {
        user = 0;
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    withPermissions();
    userPermissionsList(user);
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    idUser.selectedIndex = 0;
    user = 0;
    currentPage = 1;
    withPermissions();
    userPermissionsList(0);
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
    //iduser
    if (!query.validate(idUser.value, "int") && !query.validate(idUser.value, "numbers")) {
        user = parseInt(idUser.value);
    } else {
        user = 0;
    }
    currentPage = currentPage;
    withPermissions();
    userPermissionsList(0);
}

employedContainer.onclick = function (e) {
    let current = e.target;
    let Id = 0;
    if (current.matches('td')) {
        Id = current.parentNode.getAttribute('data-iduser');
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            userPermissionsList(parseInt(Id));
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function userPermissionsList(Id) {
    query.get({
        url: "WithPermission",
        method: 'get',
        data: { handler: "UserPermissionsList", Iduser: Id },
        success: function (data) {
            let { permissions } = JSON.parse(data);
            permissions.sort(function (a, b) {
                if (a.idpermisoNavigation.id > b.idpermisoNavigation.id) {
                    return 1;
                }
                if (a.idpermisoNavigation.id < b.idpermisoNavigation.id) {
                    return -1;
                }
                return 0;
            })
            fillContainer(permissions);
        },
        fail: function () {
            query.toast("Error", "La información de los clientes no se puede visualizar.", "error");
        }
    });
}

function fillContainer(permissions) {
    destination.innerHTML = "";
    if (permissions.length > 0) {
        permissions.forEach(function (data) {
            addDetails(data);
        });
    } else {
        let clon = document.importNode(message.content, true);
        destination.appendChild(clon);
    }
}

function addDetails(data) {
    clonNewDetail(permissionList, destination, data);
}

function clonNewDetail(template, destination, data) {
    let colum = template.content.querySelector(".col-lg-12");
    let name = colum.firstElementChild.firstElementChild.firstElementChild.firstElementChild;
    name.firstElementChild.children[0].childNodes[1].setAttribute("class", data.idpermisoNavigation.icono + " alert-info")
    name.firstElementChild.children[0].childNodes[2].textContent = data.idpermisoNavigation.nombre;
    let clon = document.importNode(template.content, true);
    destination.appendChild(clon);
}