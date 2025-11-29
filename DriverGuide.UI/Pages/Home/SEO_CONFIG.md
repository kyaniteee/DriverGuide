# SEO & Social Media Configuration

## ?? Open Graph Meta Tags

Dodaj do `index.html` w sekcji `<head>`:

```html
<!-- Open Graph / Facebook -->
<meta property="og:type" content="website">
<meta property="og:url" content="https://driverguide.pl/">
<meta property="og:title" content="DriverGuide - Twoja droga do prawa jazdy">
<meta property="og:description" content="Najnowoczeœniejsza platforma do nauki na prawo jazdy online. Oficjalne pytania WORD, symulacje egzaminu i zaawansowane statystyki.">
<meta property="og:image" content="https://driverguide.pl/images/og-image.jpg">
<meta property="og:image:width" content="1200">
<meta property="og:image:height" content="630">

<!-- Twitter -->
<meta property="twitter:card" content="summary_large_image">
<meta property="twitter:url" content="https://driverguide.pl/">
<meta property="twitter:title" content="DriverGuide - Twoja droga do prawa jazdy">
<meta property="twitter:description" content="Najnowoczeœniejsza platforma do nauki na prawo jazdy online. Oficjalne pytania WORD, symulacje egzaminu i zaawansowane statystyki.">
<meta property="twitter:image" content="https://driverguide.pl/images/twitter-image.jpg">

<!-- Additional SEO -->
<meta name="description" content="Przygotuj siê do egzaminu na prawo jazdy z DriverGuide. 5000+ pytañ WORD, symulacje egzaminu, wszystkie kategorie A, B, C, D, T. Zdaj za pierwszym razem!">
<meta name="keywords" content="prawo jazdy, egzamin, nauka online, pytania WORD, test na prawo jazdy, kategoria B, kategoria A, symulator egzaminu">
<meta name="author" content="DriverGuide Team">
<meta name="robots" content="index, follow">
<meta name="language" content="Polish">
<meta name="revisit-after" content="7 days">

<!-- Canonical URL -->
<link rel="canonical" href="https://driverguide.pl/">

<!-- Favicon -->
<link rel="icon" type="image/png" sizes="32x32" href="/images/favicon-32x32.png">
<link rel="icon" type="image/png" sizes="16x16" href="/images/favicon-16x16.png">
<link rel="apple-touch-icon" sizes="180x180" href="/images/apple-touch-icon.png">
```

## ??? Wymagane obrazy dla Social Media

### 1. Open Graph Image (Facebook, LinkedIn)
**Plik**: `og-image.jpg`
**Lokalizacja**: `wwwroot/images/`
**Wymiary**: 1200x630px
**Format**: JPG lub PNG
**Rozmiar**: Max 300KB

**Zawartoœæ**:
- Logo DriverGuide
- Tekst: "Twoja droga do prawa jazdy"
- Ikona kierownicy lub samochodu
- Kolory: #2d6cdf (primary), bia³e t³o

### 2. Twitter Card Image
**Plik**: `twitter-image.jpg`
**Lokalizacja**: `wwwroot/images/`
**Wymiary**: 1200x600px
**Format**: JPG lub PNG
**Rozmiar**: Max 300KB

### 3. Favicon Pack
**Pliki**: 
- `favicon-32x32.png` (32x32px)
- `favicon-16x16.png` (16x16px)
- `apple-touch-icon.png` (180x180px)

**Lokalizacja**: `wwwroot/images/`

## ?? Structured Data (JSON-LD)

Dodaj przed zamkniêciem `</body>` w `index.html`:

```html
<script type="application/ld+json">
{
  "@context": "https://schema.org",
  "@type": "EducationalOrganization",
  "name": "DriverGuide",
  "description": "Platforma do nauki na prawo jazdy online",
  "url": "https://driverguide.pl",
  "logo": "https://driverguide.pl/images/logo.png",
  "foundingDate": "2024",
  "address": {
    "@type": "PostalAddress",
    "addressCountry": "PL"
  },
  "contactPoint": {
    "@type": "ContactPoint",
    "contactType": "Customer Service",
    "availableLanguage": "Polish"
  },
  "sameAs": [
    "https://facebook.com/driverguide",
    "https://twitter.com/driverguide",
    "https://instagram.com/driverguide"
  ],
  "offers": {
    "@type": "Offer",
    "price": "0",
    "priceCurrency": "PLN",
    "availability": "https://schema.org/InStock"
  }
}
</script>

<script type="application/ld+json">
{
  "@context": "https://schema.org",
  "@type": "WebApplication",
  "name": "DriverGuide",
  "applicationCategory": "EducationalApplication",
  "operatingSystem": "Any",
  "browserRequirements": "Requires JavaScript. Requires HTML5.",
  "offers": {
    "@type": "Offer",
    "price": "0",
    "priceCurrency": "PLN"
  },
  "aggregateRating": {
    "@type": "AggregateRating",
    "ratingValue": "4.8",
    "ratingCount": "12453",
    "bestRating": "5",
    "worstRating": "1"
  }
}
</script>
```

