
 // Setup drag and drop image upload
 const dropzone = document.getElementById('image-dropzone');
 const imageInput = document.getElementById('featuredImageInput');
 const imagePreview = document.getElementById('featuredImagePreview');
 const previewContainer = document.querySelector('.img-container');
 const previewCaption = document.getElementById('preview-caption');
 const removeImageBtn = document.getElementById('removeImage');
 // Handle drag events
 ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
     dropzone.addEventListener(eventName, preventDefaults, false);
 });

 function preventDefaults(e) {
     e.preventDefault();
     e.stopPropagation();
 }

 ['dragenter', 'dragover'].forEach(eventName => {
     dropzone.addEventListener(eventName, highlight, false);
 });

 ['dragleave', 'drop'].forEach(eventName => {
     dropzone.addEventListener(eventName, unhighlight, false);
 });

 function highlight() {
     dropzone.classList.add('dragover');
 }

 function unhighlight() {
     dropzone.classList.remove('dragover');
 }

 // Handle file drop
 dropzone.addEventListener('drop', handleDrop, false);

 function handleDrop(e) {
     const dt = e.dataTransfer;
     const files = dt.files;
     
     if (files.length) {
         imageInput.files = files;
         handleFiles(files);
     }
 }

 // Handle file select via input
 dropzone.addEventListener('click', function() {
     imageInput.click();
 });

 imageInput.addEventListener('change', function() {
     if (this.files && this.files[0]) {
         handleFiles(this.files);
     }
 });

 function handleFiles(files) {
     const file = files[0];
     
     // Validate file type
     const fileType = file.type;
     const validImageTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
     
     if (!validImageTypes.includes(fileType)) {
         alert('Please select a valid image file (JPG, PNG, GIF, WEBP)');
         return;
     }
     
     // Validate file size (5MB)
     if (file.size > 5 * 1024 * 1024) {
         alert('Image size cannot exceed 5MB');
         return;
     }
     
     // Display preview
     const reader = new FileReader();
     reader.onload = function(e) {
         imagePreview.src = e.target.result;
         previewContainer.style.display = 'block';
         if (previewCaption) previewCaption.style.display = 'block';
     }
     reader.readAsDataURL(file);
 }