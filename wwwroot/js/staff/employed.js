//variables
let employedContainer = query.id("data-list"),
    modalBody = query.id("data-body"),
    buttonAdd = query.id("addEmployed"),
    tittleModal = query.id("modal-title-employed"),
    buttonAction = query.id("btn-action"),
    formEmployed = query.id("formEmployed"),
    userBody = query.id("user-data"),
    formUser = query.id("formUser"),
    userTitle = query.id("user-title"),
    userButton = query.id("user-button"),
    userInformation = query.id("userinformation"),
    mButtonDelete = query.id("mbtn-delete"),
    mTitleDelete = query.id("mtitle-delete"),
    mTitle = query.id("mtitle"),
    idUserEmployed = 0;

//para la creación de la paginación de empleados
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//llamado de funciones
employedList();
//funciones
function employedList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'EmployedAjax',
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
    employedList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    employedList();
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
    employedList();
}
///////end pagination

buttonAdd.onclick = (e) => {
    tittleModal.innerHTML = "Nuevo empleado";
    buttonIcon(tittleModal, "employed");
    partialView({ Id: 0 }, "PartialView", "#modal-employed", modalBody);
}

function buttonIcon(tittle, action) {
    if (action == "employed") {
        if (tittle.textContent == "Nuevo empleado") {
            if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
                buttonAction.childNodes[0].classList.remove("mdi-border-color");
            }
            buttonAction.childNodes[0].classList.add("mdi-content-save");
            buttonAction.childNodes[1].textContent = " Agregar";
        } else if (tittle.textContent == "Modificar empleado") {
            if (buttonAction.childNodes[0].classList.contains("mdi-content-save")) {
                buttonAction.childNodes[0].classList.remove("mdi-content-save");
            }
            buttonAction.childNodes[0].classList.add("mdi-border-color");
            buttonAction.childNodes[1].textContent = " Modificar";
        }
    } else if (action == "user") {
        if (tittle.textContent == "Agregar usuario") {
            if (userButton.childNodes[0].classList.contains("mdi-account-edit")) {
                userButton.childNodes[0].classList.remove("mdi-account-edit");
            }
            userButton.childNodes[0].classList.add("mdi-account-plus");
            userButton.childNodes[1].textContent = " Agregar";
        } else if (tittle.textContent == "Modificar usuario") {
            if (userButton.childNodes[0].classList.contains("mdi-account-plus")) {
                userButton.childNodes[0].classList.remove("mdi-account-plus");
            }
            userButton.childNodes[0].classList.add("mdi-account-edit");
            userButton.childNodes[1].textContent = " Modificar";
        }
    }
}

