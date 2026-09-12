# Programmdokumentation

## Kurzbeschreibung

WaagenSpiel ist eine WPF-Anwendung, in der mehrere Gruppen nacheinander Steine auf zwei Waagen platzieren. Ziel ist es, Spielzuege durchzufuehren und die Gewichte der Steine zu vergleichen.

## Projektstruktur

- `SpielWindow.xaml` und `SpielWindow.xaml.cs`: Hauptfenster und UI-Logik
- `Spiel/SpielManager.cs`: Verwaltung der Gruppenreihenfolge
- `Models/Gruppe.cs`: Modell einer Gruppe mit Steinen und Ausscheidungsstatus
- `Models/Stein.cs`: Modell eines Steins mit Farbe, Gewicht und Platzierungsstatus
- `Models/Waage.cs`: Logik fuer linke und rechte Waagenseite

## Wesentliche Funktionen

`SpielManager` verwaltet die Gruppen und waehlt die naechste aktive Gruppe. Ausgeschiedene Gruppen werden beim Weiterlaufen uebersprungen.

Der Button "Gruppe raus X" markiert die aktuell angezeigte Gruppe als ausgeschieden. Wenn keine aktive Gruppe mehr uebrig ist, zeigt die UI eine Abschlussmeldung an.

## Bekannte Grenzen

- Es gibt noch keine explizite Rundenverwaltung.
- Der Spielstand wird nicht gespeichert.
- Die geplanten Erweiterungen in `03_Erweiterungen` sind noch nicht vollstaendig umgesetzt.
