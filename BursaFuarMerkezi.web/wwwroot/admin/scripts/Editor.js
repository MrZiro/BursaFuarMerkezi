tinymce.init({
    selector: '.Editor',
    plugins: [
        'table'
    ],
    toolbar: 'undo redo | bold italic | table | tablecellverticalalign',
    menu: {
        edit: { title: 'Edit', items: 'undo redo | selectall' },
        table: { title: 'Table', items: 'inserttable | cell row column | tablecellvalign | deletetable' }
    }
});