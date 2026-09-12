# Programmdokumentation

## Kurzbeschreibung

WaagenSpiel besteht aus einer WPF-Version fuer Windows und einer Blazor-WebAssembly-PWA fuer iPad und iPhone. Beide Versionen verwenden den gleichen Spielkern. Mehrere Gruppen platzieren nacheinander Steine auf Hauptwaage und Nebenwaage. Ziel ist es, die Seitenverhaeltnisse zu vergleichen und die Gewichte logisch zu bestimmen.

## Projektstruktur

- `SpielWindow.xaml` und `SpielWindow.xaml.cs`: Hauptfenster und UI-Logik
- `Spiel/SpielManager.cs`: Verwaltung der Gruppenreihenfolge
- `Models/Gruppe.cs`: Modell einer Gruppe mit Steinen und Ausscheidungsstatus
- `Models/Stein.cs`: Modell eines Steins mit Farbe, Gewicht und Platzierungsstatus
- `Models/Waage.cs`: Logik fuer linke und rechte Waagenseite

Die PWA liegt im Ordner `IPadVersion`. Ihre Startseite ist `IPadVersion/Pages/Home.razor`. Sie zeigt keine exakten Gewichte, sondern nur die Vergleichsergebnisse `Linke Seite schwerer`, `Rechte Seite schwerer` oder `Im Gleichgewicht`. Nur ein Gleichgewicht der Hauptwaage loest die aktuelle Gruppe; die Nebenwaage dient als Hilfswaage.

## Wesentliche Funktionen

`SpielManager` verwaltet die Gruppen und waehlt die naechste aktive Gruppe. Ausgeschiedene Gruppen werden beim Weiterlaufen uebersprungen.

In der PWA prueft der Button `Loesung pruefen` die Hauptwaage. Ist sie ausgeglichen und sind die Farbkombinationen nicht identisch, wird die aktuelle Gruppe abgeschlossen und automatisch zur naechsten aktiven Gruppe gewechselt. `Neue Partie` setzt die drei technischen Startgruppen zurueck.

Die urspruengliche WPF-Oberflaeche besitzt weiterhin eigene Bedienelemente und ist nicht mit der PWA-Oberflaeche gleichzusetzen.

## Bekannte Grenzen

- Es gibt noch keine explizite Rundenverwaltung.
- Der Spielstand wird nicht gespeichert.
- Die geplanten Erweiterungen in `03_Erweiterungen` sind noch nicht vollstaendig umgesetzt.
