//variables
let buttonModalProvider = query.id("btn-addprovider"),
    buttonAction = query.id('btn-action'),
    buttonCancel = query.id('btn-cancel'),
    modalTitle = query.id("tittle-modal"),
    modalBody = query.id("data-body"),
    formProviders = query.id("formProvider"),
    providerBody = query.id("provi-body"),
    buttonCancelPhone = query.id("btn-cancelphone"),
    formPhone = query.id("formPhone"),
    phoneFormButton = query.id("btn-phone"),
    phoneModalTitle = query.id("phone-title"),
    token,
    Provider_Id = 0,
    Phone_Id = 0,
    fbody = query.id("phone-data"),
    mButtonDelete = query.id("mbtn-delete"),
    mTitleDelete = query.id("mtitle-delete"),
    mTitle = query.id("mtitle");

//para la paginación de proveedores
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//para la creación de la paginación de telefonos

let phonePagination = query.id('phone-pagination'),
    showPhoneQuantity = query.id('show-phone-quantity'),
    searchPhone = query.id('phone-searchdata');
let searchPhon = "",
    quantityPhone = 3,
    currentPhonePage = 1;

providersList();

function providersList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: "ProvidersAjax",
        method: 'get',
        data: data,
        success: function (data) {
            providerBody.innerHTML = data;
            pagination.innerHTML = query.createPagination(providerBody, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "La información de los proveedores no se puede visualizar.", "error");
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
    providersList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    providersList();
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Provider");
}


//método de los filtros telefonos
searchPhone.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        searchPhon = current.value;
    } else {
        searchPhon = "";
    }
    quantityPhone = parseInt(showPhoneQuantity.value);
    currentPhonePage = 1;
    //llamando el listado de telefonos
    getPhoneList("PhoneList", Id = parseInt(Provider_Id));
}

showPhoneQuantity.onchange = function (e) {
    let current = e.target;
    quantityPhone = parseInt(current.value);
    searchPhon = searchPhone.value = "";
    currentPhonePage = 1;
    //llamando el listado de telefonos
    getPhoneList("PhoneList", Id = parseInt(Provider_Id));
}

phonePagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPhonePage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Phone");
}


function refresh(option) {
    if (option === "Provider") {
        quantity = parseInt(showQuantity.value);
        if (searchData.value !== "") {
            search = searchData.value;
        } else {
            search = "";
        }
        currentPage = currentPage;
        providersList();
    } else {
        quantityPhone = parseInt(showPhoneQuantity.value);
        if (searchPhone.value !== "") {
            searchPhon = searchPhone.value;
        } else {
            searchPhon = "";
        }
        currentPhonePage = currentPhonePage;
        getPhoneList("PhoneList", Id = parseInt(Provider_Id));
    }
}

buttonModalProvider.onclick = (e) => {
    modalTitle.innerHTML = "Nuevo proveedor";
    partialView({ Id: 0 }, "PartialView", modalBody);
    buttonIcon(modalTitle);
    $("#modal-provider").modal("show");
}

buttonCancel.onclick = (e) => {
    $("#modal-provider").modal("hide");
}

buttonCancelPhone.onclick = (e) => {
    clearPhone();
}

function buttonIcon(tittle, action) {
    if (tittle.textContent == "Nuevo proveedor") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar proveedor") {
        if (buttonAction.childNodes[0].classList.contains("mdi-content-save")) {
            buttonAction.childNodes[0].classList.remove("mdi-content-save");
        }
        buttonAction.childNodes[0].classList.add("mdi-border-color");
        buttonAction.childNodes[1].textContent = " Modificar";
    } else {
        if (action == "Edit") {
            if (phoneFormButton.childNodes[0].classList.contains("mdi-content-save")) {
                phoneFormButton.childNodes[0].classList.remove("mdi-content-save");
                phoneFormButton.childNodes[0].classList.add("mdi-border-color");
                phoneFormButton.childNodes[1].textContent = " Modificar";
            }
        } else {
            if (phoneFormButton.childNodes[0].classList.contains("mdi-border-color")) {
                phoneFormButton.childNodes[0].classList.remove("mdi-border-color");
                phoneFormButton.childNodes[0].classList.add("mdi-content-save");
                phoneFormButton.childNodes[1].textContent = " Guardar";
            }
        }
    }
}

