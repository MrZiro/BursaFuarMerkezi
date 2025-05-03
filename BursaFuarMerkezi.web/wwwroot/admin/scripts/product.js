function initializeDataTable(entity, columns, tableId = '#tblData') {
    const $table = $(tableId);
    const baseUrl = `/admin/${entity}`;

    // Create thead and headers
    const $thead = $(`${tableId} thead`);
    const $headerRow = $('<tr>').appendTo($thead);

    // Add columns including the Actions column
    columns.concat({ title: 'Actions', width: '25%' }).forEach(col => {
        $headerRow.append(
            $('<th>').css('width', col.width).text(col.title)
        );
    });

    // Initialize DataTable with modified options
    dataTables = $table.DataTable({
        ajax: { url: `${baseUrl}/getall` },
        order: [], // Prevents default sorting
        createdRow: function (row, data, dataIndex) {
            console.log(data);
            console.log(row);
            // Add id and class to each row
            $(row).attr('id', 'row-' + data.id);
            $(row).addClass('data-row');
        },
        columns: columns.concat({
            data: 'id',
            render: function (data) {
                return `<div class="w-75 btn-group" role="group">
                    <a href="${baseUrl}/upsert?id=${data}" class="btn btn-primary mx-2">
                        <i class="bi bi-pencil-square"></i> Edit
                    </a>
                    <button class="btn btn-danger mx-2" 
                            onclick="deleteItem('${entity}', ${data})">
                        <i class="bi bi-trash-fill"></i> Delete
                    </button>
                </div>`;
            },
            width: "25%",
            orderable: false
        }),
        //processing: true,
        //pageLength: 10,
        //responsive: true,

    });
}

// Usage remains the same
$(document).ready(() => {
    initializeDataTable('product', [
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
            title: 'List Price',
            data: 'listPrice',
            width: '10%'
        },
        {
            title: 'Author',
            data: 'author',
            width: '15%'
        },
        {
            title: 'Category',
            data: 'category.name',
            width: '10%'
        }
    ]);
});