let positionsContainer = query.id('positions-list'),
    buttonModalPosition = query.id("btn-modalposition"),
    tittleModal = query.id("tittle-modal"),
    modalBody = query.id("modalBody"),
    formPosition = query.id("formPosition"),
    buttonAction = query.id('btn-action'),
    token,
    modalPositionFooter = query.id("modalposition-footer"),
    mButtonDelete = query.id("mbtn-delete");

//para la paginación de puestos
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 12,
    currentPage = 1;

positionsList();

function positionsList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'PositionsAjax',
        method: 'get',
        data: data,
        success: function (data) {
            positionsContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(positionsContainer, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los cargos no se pueden visualizar.", "error");
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
    positionsList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    positionsList();
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
    positionsList();
}

buttonModalPosition.onclick = (e) => {
    tittleModal.innerHTML = "Nuevo cargo";
    buttonIcon(tittleModal);
    partialView(0, "PartialView", "#modal-position", modalBody, "");
}

function buttonIcon(tittle) {
    if (tittle.textContent == "Nuevo cargo") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar cargo") {
        if (buttonAction.childNodes[0].classList.contains("mdi-content-save")) {
            buttonAction.childNodes[0].classList.remove("mdi-content-save");
        }
        buttonAction.childNodes[0].classList.add("mdi-border-color");
        buttonAction.childNodes[1].textContent = " Modificar";
    }
}

function partialView(Id, Handler, ModalName, ModalBody, Action) {
    token = query.names("__RequestVerificationToken")
    query.ajax({
        url: "Positions?handler=" + Handler,
        method: "post",
        headers: {
            [query.headerToken]: token[0].value,
        },
        data: {
            Id: Id
        },
        success: function (data) {
            ModalBody.innerHTML = data;
            if (Action === "") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-sm");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-lg");
                modalPositionFooter.style.display = "";
                $(ModalName).modal("show");
            } else if (Action === "Information") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-lg");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-sm");
                modalPositionFooter.style.display = "none";
                $(ModalName).modal("show");
            }
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

positionsContainer.addEventListener("click", function positionactions(e) {
    let actual = e.target;
    let Id = 0;
    if (actual.matches('span[id=edit]') || actual.classList.contains('btn-warning')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idposition");
        if (parseInt(Id) > 0 && Id != null && Id != undefined) {
            actions(Id, "edit");
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    } else if (actual.matches('span[id=delete]') || actual.classList.contains('btn-danger')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idposition");
        if (parseInt(Id) > 0 && Id != null && Id != undefined) {
            $("#modalDeletePosition").modal("show");
            mButtonDelete.onclick = (e) => {
                actions(Id, "delete");
            }
        } else {
            query.toast("Alteración de información", "Los datos del servidro han sido alterados.", "error");
        }
    }
});

function actions(Id, action) {
    if (action != "" && action != null && action != undefined) {
        if (action == "view") {
            tittleModal.innerHTML = "Información del cargo";
            partialView(Id, "PositionInformation", "#modal-position", modalBody, "Information");

        } else if (action == "edit") {
            tittleModal.innerHTML = "Modificar cargo";
            buttonIcon(tittleModal);
            partialView(Id, "PartialView", "#modal-position", modalBody, "");

        } else if (action == "delete") {
            options({
                Id
            }, "Delete", "Eliminar cargo", "Position");
        }
    } else {
        query.toast("Alteración de información", "La informacion del servidor ha sido alterada.", "error");
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Positions?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let obj = JSON.parse(data);
            query.toast(Tittle, obj.message, obj.icon);
            if (Option == "Position") {
                reboot();
                positionsList();
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

formPosition.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    token = query.names("__RequestVerificationToken");
    let Cargo_Id;
    let url = "Positions";
    if (query.id("Cargo_Id") == null) {
        Cargo_Id = 0;
    } else {
        Cargo_Id = query.id("Cargo_Id").value;
    }
    if (parseInt(Cargo_Id) > 0 && Cargo_Id != "" && Cargo_Id != null && typeof Cargo_Id != undefined) {
        formData = new FormData(this);
        url += "?handler=Edit";
    } else {
        formData = new FormData(this);
        url = url;
    }

    //validación de todos los campos
    if (query.emptyElements(this)) {
        query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
        return;
    }

    //validación del nombre del cargo
    if (query.validate(query.id("Cargo_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre del cargo.", "warning");
        return;
    }

    if (query.validate(query.id("Cargo_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }

    if (url != "" && url != undefined && url != null && Cargo_Id != null && Cargo_Id != undefined) {
        query.ajax({
            url: url,
            method: "post",
            headers: {
                [query.headerToken]: token[0].value,
            },
            data: formData,
            success: function (data) {
                let obj = JSON.parse(data);
                if (obj.action == "save" && obj.action != null && obj.action != "") {
                    if (obj.position != null) {
                        query.toast("Registro de cargo", obj.message, obj.icon);
                        reboot();
                        positionsList();
                        $("#modal-position").modal("hide");
                    } else {
                        query.toast("Registro de cargo", obj.message, obj.icon);
                    }
                } else if (obj.action == "edit" && obj.action != null && obj.action != "") {
                    if (obj.position != null) {
                        query.toast("Registro de cargo", obj.message, obj.icon);
                        reboot();
                        positionsList();
                        $("#modal-position").modal("hide");
                    } else {
                        query.toast("Registro de cargo", obj.message, obj.icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de cargo", "Los datos del servidor han sido alterados.", "error");
    }
}

function reboot() {
    showQuantity.selectedIndex = 0;
    searchData.value = "";
    search = "";
    quantity = 8;
    currentPage = 1;
}