let user = query.id("Permission_Idusuario"),
    permission = query.id("Permission_Idpermiso"),
    formPermissions = query.id("formPermissions"),
    destination = query.id('permissions-list'),
    message = query.id('message'),
    permissionList = query.id('permission'),
    buttonCancel = query.id("btn-cancel"),
    mButtonDelete = query.id("mbtn-delete"),
    Id = 0;


let clon = document.importNode(message.content, true);
destination.appendChild(clon);

user.addEventListener("change", permissionsSelect, true);
buttonCancel.onclick = function(e) {
    user.selectedIndex = 0;
    permissionsList(0);
    userPermissionsList(0);
}

function permissionsSelect(e) {
    let current = e.currentTarget;
    if (current.value === "") {
        Id = 0;
    } else {
        Id = parseInt(current.value);
    }
    permissionsList(Id);
    userPermissionsList(Id);
}

function permissionsList(Id) {
    query.get({
        url: "Permissions",
        method: 'get',
        data: { handler: "UserPermissions", Iduser: Id },
        success: function(data) {
            let { list } = JSON.parse(data);
            permission.innerHTML = "";
            query.fillSelect(
                query.id("plantilla-option"),
                permission,
                list,
                "nombre",
                "id",
                "permiso"
            );
        },
        fail: function() {
            query.toast("Error", "La información de los clientes no se puede visualizar.", "error");
        }
    });
}

function userPermissionsList(Id) {
    query.get({
        url: "Permissions",
        method: 'get',
        data: { handler: "UserPermissionsList", Iduser: Id },
        success: function(data) {
            let { permissions } = JSON.parse(data);
            //ordenando el arreglo, para que se muestre de acuerdo a la jerarquía del sidebar
            permissions.sort(function(a, b) {
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
        fail: function() {
            query.toast("Error", "La información de los clientes no se puede visualizar.", "error");
        }
    });
}

function fillContainer(permissions) {
    destination.innerHTML = "";
    if (permissions.length > 0) {
        permissions.forEach(function(data) {
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
    let colum = template.content.querySelector(".col-lg-3");
    let name = colum.firstElementChild.firstElementChild.firstElementChild.firstElementChild;
    let button = colum.firstElementChild.firstElementChild.firstElementChild.lastElementChild;
    name.firstElementChild.children[0].childNodes[1].setAttribute("class", data.idpermisoNavigation.icono + " alert-success")
    name.firstElementChild.children[0].childNodes[2].textContent = data.idpermisoNavigation.nombre;
    button.firstElementChild.lastChild.setAttribute("id", "delete-permission");
    button.firstElementChild.lastChild.setAttribute("data-iduser", data.idusuario);
    button.firstElementChild.lastChild.setAttribute("data-idpermission", data.idpermiso);
    let clon = document.importNode(template.content, true);
    destination.appendChild(clon);
}

formPermissions.onsubmit = function(e) {
    e.preventDefault();
    let formData;
    let token = query.names("__RequestVerificationToken");

    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    if (!query.validate(user.value, "string")) {
        if (query.validate(user.value, "numbers")) {
            query.toast("Advertencia", "La información del usuario seleccionado ha sido alterada.", "warning");
            return;
        }
    }

    if (query.validate(user.value, "int")) {
        query.toast("Campos vacíos", "Seleccione un usuario.", "warning");
        return;
    }

    if (!query.validate(permission.value, "string")) {
        if (query.validate(permission.value, "numbers")) {
            query.toast("Advertencia", "La información de los permisos ha sido alterada.", "warning");
            return;
        }
    }

    if (query.validate(permission.value, "int")) {
        query.toast("Campos vacíos", "Seleccione un permiso.", "warning");
        return;
    }
    formData = new FormData(this);
    query.ajax({
        url: "Permissions",
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: formData,
        success: function(data) {
            let obj = JSON.parse(data);
            if (obj.action == "save" && obj.action != null && obj.action != "") {
                if (obj.permission != null) {
                    query.toast("Registro de permissos", obj.message, obj.icon);
                    permissionsList(obj.permission.idusuario);
                    userPermissionsList(obj.permission.idusuario);
                } else {
                    query.toast("Registro de permissos", obj.message, obj.icon);
                }
            } else {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        },
        fail: function() {
            query.toast("Error", "La petición solicitada ha fallado.", "error");
        }
    });
}

destination.onclick = function(e) {
    let actual = e.target;
    let Iduser = 0,
        Idpermission = 0;
    if (actual.matches('i[id=delete-permission]') || actual.classList.contains('btn-danger')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Iduser = actual.getAttribute("data-iduser");
        Idpermission = actual.getAttribute("data-idpermission");
        if (!query.validate(Iduser, "int") && !query.validate(Iduser, "numbers") &&
            !query.validate(Idpermission, "int") && !query.validate(Idpermission, "numbers")) {
            $("#modalDeletePermission").modal("show");
            mButtonDelete.onclick = (e) => {
                let token = query.names("__RequestVerificationToken");
                query.post({
                    url: "Permissions?handler=Delete",
                    token: token[0].value,
                    content: "application/x-www-form-urlencoded",
                    data: { Iduser: parseInt(Iduser), Idpermission: parseInt(Idpermission) },
                    success: function(data) {
                        let { action, permission, message, icon } = JSON.parse(data);
                        if (action == "delete" && action != null && action != "") {
                            if (permission != null) {
                                query.toast("Eliminación de permiso", message, icon);
                                permissionsList(permission.idusuario);
                                userPermissionsList(permission.idusuario);
                            } else {
                                query.toast("Registro de permissos", message, icon);
                            }
                        } else {
                            query.toast("Error", "La petición solicitada ha fallado.", "error");
                        }
                    },
                    fail: function() {
                        query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
                    }
                });
            }
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    }
}