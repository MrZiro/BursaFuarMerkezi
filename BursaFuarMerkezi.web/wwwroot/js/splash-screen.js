function initializeSplashScreen() {
    const splashScreen = document.getElementById('splashScreen');
    const stickFormation = document.getElementById('stickFormation');
    
    // Eğer bu oturumda daha önce görüldüyse splash screen'i gösterme
    if (sessionStorage.getItem('splashShown') === 'true') {
        // Zaten gizli, hiçbir şey yapma
        return;
    }
    
    // Splash screen'i göster
    splashScreen.classList.add('show');
    
    // Start stick formation after 1 second
    setTimeout(() => {
        stickFormation.classList.add('formed');
    }, 1000);
    
    // Splash screen'i gizle ve bu oturum için işaretle
    setTimeout(() => {
        splashScreen.classList.add('slide-out');
        
        setTimeout(() => {
            splashScreen.classList.remove('show');
            splashScreen.classList.remove('slide-out');
            // Bu oturum için splash gösterildi olarak işaretle
            sessionStorage.setItem('splashShown', 'true');
        }, 1500);
    }, 3000);
}

// Run when page loads
document.addEventListener('DOMContentLoaded', initializeSplashScreen);