
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

// Modal elementlerini seç
const modalOverlay = document.getElementById('modalOverlay');
const openModalBtn = document.getElementById('openModalBtn');
const closeModalBtn = document.getElementById('closeModalBtn');

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
