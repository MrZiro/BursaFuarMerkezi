/*=============== Navbar js =============================== */
document.addEventListener('DOMContentLoaded', () => {
  const menuToggleButtons = document.querySelectorAll('.nav-toggle-button'); // Menüyü açan/kapatan tüm butonlar/linkler
  const closeMenuButton = document.querySelector('.close-menu-btn');
  const fullscreenMenu = document.querySelector('.fullscreen-menu');
  const body = document.body;
  // Menü içindeki başlıklar (bunlara tıklanınca da kapanması için) - İsteğe bağlı
  const menuTitleLinks = document.querySelectorAll('.fullscreen-menu .menu-column h3');

  // Menüyü Açma Fonksiyonu
  const openMenu = () => {
      if (!fullscreenMenu.classList.contains('active')) {
          fullscreenMenu.classList.add('active');
          body.classList.add('menu-open'); // Scroll engelleme için body'e class ekle
      }
  };

  // Menüyü Kapatma Fonksiyonu
  const closeMenu = () => {
      if (fullscreenMenu.classList.contains('active')) {
          fullscreenMenu.classList.remove('active');
          body.classList.remove('menu-open'); // Scroll engellemeyi kaldır
      }
  };

  // Menüyü Aç/Kapat Butonlarına Tıklama Olayı
  menuToggleButtons.forEach(button => {
      button.addEventListener('click', (e) => {
          e.preventDefault(); // Linklerin (#) sayfayı yukarı atmasını engelle

          // Eğer tıklanan buton mobil toggle ise veya menü kapalıysa, menünün durumunu değiştir.
          // Eğer menü zaten açıksa ve tıklanan normal bir menü linkiyse, sadece kapat.
          if (button.classList.contains('mobile-menu-toggle')) {
               if (fullscreenMenu.classList.contains('active')) {
                  closeMenu();
               } else {
                  openMenu();
               }
          } else if (button.closest('.fullscreen-menu') && fullscreenMenu.classList.contains('active')){
               // Eğer açık menü içindeki bir başlığa tıklandıysa kapat
               closeMenu();
          }
          else {
              // Diğer tüm durumlarda (ana navbardaki linkler, arama ikonu) menüyü aç
              openMenu();
          }
      });
  });

  // Kapatma (X) Butonuna Tıklama Olayı
  if (closeMenuButton) {
      closeMenuButton.addEventListener('click', () => {
          closeMenu();
      });
  }

  // İsteğe Bağlı: Esc tuşu ile menüyü kapatma
  document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && fullscreenMenu.classList.contains('active')) {
          closeMenu();
      }
  });
});

