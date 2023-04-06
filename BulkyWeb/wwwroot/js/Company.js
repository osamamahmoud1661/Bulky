var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Company/GetAll' }
        ,
        "columns":
            [
                { data: 'name', "width": "25%" },
                { data: 'streatAddress', "width": "15%" },
                { data: 'city', "width": "10%" },
                { data: 'state', "width": "20%" },
                { data: 'phoneNumber', "width": "10%" },
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
                    datatable.ajax.reload();
                    toastr.success(data.message);
                }
                })
        }
    })
}
