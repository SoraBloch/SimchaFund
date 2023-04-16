﻿$(() => {
    $("#new-contributor").on('click', function () {
        new bootstrap.Modal($('.new-contrib')[0]).show();
    })
    $(".deposit-button").on('click', function () {
        const id = $(this).data('contribid');
        $("#contributor-id").val(id);

        new bootstrap.Modal($('.deposit')[0]).show();
    })
    $(".edit-contrib").on('click', function () {
        const id = $(this).data('id');
        const firstName = $(this).data('first-name');
        const lastName = $(this).data('last-name');
        const cell = $(this).data('cell');
        const alwaysInclude = $(this).data('always-include');
        const date = $(this).data('date');
        console.log(id);
        $("#edit_id").val(id);
        $("#edit_first_name").val(firstName);
        $("#edit_last_name").val(lastName);
        $("#edit_cell_number").val(cell);
        $("#edit_always_include").prop('checked', alwaysInclude === "True");
        $("#edit_created_at").val(date);

        new bootstrap.Modal($('.edit-contributor')[0]).show();
    });
    //$(".edit-contrib").on('click', function () {
    //    const id = $(this).data('id');
    //    const firstName = $(this).data('first-name');
    //    const lastName = $(this).data('last-name');
    //    const cell = $(this).data('cell');
    //    const alwaysInclude = $(this).data('always-include');
    //    const date = $(this).data('date');
    //    console.log(date);
    //    $("#contributor_first_name").val(firstName);
    //    $("#contributor_last_name").val(lastName);
    //    $("#contributor_cell_number").val(cell);
    //    $("#contributor_created_at").val(date);
    //    if (alwaysInclude == "True") {
    //        $("#contributor_always_include").prop('checked', true);
    //    }
    //    console.log(alwaysInclude);
    //    $(".modal-title").text("Edit");
    //    $("#initialDepositDiv").hide();
    //    const form = $(".new-contrib form")
    //    form.attr("action", "/home/edit");
    //    form.append(`<input type="hidden" name="id" value="${id}"/>`)
    //    new bootstrap.Modal($('.new-contrib')[0]).show();
    //});

});