## ?? Google Analytics Setup

Dodaj przed zamkniêciem `</head>`:

```html
<!-- Google tag (gtag.js) -->
<script async src="https://www.googletagmanager.com/gtag/js?id=G-XXXXXXXXXX"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'G-XXXXXXXXXX');
</script>
```

### Eventy do trackowania:

```javascript
// Rozpoczêcie testu
gtag('event', 'start_test', {
  'event_category': 'engagement',
  'event_label': 'category_B',
  'value': 1
});

// Rejestracja
gtag('event', 'sign_up', {
  'event_category': 'conversion',
  'method': 'email'
});

// Klikniêcie CTA
gtag('event', 'cta_click', {
  'event_category': 'engagement',
  'event_label': 'hero_primary_cta'
});

// Klikniêcie linku rz¹dowego
gtag('event', 'external_link', {
  'event_category': 'engagement',
  'event_label': 'gov_link_mii'
});
```

## ?? Microsoft Clarity (Heatmaps & Session Recording)

Dodaj przed zamkniêciem `</head>`:

```html
<!-- Microsoft Clarity -->
<script type="text/javascript">
    (function(c,l,a,r,i,t,y){
        c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};
        t=l.createElement(r);t.async=1;t.src="https://www.clarity.ms/tag/"+i;
        y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y);
    })(window, document, "clarity", "script", "YOUR_CLARITY_ID");
</script>
```

## ?? Content Security Policy (CSP)

Aktualizuj CSP w `index.html` aby zezwoliæ na Unsplash:

```html
<meta http-equiv="Content-Security-Policy" 
      content="default-src 'self' 'unsafe-inline' 'unsafe-eval'; 
               img-src 'self' https://images.unsplash.com data: https:; 
               font-src 'self' data: https://cdn.jsdelivr.net; 
               style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net;
               script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.googletagmanager.com https://www.clarity.ms;
               connect-src 'self' https://www.google-analytics.com https://*.clarity.ms;">
```

## ?? robots.txt

Utwórz `wwwroot/robots.txt`:

```txt
User-agent: *
Allow: /

# Strony do crawlowania
Sitemap: https://driverguide.pl/sitemap.xml

# Blokuj zbêdne œcie¿ki
Disallow: /api/
Disallow: /_framework/
Disallow: /.well-known/

# Friendly dla botów
Crawl-delay: 1
```

## ??? Sitemap.xml

Utwórz `wwwroot/sitemap.xml`:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <!-- Home page -->
  <url>
    <loc>https://driverguide.pl/</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>weekly</changefreq>
    <priority>1.0</priority>
  </url>
  
  <!-- Test selection -->
  <url>
    <loc>https://driverguide.pl/test</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.9</priority>
  </url>
  
  <!-- Login -->
  <url>
    <loc>https://driverguide.pl/login</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>yearly</changefreq>
    <priority>0.7</priority>
  </url>
  
  <!-- Register -->
  <url>
    <loc>https://driverguide.pl/register</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>yearly</changefreq>
    <priority>0.8</priority>
  </url>
  
  <!-- Quiz pages per category -->
  <url>
    <loc>https://driverguide.pl/quiz/B</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.8</priority>
  </url>
  
  <url>
    <loc>https://driverguide.pl/quiz/A</loc>
    <lastmod>2024-01-01</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.7</priority>
  </url>
  
  <!-- Add more categories as needed -->
