var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblCamas").dataTable({
        "ajax": {
            "url": "/admin/camas/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nombreCama", "width": "5%" },
            { "data": "camaUsada", "width": "5%" },
            { "data": "estadoCama", "width": "5%" },
            { "data": "tipoCama", "width": "5%" },
            {
                "data": "urlImagen",
                "render": function (imagen) {
                    return `<img src="../${imagen}" width: "10">`
                }
            },
            { "data": "habitacion.numHabitacion", "width": "5%" },
            {
                "data": "idCama",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Camas/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:130px;">
                                    <i class="fa-regular fa-pen-to-square"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Camas/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:130px;">
                                    <i class="fa-solid fa-trash-can"></i> Borrar
                                </a>
                            </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}

function Delete(url){
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true

    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    //dataTable.ajax.reload();
                    window.location.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}