class DynamicTable {
    constructor(config) {
        this.config = {
            tableId: '#tblData',
            apiUrl: 'api/',
            entity: '',
            columns: [],
            ...config
        };

        this.init();
    }

    init() {
        this.loadData();
        //this.initSearch();
    }

    // جلب البيانات من السيرفر
    loadData() {
        $.ajax({
            url: `${this.config.apiUrl}${this.config.entity}`,
            type: 'GET',
            success: (response) => this.render(response.data),
            error: () => toastr.error('Failed to load data')
        });
    }

    // عرض البيانات في الجدول
    render(data) {
        this.createHeaders();
        this.createBody(data);
    }

    // إنشاء عناوين الجدول
    createHeaders() {
        const $header = $(`${this.config.tableId} thead tr`);
        $header.empty();

        this.config.columns.forEach(col => {
            $header.append(`<th style="width:${col.width}">${col.title}</th>`);
        });
    }

    // إنشاء محتوى الجدول
    createBody(data) {
        const $tbody = $(`${this.config.tableId} tbody`);
        $tbody.empty();

        data.forEach(item => {
            const row = $('<tr>');
            this.config.columns.forEach(col => {
                const cell = col.render
                    ? col.render(item)
                    : this.getNestedValue(item, col.data);
                row.append(`<td>${cell}</td>`);
            });
            $tbody.append(row);
        });
    }

    //initSearch() {
    //    $('.table-search').on('keyup', (e) => {
    //        const value = $(e.target).val().toLowerCase();
    //        $(`${this.config.tableId} tbody tr`).filter(function () {
    //            $(this).toggle($(this).text().toLowerCase().includes(value));
    //        });
    //    });
    //}

    getNestedValue(obj, path) {
        return path.split('.').reduce((o, p) => o?.[p] ?? '', obj);
    }
}




$(document).ready(() => {
    myExampleFunction('product',
        [
            {
                title: 'Title',  
                data: 'title',
                width: '25%'
            },
            {
                title: 'ISBN',
                data: 'isbn',
                width: '15%'
            },
            {
                title: 'Price',
                data: 'listPrice',
                width: '10%'
            },
            {
                title: 'Category',
                data: 'category.name',
                width: '15%'
            },
            {
                title: 'Actions',
                width: '25%',
                render: (item) => `
                <a href="/admin/product/upsert?id=${item.id}" class="btn btn-primary">
                    <i class="bi bi-pencil-square"></i> Edit
                </a>

            `
            }
        ]
    )
    
});

function myExampleFunction(controller, data) {
    new DynamicTable({
        entity: `${controller}/getall`,
        columns: data,
    })
}