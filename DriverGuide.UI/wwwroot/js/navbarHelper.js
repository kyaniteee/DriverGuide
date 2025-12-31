window.navbarHelper = {
    closeNavbar: function() {
        const navbar = document.getElementById('navbarNav');
        if (navbar && navbar.classList.contains('show')) {
            const bsCollapse = bootstrap.Collapse.getInstance(navbar);
            if (bsCollapse) {
                bsCollapse.hide();
            } else {
                const collapse = new bootstrap.Collapse(navbar, { toggle: false });
                collapse.hide();
            }
        }
        document.body.classList.remove('navbar-open');
    },
    
    initNavbar: function() {
        const navbar = document.getElementById('navbarNav');
        const toggler = document.querySelector('.navbar-toggler');
        
        if (!navbar || !toggler) return;
        
        navbar.addEventListener('show.bs.collapse', () => {
            document.body.classList.add('navbar-open');
        });
        
        navbar.addEventListener('hide.bs.collapse', () => {
            document.body.classList.remove('navbar-open');
        });
        
        const navLinks = navbar.querySelectorAll('.nav-link, .btn');
        navLinks.forEach(link => {
            link.addEventListener('click', (e) => {
                if (window.innerWidth < 992) {
                    setTimeout(() => {
                        this.closeNavbar();
                    }, 100);
                }
            });
        });
        
        document.addEventListener('click', (e) => {
            const isClickInsideNav = navbar.contains(e.target);
            const isToggler = toggler.contains(e.target);
            
            if (!isClickInsideNav && !isToggler && navbar.classList.contains('show')) {
                this.closeNavbar();
            }
        });
        
        navbar.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && navbar.classList.contains('show')) {
                this.closeNavbar();
                toggler.focus();
            }
        });
    }
};

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.navbarHelper.initNavbar();
    });
} else {
    window.navbarHelper.initNavbar();
}

window.addEventListener('resize', () => {
    if (window.innerWidth >= 992) {
        window.navbarHelper.closeNavbar();
    }
});
