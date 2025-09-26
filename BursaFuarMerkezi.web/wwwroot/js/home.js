/*============= Home Page Dynamic Data JS ================= */
document.addEventListener('DOMContentLoaded', function() {
    const lang = window.location.pathname.split('/')[1] || 'tr';
    
    let swiperInstance = null;
    let currentCategory = null;
    let categories = [];
    let currentFairs = []; // Store current fairs data for resize handling
    
    // DOM Elements
    const swiperWrapper = document.querySelector('.swiper-wrapper');
    const cardTemplate = document.getElementById('card-template-2');
    const categoryTabsContainer = document.getElementById('category-tabs-2');
    const cardSlider = document.querySelector('.card-slider-2');
    const upcomingFairsSlider = document.querySelector('.slider');
    const yearFilter = document.getElementById('yearFilter');

    // Initialize all sections
    async function initializePage() {
        try {
            await Promise.all([
                loadCategories(),
                loadUpcomingFairs(),
                loadFairYears(),
                loadFairFilters(),
                loadAllFairs()
            ]);
            
            // Load first category blogs
            if (categories.length > 0) {
                currentCategory = categories[0].key;
                await loadBlogs(currentCategory);
                initSwiper();
            }
        } catch (error) {
            console.error('Error initializing page:', error);
        }
    }

    // Load categories from database
    async function loadCategories() {
        try {
            const response = await fetch(`/${lang}/home/categories`);
            const data = await response.json();
            
            if (data.success) {
                categories = data.data;
                updateCategoryTabs();
            }
        } catch (error) {
            console.error('Error loading categories:', error);
        }
    }

    // Update category tabs with database data
    function updateCategoryTabs() {
        if (!categoryTabsContainer) return;

        // Remove previously added dynamic tabs to prevent duplication
        categoryTabsContainer.querySelectorAll('.dynamic-category-tab').forEach(tab => tab.remove());

        const allPostsLink = categoryTabsContainer.querySelector('a.category-tab-2:not(.dynamic-category-tab)');

        categories.forEach((category, index) => {
            const tab = document.createElement('a');
            tab.href = '#';
            tab.className = 'category-tab-2 dynamic-category-tab';
            tab.style.marginRight = '15px'; // Add some spacing
            tab.textContent = category.name.toUpperCase();
            tab.dataset.category = category.key;

            if (index === 0) {
                tab.classList.add('active');
            }

            // Insert new tabs before the "all posts" link
            if (allPostsLink) {
                categoryTabsContainer.insertBefore(tab, allPostsLink);
            } else {
                categoryTabsContainer.appendChild(tab);
            }
        });
    }

    // Load blogs by category
    async function loadBlogs(category) {
        try {
            const response = await fetch(`/${lang}/blog/getblogs?Category=${encodeURIComponent(category)}&PageSize=7&PageNumber=1`);
            const data = await response.json();
            
            if (data.success) {
                createBlogCards(data.data);
            }
        } catch (error) {
            console.error('Error loading blogs:', error);
        }
    }

    // Create blog cards
    function createBlogCards(blogs) {
        const fragment = document.createDocumentFragment();
        
        blogs.forEach(blog => {
            const clone = cardTemplate.content.cloneNode(true);
            
            clone.querySelector('img').src = blog.cardImageUrl || '/images/default-blog.png';
            clone.querySelector('img').alt = blog.title;
            
            // Parse date from "dd.MM.yyyy" format
            const dateParts = blog.createdAt.split('.');
            const day = dateParts[0] || '01';
            const month = dateParts[1] || '01';
            const year = dateParts[2] || new Date().getFullYear();
            
            clone.querySelector('.date-year-2').textContent = year;
            clone.querySelector('.date-day-2').textContent = `${day}.${month}`;
            
            clone.querySelector('.card-tag-2').textContent = blog.category;
            clone.querySelector('.card-title-2').textContent = blog.title;
            clone.querySelector('.card-discover-link-2').href = blog.detailUrl;
            
            fragment.appendChild(clone);
        });
        
        swiperWrapper.innerHTML = '';
        swiperWrapper.appendChild(fragment);
        
        if (swiperInstance) {
            swiperInstance.update();
            swiperInstance.slideTo(0, 0);
        }
    }

    // Load upcoming fairs
    async function loadUpcomingFairs() {
        try {
            const response = await fetch(`/${lang}/home/upcoming-fairs`);
            const data = await response.json();
            
            if (data.success) {
                createUpcomingFairCards(data.data);
            }
        } catch (error) {
            console.error('Error loading upcoming fairs:', error);
        }
    }

    // Get number of visible cards based on screen size (same logic as main.js)
    function getVisibleCards() {
        const containerWidth = upcomingFairsSlider?.parentElement?.offsetWidth || window.innerWidth;
        if (containerWidth >= 992) {
            return 3; // Desktop: 3 cards
        } else if (containerWidth >= 768) {
            return 2; // Tablet: 2 cards
        } else {
            return 1; // Mobile: 1 card
        }
    }

    // Create upcoming fair cards
    function createUpcomingFairCards(fairs) {
        if (!upcomingFairsSlider) return;
        
        // Store fairs data for resize handling
        currentFairs = fairs;
        
        const fairsHtml = fairs.map(fair => {
            // Extract day and month from formatted date strings (e.g., "25 December 2024")
            const startDateParts = fair.startDate.split(' ');
            const endDateParts = fair.endDate.split(' ');
            
            const startDay = startDateParts[0] || '01';
            const endDay = endDateParts[0] || '01';
            const monthName = startDateParts[1] || '';
            
            return `
                <div class="card" style="background-image: url('${fair.cardImageUrl}')">
                    <div class="card-overlay">
                        <div class="card-content">
                            <div class="card-organizer">${fair.organizer || ''}</div>
                            <h2 class="card-title">${fair.title}</h2>
                            <a href="${fair.detailUrl}" class="card-link">
                                ${lang === 'tr' ? 'Fuar Detayı' : 'Fair Details'} 
                                <i class="fas fa-arrow-right"></i>
                            </a>
                        </div>
                        <div class="card-date">
                            <div class="date-box">
                                <div class="date-day">${startDay}</div>
                                <div class="date-month">${endDay}</div>
                                <div class="date-info">${monthName}</div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }).join('');
        
        upcomingFairsSlider.innerHTML = fairsHtml;
        
        // Smart slider logic: only enable if more cards than visible
        const visibleCards = getVisibleCards();
        const shouldSlide = fairs.length > visibleCards;
        
        const sliderControls = document.querySelector('.slider-controls');
        const prevBtn = document.querySelector('.prev-btn');
        const nextBtn = document.querySelector('.next-btn');
        
        if (shouldSlide) {
            // Enable slider: show controls and allow sliding
            if (sliderControls) sliderControls.style.display = 'flex';
            if (prevBtn) {
                prevBtn.style.display = 'block';
                prevBtn.disabled = false;
            }
            if (nextBtn) {
                nextBtn.style.display = 'block';
                nextBtn.disabled = false;
            }
            
            // Enable transitions and reset position
            upcomingFairsSlider.style.transition = 'transform 0.5s ease';
            upcomingFairsSlider.style.transform = 'translateX(0px)';
            
            // Initialize slider functionality
            initUpcomingFairsSlider(fairs.length, visibleCards);
        } else {
            // Disable slider: hide controls and prevent sliding
            if (sliderControls) sliderControls.style.display = 'none';
            if (prevBtn) {
                prevBtn.style.display = 'none';
                prevBtn.disabled = true;
            }
            if (nextBtn) {
                nextBtn.style.display = 'none';
                nextBtn.disabled = true;
            }
            
            // COMPLETELY disable any animation or transform
            upcomingFairsSlider.style.transition = 'none';
            upcomingFairsSlider.style.transform = 'none';
            upcomingFairsSlider.style.webkitTransform = 'none';
        }
    }

    // Initialize slider functionality for upcoming fairs
    function initUpcomingFairsSlider(totalCards, visibleCards) {
        const slider = upcomingFairsSlider;
        const prevBtn = document.querySelector('.prev-btn');
        const nextBtn = document.querySelector('.next-btn');
        const progressFill = document.querySelector('.progress-fill');
        
        if (!slider || !prevBtn || !nextBtn) return;
        
        let currentIndex = 0;
        const maxIndex = totalCards - visibleCards;
        
        // Calculate card width (same as main.js)
        const cards = slider.querySelectorAll('.card');
        const cardWidth = cards.length > 0 ? cards[0].offsetWidth + 20 : 320; // 20px gap
        
        // Update progress bar
        function updateProgressBar() {
            if (progressFill && maxIndex > 0) {
                const progress = (currentIndex / maxIndex) * 100;
                progressFill.style.width = `${progress}%`;
            }
        }
        
        // Update slider position
        function updateSlider() {
            slider.style.transform = `translateX(-${currentIndex * cardWidth}px)`;
            updateProgressBar();
        }
        
        // Add event listeners (main.js won't interfere due to .card-1-section check)
        prevBtn.addEventListener('click', function() {
            if (currentIndex > 0) {
                currentIndex--;
                updateSlider();
            }
        });
        
        nextBtn.addEventListener('click', function() {
            if (currentIndex < maxIndex) {
                currentIndex++;
                updateSlider();
            }
        });
        
        // Initial setup
        updateProgressBar();
    }

    // Load available years for filter
    async function loadFairYears() {
        try {
            const response = await fetch(`/${lang}/home/fair-years`);
            const data = await response.json();
            
            if (data.success && yearFilter) {
                updateYearFilter(data.data);
            }
        } catch (error) {
            console.error('Error loading fair years:', error);
        }
    }

    // Update year filter dropdown
    function updateYearFilter(years) {
        // Keep the first option (placeholder)
        const firstOption = yearFilter.children[0];
        yearFilter.innerHTML = '';
        yearFilter.appendChild(firstOption);
        
        years.forEach(year => {
            const option = document.createElement('option');
            option.value = year;
            option.textContent = year;
            yearFilter.appendChild(option);
        });
    }

    // Load all fair filters (months, sectors, cities)
    async function loadFairFilters() {
        try {
            const [monthsResponse, sectorsResponse, citiesResponse] = await Promise.all([
                fetch(`/${lang}/home/fair-months`),
                fetch(`/${lang}/home/fair-sectors`),
                fetch(`/${lang}/home/fair-cities`)
            ]);

            const monthsData = await monthsResponse.json();
            const sectorsData = await sectorsResponse.json();
            const citiesData = await citiesResponse.json();

            if (monthsData.success) {
                updateMonthFilter(monthsData.data);
            }
            if (sectorsData.success) {
                updateSectorFilter(sectorsData.data);
            }
            if (citiesData.success) {
                updateLocationFilter(citiesData.data);
            }
        } catch (error) {
            console.error('Error loading fair filters:', error);
        }
    }

    // Update month filter dropdown
    function updateMonthFilter(months) {
        const monthFilter = document.getElementById('monthFilter');
        if (!monthFilter) return;

        // Keep the first option (placeholder)
        const firstOption = monthFilter.children[0];
        monthFilter.innerHTML = '';
        monthFilter.appendChild(firstOption);
        
        months.forEach(month => {
            const option = document.createElement('option');
            option.value = month.value; // Use month number for filtering
            option.textContent = month.name;
            monthFilter.appendChild(option);
        });
    }

    // Update sector filter dropdown
    function updateSectorFilter(sectors) {
        const sectorFilter = document.getElementById('sectorFilter');
        if (!sectorFilter) return;

        // Keep the first option (placeholder)
        const firstOption = sectorFilter.children[0];
        sectorFilter.innerHTML = '';
        sectorFilter.appendChild(firstOption);
        
        sectors.forEach(sector => {
            const option = document.createElement('option');
            option.value = sector.id; // Use sector ID for filtering
            option.textContent = sector.name;
            sectorFilter.appendChild(option);
        });
    }

    // Update location filter dropdown
    function updateLocationFilter(cities) {
        const locationFilter = document.getElementById('locationFilter');
        if (!locationFilter) return;

        // Keep the first option (placeholder)
        const firstOption = locationFilter.children[0];
        locationFilter.innerHTML = '';
        locationFilter.appendChild(firstOption);
        
        cities.forEach(city => {
            const option = document.createElement('option');
            option.value = city;
            option.textContent = city;
            locationFilter.appendChild(option);
        });
    }

    // Helper function to get month number from month name
    function getMonthNumber(monthName) {
        const months = {
            'January': 1, 'February': 2, 'March': 3, 'April': 4, 'May': 5, 'June': 6,
            'July': 7, 'August': 8, 'September': 9, 'October': 10, 'November': 11, 'December': 12,
            'Ocak': 1, 'Şubat': 2, 'Mart': 3, 'Nisan': 4, 'Mayıs': 5, 'Haziran': 6,
            'Temmuz': 7, 'Ağustos': 8, 'Eylül': 9, 'Ekim': 10, 'Kasım': 11, 'Aralık': 12
        };
        return months[monthName] || 1;
    }

    // All Fairs Calendar System (similar to anasayfa-fuar-takvimi.js)
    let allFairs = [];
    let filteredFairs = [];
    let currentPage = 1;
    const eventsPerPage = 6;

    // Load all fairs data
    async function loadAllFairs() {
        try {
            const response = await fetch(`/${lang}/home/all-fairs`);
            const data = await response.json();
            
            if (data.success) {
                allFairs = data.data.map(fair => {
                    // Extract year and month from formatted date string (e.g., "25 December 2024")
                    const startDateParts = fair.startDate.split(' ');
                    const year = startDateParts[2] || new Date().getFullYear().toString();
                    const monthName = startDateParts[1] || '';
                    
                    // Get month number from month name
                    const monthNumber = getMonthNumber(monthName);
                    
                    return {
                        id: fair.id,
                        title: fair.title,
                        date: `${fair.startDate} - ${fair.endDate}`,
                        year: year,
                        month: monthNumber.toString(), // Store as string for filtering
                        monthName: monthName,
                        sectors: fair.sectors || [], // Store sectors array
                        location: fair.city || 'Bursa', // Use actual city from API
                        organizer: fair.organizer || '',
                        url: fair.detailUrl
                    };
                });
                
                filteredFairs = allFairs;
                updateFairEvents();
                setupFairFilters();
            }
        } catch (error) {
            console.error('Error loading all fairs:', error);
        }
    }

    // Setup fair filter event listeners
    function setupFairFilters() {
        if (yearFilter) {
            yearFilter.addEventListener('change', () => {
                currentPage = 1;
                filterFairEvents();
            });
        }

        const monthFilter = document.getElementById('monthFilter');
        const sectorFilter = document.getElementById('sectorFilter');
        const locationFilter = document.getElementById('locationFilter');

        if (monthFilter) {
            monthFilter.addEventListener('change', () => {
                currentPage = 1;
                filterFairEvents();
            });
        }
        
        if (sectorFilter) {
            sectorFilter.addEventListener('change', () => {
                currentPage = 1;
                filterFairEvents();
            });
        }
        
        if (locationFilter) {
            locationFilter.addEventListener('change', () => {
                currentPage = 1;
                filterFairEvents();
            });
        }
    }

    // Filter fair events based on selected criteria
    function filterFairEvents() {
        const selectedYear = yearFilter?.value || '';
        const selectedMonth = document.getElementById('monthFilter')?.value || '';
        const selectedSectorId = document.getElementById('sectorFilter')?.value || '';
        const selectedLocation = document.getElementById('locationFilter')?.value || '';

        filteredFairs = allFairs;

        // Apply filters
        if (selectedYear) {
            filteredFairs = filteredFairs.filter(event => event.year === selectedYear);
        }

        if (selectedMonth) {
            filteredFairs = filteredFairs.filter(event => event.month === selectedMonth);
        }

        if (selectedSectorId) {
            // Filter by sector ID - check if any sector in the array matches
            filteredFairs = filteredFairs.filter(event => 
                event.sectors.some(sector => sector.id.toString() === selectedSectorId)
            );
        }

        if (selectedLocation) {
            filteredFairs = filteredFairs.filter(event => 
                event.location && event.location.toLowerCase().includes(selectedLocation.toLowerCase())
            );
        }

        updateFairEvents();
    }

    // Update fair events display and pagination
    function updateFairEvents() {
        displayFairEvents();
        setupFairPagination();
    }

    // Display fair events in the DOM
    function displayFairEvents() {
        const eventsList = document.getElementById('eventsList');
        if (!eventsList) return;

        eventsList.innerHTML = '';

        if (filteredFairs.length === 0) {
            eventsList.innerHTML = `<div class="alert alert-info">${lang === 'tr' ? 'Seçilen kriterlere uygun etkinlik bulunamadı.' : 'No events found for the selected criteria.'}</div>`;
            return;
        }

        // Calculate pagination
        const startIndex = (currentPage - 1) * eventsPerPage;
        const endIndex = Math.min(startIndex + eventsPerPage, filteredFairs.length);
        const paginatedEvents = filteredFairs.slice(startIndex, endIndex);

        paginatedEvents.forEach(event => {
            const eventElement = document.createElement('div');
            eventElement.className = 'event-item';

            eventElement.innerHTML = `
                <a href="${event.url}" class="event-link">
                    <div class="event-title">${event.title}</div>
                    <div class="event-date">${event.date}</div>
                    <div class="event-organizer">${event.organizer}</div>
                </a>
            `;

            eventsList.appendChild(eventElement);
        });
    }

    // Setup fair pagination
    function setupFairPagination() {
        const paginationContainer = document.getElementById('pagination');
        if (!paginationContainer) return;

        paginationContainer.innerHTML = '';

        const totalPages = Math.ceil(filteredFairs.length / eventsPerPage);

        if (totalPages <= 1) {
            return; // Don't show pagination if there's only one page
        }

        for (let i = 1; i <= totalPages; i++) {
            const pageItem = document.createElement('li');
            pageItem.className = `page-item ${i === currentPage ? 'active' : ''}`;

            const pageLink = document.createElement('a');
            pageLink.className = 'page-link';
            pageLink.href = '#';
            pageLink.textContent = i;

            pageLink.addEventListener('click', (e) => {
                e.preventDefault();
                currentPage = i;
                updateFairEvents();
            });

            pageItem.appendChild(pageLink);
            paginationContainer.appendChild(pageItem);
        }
    }

    // Initialize Swiper for blog cards
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
                }
            }
        });
    }

    // Handle category tab clicks
    function handleCategoryClick(e) {
        const clickedTab = e.target.closest('.category-tab-2[data-category]');
        if (!clickedTab) {
            return; // Exit if the click is not on a category tab
        }
    
        e.preventDefault();
        const category = clickedTab.dataset.category;

        // Update active state on tabs
        if(categoryTabsContainer) {
            categoryTabsContainer.querySelectorAll('.dynamic-category-tab').forEach(tab => tab.classList.remove('active'));
        }
        clickedTab.classList.add('active');

        currentCategory = category;
        loadBlogs(category);
    }


    // Event Listeners
    if (categoryTabsContainer) {
        categoryTabsContainer.addEventListener('click', handleCategoryClick);
    }

    // Initialize page
    initializePage();
    
    // Handle window resize to recalculate slider logic
    let resizeTimeout;
    window.addEventListener('resize', function() {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(function() {
            // Recalculate slider logic for upcoming fairs on resize
            if (currentFairs.length > 0) {
                createUpcomingFairCards(currentFairs);
            }
        }, 300); // Debounce resize events
    });
});