/*================== Card-Slider-1 Js ======================*/
document.addEventListener('DOMContentLoaded', function() {
  const slider = document.querySelector('.slider');
  
  // Skip if this is the upcoming fairs slider (managed by home.js)
  if (slider && slider.closest('.card-1-section')) {
    return;
  }
  
  const cards = document.querySelectorAll('.card');
  const prevBtn = document.querySelector('.prev-btn');
  const nextBtn = document.querySelector('.next-btn');
  const progressFill = document.querySelector('.progress-fill');
  
  let currentIndex = 0;
  const cardWidth = cards[0].offsetWidth + 20; // 20px for gap
  const visibleCards = getVisibleCards();
  const maxIndex = cards.length - visibleCards;
  
  // İlk yüklemede ilerleme çubuğunu ayarla
  updateProgressBar();
  
  // Görünür kart sayısını hesapla (responsive için)
  function getVisibleCards() {
      const containerWidth = slider.parentElement.offsetWidth;
      if (containerWidth >= 992) {
          return 3; // Büyük ekranlarda 3 kart
      } else if (containerWidth >= 768) {
          return 2; // Orta ekranlarda 2 kart
      } else {
          return 1; // Küçük ekranlarda 1 kart
      }
  }
  
  // İlerleme çubuğunu güncelle
  function updateProgressBar() {
      const progress = (currentIndex / maxIndex) * 100;
      progressFill.style.width = `${progress}%`;
  }
  
  // Slider'ı güncelle
  function updateSlider() {
      slider.style.transform = `translateX(-${currentIndex * cardWidth}px)`;
      updateProgressBar();
  }
  
  // Önceki karta git
  prevBtn.addEventListener('click', function() {
      if (currentIndex > 0) {
          currentIndex--;
          updateSlider();
      }
  });
  
  // Sonraki karta git
  nextBtn.addEventListener('click', function() {
      if (currentIndex < maxIndex) {
          currentIndex++;
          updateSlider();
      }
  });
  
  // Doknuma olayları için destek
  let touchStartX = 0;
  let touchEndX = 0;
  
  slider.addEventListener('touchstart', function(e) {
      touchStartX = e.changedTouches[0].screenX;
  });
  
  slider.addEventListener('touchend', function(e) {
      touchEndX = e.changedTouches[0].screenX;
      handleSwipe();
  });
  
  function handleSwipe() {
      const swipeThreshold = 50;
      if (touchEndX < touchStartX - swipeThreshold) {
          // Sola kaydırma
          if (currentIndex < maxIndex) {
              currentIndex++;
              updateSlider();
          }
      } else if (touchEndX > touchStartX + swipeThreshold) {
          // Sağa kaydırma
          if (currentIndex > 0) {
              currentIndex--;
              updateSlider();
          }
      }
  }
  
  // Pencere boyutu değiştiğinde slider'ı güncelle
  window.addEventListener('resize', function() {
      const newVisibleCards = getVisibleCards();
      const newMaxIndex = cards.length - newVisibleCards;
      
      // Eğer mevcut indeks yeni maksimum indeksten büyükse, güncelle
      if (currentIndex > newMaxIndex) {
          currentIndex = newMaxIndex;
      }
      
      // Kart genişliğini yeniden hesapla
      const newCardWidth = cards[0].offsetWidth + 20;
      
      // Slider'ı güncelle
      slider.style.transform = `translateX(-${currentIndex * newCardWidth}px)`;
      updateProgressBar();
  });

  // Yeni değişkenler ekleyin
  let isDragging = false;
  let startPosX = 0;
  let currentTranslate = 0;
  let prevTranslate = 0;
  let animationID = 0;
  // Autoplay variables
  let autoplayIntervalId = null;
  const AUTOPLAY_DELAY_MS = 1500;

  function stopAutoplay() {
      if (autoplayIntervalId) {
          clearInterval(autoplayIntervalId);
          autoplayIntervalId = null;
      }
  }

  function startAutoplay() {
      stopAutoplay();
      autoplayIntervalId = setInterval(() => {
          if (isDragging) return;
          if (currentIndex < maxIndex) {
              currentIndex++;
              updateSlider();
          } else {
              // Jump back to start without animation, then continue
              slider.style.transition = 'none';
              currentIndex = 0;
              currentTranslate = 0;
              slider.style.transform = `translateX(${currentTranslate}px)`;
              updateProgressBar();
              // Force reflow, then restore transition for next moves
              void slider.offsetHeight;
              slider.style.transition = 'transform 0.5s ease';
          }
      }, AUTOPLAY_DELAY_MS);
  }

  // Slider için mousedown event listener ekleyin
  slider.addEventListener('mousedown', dragStart);
  slider.addEventListener('mouseup', dragEnd);
  slider.addEventListener('mouseleave', dragEnd);
  slider.addEventListener('mousemove', drag);
  slider.addEventListener('mouseenter', stopAutoplay);
  slider.addEventListener('mouseleave', startAutoplay);

  // Dokunmatik event listener'ları güncelleyin
  slider.addEventListener('touchstart', function(e) {
      touchStartX = e.touches[0].clientX;
      dragStart(e.touches[0]);
  });
  
  slider.addEventListener('touchend', dragEnd);
  slider.addEventListener('touchmove', function(e) {
      touchEndX = e.touches[0].clientX;
      drag(e.touches[0]);
  });

  function dragStart(e) {
      isDragging = true;
      startPosX = e.clientX;
      prevTranslate = currentTranslate;
      
      // Animasyonu durdur ve transition'ı kapat
      slider.style.transition = 'none';
      cancelAnimationFrame(animationID);
      stopAutoplay();
  }

  function drag(e) {
      if(isDragging) {
          const currentPosX = e.clientX;
          const deltaX = currentPosX - startPosX;
          currentTranslate = prevTranslate + deltaX;
          
          // Gerçek zamanlı slider pozisyonunu güncelle
          slider.style.transform = `translateX(${currentTranslate}px)`;
      }
  }

  function dragEnd() {
      if(!isDragging) return;
      isDragging = false;
      
      // Transition'ı yeniden etkinleştir
      slider.style.transition = 'transform 0.5s ease';
      
      // Minimum kaydırma mesafesi
      const movedBy = currentTranslate - prevTranslate;
      const threshold = cardWidth * 0.2; // %20 kaydırma eşiği
      
      if(movedBy < -threshold && currentIndex < maxIndex) {
          currentIndex++;
      } else if(movedBy > threshold && currentIndex > 0) {
          currentIndex--;
      }
      
      // Slider'ı son pozisyona animasyonla getir
      updateSlider();
      startAutoplay();
  }

  // Mevcut updateSlider fonksiyonunu güncelleyin
  function updateSlider() {
      currentTranslate = -currentIndex * cardWidth;
      slider.style.transform = `translateX(${currentTranslate}px)`;
      updateProgressBar();
  }

  // Start autoplay after initial setup
  startAutoplay();

  // ... mevcut diğer fonksiyonlar ...
  
  // Pencere boyutu değiştiğinde currentTranslate'ı güncelle
  window.addEventListener('resize', function() {
      // ... mevcut resize kodu ...
      currentTranslate = -currentIndex * newCardWidth;
  });
});

