document.addEventListener('DOMContentLoaded', function() {
  // Modal elements
  const termsModal = document.getElementById('termsModal');
  const privacyModal = document.getElementById('privacyModal');
  const kvkkModal = document.getElementById('kvkkModal');
  const closeTermsModal = document.getElementById('closeTermsModal');
  const closePrivacyModal = document.getElementById('closePrivacyModal');
  const closeKvkkModal = document.getElementById('closeKvkkModal');

  // Get the footer links
  const termsLink = document.getElementById('termsLink');
  const privacyLink = document.getElementById('privacyLink');
  const kvkkLink = document.getElementById('kvkkLink');

  // KVKK Modal for Form (kvkk-if)
  const kvkkModalIf = document.getElementById('kvkkModal-if');
  const closeKvkkModalIf = document.getElementById('closeKvkkModal-if');
  const acceptKvkkModalIf = document.getElementById('acceptKvkkModal-if');
  const termsCheckboxIf = document.getElementById('terms-if');
  const termsLabelIf = document.querySelector('label[for="terms-if"]');

  // Function to open modal
  function openModal(modal) {
    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden'; // Prevent background scrolling
  }

  // Function to close modal
  function closeModal(modal) {
    modal.style.display = 'none';
    document.body.style.overflow = 'auto'; // Enable background scrolling
  }

  // Event listeners for opening modals
  if (termsLink) {
    termsLink.addEventListener('click', (e) => {
      e.preventDefault();
      openModal(termsModal);
    });
  }

  if (privacyLink) {
    privacyLink.addEventListener('click', (e) => {
      e.preventDefault();
      openModal(privacyModal);
    });
  }

  if (kvkkLink) {
    kvkkLink.addEventListener('click', (e) => {
      e.preventDefault();
      openModal(kvkkModal);
    });
  }

  // Event listeners for closing modals
  if (closeTermsModal) {
    closeTermsModal.addEventListener('click', () => {
      closeModal(termsModal);
    });
  }

  if (closePrivacyModal) {
    closePrivacyModal.addEventListener('click', () => {
      closeModal(privacyModal);
    });
  }

  if (closeKvkkModal) {
    closeKvkkModal.addEventListener('click', () => {
      closeModal(kvkkModal);
    });
  }

  // Close modal when clicking outside
  window.addEventListener('click', (e) => {
    if (e.target === termsModal) {
      closeModal(termsModal);
    }
    if (e.target === privacyModal) {
      closeModal(privacyModal);
    }
    if (e.target === kvkkModal) {
      closeModal(kvkkModal);
    }
  });

  // Close modal when pressing Escape key
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      closeModal(termsModal);
      closeModal(privacyModal);
      closeModal(kvkkModal);
    }
  });

  function openKvkkModalIf() {
    kvkkModalIf.style.display = 'flex';
    document.body.style.overflow = 'hidden';
  }
  function closeKvkkModalIfFunc() {
    kvkkModalIf.style.display = 'none';
    document.body.style.overflow = 'auto';
  }

  if (termsCheckboxIf) {
    termsCheckboxIf.addEventListener('click', function(e) {
      e.preventDefault();
      openKvkkModalIf();
    });
  }
  if (termsLabelIf) {
    termsLabelIf.addEventListener('click', function(e) {
      e.preventDefault();
      openKvkkModalIf();
    });
  }
  if (closeKvkkModalIf) {
    closeKvkkModalIf.addEventListener('click', function() {
      closeKvkkModalIfFunc();
    });
  }
  if (acceptKvkkModalIf) {
    acceptKvkkModalIf.addEventListener('click', function() {
      termsCheckboxIf.checked = true;
      closeKvkkModalIfFunc();
    });
  }
  // Modal dışına tıklayınca kapansın
  window.addEventListener('click', function(e) {
    if (e.target === kvkkModalIf) {
      closeKvkkModalIfFunc();
    }
  });
  // Escape ile kapansın
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape' && kvkkModalIf.style.display === 'flex') {
      closeKvkkModalIfFunc();
    }
  });

  // --- GÖNDER butonunu KVKK kabul edilmeden pasif yap ---
  function updateSubmitButtonState() {
    const submitBtn = document.querySelector('button[type="submit"].btn-submit');
    if (submitBtn && termsCheckboxIf) {
      submitBtn.disabled = !termsCheckboxIf.checked;
      if (submitBtn.disabled) {
        submitBtn.classList.add('btn-submit-disabled');
      } else {
        submitBtn.classList.remove('btn-submit-disabled');
      }
    }
  }
  // Sayfa yüklenince ve checkbox değişince kontrol et
  updateSubmitButtonState();
  if (termsCheckboxIf) {
    termsCheckboxIf.addEventListener('change', updateSubmitButtonState);
  }
  // Modal kabulünde de kontrol et
  if (acceptKvkkModalIf) {
    acceptKvkkModalIf.addEventListener('click', function() {
      updateSubmitButtonState();
    });
  }
});
