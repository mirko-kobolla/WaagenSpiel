# Fortschritt vom 13.09.2026

## Umgesetzte Funktionen

- Die iPad-Version besitzt einen Startbildschirm.
- Die Anzahl der Gruppen kann vor dem Spielstart festgelegt werden.
- Jede Gruppe kann einen eigenen Namen erhalten.
- Alle Gruppen verwenden dieselben Waagen.
- Nach dem Zug der letzten Gruppe beginnt wieder die erste Gruppe.
- Die Waagen bleiben waehrend der gesamten Partie erhalten.
- Bei einer belegten und ausgeglichenen Hauptwaage kann die Gruppe die Gewichte raten.
- Das Raten der Gewichte ist freiwillig; der Zug kann auch ohne Raten beendet werden.
- Die Anwendung zeigt das Ergebnis von `Gewichte pruefen` direkt unter dem Button an.
- Richtig geratene Farbgewichte werden ueber mehrere Rateversuche gespeichert.
- Unter gelben Steinen wird das bekannte Gewicht `10` angezeigt.
- Wenn alle Steine platziert sind, wird die Partie automatisch ausgewertet.

## Gewinnbedingung

Die Gruppe gewinnt, wenn:

1. alle Steine aller Gruppen platziert wurden,
2. die Hauptwaage ausgeglichen ist und
3. alle Farbgewichte mindestens einmal richtig geraten wurden.

Sind nach dem letzten Zug nicht alle Bedingungen erfuellt, zeigt die Anwendung eine Verlustmeldung an.

## Technische Aenderungen

- `Home.razor` verwaltet den Ratefortschritt und den Partieabschluss.
- `SpielManager` wechselt die Gruppen zyklisch.
- `Gruppe` kann einen Namen speichern.
- Die Styles enthalten die Eingabemaske und Rueckmeldung fuer die Gewichtspruefung.
- Die Erweiterungsvorschlaege dokumentieren den Rundlauf und die Gewichtsabfrage.