function partialView(data, Handler, ModalName, ModalBody) {
    token = query.names("__RequestVerificationToken")
    query.ajax({
        url: "Employed?handler=" + Handler,
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: data,
        success: function (data) {
            ModalBody.innerHTML = data;
            query.mask();
            $(ModalName).modal("show");
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

employedContainer.onclick = function (e) {
    let current = e.target;
    IdEmployed = 0;
    if (current.matches("i[class='mdi mdi-delete']") || current.classList.contains("btn-danger")) {
        if (current.classList.contains("btn-danger")) {
            IdEmployed = current.getAttribute("data-idemployed");
        } else {
            IdEmployed = current.parentNode.getAttribute("data-idemployed");
        }
        if (!query.validate(IdEmployed, "int") && !query.validate(IdEmployed, "numbers")) {
            mTitle.innerHTML = "Eliminar empleado";
            $("#modalDeleteEmployed").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ Id: parseInt(IdEmployed) }, "Delete", "Eliminar empleado", "Employed");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-pencil']") || current.classList.contains("btn-warning")) {
        if (current.classList.contains("btn-warning")) {
            IdEmployed = current.getAttribute("data-idemployed");
        } else {
            IdEmployed = current.parentNode.getAttribute("data-idemployed");
        }
        if (!query.validate(IdEmployed, "int") && !query.validate(IdEmployed, "numbers")) {
            partialView({ Id: parseInt(IdEmployed) }, "PartialView", "#modal-employed", modalBody);
            tittleModal.innerHTML = "Modificar empleado";
            buttonIcon(tittleModal, "employed");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-account-plus']") || current.classList.contains("btn-primary")) {
        if (current.classList.contains("btn-primary")) {
            IdEmployed = current.getAttribute("data-idemployed");
        } else {
            IdEmployed = current.parentNode.getAttribute("data-idemployed");
        }
        if (!query.validate(IdEmployed, "int") && !query.validate(IdEmployed, "numbers")) {
            idUserEmployed = parseInt(IdEmployed);
            userTitle.innerHTML = "Agregar usuario";
            buttonIcon(userTitle, "user");
            partialView({ Id: 0 }, "UserPartialView", "#modal-user", userBody);
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-account-edit']") || current.classList.contains("btn-secondary")) {
        if (current.classList.contains("btn-secondary")) {
            IdEmployed = current.getAttribute("data-idemployed");
        } else {
            IdEmployed = current.parentNode.getAttribute("data-idemployed");
        }
        if (!query.validate(IdEmployed, "int") && !query.validate(IdEmployed, "numbers")) {
            idUserEmployed = parseInt(IdEmployed);
            userTitle.innerHTML = "Modificar usuario";
            buttonIcon(userTitle, "user");
            partialView({ Id: parseInt(IdEmployed) }, "UserPartialView", "#modal-user", userBody);
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-eye']") || current.classList.contains("btn-info")) {
        if (current.classList.contains("btn-info")) {
            IdEmployed = current.getAttribute("data-idemployed");
        } else {
            IdEmployed = current.parentNode.getAttribute("data-idemployed");
        }
        if (!query.validate(IdEmployed, "int") && !query.validate(IdEmployed, "numbers")) {
            partialView({ Id: parseInt(IdEmployed) }, "UserInformationPartialView", "#modal-informationuser", userInformation);
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Employed?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let { message, icon } = JSON.parse(data);
            query.toast(Tittle, message, icon);
            if (Option === "Employed") {
                employedList();
            } else if ("DeleteUser") {
                employedList();
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

/**
 * Formulario para registar el empleado
 */

formEmployed.onsubmit = function (e) {
    e.preventDefault();
    let Employed_Id = 0,
        Person_Id = 0;
    let token = query.names("__RequestVerificationToken");
    let formData;
    let url = "Employed";
    if (!query.isUndefinedNull(query.id("Empleado_Id")) && !query.isUndefinedNull(query.id("Persona_Id"))) {
        Employed_Id = query.id("Empleado_Id").value;
        Person_Id = query.id("Persona_Id").value;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //validación del nombre del empleado
    if (query.validate(query.id("Persona_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del empleado.", "warning");
        return;
    }

    if (query.validate(query.id("Persona_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado del empleado es inválido.", "warning");
        return;
    }
    //validacion del apellido del empleado
    if (query.validate(query.id("Persona_Apellido").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el apellido del empleado.", "warning");
        return;
    }

    if (query.validate(query.id("Persona_Apellido").value, "letters")) {
        query.toast("Apellido invalido", "El apellido ingresado del empleado es inválido.", "warning");
        return;
    }
    //validación del DUI
    if (query.validate(query.id("Persona_Dui").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el DUI del empleado.", "warning");
        return;
    }
    if (query.validate(query.id("Persona_Dui").value, "DUI")) {
        query.toast("DUI invalido", "El número de DUI ingresado es incorrecto.", "warning");
        return;
    }
    //Validación del sexo del empleado
    if (query.validate(query.id("Empleado_Sexo").value, "string")) {
        query.toast("Campos vacíos", "Seleccione el genéro del empleado.", "warning");
        return;
    }

    if (query.validate(query.id("Empleado_Sexo").value, "letters")) {
        query.toast("Genéro invalido", "La información del servidor ha sido alterada.", "warning");
        return;
    }
    //validación de la fecha de nacimiento (opcional)
    if (!query.validate(query.id("Empleado_Fechanacimiento").value, "string")) {
        if (query.validate(query.id("Empleado_Fechanacimiento").value, "date")) {
            query.toast("Fecha inválida", "La fecha ingresada o seleccionada es incorrecta.", "warning");
            return;
        }
    }
    //validación del # de teléfono (opcional)
    if (!query.validate(query.id("Telefono_Telefono1").value, "string")) {
        if (query.validate(query.id("Telefono_Telefono1").value, "phone")) {
            query.toast("Contacto invalido", "El número de contacto ingresado es incorrecto.", "warning");
            return;
        }
    }
    //validación del cargo (opcional)
    if (!query.validate(query.id("Empleado_Idcargo").value, "string")) {
        if (query.validate(query.id("Empleado_Idcargo").value, "numbers")) {
            query.toast("Advertencia", "Los datos del cargo seleccionado han sido alterados.", "warning");
            return;
        }

        if (query.validate(query.id("Empleado_Idcargo").value, "int")) {
            query.toast("Campos vacíos", "Seleccione un cargo, para el empleado.", "warning");
            return;
        }
    }
    if (!query.validate(Employed_Id, "int") && !query.validate(Employed_Id, "numbers") &&
        !query.validate(Person_Id, "int") && !query.validate(Person_Id, "numbers")) {
        url += "?handler=Edit";
        formData = new FormData(this);
    } else {
        url = url;
        formData = new FormData(this);
    }

    if (!query.validate(url, "string") && !query.isUndefinedNull(Employed_Id) &&
        !query.isUndefinedNull(Person_Id)) {
        formData.append("Persona.Tipo", "Empleado");
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, employed, message, icon } = JSON.parse(data);
                if ((action == "add" || action == "edit") && (action != null && action != "")) {
                    if (employed != null) {
                        query.toast("Registro de empleado", message, icon);
                        employedList();
                        $("#modal-employed").modal("hide");
                    } else {
                        query.toast("Registro de empleado", message, icon);
                    }
                } else {
                    query.toast("Registro de empleado", message, icon);
                }

            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de empleado", "Los datos del servidor han sido alterados.", "error");
    }
}

userBody.onclick = function (e) {
    let element = e.target;
    if (element.matches("input[type=file]")) {
        let imagenNuevaProducto = query.q("#Usuario_Archivo");
        imagenNuevaProducto.onchange = function () {
            readURL(this);

            function readURL(input) {
                if (input.files && input.files[0]) {
                    let reader = new FileReader();
                    reader.onload = function (e) {
                        let imagen = query.q("#imageview");
                        imagen.src = e.target.result;
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
        }
    }
}

userInformation.onclick = function (e) {
    let element = e.target;
    let Id = 0,
        Status = 0;
    if (element.matches("input[type=checkbox]")) {
        if (element.tagName == "INPUT") {
            Id = element.getAttribute("data-iduser");
            if (element.checked) {
                Status = 1;
            } else {
                Status = 0;
            }
        }
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            options({ Id: parseInt(Id), Status: Status }, "UserModifiedStatus", "Modificación de estado", "Status");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (element.matches("button[type=button]")) {
        if (element.classList.contains("btn-danger")) {
            Id = element.getAttribute("data-iduser");
        }
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            mTitle.innerHTML = "Eliminar usuario";
            $("#modalDeleteEmployed").modal("show");
            mButtonDelete.onclick = (e) => {
                $("#modal-informationuser").modal("hide");
                options({ Id: parseInt(Id) }, "DeleteUser", "Eliminar usuario", "DeleteUser");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

formUser.onsubmit = function (e) {
    e.preventDefault();
    let User_Id = 0;
    let token = query.names("__RequestVerificationToken");
    let formData;
    let url = "Employed";
    if (!query.isUndefinedNull(query.id("Usuario_Id"))) {
        User_Id = query.id("Usuario_Id").value;
    }
    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //validación del nombre del usuario
    if (query.validate(query.id("Usuario_Usuario1").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del usuario.", "warning");
        return;
    }

    //validación de la contraseña
    if (!query.validate(User_Id, "int") && !query.validate(User_Id, "numbers")) {
        if (!query.validate(query.id("Usuario_Contrasena").value, "string")) {
            let password = query.id("Usuario_Contrasena").value;
            if (password.length < 8) {
                query.toast("Contraseña no segura", "La contraseña debe contener como minímo 8 carácteres.", "warning");
                return;
            }
        }
    } else {
        if (query.validate(query.id("Usuario_Contrasena").value, "string")) {
            query.toast("Campos vacíos", "Ingrese la contraseña del usuario.", "warning");
            return;
        }
        let password = query.id("Usuario_Contrasena").value;
        if (password.length < 8) {
            query.toast("Contraseña no segura", "La contraseña debe contener como minímo 8 carácteres.", "warning");
            return;
        }
    }
    //validación del correo
    if (query.validate(query.id("Usuario_Correo").value, "string")) {
        query.toast("Campos vacíos", "Ingrese un correo electrónico para el usuario.", "warning");
        return;
    }

    if (!query.validate(query.id("Usuario_Correo").value, "string")) {
        if (query.validate(query.id("Usuario_Correo").value, "email")) {
            query.toast("Correo inválido", "El correo ingresado es incorrecto.", "warning");
            return;
        }
    }
    //validación del rol
    if (query.validate(query.id("Usuario_Rol").value, "string")) {
        query.toast("Campos vacíos", "Seleccione el rol para el usuario.", "warning");
        return;
    }
    if (query.validate(query.id("Usuario_Rol").value, "letters")) {
        query.toast("Rol inválido", "La información del rol seleccionado ha sido alterada.", "warning");
        return;
    }
    /*if (query.id("Usuario_Rol").value != "Administrador" ||
        query.id("Usuario_Rol").value != "Empleado") {
        query.toast("Rol inválido", "La información del rol seleccionado ha sido alterada.", "warning");
        return;
    }*/

    if (!query.validate(User_Id, "int") && !query.validate(User_Id, "numbers") && !query.isUndefinedNull(User_Id)) {
        url += "?handler=EditUser";
        formData = new FormData(this);
    } else {
        url += "?handler=AddUser";
        formData = new FormData(this);
    }

    if (!query.validate(url, "string") && !query.isUndefinedNull(User_Id)) {
        formData.append("Usuario.Idempleado", idUserEmployed);
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, user, message, icon } = JSON.parse(data);
                if ((action == "add" || action == "edit") && (action != null && action != "")) {
                    if (user != null) {
                        query.toast("Registro de usuario", message, icon);
                        $("#modal-user").modal("hide");
                        cleanData();
                        employedList();
                    } else {
                        query.toast("Registro de usuario", message, icon);
                    }
                } else {
                    query.toast("Registro de usuario", message, icon);
                }

            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de usuario", "Los datos del servidor han sido alterados.", "error");
    }
}

//clonando la información del usuario

function cleanData() {
    idUserEmployed = 0;
    userTitle.innerHTML = "Agregar usuario";
}