/*====== index.html js end =============*/

/*====== iletisim.html js start ====*/
// Wrap in DOMContentLoaded if this script might run before the form exists
document.addEventListener('DOMContentLoaded', () => {
    const contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', function(e) {
            e.preventDefault();

            // CAPTCHA doğrulama
            const captchaTextEl = document.getElementById('captchaText');
            const captchaInputEl = document.getElementById('captchaInput');
            if (!captchaTextEl || !captchaInputEl) {
                console.error("Captcha elements not found");
                alert('CAPTCHA elements not found!');
                return;
            }
            const captchaText = captchaTextEl.textContent;
            const captchaInput = captchaInputEl.value;


            if (captchaText !== captchaInput) {
                alert('CAPTCHA kodu hatalı!');
                return;
            }

            // Form verilerini al
            const name = document.getElementById('name').value;
            const phone = document.getElementById('phone').value;
            const email = document.getElementById('email').value;
            const message = document.getElementById('message').value;

            // Normalde burada bir AJAX isteği ile form verileri sunucuya gönderilir
            console.log('Form gönderildi:', { name, phone, email, message });
            alert('Mesajınız başarıyla gönderildi!');

            // Formu sıfırla
            this.reset();
             // Optionally refresh captcha after successful submission
             refreshCaptcha();
        });
    }

     // CAPTCHA yenileme
     const refreshButton = document.querySelector('.captcha-refresh');
     if (refreshButton) {
         refreshButton.addEventListener('click', refreshCaptcha);
         // Initial captcha generation
         refreshCaptcha();
     }

     function refreshCaptcha() {
        const captchaTextEl = document.getElementById('captchaText');
        if(!captchaTextEl) return;
        // Rastgele CAPTCHA kodu oluştur
        const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        let captcha = '';
        for (let i = 0; i < 5; i++) {
            captcha += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        captchaTextEl.textContent = captcha;
        // Clear previous input
        const captchaInputEl = document.getElementById('captchaInput');
        if(captchaInputEl) captchaInputEl.value = '';
     }
});

/*================ Hakkımızda Js ===============*/

// Header scroll effect
window.addEventListener('scroll', function() {
  const header = document.querySelector('.main-header');
  if (window.scrollY > 50) {
    header.classList.add('scrolled');
  } else {
    header.classList.remove('scrolled');
  }
});


/* Search Functionality */
document.addEventListener('DOMContentLoaded', function () {
    // Arama butonunu seç
    const searchToggleButton = document.querySelector('.search-toggle-button');
    const searchOverlay = document.querySelector('.search-overlay');
    const searchClose = document.querySelector('.search-close');
    const searchInput = document.querySelector('.search-input');
  
    // Arama butonuna tıklandığında
    searchToggleButton.addEventListener('click', function () {
      searchOverlay.classList.add('active');
      searchInput.focus();
    });
  
    // Kapatma butonuna tıklandığında
    searchClose.addEventListener('click', function () {
      searchOverlay.classList.remove('active');
    });
  
    // ESC tuşuna basıldığında
    document.addEventListener('keydown', function (e) {
      if (e.key === 'Escape' && searchOverlay.classList.contains('active')) {
        searchOverlay.classList.remove('active');
      }
    });
  
    // Arama çubuğuna tıklandığında overlay'i kapatma
    searchOverlay.addEventListener('click', function (e) {
      if (e.target === searchOverlay) {
        searchOverlay.classList.remove('active');
      }
    });
  });