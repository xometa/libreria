//variables
let container = query.id("listBinnacle"),
    formBinnacle = query.id("formBinnacle"),
    employed = query.id("employed"),
    homeDate = query.id("home-date"),
    endDate = query.id("end-date"),
    date = new Date();
let day = date.getDate(),
    month = date.getMonth() + 1,
    year = date.getFullYear();

//llamada de funciones
timeLine(0, year + "-" + month + "-" + day, year + "-" + month + "-" + day);

function timeLine(IdEmployed, HomeDate, EndDate) {
    let data = {
        handler: "TimeLine",
        IdEmployed: parseInt(IdEmployed),
        HomeDate: HomeDate,
        EndDate: EndDate
    }
    query.get({
        url: 'TimeLine',
        method: 'get',
        data: data,
        success: function(data) {
            container.innerHTML = data;
        },
        fail: function() {
            query.toast("Error", "La bitácora no se pueden visualizar.", "error");
        }
    });
}

formBinnacle.onsubmit = function(e) {
    e.preventDefault();

    if (!query.validate(employed.value, "string")) {
        if (query.validate(employed.value, "int")) {
            query.toast("Seleccionar un empleado", "Seleccione un empleado.", "error");
            return;
        }
    }
    if (homeDate.value !== "") {
        if (query.validate(homeDate.value, "date")) {
            query.toast("Fecha incorrecta", "La fecha inicio ingresada del filtro es es incorrecta.", "warning");
            return;
        }
    }
    if (endDate.value !== "") {
        if (query.validate(endDate.value, "date")) {
            query.toast("Fecha incorrecta", "La fecha fin ingresada del filtro es incorrecta.", "warning");
            return;
        }
    }

    if (employed.value === "" &&
        homeDate.value === "" && endDate.value === "") {
        timeLine(0, year + "-" + month + "-" + day, year + "-" + month + "-" + day);
    }
    if (employed.value !== "" &&
        homeDate.value === "" && endDate.value === "") {
        timeLine(employed.value, homeDate.value, endDate.value)
    }
    if (employed.value === "" &&
        homeDate.value !== "" && endDate.value !== "") {
        timeLine(0, homeDate.value, endDate.value)
    }
    if (employed.value !== "" &&
        homeDate.value !== "" && endDate.value !== "") {
        timeLine(employed.value, homeDate.value, endDate.value)
    }
}