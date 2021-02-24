let container = query.id('user-information');

//llamado de funciones
userInformation();
//funciones
function userInformation() {
    query.get({
        url: 'UserInformation',
        method: 'get',
        data: { handler: "UserInformation" },
        success: function (data) {
            container.innerHTML = data;
            query.mask();
            events();
        },
        fail: function () {
            query.toast("Error", "La información del usuario no se puede visualizar.", "error");
        }
    });
}

container.onclick = function (e) {
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

function events() {
    let formEmployed = query.id('formUser');
    let btnCancel = query.id('btn-cancel');
    formEmployed.onsubmit = function (e) {
        e.preventDefault();
        let Employed_Id = 0,
            Person_Id = 0
        User_Id = 0;
        let token = query.names("__RequestVerificationToken");
        let formData;
        if (!query.isUndefinedNull(query.id("Empleado_Id")) && !query.isUndefinedNull(query.id("Persona_Id"))
            && !query.isUndefinedNull(query.id("Usuario_Id"))) {
            Employed_Id = query.id("Empleado_Id").value;
            Person_Id = query.id("Persona_Id").value;
            User_Id = query.id("Usuario_Id").value;
        }

        //validación de todos los campos
        if (query.emptyElements(this)) {
            query.toast("Campos vacíos", "Complete la información solicitada en el formulario.", "warning");
            return;
        }

        //validación del nombre del empleado
        if (query.validate(query.id("Persona_Nombre").value, "string")) {
            query.toast("Campos vacíos", "Ingrese su nombre.", "warning");
            return;
        }

        if (query.validate(query.id("Persona_Nombre").value, "letters")) {
            query.toast("Nombre invalido", "El nombre ingresado es inválido.", "warning");
            return;
        }
        //validacion del apellido del empleado
        if (query.validate(query.id("Persona_Apellido").value, "string")) {
            query.toast("Campos vacíos", "Ingrese su apellido.", "warning");
            return;
        }

        if (query.validate(query.id("Persona_Apellido").value, "letters")) {
            query.toast("Apellido invalido", "El apellido ingresado es inválido.", "warning");
            return;
        }
        //validación del DUI
        if (query.validate(query.id("Persona_Dui").value, "string")) {
            query.toast("Campos vacíos", "Ingrese su # de DUI.", "warning");
            return;
        }
        if (query.validate(query.id("Persona_Dui").value, "DUI")) {
            query.toast("DUI invalido", "El número de DUI ingresado es incorrecto.", "warning");
            return;
        }
        //Validación del sexo del empleado
        if (query.validate(query.id("Empleado_Sexo").value, "string")) {
            query.toast("Campos vacíos", "Seleccione un genéro.", "warning");
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


        //validación del nombre del usuario
        if (query.validate(query.id("Usuario_Usuario1").value, "string")) {
            query.toast("Campos vacíos", "Ingrese el nuevo nombre de su usuario.", "warning");
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

        if (isNaN(User_Id) || isNaN(Employed_Id) || isNaN(Person_Id)) {
            query.toast("Error", "Los datos del servidor han sido alterados.", "error");
            return;
        }

        if (!query.isUndefinedNull(User_Id) && !query.isUndefinedNull(Employed_Id) &&
            !query.isUndefinedNull(Person_Id)) {
            formData = new FormData(this);
            formData.append("Persona.Tipo", "Empleado");
            query.ajax({
                url: "UserInformation",
                method: "post",
                headers: {
                    [query.headerToken]: token[0].value,
                },
                data: formData,
                success: function (data) {
                    let { action, employed, message, icon } = JSON.parse(data);
                    if (action == "edit" && action != null && action != "") {
                        if (employed != null) {
                            query.toast("Actualiación de información", message, icon);
                            userInformation();
                        } else {
                            query.toast("Actualiación de información", message, icon);
                        }
                    } else {
                        query.toast("Actualiación de información", message, icon);
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

    btnCancel.onclick = function (e) {
        userInformation();
    }
}