let categoriesContainer = query.id('categories-list'),
    buttonModalCategory = query.id("btn-modalcategory"),
    tittleModal = query.id("tittle-modal"),
    modalBody = query.id("modalBody"),
    formCategory = query.id("formCategory"),
    buttonAction = query.id('btn-action'),
    mButtonDelete = query.id("mbtn-delete"),
    token,
    modalCategoryFooter = query.id("modalcategory-footer");

//para la paginación de categorías
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 8,
    currentPage = 1;

categoriesList();

function categoriesList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage
    }
    query.get({
        url: 'CategoriesAjax',
        method: 'get',
        data: data,
        success: function (data) {
            categoriesContainer.innerHTML = data;
            pagination.innerHTML = query.createPagination(categoriesContainer, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Las categorías no se pueden visualizar.", "error");
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
    categoriesList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    categoriesList();
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
    categoriesList();
}

buttonModalCategory.onclick = (e) => {
    tittleModal.innerHTML = "Nueva categoría";
    buttonIcon(tittleModal);
    partialView(0, "PartialView", "#modal-category", modalBody, "");
}

function buttonIcon(tittle) {
    if (tittle.textContent == "Nueva categoría") {
        if (buttonAction.childNodes[0].classList.contains("mdi-border-color")) {
            buttonAction.childNodes[0].classList.remove("mdi-border-color");
        }
        buttonAction.childNodes[0].classList.add("mdi-content-save");
        buttonAction.childNodes[1].textContent = " Guardar";
    } else if (tittle.textContent == "Modificar categoría") {
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
        url: "Categories?handler=" + Handler,
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
                modalCategoryFooter.style.display = "";
                $(ModalName).modal("show");
            } else if (Action === "Information") {
                tittleModal.parentNode.parentNode.parentNode.classList.remove("modal-lg");
                tittleModal.parentNode.parentNode.parentNode.classList.add("modal-sm");
                modalCategoryFooter.style.display = "none";
                $(ModalName).modal("show");
            }
        },
        fail: function () {
            query.toast("Error", "El formulario no se puede visualizar.", "error");
        }
    });
}

categoriesContainer.addEventListener("click", function categoryactions(e) {
    let actual = e.target;
    let Id = 0;
    if (actual.matches('span[id=edit]') || actual.classList.contains('btn-warning')) {
        if (actual.nodeName == "BUTTON") {
            actual = actual.firstChild;
        } else {
            actual = actual;
        }
        Id = actual.getAttribute("data-idcategory");
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
        Id = actual.getAttribute("data-idcategory");
        if (parseInt(Id) > 0 && Id != null && Id != undefined) {
            $("#modalDeleteCategory").modal("show");
            mButtonDelete.onclick = (e) => {
                actions(Id, "delete");
            }
        } else {
            query.toast("Alteración de información", "Los datos del servidor han sido alterados.", "error");
        }
    } else if (actual.matches("input[type=checkbox]")) {
        let Status = 0;
        Id = actual.getAttribute("data-idcategory");
        if (actual.checked) {
            Status = 1;
        } else {
            Status = 0;
        }
        if (!query.validate(Id, "int") && !isNaN(Status) && Status >= 0 && Status <= 1) {
            options({ Id: parseInt(Id), Status }, "EditStatus", "Estado de la categoría", "");
        } else {
            query.toast("Información alterada", "La información del servidor ha sido alterada.", "error");
        }
    }
});

function actions(Id, action) {
    if (action != "" && action != null && action != undefined) {
        if (action == "view") {
            tittleModal.innerHTML = "Información de la categoría";
            partialView(Id, "CategoryInformation", "#modal-category", modalBody, "Information");

        } else if (action == "edit") {
            tittleModal.innerHTML = "Modificar categoría";
            buttonIcon(tittleModal);
            partialView(Id, "PartialView", "#modal-category", modalBody, "");

        } else if (action == "delete") {
            options({
                Id
            }, "Delete", "Eliminar categoría", "Category");
        }
    } else {
        query.toast("Alteración de información", "La informacion del servidor ha sido alterada.", "error");
    }
}

function options(data, Handler, Tittle, Option) {
    token = query.names("__RequestVerificationToken");
    query.post({
        url: "Categories?handler=" + Handler,
        token: token[0].value,
        content: "application/x-www-form-urlencoded",
        data: data,
        success: function (data) {
            let obj = JSON.parse(data);
            query.toast(Tittle, obj.message, obj.icon);
            if (Option == "Category") {
                reboot();
                categoriesList();
            }
        },
        fail: function () {
            query.toast("Error en el servidor", "La petición del servidor ha fallado.", "error");
        }
    });
}

formCategory.onsubmit = function (e) {
    e.preventDefault();
    let formData;
    token = query.names("__RequestVerificationToken");
    let Categoria_Id;
    let url = "Categories";
    if (query.id("Categoria_Id") == null) {
        Categoria_Id = 0;
    } else {
        Categoria_Id = query.id("Categoria_Id").value;
    }
    if (parseInt(Categoria_Id) > 0 && Categoria_Id != "" && Categoria_Id != null && typeof Categoria_Id != undefined) {
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

    //validación del nombre de la categoria
    if (query.validate(query.id("Categoria_Nombre").value, "string")) {
        query.toast("Campos vacíos", "Ingrese el nombre de la categoría.", "warning");
        return;
    }

    if (query.validate(query.id("Categoria_Nombre").value, "letters")) {
        query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
        return;
    }

    if (url != "" && url != undefined && url != null && Categoria_Id != null && Categoria_Id != undefined) {
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
                    if (obj.category != null) {
                        query.toast("Registro de categoría", obj.message, obj.icon);
                        reboot();
                        categoriesList();
                        $("#modal-category").modal("hide");
                    } else {
                        query.toast("Registro de categoría", obj.message, obj.icon);
                    }
                } else if (obj.action == "edit" && obj.action != null && obj.action != "") {
                    if (obj.category != null) {
                        query.toast("Modificación de categoría", obj.message, obj.icon);
                        reboot();
                        categoriesList();
                        $("#modal-category").modal("hide");
                    } else {
                        query.toast("Modificación de categoría", obj.message, obj.icon);
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