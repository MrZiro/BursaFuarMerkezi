function populateTable(users) {
    const tbody = document.querySelector('#table-container tbody');
    tbody.innerHTML = users.map((user, index) => `
        <tr>
            <th scope="row">${index}</th>
            <td>${user.FirstName}</td>
            <td>${user.LastName}</td>
            <td class="text-center">
                <div class="btn-container">
                    <a href="#" class="btn btn-mine1 edit-btn" data-id="${user.Id}">
                        <span class="svv-2 material-symbols-rounded">edit</span>
                    </a>
                    <a href="#" class="btn btn-mine1 delete-btn" data-id="${user.Id}">
                        <span class="svv-2 material-symbols-rounded">delete</span>
                    </a>
                </div>
            </td>
        </tr>
    `).join('');
}


const sampleJsonData = [
    { Id: 1, FirstName: "Mark", LastName: "Otto" },
    { Id: 2, FirstName: "Jacob", LastName: "Thornton" },
    { Id: 3, FirstName: "Larry", LastName: "the Bird" },
];

populateTable(sampleJsonData);


// const addBtn = document.querySelector('.add-btn');
// addBtn.addEventListener('click', (e) => {
//     e.preventDefault();
//     console.log('add btn clicked');


// });



document.addEventListener('DOMContentLoaded', function () {
    const addBtn = document.querySelector('.add-btn a');
    const formContainer = document.querySelector('.form-container');
    const cancelBtn = document.querySelector('button[type="reset"]');
    const addPageForm = document.getElementById('addPageForm');

    // Show form when add button is clicked
    addBtn.addEventListener('click', function (e) {
        e.preventDefault();
        formContainer.classList.add('show');
    });

    // Hide form when cancel button is clicked
    cancelBtn.addEventListener('click', function () {
        formContainer.classList.remove('show');
    });

    // Close form when clicking outside of it
    formContainer.addEventListener('click', function (e) {
        if (e.target === formContainer) {
            formContainer.classList.remove('show');
        }
    });

    // Auto-generate slug from title
    const pageTitleInput = document.getElementById('pageTitle');
    const pageSlugInput = document.getElementById('pageSlug');

    pageTitleInput.addEventListener('input', function () {
        // Convert title to slug format
        const slug = this.value.toLowerCase()
            .replace(/[^\w\s-]/g, '') // Remove special characters
            .replace(/\s+/g, '-')     // Replace spaces with hyphens
            .replace(/-+/g, '-');     // Replace multiple hyphens with single hyphen

        pageSlugInput.value = slug;
    });

    // Form submission
    addPageForm.addEventListener('submit', function (event) {
        event.preventDefault();

        // Get form values
        const pageTitle = document.getElementById('pageTitle').value;
        const pageSlug = document.getElementById('pageSlug').value;
        const pageType = document.getElementById('pageType').value;
        const pageTemplate = document.getElementById('pageTemplate').value;
        const pageDescription = document.getElementById('pageDescription').value;
        const featuredImage = document.getElementById('featuredImage').files[0];
        const publishImmediately = document.getElementById('publishSwitch').checked;

        // Log form data (you would normally send this to a server)
        console.log('Page created:', {
            title: pageTitle,
            slug: pageSlug,
            type: pageType,
            template: pageTemplate,
            description: pageDescription,
            image: featuredImage ? featuredImage.name : 'No image selected',
            published: publishImmediately
        });

        // Show success message
        alert('Page created successfully!');

        // Hide form and reset it
        formContainer.classList.remove('show');
        this.reset();
    });
});