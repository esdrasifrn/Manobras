function generateDataTable(selector, model, listUrl, podeEditar = false, urlEditar = "", podeApagar = false, urlApagar = "", extrabuttons = [], defaultsearch="") {
    $(selector).DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: listUrl,
            method: "POST"
        },
        responsive: false,
        paging: true,
        ordering: true,
        pagingType: "full_numbers",
        "oSearch": { "sSearch": defaultsearch },
        language: {
            search: "Buscar",
            lengthMenu: "Mostrando _MENU_ "+ model +"s por página",
            zeroRecords: "Nenhum "+ model +" encontrado",
            info: "Mostrando _START_ até _END_ de _TOTAL_ "+ model +"s",
            infoEmpty: "Nenhum "+ model +" a exibir",
            processing: "Processando ...",
            paginate: {
                first: "Primeiro",
                last: "Último",
                next: "Próximo",
                previous: "Anterior"
            },
        },
        columnDefs: [
            {
                targets: 0,
                visible: false
            },
            {
                targets: -1,
                data: 'actions',
                render: function (data, type, row, meta) {
                    var opts = "";
                    extrabuttons.forEach(function (button, index, array) {
                        if (button["permission"] === true || typeof button["permission"] === "undefined") {
                            var color = "success";
                            if (typeof button["style"] !== "undefined") {
                                color = button["style"];
                            }
                            opts += '<a title="' + button["title"] + '" class="btn btn-sm btn-' + color + '  white-text" href="' + button["url"] + row[0] + '"><i class="fas fa-shield-alt"></i> ' + button["title"] + '</a>';
                        }
                    });
                    if (podeEditar === true) {
                        opts += '<a title="Editar" class="btn btn-sm btn-primary white-text" href="' + urlEditar + row[0] + '"> <i class="fa fa-edit fa-lg"></i>Editar</a>&nbsp';
                    }

                    if (podeApagar === true) {
                        opts += '<a title="Excluir" class="btn btn-sm btn-danger white-text confirmar-exclusao" href="#' + urlApagar + row[0] + '" onclick="return confirmarExclusao(\'' + model + '\',\''+ urlApagar +'\',\''+row[0]+'\',\''+row[1]+'\');"><i class="fa fa-trash fa-lg"></i>Excluir</a>';
                    }
                    return opts;
                },
                className: "text-right"
            }
        ]
    });
}

function jsUcfirst(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
function confirmarExclusao(model, url, identificador, nome) {
    swal({
        title: 'Confirma exclusão?',
        text: jsUcfirst(model) + ': ' + nome,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim pode apagar!',
        cancelButtonText: 'Cancelar'
    }, function (inputValue) {

        if (inputValue === true) {
            window.location.href = url + identificador;

        }
    });
};