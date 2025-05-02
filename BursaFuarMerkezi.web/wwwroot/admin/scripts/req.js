function deleteRow(id, url) {
    console.log("triggered")
    if (!confirm("Are you sure you want to delete this category?")) {
        return;
    }


    $.ajax({
        url: url,
        type: 'POST',
        data: {
            id: id
        },
        success: function (res) {
            if (res.success) {
                // Option A: remove the row in-place:
                $('#row-' + id).fadeOut(300, function () { $(this).remove(); });
                // Option B: or simply reload the page:
                // location.reload();
                // 2) build & show the Bootstrap alert
                const alertHtml = `
                          <div class="alert alert-success alert-dismissible fade show" role="alert">
                            ${res.message}
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                          </div>`;

                // inject into the placeholder
                // $('#alert-placeholder').html(alertHtml);

                // optional: auto‐dismiss after 3 seconds
                // setTimeout(() => {
                //   $('.alert').alert('close');
                // }, 2500);

            } else {
                alert("Delete failed: " + res.message);
            }
        },
        error: function () {
            alert("An error occurred while trying to delete.");
        }
    });
} 