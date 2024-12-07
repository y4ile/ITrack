document.addEventListener('DOMContentLoaded', () => {
    const overlay = document.getElementById('page-transition-overlay');

    if (sessionStorage.getItem('internal-nav') === '1') {
        sessionStorage.removeItem('internal-nav');
        overlay.classList.add('active');
        setTimeout(() => {
            overlay.classList.remove('active');
        }, 150);
    }
});

document.addEventListener('click', (e) => {
    const overlay = document.getElementById('page-transition-overlay');
    const link = e.target.closest('a[href]');
    
    if (!link) return;
    if (link.classList.contains('no-pt')) return;
    if (!link.classList.contains('nav-link') &&
        !link.classList.contains('navbar-brand') &&
        !link.classList.contains('dropdown-item')) return;
    
    if (link && link.hostname === window.location.hostname && !link.hasAttribute('target') && link.getAttribute('href') !== '#') {
        e.preventDefault();
        sessionStorage.setItem('internal-nav', '1');
        overlay.classList.add('active');
        setTimeout(() => {
            window.location = link.href;
        }, 150);
    }
});