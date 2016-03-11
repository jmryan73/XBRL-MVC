$(function () {
    $("#grid").jqGrid({
        url: "/TechnicalValidation/GetLists",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['ID', 'Parent', 'Child', 'Prefix', 'Description', 'Sandi', 'Path'],
        colModel: [
            { key: true, hidden: true, name: 'ID', index: 'ID', editable: true },
            { key: false, name: 'parent', index: 'parent', editable: true },
            { key: false, name: 'child', index: 'child', editable: true },
            { key: false, name: 'Prefix', index: 'Prefix', editable: true /*, formatter: 'date', formatoptions: { newformat: 'd/m/Y' } */},
            { key: false, name: 'Description', index: 'Description', editable: true /*, edittype: 'select', editoptions: { value: { 'L': 'Low', 'M': 'Medium', 'H': 'High' } } */ },
            { key: false, name: 'Sandi', index: 'Sandi', editable: true /*, edittype: 'select', editoptions: { value: { 'A': 'Active', 'I': 'InActive' } } */ },
            { key: false, name: 'path', index: 'path', editable: true },
        ],

        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        viewrecords: true,
        caption: 'List Form 01',
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false
    }).navGrid('#pager', {
        edit: true,
        editicon: 'ace-icon fa fa-pencil blue',
        add: true,
        addicon: 'ace-icon fa fa-plus-circle purple',
        del: true,
        delicon: 'ace-icon fa fa-trash-o red',
        search: true,
        searchicon: 'ace-icon fa fa-search orange',
        refresh: true,
        refreshicon: 'ace-icon fa fa-refresh green',
        view: true,
        viewicon: 'ace-icon fa fa-search-plus grey',
    },
        {
            // edit options
            //zIndex: 100,
            url: '/TodoList/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // add options
            //zIndex: 100,
            url: "/TodoList/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // delete options
            //zIndex: 100,
            url: "/TodoList/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete this task?",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});