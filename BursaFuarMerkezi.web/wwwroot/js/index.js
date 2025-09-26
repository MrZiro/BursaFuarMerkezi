
const buttons = document.querySelectorAll('#pagination .page-button');

buttons.forEach(button => {
  button.addEventListener('click', () => {
    buttons.forEach(btn => {
      btn.classList.remove('btn-danger');
      btn.classList.add('btn-outline-danger');
    });

    button.classList.remove('btn-outline-danger');
    button.classList.add('btn-danger');
  });
});

// Modal functionality - only if elements exist
document.addEventListener('DOMContentLoaded', function() {
  const modalOverlay = document.getElementById('modalOverlay');
  const openModalBtn = document.getElementById('openModalBtn');
  const closeModalBtn = document.getElementById('closeModalBtn');

  // Only set up modal functionality if elements exist
  if (modalOverlay && openModalBtn && closeModalBtn) {
    // Modal açma fonksiyonu
    function openModal() {
      modalOverlay.style.display = 'flex';
      document.body.style.overflow = 'hidden'; // Sayfa kaydırmayı engelle
    }

    // Modal kapatma fonksiyonu
    function closeModal() {
      modalOverlay.style.display = 'none';
      document.body.style.overflow = 'auto'; // Sayfa kaydırmayı etkinleştir
    }

    // Buton tıklama olayları
    openModalBtn.addEventListener('click', openModal);
    closeModalBtn.addEventListener('click', closeModal);

    // Modal dışına tıklayınca kapatma
    modalOverlay.addEventListener('click', function (event) {
      if (event.target === modalOverlay) {
        closeModal();
      }
    });

    // ESC tuşuna basınca kapatma
    document.addEventListener('keydown', function (event) {
      if (event.key === 'Escape') {
        closeModal();
      }
    });
  }
});

// Language dropdown functionality
document.addEventListener('DOMContentLoaded', function() {
  const langDropdown = document.querySelector('.lang-dropdown');
  const langDropdownBtn = document.getElementById('langDropdownBtn');
  
  console.log('Language dropdown elements:', { langDropdown, langDropdownBtn }); // Debug log
  
  if (langDropdownBtn && langDropdown) {
    console.log('Setting up language dropdown functionality'); // Debug log
    
    // Toggle dropdown on button click
    langDropdownBtn.addEventListener('click', function(e) {
      e.preventDefault();
      e.stopPropagation();
      console.log('Language dropdown clicked'); // Debug log
      langDropdown.classList.toggle('active');
    });
    
    // Close dropdown when clicking outside
    document.addEventListener('click', function(e) {
      if (!langDropdown.contains(e.target)) {
        langDropdown.classList.remove('active');
      }
    });
    
    // Close dropdown when pressing ESC
    document.addEventListener('keydown', function(e) {
      if (e.key === 'Escape') {
        langDropdown.classList.remove('active');
      }
    });
  } else {
    console.log('Language dropdown elements not found'); // Debug log
  }
});