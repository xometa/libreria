let query = {
    q: function (selector) {
        //a diferencia del qall esta función nos devuelve un elemento unico ya sea mediante su id o una clase etc
        return document.querySelector(selector);
    },
    qall: function (selector) {
        //nos devolvera un arreglo de elementos, ya que que el selector sea un id o una clase, etc
        return document.querySelectorAll(selector);
    },
    id: function (selector) {
        //nos devolvera el elemento con el id que se especifique
        return document.getElementById(selector);
    },
    names: function (selector) {
        //nos devolvera un arreglo de elementos con el nombre que se especifique
        return document.getElementsByName(selector);
    },
    atr: function (selector, atributo) {
        //nos devolvera el attribute de un elemento 
        return selector.getAttribute(atributo);
    },
    create: function (selector) {
        //esta función nos crear cualquier elemento (example: tr,td,div,inpu), de igual manera podemos asignarles atributos
        return document.createElement(selector);
    },
    headerToken: "RequestVerificationToken",
    contentType: "Content-type",
    get: function (config) {
        //esta funcion nos devolvera los datos en formato json, de igual manera se pueden enviar 
        //parametros en especificos para recuperar un registro(example: let data={"nombre":nombre}))
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4) {
                switch (this.status) {
                    case 200:
                        config.success.call(this, this.responseText);
                        break;
                    default:
                        if (config.fail) {
                            config.fail.call(this);
                        }
                        break;
                }
            }
        };
        config.url = config.url + (config.data == undefined ? "" : "?" + this.params(config.data));
        xhttp.open("GET", config.url, true);
        xhttp.send();
    },
    post: function (config) {
        //esta funcion solo servira para modificar campos en especifico de cualquier 
        //tabla(mandar un data object, example: let data="id="id
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4) {
                switch (this.status) {
                    case 200:
                        config.success.call(this, this.responseText);
                        break;
                    default:
                        if (config.fail) {
                            config.fail.call(this);
                        }
                        break;
                }
            }
        };
        xhttp.open("POST", config.url, true);
        if (config.content != undefined) {
            xhttp.setRequestHeader("Content-type", config.content);
        }
        xhttp.setRequestHeader("RequestVerificationToken", config.token);
        xhttp.send(this.params(config.data));

    },
    ajax: function (config) {
        //función que nos servira para guardar y recuperar los datos, en el caso de guardar(method:"post") los datos
        //se recomienda mandar un objeto de tipo FormData(), y para recuperar(method:"get") no se deben enviar parametros
        //ya que solo realizara recuperaciones directas
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4) {
                switch (this.status) {
                    case 200:
                        config.success.call(this, this.responseText);
                        break;
                    default:
                        if (config.fail) {
                            config.fail.call(this);
                        }
                        break;
                }
            }
        };
        xhttp.open(config.method.toUpperCase(), config.url, true);
        if (config.headers != undefined) {
            for (let header in config.headers) {
                if (config.headers.hasOwnProperty(header)) {
                    xhttp.setRequestHeader(header, config.headers[header]);
                }
            }
        }
        if (config.data == undefined) {
            xhttp.send();
        } else {
            let formData;
            if (config.data.nodeName == 'FORM') {
                formData = new FormData(config.data);
                xhttp.send(formData);
            } else if (config.data instanceof FormData) {
                xhttp.send(config.data);
            } else {
                formData = new FormData();
                for (let property in config.data) {
                    if (config.data.hasOwnProperty(property)) {
                        formData.append(property, config.data[property]);
                    }
                }
                xhttp.send(formData);
            }
        }
    },
    resetForm: function (myFormElement) {
        //nos limpiara el formulario omitiendo no limpiar el token
        let elements = myFormElement.elements;
        for (i = 0; i < elements.length; i++) {
            if (elements[i].name !== "__RequestVerificationToken") {
                field_type = elements[i].type.toLowerCase();
                switch (field_type) {
                    case "text":
                    case "file":
                    case "date":
                    case "password":
                    case "textarea":
                    case "hidden":
                        elements[i].value = "";
                        break;
                    case "radio":
                    case "checkbox":
                        if (elements[i].checked) {
                            elements[i].checked = false;
                        }
                        break;
                    case "select-one":
                    case "select-multi":
                        elements[i].selectedIndex = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    },
    params: function (object) {
        let data = [];
        for (obj in object) {
            let val = encodeURIComponent(obj) + "=" + encodeURIComponent(object[obj]);
            data.push(val);
        }
        return data.join("&");
    },
    getAttributes: function (elemento) {
        let dataAttributes = {};
        let attributesList = elemento.attributes;
        let atributos;
        for (let i = 0; i < attributesList.length; i++) {
            atributos = attributesList[i];
            if (atributos.nodeName.startsWith("data-")) {
                dataAttributes[atributos.nodeName.substring(5)] = atributos.nodeValue;
            }
        }
        return dataAttributes;
    },
    toast: function (title, message, icon) {
        /**
         * info: icon = info
         * warning: icon = warning
         * success: icon = success
         * error: icon=error
         */
        $.NotificationApp.send(title, message, "top-right", "rgba(0,0,0,0.2)", icon);
    },
    getDataForm: function (form) {
        let data = {};
        let elements = form.elements;
        if (form.tagName = "FORM") {
            for (i = 0; i < elements.length; i++) {
                if (elements[i].name !== "__RequestVerificationToken" && elements[i].name !== "") {
                    field_type = elements[i].type.toLowerCase();
                    switch (field_type) {
                        case "text":
                        case "file":
                        case "date":
                        case "password":
                        case "textarea":
                        case "hidden":
                        case "number":
                            data[elements[i].name] = elements[i].value;
                            break;
                        case "radio":
                        case "checkbox":
                            if (elements[i].checked) {
                                data[elements[i].name] = elements[i].value;
                            }
                            break;
                        case "select-one":
                        case "select-multi":
                            data[elements[i].name] = elements[i].value;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return data;
    },
    emptyElements: function (form) {
        let status = false;
        let empty = 0,
            j = 0;
        let elements = form.elements;
        if (form.tagName = "FORM") {
            for (i = 0; i < elements.length; i++) {
                if (elements[i].name !== "" && elements[i].id !== "") {
                    field_type = elements[i].type.toLowerCase();
                    switch (field_type) {
                        case "text":
                        case "file":
                        case "date":
                        case "password":
                        case "textarea":
                        case "hidden":
                        case "number":
                            if (elements[i].value == "" || elements[i].value == " ") {
                                empty++;
                            }
                            j++;
                            break;
                        case "radio":
                        case "checkbox":
                            if (!elements[i].checked) {
                                empty++;
                            }
                            j++;
                            break;
                        case "select-one":
                        case "select-multi":
                            if (elements[i].selectedIndex == 0) {
                                empty++;
                            }
                            j++;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (empty == j) {
                status = true;
            } else {
                status = false;
            }
        }
        return status;
    },
    validate: function (data, type) {
        if (type === "int") {
            if (!isNaN(data) && parseInt(data) > 0 &&
                data != undefined && data != null && /^([0-9])*$/.test(data)) {
                return false;
            } else {
                return true;
            }
        } else if (type === "string") {
            if (typeof data !== "string" || typeof data == 'undefined' || !data || data.length === 0 || data === "" ||
                !/[^\s]/.test(data) || /^\s*$/.test(data) || data.replace(/\s/g, "") === "" ||
                !/\S/.test(data) || data === undefined || data === null) {
                return true;
            } else {
                return false;
            }
        } else if (type === "double") {
            if (!isNaN(data) && parseFloat(data) > 0 && /^\d*(\.\d{1})?\d{1,2}$/.test(data) && data !== undefined &&
                data !== null) {
                return false;
            } else {
                return true;
            }
        } else if (type === "email") {
            //^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$
            ///\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)/
            if (/^(([^<>()\[\]\\.,;:\s@”]+(\.[^<>()\[\]\\.,;:\s@”]+)*)|(“.+”))@((\[[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}])|(([a-zA-Z\-0–9]+\.)+[a-zA-Z]{2,}))$/.test(data)) {
                return false;
            } else {
                return true;
            }
        } else if (type === "phone") {
            if (/^\d{4}-\d{4}$/.test(data)) {
                return false;
            } else {
                return true;
            }
        } else if (type === "date") {
            if ((this.trim(data) == "") || (this.trim(data).length != 10) || data === null || data === undefined)
                return true;

            let day = parseInt(data.substr(8, 2), 10);
            let month = parseInt(data.substr(5, 2), 10);
            let year = parseInt(data.substr(0, 4), 10);
            // year
            if (isNaN(year) || (year < 1900))
                return true;
            // month
            if (isNaN(month) || (month < 1) || (month > 12))
                return true;
            // day
            if (isNaN(day) || (day < 1) || (day > 31))
                return true;
            else {
                if ((day == 31) && ((month == 4) || (month == 6) || (month == 9) || (month == 11)))
                    return true;
                let diaMax = 31;
                if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0))
                    diaMax = 29;
                else
                    diaMax = 28;
                if (day > diaMax)
                    return true;
            }
            return false;
        } else if (type === "letters") {
            if (/^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$/g.test(data)) {
                return false;
            } else {
                return true;
            }
        } else if (type === "numbers") {
            if (/^([0-9])*$/.test(data)) {
                return false;
            } else {
                return true;
            }
        } else if (type === "DUI") {
            if (/^\D*\d{8}-\D*\d{1}$/g.test(data)) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    },
    trim: function (string) {
        string += "";
        string = string.replace(/^\s+/, '');
        return string.replace(/\s+$/, '');
    },
    isUndefinedNull: function (element) {
        if (element === null || element === undefined) {
            return true;
        } else {
            return false;
        }
    },
    fillSelect: function (template, destino, lista, name, value, identifier) {
        elems = template.content.querySelectorAll("option");
        //elems[0] es <option>
        elems[0].textContent = "--- Seleccionar " + identifier + " ---";
        elems[0].value = "";
        var clon = document.importNode(template.content, true);
        destino.appendChild(clon);

        lista.forEach(function (item) {
            //elems[0] es <option>
            //se accedera al primer elemento ya que solo hay un elemento <option>
            elems[0].textContent = item[name];
            elems[0].value = item[value];
            var clon = document.importNode(template.content, true);
            destino.appendChild(clon);
        });
    },
    mask: function () {
        $('[data-toggle="input-mask"]').each(function (idx, obj) {
            var maskFormat = $(obj).data("maskFormat");
            var reverse = $(obj).data("reverse");
            if (reverse != null)
                $(obj).mask(maskFormat, { 'reverse': reverse });
            else
                $(obj).mask(maskFormat);
        });
    },
    createPagination: function (container, filter) {
        let div = container.querySelector(filter);
        let total = parseInt(div.getAttribute('data-total')),
            page = parseInt(div.getAttribute('data-current-page')),
            max = parseInt(div.getAttribute('data-max')),
            show = parseInt(div.getAttribute('data-showregisters'));
        let pages = total / max;
        let home = 0, end = 0;
        if (total == 0) {
            pages = 0;
            home = 0;
            end = 0;
        } else if (total != 0 && (pages == 0.00 || pages <= 1.00)) {
            pages = 1;
            home = 1;
            end = total;
        } else {
            if (pages > 1.00 && pages < 2.00) {
                pages = 2;
            } else {
                pages = pages.toFixed(0);
            }
            if (page == 1) {
                home = 1;
                end = max;
            } else {
                if (parseInt(pages) == page) {
                    home = (total - show) + 1;
                    end = total;
                } else {
                    home = ((page * max) - show) + 1;
                    end = page * max;
                }
            }
        }
        let adjacents = 2;
        let html = '';

        //iniciando la paginación
        let prevlabel = `<span aria-hidden='true'>&laquo;</span><span class='sr-only'>Anterior</span>`;
        let nextlabel = `<span aria-hidden='true'>&raquo;</span><span class='sr-only'>Siguiente</span>`;

        //texto, que muestra la cantidad de registros que se muestran por página
        html += `<h5 class='card-title col-sm-4 pt-2'>Registro de ` + home + ` a ` + end + `, de un total de ` + total + `</h5>`;

        //creación del NAV y UL
        html += `<nav class='col-sm-8'>`;
        html += `<ul class='pagination pagination-rounded mb-0 justify-content-end'>`;

        if (page == 1) {
            html += `<li class='page-item disabled'><a class='page-link'>` + prevlabel + `</a></li>`;
        } else if (page == 2) {
            html += `<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='${1}'>` + prevlabel + `</a></li>`;
        } else {
            html += `<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='${page - 1}'>` + prevlabel + `</a></li>`;

        }

        if (page > (adjacents + 1)) {
            html += `<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='${1}'>` + 1 + `</a></li>`;
        }

        if (page > (adjacents + 2)) {
            html += `<li class='page-item'><a class='page-link'>...</a></li>`;
        }

        let pmin = (page > adjacents) ? (page - adjacents) : 1;
        let pmax = (page < (pages - adjacents)) ? (page + adjacents) : pages;
        for (let i = pmin; i <= pmax; i++) {
            if (i == page) {
                html += `<li class='page-item active pointer'><a class='page-link' data-page='${i}'>` + i + `</a></li>`;
            } else if (i == 1) {
                html += `<li class='page-item pointer'><a class='page-link' href='javascript:void(0);' data-page='${1}'>` + i + `</a></li>`;
            } else {
                html += `<li class='page-item pointer'><a class='page-link' href='javascript:void(0);' data-page='${i}'>` + i + `</a></li>`;
            }
        }

        if (page < (pages - adjacents - 1)) {
            html += `<li class='page-item'><a class='page-link'>...</a></li>`;
        }

        if (page < (pages - adjacents)) {
            html += `<li class='page-item pointer'><a class='page-link' href='javascript:void(0);' data-page='${pages}'>` + pages + `</a></li>`;
        }

        if (page < pages) {
            html += `<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='${page + 1}'>` + nextlabel + `</a></li>`;
        } else {
            html += `<li class='page-item disabled'><a class='page-link'>` + nextlabel + `</a></li>`;
        }

        html += `</ul>`;
        html += `</nav>`;
        return html;
    }
}