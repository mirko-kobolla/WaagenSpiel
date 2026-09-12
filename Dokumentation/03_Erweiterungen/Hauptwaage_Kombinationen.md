# Erweiterung: Identische Steinkombinationen verhindern

## Beschreibung

Diese Erweiterung verhindert, dass durch einen neuen Stein auf der Hauptwaage auf beiden Seiten identische Steinkombinationen nach Farbe entstehen.

## Verhalten

Vor dem Platzieren wird die Aktion simuliert. Sind die Farbmengen auf der linken und rechten Seite danach gleich, wird das Platzieren abgebrochen. Die Statusanzeige informiert die Spieler ueber den Grund; der Stein bleibt unplatziert.

Die Pruefung gilt nur fuer die Hauptwaage. Die Zweitwaage bleibt unveraendert.

## Betroffene Programmteile

- `SpielWindow.xaml.cs`: Simulation und Abbruch in `PlatziereSteinAufWaage`
- `Models/Waage.cs`: Seiten- und Kombinationsdaten fuer die Pruefung
