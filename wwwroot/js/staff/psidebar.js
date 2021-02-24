let sidebar = query.q("ul");
let permissions = ["Inicio", "Ventas", "Compras", "Abonos", "Marcas",
    "Categorías", "Productos", "Clientes", "Proveedores",
    "Personal", "Permisos", "Bitácora", "Respaldo de datos",
    "Listado de consultas", "Listado de reportes", "Acerca de"
];
let name = [],
    icons = [],
    error = [];
let ok = false;
let modules,
    children,
    moduleIcons,
    nameModules,
    description = "Se han encontrado errores";
let URLactual = window.location.pathname;
userPermissions();

function userPermissions() {
    if (sidebar.nodeName === "UL" && sidebar.classList.contains("side-nav")) {
        modules = sidebar.querySelectorAll("li>a");
        for (let i = 0; i < modules.length; i++) {
            if (modules[i].classList.contains("side-nav-link")) {
                nameModules = modules[i].children[1];
                moduleIcons = modules[i].children[0];
                if (nameModules.nodeName === "SPAN" && moduleIcons.nodeName === "I") {
                    nameModules = nameModules.textContent;
                    nameModules = nameModules.replace(/^[ ]+|[ ]+$/g, '');
                    moduleIcons = moduleIcons.className;
                    if (typeof nameModules === "string" && typeof moduleIcons === "string") {
                        name.push(nameModules);
                        icons.push(moduleIcons);
                    } else {
                        error.push(description);
                    }
                } else {
                    error.push(description);
                }
            }
        }
        if (name.length === 16) {
            for (let j = 0; j < name.length; j++) {
                if (name[j] == permissions[j]) {
                    ok = true;
                } else {
                    error.push(description);
                }
            }
        } else {
            error.push(description);
        }
        if (error.length === 0 && ok) {
            let formData = new FormData();
            for (let k = 0; k < name.length; k++) {
                formData.append("modules[" + k + "]", name[k]);
                formData.append("icons[" + k + "]", icons[k]);
            }
            //guardando la información
            let url;
            let token = query.names("__RequestVerificationToken");
            if (URLactual.length > 1) {
                url = "/UserPermissions/UserPermissions?handler=UserPermissions";
            } else {
                url = "../UserPermissions/UserPermissions?handler=UserPermissions";
            }
            query.ajax({
                url: url,
                method: "post",
                headers: {
                    [query.headerToken]: token[0].value,
                },
                data: formData,
                success: function(data) {
                    let { count } = JSON.parse(data);
                    if (!(parseInt(count) === 16)) {
                        query.toast("Advertencia", "La información del servidor ha sido alterada.", "error");
                        redirect();
                    }
                },
                fail: function() {
                    query.toast("Error", "La petición solicitada ha fallado.", "error");
                    redirect();
                }
            });

        } else {
            query.toast("Advertencia", "La información del servidor ha sido alterada.", "error");
            redirect();
        }
    } else {
        query.toast("Advertencia", "La información del servidor ha sido alterada.", "error");
        redirect();
    }
}

//mostrado el mensaje de error, se ejecutara esta función, redirigiendo al login
function redirect() {
    setTimeout(function() {
        document.location.href = '/Login/Logout';
    }, 3000);
}