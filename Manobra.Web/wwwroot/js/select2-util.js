$(document).ready(function () {
    // procura por selects com class="has-select2" e aplica o select2 ao elemento
    $("select.has-select2").each(function () {
        aplicar_select2_to_elem(this);
    });
});

function aplicar_select2_to_elem(elem) {
    var placeholder = $(elem).attr("data-select2-placeholder"),
        url = $(elem).attr("data-select2-url"),
        is_materialize = $(elem).attr("data-is-materialize") === "true",
        callback_init_data = $(elem).attr("data-js-callback-init-data"),
        callback_extra_request_params = $(elem).attr("data-js-callback-extra-request-params");

    if (placeholder === undefined)
        placeholder = "Pesquisar ...";

    if (url === undefined)
        ulr = "";

    try {
        if (is_materialize) {
            var info_materialize = materialize_ajustes(elem);
            placeholder = info_materialize.label_text;
        }

        aplicar_select2(elem, placeholder, url, callback_extra_request_params);

        if (is_materialize) {
            materialize_label_evento(elem, info_materialize.label_elem);
        }

        if (callback_init_data !== undefined) {
            var fn = window[callback_init_data];
            var data_init = fn();
            $(data_init).each(function () {
                try {
                    if (parseInt(this.id) > 0) {
                        $(elem).select2("trigger", "select", {
                            data: this
                        })
                    } else {
                        console.log(this, "select2: item inválido!");
                    }
                } catch (e) {
                    console.log(this, "select2: item inválido!");
                }
            });
        }

        form_submit_evento(elem);
    } catch (e) {
    }
}

function aplicar_select2(elem, placeholder, url, callback_extra_request_params) {
    $(elem).select2({
        placeholder: placeholder,
        allowClear: true,
        ajax: {
            url: url,
            dataType: 'json',
            data: function (params) {
                var query = {
                    searchTerm: params.term,
                    pageNumber: params.page || 1
                }

                if (callback_extra_request_params !== undefined) {
                    var fn = window[callback_extra_request_params];
                    var extra_query = fn(elem);
                    for (var param_name in extra_query) {
                        var param_value = extra_query[param_name];
                        query[param_name] = param_value;
                    }
                }

                return query;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.results,
                    pagination: {
                        more: (params.page * data.pageSize) < data.totalResults
                    }
                };
            }
        },
        width: "100%",
        "language": {
            "searching": function () {
                return "Pesquisando ...";
            },
            "noResults": function () {
                return "Nenhum resultado encontrado";
            },
            "errorLoading": function () {
                return "Erro ao carregar o resultado";
            },
            "loadingMore": function () {
                return "Carregando mais resultados ...";
            }
        },
        escapeMarkup: function (markup) {
            return markup;
        }
    });
}

function materialize_ajustes(elem) {
    var materialize_select_wrapper = $(elem).parent();

    // -------------------------
    // label estratégia 1
    var label_elem = $("label[for='" + $(elem).attr("id") + "']");

    if (label_elem.length == 0) {
        // label estratégia 2
        label_elem = $(elem).parent().find("label:first");

        if (label_elem.length == 0) {
            // label estratégia 3
            label_elem = $(elem).parent().parent().find("label:first");
        }
    }

    if (label_elem.length == 0)
        console.log("select2: não foi encontrado um label para o id '" + $(elem).attr("id") + "'");
    // -------------------------

    $(materialize_select_wrapper).find("input.select-dropdown").hide();
    $(materialize_select_wrapper).find("ul.select-dropdown").hide();
    $(materialize_select_wrapper).find("svg.caret").hide();

    return {
        label_elem: label_elem,
        label_text: $(label_elem).html()
    }
}

function materialize_label_evento(elem, label_elem) {
    $(label_elem).hide();

    $(label_elem).css("font-size", "1rem");
    $(label_elem).css("top", "-1px");

    $(elem).parent().find(".select2-search--inline input.select2-search__field").focusin(function () {
        $(this).attr("placeholder", "");
        $(label_elem).show();
        $(label_elem).addClass("active");
    });

    $(elem).parent().find(".select2-search--inline input.select2-search__field").focusout(function () {
        $(label_elem).hide();
        $(label_elem).removeClass("active");
        $(this).attr("placeholder", $(label_elem).html());
    });

    $(elem).on("select2:select", function (e) {
        $(this).attr("placeholder", "");
        $(label_elem).show();
        $(label_elem).addClass("active");
    });

    $(elem).on("select2:unselect", function (e) {
        if ($(elem).val().length === 0) {
            $(label_elem).hide();
            $(label_elem).removeClass("active");
            $(this).attr("placeholder", $(label_elem).html());
        }
    });
}

function form_submit_evento(elem) {
    var data_mapper_attr_id = $(elem).attr("data-mapper-attr-id"),
        data_mapper_attr_text = $(elem).attr("data-mapper-attr-text"),
        elem_name = $(elem).attr("name"),
        elem_form = $(elem)[0].form,
        select_is_multiple = $(elem).prop("multiple");

    $(elem_form).submit(function () {
        var seq = 0;
        $(elem_form).find("input[data-name=" + elem_name + "]").remove();
        $(elem).attr("name", elem_name + "___");
        $(elem).find("option:selected").each(function () {
            var item_value = $(this).attr("value"),
                item_text = $(this).text();

            if (select_is_multiple) {
                $(elem_form).append("<input data-name='" + elem_name + "' type='hidden' name='" + elem_name + "[" + seq + "][" + data_mapper_attr_id + "]' value='" + item_value + "'/>");
                $(elem_form).append("<input data-name='" + elem_name + "' type='hidden' name='" + elem_name + "[" + seq + "][" + data_mapper_attr_text + "]' value='" + item_text + "'/>");
            } else {
                $(elem_form).append("<input data-name='" + elem_name + "' type='hidden' id='" + data_mapper_attr_id + "' name='" + data_mapper_attr_id + "' value='" + item_value + "'/>");
                $(elem_form).append("<input data-name='" + elem_name + "' type='hidden' id='" + data_mapper_attr_text + "' name='" + data_mapper_attr_text + "' value='" + item_text + "'/>");
            }

            seq++;

            if (!select_is_multiple && seq === 1)
                return false; // select single tem apenas uma seleção
        });
    });
}