var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}



//// jQuery Code
//$(document).ready(function () {
//    loadData();
//    setupSearch();
//});

//function loadData() {
//    $.ajax({
//        url: '/admin/product/getall',
//        type: 'GET',
//        success: function (response) {
//            renderTable(response.data);
//        },
//        error: function (xhr) {
//            toastr.error('حدث خطأ أثناء جلب البيانات');
//        }
//    });
//}

//function renderTable(data) {
//    const $tbody = $('#tblData tbody');
//    $tbody.empty();

//    $.each(data, function (index, item) {
//        const row = `
//            <tr>
//                <td>${item.title}</td>
//                <td>${item.isbn}</td>
//                <td>${item.listPrice}</td>
//                <td>${item.author}</td>
//                <td>${item.category.name}</td>
//                <td>
//                    <div class="w-75 btn-group" role="group">
//                        <a href="/admin/product/upsert?id=${item.id}" class="btn btn-primary mx-2">
//                            <i class="bi bi-pencil-square"></i> تعديل
//                        </a>
//                        <button onclick="Delete('/admin/product/delete/${item.id}')" class="btn btn-danger mx-2">
//                            <i class="bi bi-trash-fill"></i> حذف
//                        </button>
//                    </div>
//                </td>
//            </tr>
//        `;
//        $tbody.append(row);
//    });
//}

//function Delete(url) {
//    Swal.fire({
//        title: 'هل أنت متأكد؟',
//        text: "لن تتمكن من التراجع عن هذا!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'نعم، احذف!',
//        cancelButtonText: 'إلغاء'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    loadData(); // إعادة تحميل البيانات
//                    toastr.success(data.message);
//                },
//                error: function (xhr) {
//                    toastr.error('حدث خطأ أثناء الحذف');
//                }
//            });
//        }
//    });
//}

//// وظيفة البحث
//function setupSearch() {
//    $('#searchInput').on('keyup', function () {
//        const value = $(this).val().toLowerCase();
//        $('#tblData tbody tr').filter(function () {
//            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
//        });
//    });
//}