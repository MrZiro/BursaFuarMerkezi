const dropdownBtns = document.querySelectorAll('.dropdown-btn');
const toggleBtn = document.querySelector('#toggle-btn');
const sidebar = document.querySelector('#sidebar');
const logo = document.querySelector('#sidebar .logo');

// Toggle sidebar
toggleBtn.addEventListener('click', () => {
    sidebar.classList.toggle('close');
    toggleBtn.classList.toggle("rotate");
    
    // Close any open submenus when sidebar is collapsed
    dropdownBtns.forEach(btn => {
        const subMenu = btn.nextElementSibling;
        if(subMenu.classList.contains('show')) {
            subMenu.classList.remove('show');
            btn.classList.remove("rotate");
        }
    });
});

// Handle dropdown buttons
dropdownBtns.forEach(btn => {
    const subMenu = btn.nextElementSibling; 
    
    btn.addEventListener('click', () => {
        subMenu.classList.toggle('show');
        btn.classList.toggle("rotate");
        
        // Open sidebar if it's closed when clicking a dropdown
        if(sidebar.classList.contains('close')) {
            sidebar.classList.remove('close');
            toggleBtn.classList.remove("rotate");
        }
    });
});

// Initialize active nav item highlighting
document.addEventListener('DOMContentLoaded', () => {
    const navItems = document.querySelectorAll('#sidebar .nav-item');
    console.log("navItems : ");
    console.log(navItems);
    
    navItems.forEach(item => {
        const link = item.querySelector('.nav-link');
        console.log("link : ");
        console.log(link);
        if (!link) return;
        
        link.addEventListener('click', () => {
            // Remove active class from all items
            navItems.forEach(i => i.classList.remove('active'));
            // Add active class to clicked item
            item.classList.add('active');
        });
    });
});





