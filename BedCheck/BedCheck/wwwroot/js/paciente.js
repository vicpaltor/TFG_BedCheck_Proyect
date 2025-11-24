var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblPacientes").DataTable({
        "ajax": {
            "url": "/Admin/Pacientes/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "edad", "width": "5%" },
            { "data": "sexo", "width": "10%" },
            {
                "data": "enfermedades",
                "width": "20%",
                "render": function (data) {
                    return data ? data : '<span class="text-muted">-</span>';
                }
            },
            {
                "data": "tratamientos",
                "width": "20%",
                "render": function (data) {
                    return data ? data : '<span class="text-muted">-</span>';
                }
            },
            {
                "data": "idPaciente",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Pacientes/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:80px;">
                                <i class="fas fa-edit"></i> Editar
                            </a>
                            &nbsp;
                            <a onclick=Delete("/Admin/Pacientes/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="fas fa-trash-alt"></i> Borrar
                            </a>
                        </div>
                    `;
                }, "width": "20%"
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay datos disponibles en la tabla",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 entradas",
            "infoFiltered": "(filtrado de _MAX_ entradas totales)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron coincidencias",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro?",
        text: "No se podrá recuperar la información",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Sí, borrar",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                } else {
                    toastr.error(data.message);
                }
            }
        });
    });
}