# Analiza paternelor de arhitectură pentru dezvoltarea aplicatiilor web

Teza de licența

- [Analiza paternelor de arhitectură pentru dezvoltarea aplicatiilor web](#analiza-paternelor-de-arhitectură-pentru-dezvoltarea-aplicatiilor-web)
  - [Întroducere](#întroducere)
  - [Capitolul 1. Șabloane de proiectare pentru dezvoltarea aplicatiilor web](#capitolul-1-șabloane-deproiectare-pentru-dezvoltarea-aplicatiilor-web)
    - [Șabloane de proiectare](#șabloane-de-proiectare)
  - [Capitolul 2. Analiza paternelor de proiectare](#capitolul-2-analiza-paternelor-de-proiectare)
    - [Model-View-Controller](#model-view-controller)
    - [Model-View-View-Model](#model-view-viewmodel)
    - [Model-View-Presenter](#model-view-presenter)
    - [Compararea șabloanelor de arhitectură](#compararea-șabloanelor-de-arhitectură)
  - [Capitolul 3. Un exemplu de implementare a MVC](#capitolul-3-un-exemplu-de-implementare-a-mvc)
    - [Formularea problemei](#formularea-problemei)
    - [Proiectarea soluției](#proiectarea-soluției)
    - [Particularitățile implementării](#particularitățile-implementării)
    - [Testare](#testare)
  - [Concluzie](#concluzie)
  - [Bibliografie](#bibliografie)
  - [Anexe](#anexe)

## Întroducere

## Capitolul 1. Șabloane de proiectare pentru dezvoltarea aplicatiilor web

### Șabloane de proiectare

## Capitolul 2. Analiza paternelor de proiectare




### Model-View-ViewModel

### Model-View-Presenter

### Compararea șabloanelor de arhitectură

## Capitolul 3. Un exemplu de implementare a MVC

### Formularea problemei

### Proiectarea soluției

### Particularitățile implementării

### Testare

## Concluzie

## Bibliografie

1. [iamprovidence, MVC vs MVP vs MVVM with C# examples, https://medium.com, 2022](https://medium.com/@iamprovidence/mvc-vs-mvp-vs-mvvm-with-c-examples-8013745e3c4c)
2. [Tian Lou, A Comparison of Android Native App Architecture – MVC, MVP and MVVM, Master’s Thesis, Espoo, September 06, 2016](https://pure.tue.nl/ws/portalfiles/portal/48628529/Lou_2016.pdf)

3. https://books.google.md/books?hl=en&lr=&id=vqTfNFDzzdIC&oi=fnd&pg=PR7&dq=Web+Application+Architectural+Patterns+definitions&ots=oVCiCtPQty&sig=grFjsIHUJ-hReSEv96bVSDJmAbI&redir_esc=y#v=onepage&q&f=false

4.https://www.researchgate.net/profile/Lech-Madeyski/publication/221679095_Architectural_Design_of_Modern_Web_Applications/links/5c8fdf3292851c1df94a5233/Architectural-Design-of-Modern-Web-Applications.pdf


## Anexe

Capitolul 1. Șabloane de proiectare pentru dezvoltarea aplicatiilor web
1.1 Introducerea în Șabloane de Proiectare
Șabloanele de proiectare (cunoscut și sub numele de Design Patterns) reprezintă un set de soluții generale acceptate pentru rezolvarea problemelor comune de design software ce apar în cadrul dezvoltării unei aplicații aplicațiilor desktop/web. Acestea sunt metode reutilizabile, testate în practică care și-au arătat eficiența. În plus, permit dezvoltatorilor să construiască sisteme informatice scalabile, eficiente și care pot fi întreținute mai ușor și de dezvoltat pe un termen lung [1][2].
Cu toate aceste, scopul principal al unui șablon de proiectare nu este de a rezolva toate problemele întâlnite într-un proiect software, ci de a aborda în mod sistematic obstacole recurente în dezvoltare, cum ar fi gestionarea complexității sau organizarea codului. Un exemplu relevant în acest sens este atunci când se construiesc aplicațiile web mari, ce au multe interacțiuni între utilizator și server, șabloanele de proiectare permit dezvoltatorilor să gestioneze aceste interacțiuni într-un mod structurat, fără să crească arhitectura aplicației [1][3].
De asemenea, șabloanele de proiectare ajută dezvoltatorii și arhitecții software să adopte soluții deja testare, care au avut rezultate pozitive pe parcursul dezvoltării sferei tehnologiilor informaționale. Prin utilizarea acestor soluții standardizare, riscurile care apar la efectuarea unor decizii anumite este redus, iar eficiența dezvoltării crește semnificativ. Astfel, șabloanele nu doar economisesc tip, dar și îmbunătățesc drastic calitatea și ușurează întreținerea produsului software final [2].
 Un alt beneficiu al șabloanelor de proiectare este faptul că acestea contribuie la crearea unui cod modular, urmând principiile POO (Programarea Orientată pe Obiecte), permițând reutilizarea și adaptabilitatea codului. În dezvoltarea aplicațiilor web, care adesea sunt complexe și în continuă dezvoltare, această modularitate este esențială pentru a putea adăuga noi funcționalități sau pentru a modifica aplicația fără a perturba întregul sistem. Un exemplu important de șablonului de proiectare este Model-View-Controller (MVC) în care logica funcționării aplicației este separată de interfața ce interacționează cu utilizatorul, facilitând actualizările și extinderile aplicației fără a afecta alte componente[3].
Prin urmare, șabloanele de proiectare oferă soluții testate de timp pentru probleme comune ca apar în cadrul majorității proceselor de dezvoltare, asigurând o structură clară a codului și reutilizarea acestuia, cu posibilitatea de a face modificări fără a afecta lucrul aplicației datorită separarea elementelor de interfață și logică a programului.
