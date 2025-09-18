document.addEventListener('DOMContentLoaded', function() {
    // Get current path and language
    var currentPath = window.location.pathname;
    var currentLang = currentPath.startsWith('/en') ? 'en' : 'tr';
    var otherLang = currentLang === 'en' ? 'tr' : 'en';

    // --- Dropdown Interactivity ---
    const langBtn = document.getElementById('langDropdownBtn');
    const langMenu = document.getElementById('langDropdownMenu');
    const selectedLang = document.getElementById('selectedLang');

    if (langBtn && langMenu) {
        // Set initial language display
        if(selectedLang) {
            selectedLang.textContent = currentLang.toUpperCase();
        }

        langBtn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation(); // Prevents the click from bubbling up to the document
            langMenu.classList.toggle('show');
        });
    }

    // Close dropdown when clicking outside
    document.addEventListener('click', function (e) {
        if (langMenu && langMenu.classList.contains('show') && !langBtn.contains(e.target)) {
            langMenu.classList.remove('show');
        }
    });
    
    // --- URL and Link Handling ---
    var langLinks = document.querySelectorAll('a[data-lang]');
    langLinks.forEach(function(link) {
        var linkLang = link.getAttribute('data-lang');
        var newHref = '';
        
        if (linkLang === 'tr') {
            if (currentPath === '/' || currentPath === '') {
                // Root homepage - stay as root for Turkish (default)
                newHref = '/';
            } else if (currentPath.startsWith('/en')) {
                // English page - convert to Turkish
                newHref = currentPath.replace('/en', '/tr');
            } else {
                // Already Turkish path or other
                newHref = currentPath;
            }
            link.href = newHref;
            link.classList.toggle('active', currentLang === 'tr');
        } else if (linkLang === 'en') {
            if (currentPath === '/' || currentPath === '') {
                // Root homepage - go to English
                newHref = '/en';
            } else if (currentPath.startsWith('/tr')) {
                // Turkish page - convert to English  
                newHref = currentPath.replace('/tr', '/en');
            } else {
                // Already English path or other
                newHref = currentPath;
            }
            link.href = newHref;
            link.classList.toggle('active', currentLang === 'en');
        }
    });

    // Hide the currently active language from the dropdown
    if(langMenu) {
        const langOptions = langMenu.querySelectorAll('.lang-option');
        langOptions.forEach(function (opt) {
            if (opt.getAttribute('data-lang') === currentLang) {
                opt.style.display = 'none';
            } else {
                opt.style.display = 'block';
            }
        });
    }
}); 