function partialView(data, Handler, ModalBody) {
    token = query.names("__RequestVerificationToken")
    query.ajax({
        url: "Providers?handler=" + Handler,
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: data,
        success: function (data) {
            ModalBody.innerHTML = data;
            query.mask();
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

formProviders.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    token = query.names("__RequestVerificationToken");
    let Person_Id = 0,
        Provider_Id = 0;
    let url = "Providers";
    if (!query.isUndefinedNull(query.id("Person_Id")) && !query.isUndefinedNull(query.id("Provider_Id"))) {
        Person_Id = query.id("Person_Id").value;
        Provider_Id = query.id("Provider_Id").value;
    }

    if (!query.isUndefinedNull(query.id("Person_Id")) && query.isUndefinedNull(query.id("Provider_Id"))) {
        Person_Id = query.id("Person_Id").value;
        Provider_Id = 0;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //nombre
    if (query.validate(query.id("Person_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del responsable.", "warning");
        return;
    }

    if (query.validate(query.id("Person_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }
    //apellido
    if (query.validate(query.id("Person_Apellido").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el apellido del responsable.", "warning");
        return;
    }

    if (query.validate(query.id("Person_Apellido").value, "letters")) {
        query.toast("Apellido invalido", "El apellido ingresado es inválido.", "warning");
        return;
    }
    //dui
    if (query.validate(query.id("Person_Dui").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el DUI del responsable.", "warning");
        return;
    }
    if (query.validate(query.id("Person_Dui").value, "DUI")) {
        query.toast("DUI invalido", "El número de DUI ingresado es incorrecto.", "warning");
        return;
    }
    //nombre del proveedor
    if (query.id("Provider_Nombre") != null) {
        if (query.validate(query.id("Provider_Nombre").value, "string")) {
            query.toast("Campos vacíos", "Ingrese el nombre del proveedor.", "warning");
            return;
        }
    }

    if (query.validate(query.id("Provider_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }

    //dirección del proveedor
    if (query.id("Provider_Direccion") != null) {
        if (query.validate(query.id("Provider_Direccion").value, "string")) {
            query.toast("Campos vacíos", "Ingrese la dirección del proveedor.", "warning");
            return;
        }
    }

    if (!query.validate(Person_Id, "int") && !query.validate(Provider_Id, "int") &&
        !query.validate(Person_Id, "numbers") && !query.validate(Provider_Id, "numbers")) {
        formData = new FormData(this);
        url += "?handler=Edit";
    } else {
        formData = new FormData(this);
        url = url;
    }

    if (!query.validate(url, "string") && (!query.isUndefinedNull(Person_Id) || !query.isUndefinedNull(Provider_Id))) {
        formData.append("Person.Tipo", "Proveedor");
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, provi, provider, message, icon } = JSON.parse(data);
                if (action == "save" && action != null && action != "") {
                    if (provi != null) {
                        query.toast("Registro de proveedor", message, icon);
                        if (provider != null) {
                            reboot("Provider");
                            providersList();
                        }
                        $("#modal-provider").modal("hide");
                    } else {
                        query.toast("Registro de proveedor", message, icon);
                    }
                } else if (action == "edit" && action != null && action != "") {
                    if (provi != null) {
                        query.toast("Modificación de proveedor", message, icon);
                        if (provider != null) {
                            reboot("Provider");
                            providersList();
                        }
                        $("#modal-provider").modal("hide");
                    } else {
                        query.toast("Modificación de proveedor", message, icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de proveedor", "Los datos del servidor han sido alterados.", "error");
    }
}

//metodo para eliminar y editar un proveedor
providerBody.onclick = function (e) {
    let current = e.target;
    IdPerson = 0;
    if (current.matches("i[class='mdi mdi-delete']") || current.classList.contains("btn-danger")) {
        if (current.classList.contains("btn-danger")) {
            IdPerson = current.getAttribute("data-idperson");
        } else {
            IdPerson = current.parentNode.getAttribute("data-idperson");
        }
        if (!query.validate(IdPerson, "int") && !query.validate(IdPerson, "numbers")) {
            mTitle.innerHTML = "Eliminar proveedor";
            $("#modalDeleteProvider").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ IdPerson: parseInt(IdPerson), Option: "Provi" }, "Delete", "Eliminar proveedor", "Provider");
            }

        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-pencil']") || current.classList.contains("btn-warning")) {
        if (current.classList.contains("btn-warning")) {
            IdPerson = current.getAttribute("data-idperson");
        } else {
            IdPerson = current.parentNode.getAttribute("data-idperson");
        }
        if (!query.validate(IdPerson, "int") && !query.validate(IdPerson, "numbers")) {
            partialView({ Id: parseInt(IdPerson), Option: "Provi" }, "PartialView", modalBody);
            modalTitle.innerHTML = "Modificar proveedor";
            buttonIcon(modalTitle, "");
            $("#modal-provider").modal("show");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-plus']") || current.classList.contains("btn-info")) {
        if (current.classList.contains("btn-info")) {
            Provider_Id = current.getAttribute("data-idprovider");
        } else {
            Provider_Id = current.parentNode.getAttribute("data-idprovider");
        }
        if (!query.validate(Provider_Id, "int") && !query.validate(Provider_Id, "numbers")) {
            reboot("Phone");
            getPhoneList("PhoneList", Id = parseInt(Provider_Id));
            $('#modal-phone').modal('show');
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Providers?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let { message, icon } = JSON.parse(data);
            query.toast(Tittle, message, icon);
            if (Option == "Provider") {
                reboot("Provider");
                providersList();
            }
            if (Option == "Phone") {
                reboot("Phone");
                getPhoneList("PhoneList", Id = parseInt(Provider_Id));
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

function getPhoneList(Handler, Id) {
    let data = {
        handler: Handler,
        Id: Id,
        search: searchPhon,
        quantity: quantityPhone,
        currentPage: currentPhonePage
    }
    query.get({
        url: "Providers",
        data: data,
        success: function (data) {
            fbody.innerHTML = data;
            phonePagination.innerHTML = query.createPagination(fbody, "#phone-filters");
        },
        fail: function () {
            query.toast("Error", "Los números de contacto no se pueden visualizar.", "error");
        }
    });
}

formPhone.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    let url = "Providers?handler=";
    token = query.names("__RequestVerificationToken");
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    if (query.validate(query.id("Phone_Telefono1").value, "phone")) {
        query.toast("Contacto invalido", "El número de contacto ingresado es incorrecto.", "warning");
        return;
    }

    if (!query.validate(Phone_Id.toString(), "numbers") && !query.validate(Phone_Id.toString(), "int") &&
        !query.isUndefinedNull(Phone_Id)) {
        url += "EditPhone";
        formData = new FormData(this);
        formData.append("Phone.Id", Phone_Id);
    } else if (Phone_Id == 0 && !query.validate(Phone_Id.toString(), "numbers") && !query.isUndefinedNull(Phone_Id)) {
        url += "Phone";
        formData = new FormData(this);
    } else {
        query.toast("Registro de contacto", "Los datos del servidor han sido alterados.", "error");
        return;
    }

    if (!query.isUndefinedNull(Phone_Id) && Phone_Id > -1 && !isNaN(Phone_Id) &&
        !query.isUndefinedNull(Provider_Id) && !query.validate(Provider_Id.toString(), "int") &&
        !query.validate(Provider_Id.toString(), "numbers")) {
        formData.append("ProviderPhone.Idproveedor", Provider_Id);
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, phone: { id, idproveedor, idtelefono }, message, icon } = JSON.parse(data);
                if (action == "savephone" && action != null && action != "") {
                    if (!query.isUndefinedNull(id)) {
                        clearPhone();
                        Provider_Id = idproveedor;
                        query.toast("Registro de contacto", message, icon);
                        reboot("Phone");
                        getPhoneList("PhoneList", Id = idproveedor);
                    } else {
                        query.toast("Registro de contacto", message, icon);
                    }
                } else if (action == "editphone" && action != null && action != "") {
                    if (!query.isUndefinedNull(id)) {
                        clearPhone();
                        Provider_Id = idproveedor;
                        query.toast("Modificación de contacto", message, icon);
                        reboot("Phone");
                        getPhoneList("PhoneList", Id = Provider_Id);
                    } else {
                        query.toast("Modificación de contacto", message, icon);
                    }
                }
                buttonIcon(phoneModalTitle, "Save");
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de contacto", "Los datos del servidor han sido alterados.", "error");
    }
}

//evento escucha para eliminar y editar los números del proveedor
fbody.addEventListener("click", (e) => {
    const current = e.target;
    let Id = 0;
    if (current.matches("i[class='mdi mdi-delete mdi-24px']")) {
        Id = current.parentNode.getAttribute("data-idphone");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            mTitle.innerHTML = "Eliminar número de contacto";
            $("#modalDeleteProvider").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ Id: parseInt(Id) }, "DeletePhone", "Eliminar contacto", "Phone");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-pencil mdi-24px']")) {
        Id = current.parentNode.getAttribute("data-idphone");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            Phone_Id = parseInt(Id);
            let value = current.parentNode.parentNode.parentNode.children[1];
            if (!query.isUndefinedNull(query.id("Phone_Telefono1")) && value.nodeName == "TD") {
                buttonIcon(phoneModalTitle, "Edit");
                query.id("Phone_Telefono1").value = value.innerText;
            } else {
                query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("input[type=checkbox]")) {
        let Status = 0;
        Id = current.getAttribute("data-idphone");
        if (current.checked) {
            Status = 1;
        } else {
            Status = 0;
        }
        if (!query.validate(Id, "int") && !isNaN(Status) && Status >= 0 && Status <= 1) {
            options({ Id: parseInt(Id), Status }, "EditStatus", "Estado del contacto", "");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}, true);

function clearPhone() {
    query.resetForm(formPhone);
    Phone_Id = 0;
    buttonIcon(phoneModalTitle, "Save");
}

function reboot(action) {
    if (action === "Provider") {
        showQuantity.selectedIndex = 0;
        searchData.value = "";
        search = "";
        quantity = 6;
        currentPage = 1;
    } else {
        showPhoneQuantity.selectedIndex = 0;
        searchPhone.value = "";
        searchPhon = "";
        quantityPhone = 3;
        currentPhonePage = 1;
    }
}