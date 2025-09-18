document.addEventListener("DOMContentLoaded", () => {
  // Blog ile ilgili görseller (örnek olarak)
  const images = [
    {
      src: "./images/111.jpg",
      alt: "Blog Görseli 1",
    },
    {
      src: "./images/112.jpg",
      alt: "Blog Görseli 2",
    },
    {
      src: "./images/113.jpg",
      alt: "Blog Görseli 3",
    },
    {
      src: "./images/114.jpg",
      alt: "Blog Görseli 4",
    },
  ]

  // DOM elementleri
  const galleryWrapper = document.getElementById("galleryWrapper")
  const prevBtn = document.getElementById("prevBtn")
  const nextBtn = document.getElementById("nextBtn")
  const lightbox = document.getElementById("lightbox")
  const lightboxImage = document.getElementById("lightboxImage")
  const closeLightbox = document.getElementById("closeLightbox")

  let currentPosition = 0
  let itemsPerView = getItemsPerView()

  // Ekran boyutuna göre görüntülenecek öğe sayısını belirle
  function getItemsPerView() {
    if (window.innerWidth < 576) return 1
    if (window.innerWidth < 992) return 2
    return 3
  }

  // Galeriyi JavaScript ile oluştur
  function createGallery() {
    galleryWrapper.innerHTML = ""

    images.forEach((image, index) => {
      const galleryItem = document.createElement("div")
      galleryItem.className = "gallery-item-bspg"

      const galleryItemInner = document.createElement("div")
      galleryItemInner.className = "gallery-item-inner-bspg"

      const img = document.createElement("img")
      img.src = image.src
      img.alt = image.alt
      img.dataset.index = index

      img.addEventListener("click", function () {
        openLightbox(this.src, this.alt)
      })

      galleryItemInner.appendChild(img)
      galleryItem.appendChild(galleryItemInner)
      galleryWrapper.appendChild(galleryItem)
    })
  }

  // Slider'ı hareket ettir
  function moveSlider() {
    const itemWidth = galleryWrapper.querySelector(".gallery-item-bspg").offsetWidth
    galleryWrapper.style.transform = `translateX(-${currentPosition * itemWidth}px)`
  }

  // Lightbox'ı aç
  function openLightbox(src, alt) {
    lightboxImage.src = src
    lightboxImage.alt = alt
    lightbox.style.display = "flex"

    // Animasyon için setTimeout kullanımı
    setTimeout(() => {
      lightbox.classList.add("active-bspg")
    }, 10)
  }

  // Lightbox'ı kapat
  function closeLightboxFunc() {
    lightbox.classList.remove("active-bspg")

    // Animasyon tamamlandıktan sonra display'i none yap
    setTimeout(() => {
      lightbox.style.display = "none"
    }, 400)
  }

  // Event listeners
  prevBtn.addEventListener("click", () => {
    if (currentPosition > 0) {
      currentPosition--
      moveSlider()
    }
  })

  nextBtn.addEventListener("click", () => {
    if (currentPosition < images.length - itemsPerView) {
      currentPosition++
      moveSlider()
    }
  })

  closeLightbox.addEventListener("click", closeLightboxFunc)

  lightbox.addEventListener("click", (e) => {
    if (e.target === lightbox) {
      closeLightboxFunc()
    }
  })

  // Ekran boyutu değiştiğinde
  window.addEventListener("resize", () => {
    const newItemsPerView = getItemsPerView()

    if (newItemsPerView !== itemsPerView) {
      itemsPerView = newItemsPerView

      // Eğer mevcut pozisyon, yeni görünüm için çok ilerideyse düzelt
      if (currentPosition > images.length - itemsPerView) {
        currentPosition = Math.max(0, images.length - itemsPerView)
      }

      moveSlider()
    }
  })

  // Klavye navigasyonu
  document.addEventListener("keydown", (e) => {
    if (lightbox.style.display === "flex") {
      if (e.key === "Escape") {
        closeLightboxFunc()
      }
    }
  })

  // Galeriyi oluştur
  createGallery()
})
