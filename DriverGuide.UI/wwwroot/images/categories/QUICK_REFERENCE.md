# Quick Reference - Obrazki Kategorii

## ?? Lista plików (17 obrazków):

```
? am.jpg    - Motorower/skuter
? a1.jpg    - Motocykl 125cc
? a2.jpg    - Motocykl 35kW
? a.jpg     - Motocykl bez ograniczeñ
? b1.jpg    - Quad/pojazd czteroko³owy
? b.jpg     - Samochód osobowy ? PRIORYTET
? be.jpg    - Samochód z przyczep¹
? c1.jpg    - Ciê¿arówka 7.5t
? c.jpg     - Ciê¿arówka du¿a
? c1e.jpg   - C1 z przyczep¹
? ce.jpg    - C z przyczep¹
? d1.jpg    - Minibus
? d.jpg     - Autobus
? d1e.jpg   - Minibus z przyczep¹
? de.jpg    - Autobus z przyczep¹
? t.jpg     - Ci¹gnik rolniczy
? default.jpg - Fallback obrazek
```

## ?? Szybki start:

### 1. Pobierz z Unsplash:

**Kategoria B (najpopularniejsza):**
```
https://unsplash.com/s/photos/modern-car
Szukaj: "blue sedan", "modern family car"
```

**Kategoria A (motocykl):**
```
https://unsplash.com/s/photos/motorcycle
Szukaj: "sport motorcycle", "superbike"
```

**Kategoria C (ciê¿arówka):**
```
https://unsplash.com/s/photos/truck
Szukaj: "semi truck", "lorry side view"
```

**Kategoria D (autobus):**
```
https://unsplash.com/s/photos/bus
Szukaj: "city bus", "public bus"
```

**Kategoria T (ci¹gnik):**
```
https://unsplash.com/s/photos/tractor
Szukaj: "modern tractor", "agricultural tractor"
```

### 2. Alternatywnie Pexels:

```
https://www.pexels.com/search/car/
https://www.pexels.com/search/motorcycle/
https://www.pexels.com/search/truck/
https://www.pexels.com/search/bus/
https://www.pexels.com/search/tractor/
```

## ? Szybka optymalizacja online:

### Metoda 1: TinyJPG
1. IdŸ do: https://tinyjpg.com
2. Przeci¹gnij wszystkie obrazki
3. Pobierz skompresowane

### Metoda 2: Squoosh
1. IdŸ do: https://squoosh.app
2. Przeci¹gnij obrazek
3. Ustaw jakoœæ: 80-85%
4. Pobierz

## ?? Jeœli potrzebujesz zmieniæ rozmiar:

### Online (bulk):
- https://bulkresizephotos.com
  - Ustaw: 800x600px
  - Quality: 85%

### Photoshop:
```
File ? Automate ? Batch
Image Size: 800x600px
Save for Web: Quality 80%
```

### GIMP:
```
Image ? Scale Image ? 800x600
File ? Export ? Quality 80-85%
```

## ?? Priorytet pobierania:

### Etap 1 (Podstawowe - 5 obrazków):
```
1. b.jpg    - Samochód (NAJPOPULARNIEJSZY!)
2. a.jpg    - Motocykl
3. c.jpg    - Ciê¿arówka
4. d.jpg    - Autobus
5. default.jpg - Fallback
```

### Etap 2 (Rozszerzone - 6 obrazków):
```
6. t.jpg    - Ci¹gnik
7. am.jpg   - Motorower
8. be.jpg   - Samochód z przyczep¹
9. c1.jpg   - Œrednia ciê¿arówka
10. d1.jpg  - Minibus
11. b1.jpg  - Quad
```

### Etap 3 (Pe³ne - pozosta³e 6):
```
12. a1.jpg   - Motocykl 125
13. a2.jpg   - Motocykl 35kW
14. ce.jpg   - Ciê¿arówka z naczep¹
15. c1e.jpg  - C1 z przyczep¹
16. de.jpg   - Autobus przegubowy
17. d1e.jpg  - Minibus z przyczep¹
```

## ?? Po pobraniu:

### SprawdŸ nazwê:
```bash
# Poprawne:
b.jpg ?
c.jpg ?
d.jpg ?

# Niepoprawne:
B.jpg ? (wielka litera)
car.jpg ? (z³a nazwa)
b.png ? (z³y format)
```

### SprawdŸ rozmiar:
```
Plik < 150KB ?
```

### SprawdŸ wymiary:
```
800x600px lub podobne proporcje 4:3 ?
```

## ?? Umieszczanie w projekcie:

```bash
# Lokalizacja:
DriverGuide.UI/wwwroot/images/categories/

# Struktura powinna wygl¹daæ tak:
wwwroot/
??? images/
    ??? categories/
        ??? am.jpg
        ??? a.jpg
        ??? b.jpg
        ??? ... (pozosta³e)
```

## ? Weryfikacja:

### W Visual Studio:
1. Solution Explorer
2. DriverGuide.UI ? wwwroot ? images ? categories
3. Powinno byæ widocznych 17 plików .jpg

### W przegl¹darce:
1. Uruchom aplikacjê (F5)
2. PrzejdŸ do: `/test`
3. Powinny za³adowaæ siê obrazki

### Test URL:
```
http://localhost:5000/images/categories/b.jpg
```
Powinien wyœwietliæ obrazek samochodu.

## ?? Troubleshooting:

### Obrazek siê nie ³aduje:
- ? SprawdŸ nazwê pliku (ma³e litery!)
- ? SprawdŸ rozszerzenie (.jpg nie .jpeg)
- ? SprawdŸ œcie¿kê (categories nie category)
- ? Zrestartuj aplikacjê (Ctrl+F5)

### Obrazek jest rozmazany:
- ? Zwiêksz jakoœæ do 90%
- ? U¿yj wiêkszego Ÿród³a (1200x900)

### Obrazek jest za du¿y (>150KB):
- ? U¿yj TinyJPG do kompresji
- ? Zmniejsz jakoœæ do 75%

## ?? Przyk³adowe kolory preferowane:

```
Samochody:    Niebieski, Srebrny, Bia³y
Motocykle:    Czerwony, Czarny, Niebieski
Ciê¿arówki:   Bia³y, Srebrny, Czerwony
Autobusy:     Bia³y, Czerwony, ¯ó³ty
Ci¹gniki:     Zielony, Czerwony, Niebieski
```

## ?? Szacowany czas:

```
Etap 1 (5 obrazków):    15-20 minut
Etap 2 (6 obrazków):    20-25 minut
Etap 3 (6 obrazków):    20-25 minut
Optymalizacja:          10-15 minut
-----------------------------------------
RAZEM:                  ~60-85 minut
```

## ?? Gotowe!

Po dodaniu obrazków aplikacja bêdzie dzia³aæ z piêknymi, profesjonalnymi zdjêciami pojazdów!

Fallback (`default.jpg`) mo¿e byæ prostym obrazkiem z ikon¹ lub tekstem "Brak obrazka".
