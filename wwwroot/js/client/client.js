//variables
let buttonModalProduct = query.id("btn-addclient"),
    buttonAction = query.id('btn-action'),
    buttonCancel = query.id('btn-cancel'),
    buttonCancelPhone = query.id("btn-cancelphone"),
    modalTitle = query.id("tittle-modal"),
    modalBody = query.id("data-body"),
    selected = query.id("Person_Tipo"),
    formClients = query.id("formClient"),
    cgbody = query.id("cg-body"),
    cibody = query.id("ci-body"),
    formPhone = query.id("formPhone"),
    phoneFormButton = query.id("btn-phone"),
    phoneModalTitle = query.id("phone-title"),
    token,
    Institution_Id = 0,
    Phone_Id = 0,
    fbody = query.id("phone-data"),
    mButtonDelete = query.id("mbtn-delete"),
    mTitleDelete = query.id("mtitle-delete"),
    mTitle = query.id("mtitle");

//para la creación de la paginación de público en general
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 6,
    currentPage = 1;

//para la creación de la paginación de clientes institucionales
let pricePagination = query.id('institution-pagination'),
    showInstitutionQuantity = query.id('show-institution-quantity'),
    searchInstitution = query.id('institution-searchdata');
let searchInst = "",
    quantityInstitution = 6,
    currentInstitutionPage = 1;

//para la creación de la paginación de telefonos

let phonePagination = query.id('phone-pagination'),
    showPhoneQuantity = query.id('show-phone-quantity'),
    searchPhone = query.id('phone-searchdata');
let searchPhon = "",
    quantityPhone = 3,
    currentPhonePage = 1;

//metodos
clientsList({ Option: "Público" }, cgbody);
clientsList({ Option: "Institución" }, cibody);

function clientsList(data, body) {
    let dataCleint = {}
    if (data.Option === "Público") {
        dataCleint = {
            Option: data.Option,
            search: search,
            quantity: quantity,
            currentPage: currentPage
        }
    } else {
        dataCleint = {
            Option: data.Option,
            search: searchInst,
            quantity: quantityInstitution,
            currentPage: currentInstitutionPage
        }
    }
    query.get({
        url: "ClientsAjax",
        method: 'get',
        data: dataCleint,
        success: function (list) {
            body.innerHTML = list;
            if (data.Option === "Público") {
                pagination.innerHTML = query.createPagination(body, "#filter-results");
            } else {
                pricePagination.innerHTML = query.createPagination(body, "#institution-filters");
            }
        },
        fail: function () {
            query.toast("Error", "La información de los clientes no se puede visualizar.", "error");
        }
    });
}

//método de filtros público
searchData.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    clientsList({ Option: "Público" }, cgbody);
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    clientsList({ Option: "Público" }, cgbody);
}

pagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Public");
}

//método de los filtros institución
searchInstitution.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        searchInst = current.value;
    } else {
        searchInst = "";
    }
    quantityInstitution = parseInt(showInstitutionQuantity.value);
    currentInstitutionPage = 1;
    //llamando el listado de precios
    clientsList({ Option: "Institución" }, cibody);
}

showInstitutionQuantity.onchange = function (e) {
    let current = e.target;
    quantityInstitution = parseInt(current.value);
    searchInst = searchInstitution.value = "";
    currentInstitutionPage = 1;
    clientsList({ Option: "Institución" }, cibody);
}

pricePagination.onclick = function (e) {
    let current = e.target;
    if (current.tagName == 'UL' || current.tagName == 'NAV') {
        return;
    }
    let child = current.firstElementChild;
    currentInstitutionPage = current.getAttribute('data-page') === null ? child.getAttribute('data-page') : current.getAttribute('data-page');
    refresh("Institution");
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
    getPhoneList("PhoneList", Id = parseInt(Institution_Id));
}

