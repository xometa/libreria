let trademarksContainer = query.id('trademarks-list'),
    buttonModalTrademark = query.id("btn-modaltrademark"),
    tittleModal = query.id("tittle-modal"),
    modalBody = query.id("modalBody"),
    formTrademark = query.id("formTrademark"),
    buttonAction = query.id('btn-action'),
    mButtonDelete = query.id("mbtn-delete"),
    token,
    modalTrademarkFooter = query.id("modaltrademark-footer");

//para la paginación de marcas
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 8,
    currentPage = 1;

trademarksList();

function trademarksList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'TrademarksAjax',
        method: 'get',
        data: data,
        success: function (data) {
            trademarksContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(trademarksContainer, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Las marcas no se pueden visualizar.", "error");
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
    trademarksList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    trademarksList();
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
    trademarksList();
}

buttonModalTrademark.onclick = (e) => {
    tittleModal.innerHTML = "Nueva marca";
    buttonIcon(tittleModal);
    partialView(0, "PartialView", "#modal-trademark", modalBody, "");
}

function buttonIcon(tittle) {
    if (tittle.textContent == "Nueva marca") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar marca") {
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
        url: "Trademarks?handler=" + Handler,
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
                modalTrademarkFooter.style.display = "";
                $(ModalName).modal("show");
            } else if (Action === "Information") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-lg");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-sm");
                modalTrademarkFooter.style.display = "none";
                $(ModalName).modal("show");
            }
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

trademarksContainer.addEventListener("click", function trademarkactions(e) {
    let actual = e.target;
    let Id = 0;
    if (actual.matches('span[id=edit]') || actual.classList.contains('btn-warning')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idtrademark");
        if (parseInt(Id) > 0 && Id != null && Id != undefined) {
            actions(Id, "edit");
        } else {
            query.toast("Alteración de información", "Los datos del servidor han sido alterados.", "error");
        }
    } else if (actual.matches('span[id=delete]') || actual.classList.contains('btn-danger')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idtrademark");
        if (parseInt(Id) > 0 && Id != null && Id != undefined) {
            $("#modalDeleteTrademark").modal("show");
            mButtonDelete.onclick = (e) => {
                actions(Id, "delete");
            }
        } else {
            query.toast("Alteración de información", "Los datos del servidor han sido alterados.", "error");
        }
    } else if (actual.matches("input[type=checkbox]")) {
        let Status = 0;
        Id = actual.getAttribute("data-idtrademark");
        if (actual.checked) {
            Status = 1;
        } else {
            Status = 0;
        }
        if (!query.validate(Id, "int") && !isNaN(Status) && Status >= 0 && Status <= 1) {
            options({ Id: parseInt(Id), Status }, "EditStatus", "Estado de la marca", "");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
});

function actions(Id, action) {
    if (action != "" && action != null && action != undefined) {
        if (action == "view") {
            tittleModal.innerHTML = "Información de la marca";
            partialView(Id, "TrademarkInformation", "#modal-trademark", modalBody, "Information");

        } else if (action == "edit") {
            tittleModal.innerHTML = "Modificar marca";
            buttonIcon(tittleModal);
            partialView(Id, "PartialView", "#modal-trademark", modalBody, "");

        } else if (action == "delete") {
            options({
                Id
            }, "Delete", "Eliminar marca", "Trademark");
        }
    } else {
        query.toast("Alteración de información", "La informacion del servidor ha sido alterada.", "error");
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Trademarks?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let obj = JSON.parse(data);
            query.toast(Tittle, obj.message, obj.icon);
            if (Option == "Trademark") {
                reboot();
                trademarksList();
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

formTrademark.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    token = query.names("__RequestVerificationToken");
    let Marca_Id;
    let url = "Trademarks";
    if (query.id("Marca_Id") == null) {
        Marca_Id = 0;
    } else {
        Marca_Id = query.id("Marca_Id").value;
    }
    if (parseInt(Marca_Id) > 0 && Marca_Id != "" && Marca_Id != null && typeof Marca_Id != undefined) {
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

    //validación del nombre de la marca
    if (query.validate(query.id("Marca_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre de la marca.", "warning");
        return;
    }

    if (query.validate(query.id("Marca_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }

    if (url != "" && url != undefined && url != null && Marca_Id != null && Marca_Id != undefined) {
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
                    if (obj.trademark != null) {
                        query.toast("Registro de marca", obj.message, obj.icon);
                        reboot();
                        trademarksList();
                        $("#modal-trademark").modal("hide");
                    } else {
                        query.toast("Registro de marca", obj.message, obj.icon);
                    }
                } else if (obj.action == "edit" && obj.action != null && obj.action != "") {
                    if (obj.trademark != null) {
                        query.toast("Modificación de marca", obj.message, obj.icon);
                        reboot();
                        trademarksList();
                        $("#modal-trademark").modal("hide");
                    } else {
                        query.toast("Modificación de marca", obj.message, obj.icon);
                    }
                }
            },
            fail: function () {
                query.toast("Error", "La petición solicitada ha fallado.", "error");
            }
        });
    } else {
        query.toast("Registro de marca", "Los datos del servidor han sido alterados.", "error");
    }
}

function reboot() {
    showQuantity.selectedIndex = 0;
    searchData.value = "";
    search = "";
    quantity = 8;
    currentPage = 1;
}