</urlset>
```

## ?? PWA Manifest (Progressive Web App)

Utwórz `wwwroot/manifest.json`:

```json
{
  "name": "DriverGuide - Przygotowanie do egzaminu na prawo jazdy",
  "short_name": "DriverGuide",
  "description": "Najnowoczeœniejsza platforma do nauki na prawo jazdy online",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#2d6cdf",
  "orientation": "portrait-primary",
  "icons": [
    {
      "src": "/images/icon-72x72.png",
      "sizes": "72x72",
      "type": "image/png"
    },
    {
      "src": "/images/icon-96x96.png",
      "sizes": "96x96",
      "type": "image/png"
    },
    {
      "src": "/images/icon-128x128.png",
      "sizes": "128x128",
      "type": "image/png"
    },
    {
      "src": "/images/icon-144x144.png",
      "sizes": "144x144",
      "type": "image/png"
    },
    {
      "src": "/images/icon-152x152.png",
      "sizes": "152x152",
      "type": "image/png"
    },
    {
      "src": "/images/icon-192x192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "/images/icon-384x384.png",
      "sizes": "384x384",
      "type": "image/png"
    },
    {
      "src": "/images/icon-512x512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ],
  "categories": ["education", "lifestyle"],
  "lang": "pl",
  "dir": "ltr"
}
```

Dodaj link do manifestu w `index.html`:

```html
<link rel="manifest" href="/manifest.json">
```

## ?? Performance Optimizations

### 1. Preconnect do zewnêtrznych domen

Dodaj w `<head>`:

```html
<!-- Preconnect dla szybszego ³adowania -->
<link rel="preconnect" href="https://images.unsplash.com">
<link rel="dns-prefetch" href="https://images.unsplash.com">
<link rel="preconnect" href="https://www.googletagmanager.com">
<link rel="preconnect" href="https://www.clarity.ms">
```

### 2. Critical CSS

Rozwa¿ inline critical CSS dla first paint:

```html
<style>
  /* Critical CSS dla above-the-fold content */
  body { 
    font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
    margin: 0;
    padding: 0;
  }
  .hero-section {
    min-height: 90vh;
    background: linear-gradient(135deg, #2d6cdf, #1b417a);
  }
</style>
```

### 3. Lazy Loading Images

Ju¿ zaimplementowane w komponencie:

```html
<img src="..." loading="lazy" alt="..." />
```

## ?? A/B Testing Configuration

Przyk³ad z Google Optimize:

```html
<!-- Google Optimize -->
<script src="https://www.googleoptimize.com/optimize.js?id=OPT-XXXXXX"></script>
```

### Warianty do testowania:

**Test 1: Hero CTA Text**
- Wariant A: "Zacznij za darmo"
- Wariant B: "Rozpocznij naukê"
- Wariant C: "Wypróbuj teraz"

**Test 2: Hero Colors**
- Wariant A: Niebieski gradient (current)
- Wariant B: Zielony gradient
- Wariant C: Fioletowy gradient

**Test 3: Statistics Display**
- Wariant A: Floating cards (current)
- Wariant B: Static grid
- Wariant C: Animated counter

## ?? Push Notifications (Future)

Setup dla web push:

```javascript
// service-worker.js
self.addEventListener('push', function(event) {
  const data = event.data.json();
  const options = {
    body: data.body,
    icon: '/images/icon-192x192.png',
    badge: '/images/badge-72x72.png',
    vibrate: [200, 100, 200],
    actions: [
      { action: 'start', title: 'Rozpocznij test' },
      { action: 'close', title: 'Zamknij' }
    ]
  };
  
  event.waitUntil(
    self.registration.showNotification(data.title, options)
  );
});
```

## ? SEO Checklist

- [ ] Meta tags (title, description) na wszystkich stronach
- [ ] Open Graph tags
- [ ] Twitter Card tags
- [ ] Structured Data (JSON-LD)
- [ ] Canonical URLs
- [ ] robots.txt
- [ ] sitemap.xml
- [ ] Alt text na wszystkich obrazach
- [ ] Favicon pack
- [ ] Mobile-friendly test passed
- [ ] Core Web Vitals OK
- [ ] HTTPS enabled
- [ ] Google Search Console configured
- [ ] Bing Webmaster Tools configured

## ?? Social Media Links

Dodaj do footera lub nawigacji:

```html
<div class="social-links">
  <a href="https://facebook.com/driverguide" target="_blank" rel="noopener noreferrer">
    <i class="bi bi-facebook"></i>
  </a>
  <a href="https://twitter.com/driverguide" target="_blank" rel="noopener noreferrer">
    <i class="bi bi-twitter"></i>
  </a>
  <a href="https://instagram.com/driverguide" target="_blank" rel="noopener noreferrer">
    <i class="bi bi-instagram"></i>
  </a>
  <a href="https://linkedin.com/company/driverguide" target="_blank" rel="noopener noreferrer">
    <i class="bi bi-linkedin"></i>
  </a>
</div>
```

---

**Status**: ?? Ready for implementation
**Priority**: High (SEO is critical for discoverability)
**Estimated time**: 2-4 hours
