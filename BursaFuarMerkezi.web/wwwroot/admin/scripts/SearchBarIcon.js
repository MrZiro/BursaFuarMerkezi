function addSearchBarIcon() {
    
    // Add custom clear button
    $('.dt-search').append('<button class="search-clear-btn" aria-label="Clear search">×</button>');

    // Handle input events
    $('.dt-search input').on('input', function() {
    const clearBtn = $(this).siblings('.search-clear-btn');
    clearBtn.toggleClass('visible', this.value.length > 0);
    });

    // Handle clear button click
    $('.dt-search').on('click', '.search-clear-btn', function() {
    const input = $(this).siblings('input');
    input.val('').focus();
    dataTables.search('').draw();
    $(this).removeClass('visible');
    });
}
