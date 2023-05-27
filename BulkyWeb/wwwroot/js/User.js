var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/User/GetAll' }
        ,
        "columns":
            [
                { data: 'name', "width": "25%" },
                { data: 'email', "width": "15%" },
                { data: 'phoneNumber', "width": "10%" },
                { data: 'company.name', "width": "20%" },
                { data: 'role', "width": "10%" },
                {
                    data: 'id',
                    "render": function (data) {
                        return `<div class="w-100 btn-group" role="group">
                                <a href="/admin/Company/upsert/${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a onClick=Delete('/admin/Company/delete/${data}') class="btn btn-danger mx-2">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                                </div>`
                    }
                    , "width": "25%" }
            ]
    });
}
