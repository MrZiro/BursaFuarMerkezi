/*============= Card-slider 2 JS ================= */
document.addEventListener('DOMContentLoaded', function() {
  const cardsData = {
    gundem: [
      { image: "./images/111.jpg", date: "2024-05-01", tag: "Gündem", title: "Rising City Yapı ve Yaşam Fuarı Kapılarını Açtı", link: "rising-city-blog-page.html" },
      { image: "./images/hometexblog.png", date: "2024-05-02", tag: "Gündem", title: "Tekstil sektörünün kalbi Bursa Textile Show Fuarı’nda attı.", link: "blog-single-page.html" },
      { image: "./images/222.jpg", date: "2024-05-02", tag: "Gündem", title: "Rising City & Yapı ve Yaşam Fuarı Sona Erdi", link: "rising-city-blog-2.html" },
      { image: "./images/card-2-slider-4.png", date: "2024-05-02", tag: "Gündem", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
      { image: "./images/card-2-slider-5.png", date: "2024-05-02", tag: "Gündem", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
      { image: "./images/card-2-slider-6.png", date: "2024-05-02", tag: "Gündem", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
      { image: "./images/card-2-slider-7.png", date: "2024-05-02", tag: "Gündem", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
    ],
    blog: [
      { image: "./images/card-2-slider-8.png", date: "2024-03-07", tag: "Blog", title: "Tekstil sektörünün kalbi Bursa Textile Show Fuarı'nda attı.", link: "blog-single-page.html" },
    ],
    duyurular: [
      { image: "./images/card-2-slider-10.jpg", date: "2024-05-04", tag: "Duyuru", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
      { image: "./images/card-2-slider-2.png", date: "2024-05-04", tag: "Duyuru", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
    ],
    haberler: [
      { image: "./images/card-2-slider-4.png", date: "2024-05-05", tag: "Haber", title: "Güneş Sektörünün Markalarını Dünya Liderleriyle Buluşturacak 17. SolarEX İstanbul İçin Geri Sayım Başladı!", link: "blog-single-page.html" },
    ]
  };

  let swiperInstance = null;
  const swiperWrapper = document.querySelector('.swiper-wrapper');
  const cardTemplate = document.getElementById('card-template-2');
  const categoryTabs = document.querySelectorAll('.category-tab-2');
  const cardSlider = document.querySelector('.card-slider-2');

  function formatDate(dateString) {
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return {
      year: date.getFullYear(),
      day: date.toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' })
    };
  }

  function createCards(category) {
    const fragment = document.createDocumentFragment();
    cardsData[category].forEach(data => {
      const clone = cardTemplate.content.cloneNode(true);
      const formattedDate = formatDate(data.date);
      
      clone.querySelector('img').src = data.image;
      clone.querySelector('.date-year-2').textContent = formattedDate.year;
      clone.querySelector('.date-day-2').textContent = formattedDate.day;
      clone.querySelector('.card-tag-2').textContent = data.tag;
      clone.querySelector('.card-title-2').textContent = data.title;
      clone.querySelector('.card-discover-link-2').href = data.link;
      
      fragment.appendChild(clone);
    });
    return fragment;
  }

  function initSwiper() {
    swiperInstance = new Swiper('.swiper', {
      slidesPerView: 'auto',
      spaceBetween: 30,
      grabCursor: true,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
        disabledClass: 'swiper-button-disabled',
      },
      observer: true,
      observeParents: true,
      observeSlideChildren: true,
      mousewheel: {
        forceToAxis: true,
      },
      touchEventsTarget: 'container',
      touchRatio: 1,
      touchAngle: 45,
      grabCursor: true,
      breakpoints: {
        640: {
          slidesPerView: 2,
        },
        1024: {
          slidesPerView: 3,
        },
      }
    });
  }

  function handleCategoryClick(e) {
    const category = e.target.dataset.category;
    
    categoryTabs.forEach(tab => tab.classList.remove('active'));
    e.target.classList.add('active');
    
    // Kartları gizle
    cardSlider.style.display = 'none';
    
    // Yeni kartları oluştur
    const newCards = createCards(category);
    
    // Eski kartları temizle ve yenilerini ekle
    swiperWrapper.innerHTML = '';
    swiperWrapper.appendChild(newCards);
    
    // Apply translations to the newly added cards
    if (typeof i18n_applyTranslations === 'function') {
        i18n_applyTranslations(i18n_currentLang); // Re-apply translations to the whole page
    } else {
        console.error("Translation function (i18n_applyTranslations) not accessible.");
        // As a fallback, you might manually translate items within swiperWrapper if needed
    }
    
    // Swiper'ı güncelle
    if (swiperInstance) {
      swiperInstance.update();
      swiperInstance.slideTo(0, 0);
    } else {
      initSwiper(); // Initialize if it wasn't (should be initialized on load)
    }
    
    // Kartları göster
    requestAnimationFrame(() => {
      cardSlider.style.display = 'block';
    });
  }

  // Event Listeners
  categoryTabs.forEach(tab => {
    tab.addEventListener('click', handleCategoryClick);
  });

  // Initial Load
  const initialCards = createCards('gundem');
  swiperWrapper.appendChild(initialCards);
  initSwiper();
});