showPhoneQuantity.onchange = function (e) {
    let current = e.target;
    quantityPhone = parseInt(current.value);
    searchPhon = searchPhone.value = "";
    currentPhonePage = 1;
    //llamando el listado de telefonos
    getPhoneList("PhoneList", Id = parseInt(Institution_Id));
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
function refresh(action) {
    if (action === "Public") {
        quantity = parseInt(showQuantity.value);
        if (searchData.value !== "") {
            search = searchData.value;
        } else {
            search = "";
        }
        currentPage = currentPage;
        clientsList({ Option: "Público" }, cgbody);
    } else if (action === "Institution") {
        quantityInstitution = parseInt(showInstitutionQuantity.value);
        if (searchInstitution.value !== "") {
            searchInst = searchInstitution.value;
        } else {
            searchInst = "";
        }
        currentInstitutionPage = currentInstitutionPage;
        clientsList({ Option: "Institución" }, cibody);
    } else {
        quantityPhone = parseInt(showPhoneQuantity.value);
        if (searchPhone.value !== "") {
            searchPhon = searchPhone.value;
        } else {
            searchPhon = "";
        }
        currentPhonePage = currentPhonePage;
        getPhoneList("PhoneList", Id = parseInt(Institution_Id));
    }
}

buttonModalProduct.onclick = (e) => {
    modalTitle.innerHTML = "Nuevo cliente";
    selected.parentNode.parentNode.parentNode.style.display = "block";
    clear();
    buttonIcon(modalTitle);
    $("#modal-client").modal("show");
}
buttonCancel.onclick = (e) => {
    clear();
    $("#modal-client").modal("hide");
}

buttonCancelPhone.onclick = (e) => {
    clearPhone();
}

selected.onchange = (e) => {
    let current = e.currentTarget.value;
    let Id = 0,
        Option = "Public";
    if (current == "Público") {
        Option = "Public";
        partialView({ Id, Option }, "PartialView", modalBody);
    } else if (current == "Institución") {
        Option = "Client";
        partialView({ Id, Option }, "PartialView", modalBody);
    } else if (current == "" || current == " ") {
        modalBody.innerHTML = "";
    } else {
        query.toast("Error", "La información del servidor ha sido alterada.", "error");
    }
}

function buttonIcon(tittle, action) {
    if (tittle.textContent == "Nuevo cliente") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar cliente") {
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
        url: "Clients?handler=" + Handler,
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

formClients.onsubmit = function (e) {
    e.preventDefault();
    let formData, select = selected.value;
    token = query.names("__RequestVerificationToken");
    let Person_Id = 0,
        Institution_Id = 0;
    let url = "Clients";
    if (!query.isUndefinedNull(query.id("Person_Id")) && !query.isUndefinedNull(query.id("Institution_Id"))) {
        Person_Id = query.id("Person_Id").value;
        Institution_Id = query.id("Institution_Id").value;
    }

    if (!query.isUndefinedNull(query.id("Person_Id")) && query.isUndefinedNull(query.id("Institution_Id"))) {
        Person_Id = query.id("Person_Id").value;
        Institution_Id = 0;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //validación de cada campo
    if (query.validate(query.id("Person_Tipo").value, "string")) {
        query.toast("Seleccionar tipo de registro", "Seleccione el tipo de cliente a registrar.", "warning");
        return;
    }

    //nombre
    if (query.validate(query.id("Person_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del cliente.", "warning");
        return;
    }

    if (query.validate(query.id("Person_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }
    //apellido
    if (query.validate(query.id("Person_Apellido").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el apellido del cliente.", "warning");
        return;
    }

    if (query.validate(query.id("Person_Apellido").value, "letters")) {
        query.toast("Apellido invalido", "El apellido ingresado es inválido.", "warning");
        return;
    }
    //dui
    if (query.validate(query.id("Person_Dui").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el DUI del cliente.", "warning");
        return;
    }
    if (query.validate(query.id("Person_Dui").value, "DUI")) {
        query.toast("DUI invalido", "El número de DUI ingresado es incorrecto.", "warning");
        return;
    }
    //nombre de la institución
    if (query.id("Institution_Nombre") != null) {
        if (query.validate(query.id("Institution_Nombre").value, "string")) {
            query.toast("Campos vacíos", "Ingrese el nombre de la institución.", "warning");
            return;
        }
    }
    //dirección de la institución
    if (query.id("Institution_Direccion") != null) {
        if (query.validate(query.id("Institution_Direccion").value, "string")) {
            query.toast("Campos vacíos", "Ingrese la dirección de la institución.", "warning");
            return;
        }
    }
    //validación del tipo de cliente
    if (select === "Público") {
        if (!query.validate(Person_Id, "int") && !query.validate(Person_Id, "numbers")) {
            formData = new FormData(this);
            url += "?handler=Edit";
        } else {
            formData = new FormData(this);
            url = url;
        }
    } else if (select === "Institución") {
        if (!query.validate(Person_Id, "int") && !query.validate(Institution_Id, "int") &&
            !query.validate(Person_Id, "numbers") && !query.validate(Institution_Id, "numbers")) {
            formData = new FormData(this);
            url += "?handler=Edit";
        } else {
            formData = new FormData(this);
            url = url;
        }
    } else {
        query.toast("Seleccionar tipo de registro", "Seleccione el tipo de cliente a registrar.", "warning");
        return;
    }
    if (!query.validate(url, "string") && (!query.isUndefinedNull(Person_Id) || !query.isUndefinedNull(Institution_Id))) {
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, client, institution, message, icon } = JSON.parse(data);
                if (action == "save" && action != null && action != "") {
                    if (client != null) {
                        query.toast("Registro de cliente", message, icon);
                        if (institution != null) {
                            reboot("Institution");
                            clientsList({ Option: "Institución" }, cibody);
                        } else {
                            reboot("Public");
                            clientsList({ Option: "Público" }, cgbody);
                        }
                        clear();
                        $("#modal-client").modal("hide");
                    } else {
                        query.toast("Registro de cliente", message, icon);
                    }
                } else if (action == "edit" && action != null && action != "") {
                    if (client != null) {
                        query.toast("Modificación de cliente", message, icon);
                        if (institution != null) {
                            reboot("Institution");
                            clientsList({ Option: "Institución" }, cibody);
                        } else {
                            reboot("Public");
                            clientsList({ Option: "Público" }, cgbody);
                        }
                        clear();
                        $("#modal-client").modal("hide");
                    } else {
                        query.toast("Modificación de cliente", message, icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de cliente", "Los datos del servidor han sido alterados.", "error");
    }
}

//metodo para eliminar y editar un cliente normal
cgbody.onclick = function (e) {
    let current = e.target;
    IdPerson = 0;
    if (current.matches("i[class='mdi mdi-delete']") || current.classList.contains("btn-danger")) {
        if (current.classList.contains("btn-danger")) {
            IdPerson = current.getAttribute("data-idclient");
        } else {
            IdPerson = current.parentNode.getAttribute("data-idclient");
        }
        if (!query.validate(IdPerson, "int") && !query.validate(IdPerson, "numbers")) {
            mTitle.innerHTML = "Eliminar cliente general";
            $("#modalDeleteClient").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ IdPerson: parseInt(IdPerson), Option: "Public" }, "Delete", "Eliminar cliente", "Public");
            }
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-pencil']") || current.classList.contains("btn-warning")) {
        if (current.classList.contains("btn-warning")) {
            IdPerson = current.getAttribute("data-idclient");
        } else {
            IdPerson = current.parentNode.getAttribute("data-idclient");
        }
        if (!query.validate(IdPerson, "int") && !query.validate(IdPerson, "numbers")) {
            selected.selectedIndex = 1;
            selected.parentNode.parentNode.parentNode.style.display = "none";
            partialView({ Id: parseInt(IdPerson), Option: "Public" }, "PartialView", modalBody);
            modalTitle.innerHTML = "Modificar cliente";
            buttonIcon(modalTitle, "");
            $("#modal-client").modal("show");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

//metodo para eliminar y editar un cliente institucional
cibody.onclick = function (e) {
    let current = e.target;
    IdPerson = 0;
    if (current.matches("i[class='mdi mdi-delete']") || current.classList.contains("btn-danger")) {
        if (current.classList.contains("btn-danger")) {
            IdPerson = current.getAttribute("data-idperson");
        } else {
            IdPerson = current.parentNode.getAttribute("data-idperson");
        }
        if (!query.validate(IdPerson, "int") && !query.validate(IdPerson, "numbers")) {
            mTitle.innerHTML = "Eliminar cliente institucional";
            $("#modalDeleteClient").modal("show");
            mButtonDelete.onclick = (e) => {
                options({ IdPerson: parseInt(IdPerson), Option: "Client" }, "Delete", "Eliminar cliente", "Institution");
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
            selected.selectedIndex = 2;
            selected.parentNode.parentNode.parentNode.style.display = "none";
            partialView({ Id: parseInt(IdPerson), Option: "Client" }, "PartialView", modalBody);
            modalTitle.innerHTML = "Modificar cliente";
            buttonIcon(modalTitle, "");
            $("#modal-client").modal("show");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    } else if (current.matches("i[class='mdi mdi-plus']") || current.classList.contains("btn-info")) {
        if (current.classList.contains("btn-info")) {
            Institution_Id = current.getAttribute("data-idinstitution");
        } else {
            Institution_Id = current.parentNode.getAttribute("data-idinstitution");
        }
        if (!query.validate(Institution_Id, "int") && !query.validate(Institution_Id, "numbers")) {
            reboot("Phone");
            getPhoneList("PhoneList", Id = parseInt(Institution_Id));
            $('#modal-phone').modal('show');
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Clients?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let { message, icon } = JSON.parse(data);
            query.toast(Tittle, message, icon);
            if (Option == "Public") {
                reboot("Public");
                clientsList({ Option: "Público" }, cgbody);
            } else if (Option == "Institution") {
                reboot("Institution");
                clientsList({ Option: "Institución" }, cibody);
            }
            if (Option == "Phone") {
                reboot("Phone");
                getPhoneList("PhoneList", Id = parseInt(Institution_Id));
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
        url: "Clients",
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
    let url = "Clients?handler=";
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
        !query.isUndefinedNull(Institution_Id) && !query.validate(Institution_Id.toString(), "int") &&
        !query.validate(Institution_Id.toString(), "numbers")) {
        formData.append("InstitutionPhone.Idinstitucion", Institution_Id);
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let { action, phone: { id, idinstitucion, idtelefono }, message, icon } = JSON.parse(data);
                if (action == "savephone" && action != null && action != "") {
                    if (!query.isUndefinedNull(id)) {
                        clearPhone();
                        Institution_Id = idinstitucion;
                        query.toast("Registro de contacto", message, icon);
                        reboot("Phone");
                        getPhoneList("PhoneList", Id = idinstitucion);
                    } else {
                        query.toast("Registro de contacto", message, icon);
                    }
                } else if (action == "editphone" && action != null && action != "") {
                    if (!query.isUndefinedNull(id)) {
                        clearPhone();
                        Institution_Id = idinstitucion;
                        query.toast("Modificación de contacto", message, icon);
                        reboot("Phone");
                        getPhoneList("PhoneList", Id = Institution_Id);
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

//evento escucha para eliminar y editar los números de contacto de la isntitución
fbody.addEventListener("click", (e) => {
    const current = e.target;
    let Id = 0;
    if (current.matches("i[class='mdi mdi-delete mdi-24px']")) {
        Id = current.parentNode.getAttribute("data-idphone");
        if (!query.validate(Id, "int") && !query.validate(Id, "numbers")) {
            mTitle.innerHTML = "Eliminar número de contacto";
            $("#modalDeleteClient").modal("show");
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

function clear() {
    selected.selectedIndex = 0;
    modalBody.innerHTML = "";
}

function clearPhone() {
    query.resetForm(formPhone);
    Phone_Id = 0;
    buttonIcon(phoneModalTitle, "Save");
}

function reboot(action) {
    if (action === "Public") {
        showQuantity.selectedIndex = 0;
        searchData.value = "";
        search = "";
        quantity = 6;
        currentPage = 1;
    } else if (action === "Institution") {
        showInstitutionQuantity.selectedIndex = 0;
        searchInstitution.value = "";
        searchInst = "";
        quantityInstitution = 6;
        currentInstitutionPage = 1;
    } else {
        showPhoneQuantity.selectedIndex = 0;
        searchPhone.value = "";
        searchPhon = "";
        quantityPhone = 3;
        currentPhonePage = 1;
    }
}