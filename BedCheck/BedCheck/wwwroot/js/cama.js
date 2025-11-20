var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    // CAMBIO 1: Usar 'DataTable' (con D mayúscula) es la versión moderna.
    // Esto arregla estilos de Bootstrap y funciones como ajax.reload()
    dataTable = $("#tblCamas").DataTable({
        "ajax": {
            "url": "/admin/camas/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            // Nombre un poco más ancho
            { "data": "nombreCama", "width": "15%" },

            // CAMBIO 2: Renderizar Ocupada/Libre con colores (Badges)
            {
                "data": "camaUsada",
                "width": "10%",
                "render": function (data) {
                    if (data == true) {
                        return '<span class="badge bg-danger">Ocupada</span>';
                    } else {
                        return '<span class="badge bg-success">Libre</span>';
                    }
                }
            },
            { "data": "estadoCama", "width": "10%" },
            { "data": "tipoCama", "width": "10%" },

            // CAMBIO 3: Arreglado el HTML de la imagen y añadido estilo
            {
                "data": "urlImagen",
                "width": "15%",
                "render": function (imagen) {
                    if (imagen) {
                        // Ajustamos src, width correcto y un borde redondeado
                        return `<div class="text-center">
                                    <img src="${imagen}" style="width: 50px; border-radius: 5px; border: 1px solid #ccc;" />
                                </div>`;
                    } else {
                        return '<div class="text-center">Sin imagen</div>';
                    }
                }
            },

            // CAMBIO 4: Habitación centrada
            {
                "data": "numeroHabitacion",
                "width": "10%",
                "className": "text-center" // Centra el número
            },

            // CAMBIO 5: Botones con iconos y ancho ajustado
            {
                "data": "idCama",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Camas/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:80px;">
                                <i class="fas fa-edit"></i> Editar
                            </a>
                            &nbsp;
                            <a onclick=Delete("/Admin/Camas/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:80px;">
                                <i class="fas fa-trash-alt"></i> Borrar
                            </a>
                        </div>
                    `;
                }, "width": "30%"
            }
        ],
        // CAMBIO 6: Usamos el idioma remoto (más limpio) o el tuyo personalizado.
        // He dejado el tuyo pero ajustado para que se vea mejor
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 Entradas",
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
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de borrar?",
        text: "¡Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "¡Sí, borrar!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    // CAMBIO 7: Ahora sí funciona la recarga sin F5
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}