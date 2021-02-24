let container = query.id('data-body');

//para la creación de la paginación de productos
let pagination = query.id('create-pagination'),
    showQuantity = query.id('showQuantity'),
    searchData = query.id('searchData');
let search = "",
    quantity = 4,
    currentPage = 1;


//llamada de funciones
productsDayList();
weekDay();

function productsDayList() {
    let data = {
        search: search,
        quantity: quantity,
        currentPage: currentPage,
        handler: "ProductsDay"
    }
    query.get({
        url: 'Index',
        method: 'get',
        data: data,
        success: function (data) {
            container.innerHTML = data;
            pagination.innerHTML = query.createPagination(container, "#filter-results");
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}

searchData.onkeyup = function (e) {
    let current = e.target;
    if (current.value !== "") {
        search = current.value;
    } else {
        search = "";
    }
    quantity = parseInt(showQuantity.value);
    currentPage = 1;
    productsDayList();
}

showQuantity.onchange = function (e) {
    let current = e.target;
    quantity = parseInt(current.value);
    search = searchData.value = "";
    currentPage = 1;
    productsDayList();
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
    productsDayList();
}


////////para la gráfica
function weekDay() {
    query.get({
        url: 'Index',
        method: 'get',
        data: { handler: "WeekDay" },
        success: function (data) {
            let { cWeekDay, lWeekDay } = JSON.parse(data);
            var days = [
                { day: 'Lunes' },
                { day: 'Martes' },
                { day: 'Miercoles' },
                { day: 'Jueves' },
                { day: 'Viernes' },
                { day: 'Sabado' },
                { day: 'Domingo' }
            ];

            var modelCurrent = [0, 0, 0, 0, 0, 0, 0
            ];
            var modelLast = [0, 0, 0, 0, 0, 0, 0
            ];

            cWeekDay.forEach(element => {
                for (let index = 0; index < days.length; index++) {
                    if (days[index].day === element.cadena) {
                        modelCurrent[index] = parseFloat(parseFloat((element.ingresos + (element.ingresos * 0.13))).toFixed(2));
                    }
                }
            });

            lWeekDay.forEach(element => {
                for (let index = 0; index < days.length; index++) {
                    if (days[index].day === element.cadena) {
                        modelLast[index] = parseFloat(parseFloat((element.ingresos + (element.ingresos * 0.13))).toFixed(2));
                    }
                }
            });
            graphData(modelCurrent, modelLast);
        },
        fail: function () {
            query.toast("Error", "Los productos no se pueden visualizar.", "error");
        }
    });
}
function graphData(current, last) {
    console.log(current, last);
    var e = ["#727cf5", "#0acf97", "#fa5c7c", "#ffbc00"];
    var t = {
        chart: {
            height: 364,
            type: "line",
            dropShadow: {
                enabled: !0,
                opacity: .2,
                blur: 7,
                left: -7,
                top: 7
            },
            toolbar: {
                show: !1
            }
        },
        dataLabels: {
            enabled: !1
        },
        stroke: {
            curve: "smooth",
            width: 4
        },
        series: [{
            name: "Semana actual",
            data: current
        }, {
            name: "Semana anterior",
            data: last
        }],
        colors: e,
        zoom: {
            enabled: !1
        },
        legend: {
            show: !1
        },
        xaxis: {
            type: "string",
            categories: ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"],
            tooltip: {
                enabled: !1
            },
            axisBorder: {
                show: !1
            }
        },
        yaxis: {
            labels: {
                formatter: function (e) {
                    return "$ " + e
                },
                offsetX: -15
            }
        }
    };
    new ApexCharts(document.querySelector("#revenue-chart"), t).render();
}